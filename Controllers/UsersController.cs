using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Artaplan.Models;
using Microsoft.AspNetCore.Authorization;
using Artaplan.MapModels.Users;
using Artaplan.Services;
using AutoMapper;
using Artaplan.Errors;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Artaplan.Helpers;

namespace Artaplan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ArtaplanContext _context;
        private IUserService _userService;
        private readonly IUserProvider _userProvider;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        public UsersController(
            ArtaplanContext context, 
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IUserProvider userProvider
            )
        {

            _appSettings = appSettings.Value;
            _context = context;
            _userService = userService;
            _userProvider = userProvider;
            _mapper = mapper;

        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(ErrorMessage.ShowErrorMessage(Error.FailedAuthentication));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                User = _mapper.Map<UserDTO>(user),
                Token = tokenString
            }); 
        }
        [AllowAnonymous]
        [Authorize]
        [HttpGet("current")]    
        public async Task<IActionResult> Current()
        {
           // int id = int.Parse(HttpContext.Prin.Identity.Name); 
            int id = _userProvider.GetUserId();
              var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(ErrorMessage.ShowErrorMessage(Error.UserNotFound));
            }

            return  Ok(_mapper.Map<UserDTO>(user));
        }
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUsers(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }
            if (await _context.Users.FindAsync(user.UserId) == null)
            {
                return NotFound();
            }
            user = await _userService.Update(user);
            return user;
        }
        public async Task<ActionResult<User>> ChangePassword(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }
            if (await _context.Users.FindAsync(user.UserId) == null)
            {
                return NotFound();
            }
            user = await _userService.Update(user);
            return user;
        }

        // POST: api/Users/register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                _userService.Create(user, model.Password);
                return Ok("done");
            }
            catch
            {
                // return error message if there was an exception
                return BadRequest(ErrorMessage.ShowErrorMessage(Error.InternalServerError));
            }
        }
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return users;
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
