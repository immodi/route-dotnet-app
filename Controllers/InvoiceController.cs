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
    public class InvoiceController : ControllerBase
    {
        private readonly IRepository<Invoice> _repository;

        public InvoiceController(IRepository<Invoice> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetAllInvoices()
        {
            var Items = await _repository.GetAllAsync();
            return Ok(Items);
        }
    

    }
}
