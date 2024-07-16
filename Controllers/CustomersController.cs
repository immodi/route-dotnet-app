using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SqlServerWebApi.Data;
using SqlServerWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlServerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
       private readonly IRepository<Customer> _repository;
        private readonly IMapper _mapper;

        public CustomersController(IRepository<Customer> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAllCustomers()
        {
            var customers = await _repository.GetAllAsync();
            var customerDTOs = _mapper.Map<List<CustomerDTO>>(customers);

            return Ok(customerDTOs);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> AddCustomer(CustomerDTO _customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var customer = await _repository.AddAsync(
                        _mapper.Map<Customer>(_customer)
                    );

                    var customerDTO = _mapper.Map<CustomerDTO>(customer);

                    return Ok(customerDTO);
                }

                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    
        [Route("/[controller]/{customerId}/orders")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrders(int customerId)
        {
            try
            {
                var customer = await _repository.GetByIdAsync(customerId);
                var orders = customer?.Orders;
                var orderDTOs = new List<OrderDTO>();
                
                foreach (var order in orders)
                {
                    orderDTOs.Add( _mapper.Map<OrderDTO>(order));
                }
                
                return Ok(orderDTOs);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
