using api_src.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace api_src.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public CompanyController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("check")]
        public object check() {
            return "works";
        }

        [HttpGet]
        public List<Company> Get()
        {
            return _context.Companies.ToList<Company>();
        }

        [HttpGet("{isin}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByISIN(string isin)
        {
            var obj = _context.Companies.Where(b => b.ISIN == isin).FirstOrDefault();
            if (obj != null)
                return Ok(obj);
            else
                return NotFound();
           
        }

    }
}