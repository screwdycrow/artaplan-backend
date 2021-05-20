using Artaplan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Artaplan.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Artaplan.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User Create(User user, string password);
        Task<User> Update(User user, string password = null);
        Task<bool> ChangePassword(User user, string password);
        void Delete(int id);
        User GetById(int id);
    }
    public class UserService : IUserService
    {
        private ArtaplanContext _context;
        private IUserProvider _userProvider;

        public UserService(ArtaplanContext context, IUserProvider userProvider)
        {
            _context = context;
            _userProvider = userProvider;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            var user = _context.Users.SingleOrDefault(x => x.Username == username);
            if (user == null)
                return null;
            if (!VerifyHash(password, user))
            {
                return null;
            }
            return user;
        }

    
        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.Username == user.Username))
                throw new AppException("Username \"" + user.Username + "\" is already taken");

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = Convert.ToBase64String(passwordHash);
            user.Salt = Convert.ToBase64String(passwordSalt);

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }
     
        static bool VerifyHash(string password, User user)
        {

            if (password == null) throw new ArgumentNullException("password");
            byte[] storedHash = Convert.FromBase64String(user.PasswordHash);
            byte[] storedSalt = Convert.FromBase64String(user.Salt);

            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
           
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ChangePassword(User user, string password = null)
        {
            if (user.UserId != _userProvider.GetUserId())
            {
                return false;
            }
       CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = Convert.ToBase64String(passwordHash);
            user.Salt = Convert.ToBase64String(passwordSalt);            
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }
    
        public async Task<User> Update(User user, string password = null)
        {
            if(user.UserId != _userProvider.GetUserId())
            {
                return null; 
            }
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
