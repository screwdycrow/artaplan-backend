using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Artaplan.Services
{
    public interface IUserProvider
    {
        int  GetUserId();
    }
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _context;

        public UserProvider(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public int GetUserId()
        {

            return int.Parse( _context.HttpContext.User
                       .Identity.Name);
        }
    }
}
