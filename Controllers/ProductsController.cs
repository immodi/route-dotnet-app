using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;
using SqlServerWebApi.Data;
using SqlServerWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SqlServerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _repository;
        private readonly IMapper _mapper;

        public ProductsController(IRepository<Product> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            var products = await _repository.GetAllAsync();
            var productDTOs = _mapper.Map<List<ProductDTO>>(products);
            return Ok(productDTOs);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> AddProduct(ProductDTO _product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var product = await _repository.AddAsync(
                        new Product{
                            Name = _product.Name,
                            Price = _product.Price,
                            Stock = _product.Stock
                        }
                    );
                
                    var productDTO = _mapper.Map<ProductDTO>(product);
                    return Ok(productDTO);
                } 

                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("/[controller]/{productId}")]
        [HttpGet]
        public async Task<ActionResult<OrderDTO>> GetProductById(int productId) 
        {
            try
            {
                var product = await _repository.GetByIdAsync(productId);
                if (product != null)
                {
                    var productDTO = _mapper.Map<ProductDTO>(product);
                    return Ok(productDTO);
                } 
                
                throw new Exception(string.Format("Product with ID: {0} doesn't exist", productId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("/[controller]/products/{productId}")]
        [HttpPut]
        public async Task<ActionResult<ProductDTO>> UpdateOrderById(int productId, [FromBody] ProductDetailsDTO newProductDetails) 
        {
            try
            {
                var product = await _repository.GetByIdAsync(productId);
                if (product == null)
                {
                    return NotFound();
                } else if (!ModelState.IsValid){
                    return BadRequest(ModelState);
                } else {
                    product.Name = newProductDetails.Name;
                    product.Price = newProductDetails.Price;
                    product.Stock = newProductDetails.Stock;

                    await _repository.UpdateAsync(product);
                    var productDTO = _mapper.Map<ProductDTO>(product);
                    return Ok(productDTO);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
