using Artaplan.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.Services
{
    public interface ICustomerService
    {
        Task<Customer> GetById(int id);

        Task<IEnumerable<Customer>> GetAll();
        void Delete();
        Task<Customer> Update(Customer customer);
        Task<Customer> Create(Customer customer);
    }

    public class CustomerService : ICustomerService
    {
        private ArtaplanContext _context;
        //Todo: useless?
        private IUserProvider _userProvider;
        private int userId;
        public CustomerService(ArtaplanContext context, IUserProvider userProvider)
        {
            _context = context;
            _userProvider = userProvider;
            userId = userProvider.GetUserId();
        }
        public async Task<Customer> Create(Customer customer)
        {
            customer.UserId = userId;
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        //Todo: delete by id/all?
        public void Delete()
        {
            throw new NotImplementedException();
        }
        
        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _context.Customers.Where(x => x.UserId == userId).ToListAsync();
        }
        
        public async Task<Customer> GetById(int id)
        {
            return await _context.Customers.Where(x => x.CustomerId == id && x.UserId == userId).Include(x => x.Jobs).FirstOrDefaultAsync();
        }

        public Task<Customer> Update(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
