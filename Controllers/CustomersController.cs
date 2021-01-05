using Artaplan.Errors;
using Artaplan.MapModels.Customers;
using Artaplan.Models;
using Artaplan.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUserProvider _userProvider;
        private readonly ArtaplanContext _context;
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;

        public CustomersController(
            ArtaplanContext context,
            IUserProvider userProvider,
            IMapper mapper,
            ICustomerService customerService
            )
        {
            _mapper = mapper;
            _userProvider = userProvider;
            _context = context;
            _customerService = customerService;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            var customers = await _customerService.GetAll();
            if (customers.Any())
            {
                return NotFound();
            }

            return _mapper.Map<List<CustomerDTO>>(customers);

        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomers(int id)
        {

            var customer = await _customerService.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }

            return _mapper.Map<CustomerDTO>(customer);
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> PostCustomers(CustomerDTO customerDTO)
        {
            var customer = _mapper.Map<Customer>(customerDTO);
            customer = await _customerService.Create(customer);
            return _mapper.Map<CustomerDTO>(customer);
        }

        //DELETE: api/Customers
        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomerDTO>> DeleteCustomer(int id, CustomerDTO customerDTO)
        {
            var customer = _mapper.Map<Customer>(customerDTO);
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }
            if (!_context.Customers.Where(x => x == customer).Any())
            {
                return NotFound();  
            }
            var jobs = _context.Jobs.Where(x => x.CustomerId == customer.CustomerId);
            if (jobs.Any())
            {
                return Conflict();
            }
            customer = await _customerService.Delete(customer);
            if (customer == null)
            {
                return NotFound();
            }
            return _mapper.Map<CustomerDTO>(customer);
        }

        //PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDTO>> UpdateCustomer(int id, CustomerDTO customerDTO)
        {
            var customer = _mapper.Map<Customer>(customerDTO);
            if (customer.CustomerId != id)
            {
                return BadRequest();
            }
            if(!_context.Customers.Where(x => x == customer).Any())
            {
                await _customerService.Create(customer);
                return _mapper.Map<CustomerDTO>(customer);
            }
            customer = await _customerService.Update(customer);
            if (customer == null)
            {
                return NotFound();
            }
            return _mapper.Map<CustomerDTO>(customer);
        }
    }   
}
