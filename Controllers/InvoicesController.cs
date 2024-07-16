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
        
    public class InvoicesController : ControllerBase
    {
        private readonly IRepository<Invoice> _repository;
        private readonly IMapper _mapper;

        public InvoicesController(IRepository<Invoice> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceDTO>>> GetAllInvoices()
        {
            var invoices = await _repository.GetAllAsync();
            var invoiceDTOs = _mapper.Map<List<InvoiceDTO>>(invoices);
            return Ok(invoiceDTOs);
        }
    
        [Route("/[controller]/{invoiceId}")]
        [HttpGet]
        public async Task<ActionResult<InvoiceDTO>> GetInvoiceById(int invoiceId) 
        {
            try
            {
                var invoice = await _repository.GetByIdAsync(invoiceId);
                if (invoice != null)
                {
                    var invoiceDTO = _mapper.Map<InvoiceDTO>(invoice);
                    return Ok(invoiceDTO);
                } 
                
                throw new Exception(string.Format("Invoice with ID: {0} doesn't exist", invoiceId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
