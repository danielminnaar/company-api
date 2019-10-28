using api_src.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace api_src.Controllers
{
    [Route("[controller]")]
    [Authorize]
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var obj = _context.Companies.Where(b => b.Id == id).FirstOrDefault();
            if (obj != null)
                return Ok(obj);
            else
                return NotFound();
           
        }

        [Route("[action]/{isin}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByISIN(string isin)
        {
            var obj = _context.Companies.Where(b => b.ISIN == isin).FirstOrDefault();
            if (obj != null)
                return Ok(obj);
            else
                return NotFound("Couldn't find ISIN " + isin);
           
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(int id, Company c)
        {
            if(ISINExists(c.ISIN, id))
                return BadRequest("ISIN already exists");
            if(!IsValidISINFormat(c.ISIN))
                return BadRequest("Invalid ISIN");

            var existingCompany = _context.Companies.Where(comp => comp.Id == id).FirstOrDefault();
            if(existingCompany != null)
            {
                existingCompany.Name = c.Name;
                existingCompany.ISIN = c.ISIN;
                existingCompany.Website = c.Website;
                existingCompany.Ticker = c.Ticker;
                existingCompany.Exchange = c.Exchange;
                _context.SaveChanges();
                return Ok("Company updated");
            }
            else
                return NotFound("The company does not exist");

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create(Company c)
        {
            if(ISINExists(c.ISIN))
                return BadRequest("ISIN already exists");
            if(!IsValidISINFormat(c.ISIN))
                return BadRequest("Invalid ISIN");

            // Sanitize
            var comp = new Company() { Name = c.Name, Exchange = c.Exchange, ISIN = c.ISIN, Ticker = c.Ticker, Website = c.Website};
            _context.Companies.Add(comp);
            _context.SaveChanges();
            return Ok("Company created");
        }

        private bool ISINExists(string ISIN, int excludeId = -1)
        {
            if(_context.Companies.Where(company => company.ISIN.ToUpper() == ISIN.ToUpper() && company.Id != excludeId).FirstOrDefault() == null)
                return false;
            else
                return true;
        }

        private bool IsValidISINFormat(string ISIN) 
        {
            ISIN = ISIN.Trim().ToUpper();
            Regex r = new Regex("^[A-Z]{2}[A-Z0-9]{10}");
            if (!r.IsMatch(ISIN)) {
                return false;
            }
            return true;
        }
    }
}