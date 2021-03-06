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
        Task<Customer> Delete(Customer customer);
        Task<Customer> Update(Customer customer);
        Task<Customer> Create(Customer customer);
    }

    public class CustomerService : ICustomerService
    {
        private ArtaplanContext _context;
        private int userId;
        public CustomerService(ArtaplanContext context, IUserProvider userProvider)
        {
            _context = context;
            userId = userProvider.GetUserId();
        }
        public async Task<Customer> Create(Customer customer)
        {
            customer.UserId = userId;
            customer.CustomerId = 0;
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> Delete(Customer customer)
        {
            if (userId != customer.UserId)
            {    
                return null;
            }
            try
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                return customer;
            }
            catch 
            { 
                return null;
            }
        }
        
        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _context.Customers.Where(x => x.UserId == userId).Include(x => x.User).Include(x => x.Jobs).ToListAsync();
        }
        
        public async Task<Customer> GetById(int id)
        {
            return await _context.Customers.Where(x => x.CustomerId == id && x.UserId == userId).Include(x => x.User).Include(x => x.Jobs).FirstOrDefaultAsync();
        }

        public async Task<Customer> Update(Customer customer)
        {
            if(customer.UserId != userId)
            {
                return null;
            }
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return customer;
        }
    }
}
