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
        public async Task<ActionResult<IEnumerable<Slot>>> GetCustomers()
        {
            try
            {
                var customers = await _customerService.GetAll();
                return Ok(_mapper.Map<List<CustomerDTO>>(customers));

            }
            catch
            {
                return BadRequest(ErrorMessage.ShowErrorMessage(Error.InternalServerError));
            }

        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomers(int id)
        {

            var customer = await _customerService.GetById(id);
            if (customer == null)
            {
                return NotFound(ErrorMessage.ShowErrorMessage(Error.CustomerNotFound));
            }

            return _mapper.Map<CustomerDTO>(customer);
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> PostCustomers(Customer customer)
        { 
            customer = await _customerService.Create(customer);
            return _mapper.Map<CustomerDTO>(customer);
        }

    }   
}
