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
    public class OrderController : ControllerBase
    {
        private readonly IRepository<Order> _repository;
        private readonly IRepository<Customer> _customer_repository;
        private readonly IRepository<Product> _product_repository;
        private readonly IRepository<Invoice> _invoice_repository;
        private readonly IRepository<OrderItem> _order_item_repository;


        private readonly IMapper _mapper;


        public OrderController(
            IRepository<Order> repository,
            IRepository<Customer> customerRepository,
            IRepository<Product> productRepository,
            IRepository<Invoice> invoiceRepository,
            IRepository<OrderItem> orderItemRepository,
            IMapper mapper
            )
        {
            _customer_repository = customerRepository;
            _repository = repository;
            _product_repository = productRepository;
            _invoice_repository = invoiceRepository;
            _order_item_repository = orderItemRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrders()
        {
            var orders = await _repository.GetAllAsync();
            var orderstDTOs = _mapper.Map<List<OrderDTO>>(orders);

            return Ok(orderstDTOs);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddOrder(OrderDTO _order)
        {
            try
            {
                var customer = await _customer_repository.GetByIdAsync(_order.CustomerId);
                if (customer != null)
                {
                    var orderObject = new Order{
                        CustomerId = _order.CustomerId,
                        PaymentMethod = _order.PaymentMethod,
                        Status = "Ongoing",
                        Customer = customer,
                    };

                    var order = await _repository.AddAsync(orderObject);
                    var totalMoney = 0m;

                    if (order != null)
                    {
                        var orderItems = new List<OrderItem>();
                        foreach (var orderItem in _order.OrderItems)
                        {
                            var product = await _product_repository.GetByIdAsync(orderItem.ProductId);
                            if (product != null && product.Stock > 0)
                            {
                                totalMoney += orderItem.Quantity * product.Price;
                                orderItems.Add(
                                    new OrderItem{
                                        OrderId = order.Id,
                                        ProductId = product.Id,
                                        Quantity = orderItem.Quantity,
                                        UnitPrice = product.Price,
                                        Discount = order.TotalAmount < 100 ? 0.05m : 0.1m,
                                        Order = order,
                                        Product = product
                                    }
                                );
                                product.Stock -= orderItem.Quantity;
                                await _product_repository.UpdateAsync(product);
                            } else continue;
                        }

                        order.OrderItems = orderItems;
                        order.TotalAmount = totalMoney;
                        order.Invoice = new Invoice{
                            Order = order,
                            OrderId = order.Id,
                            TotalAmount = order.TotalAmount
                        };

                        await _order_item_repository.AddRangeAsync(orderItems);
                        await _invoice_repository.AddAsync(order.Invoice);

                        await _repository.UpdateAsync(order);

                        var orderDTO = _mapper.Map<OrderDTO>(order);

                        return Ok(orderDTO);    
                    }
                    throw new Exception("Failed making the order, Please try again");
                }
                throw new Exception("This Customer Doesn't Exist!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    

    }
}
