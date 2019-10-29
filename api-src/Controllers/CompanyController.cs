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
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _service;

        public CompanyController(ICompanyService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var company = _service.GetById(id);
            if (company != null)
                return Ok(company);
            else
                return NotFound();
           
        }

        [Route("[action]/{isin}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByISIN(string isin)
        {
            var company = _service.GetByISIN(isin);
            if (company != null)
                return Ok(company);
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

            try
            {
                _service.Update(id, c);
                return Ok("Company updated");
            }
            catch (System.Exception ex)
            {
                return BadRequest("Cannot update company - does it exist? Error: " + ex.Message);
            }

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

            try
            {
                // Sanitize
                var comp = new Company() { Name = c.Name, Exchange = c.Exchange, ISIN = c.ISIN, Ticker = c.Ticker, Website = c.Website};
                _service.Create(comp);
                return Ok("Company created");
            }
            catch (System.Exception ex)
            {
                // Sanitize
                return BadRequest("Cannot create company. Error: " + ex.Message);
            }
            
            
        }

        public bool ISINExists(string ISIN, int excludeId = -1)
        {
            var company = _service.GetByISIN(ISIN);
            if(excludeId >= 0)
            {
                if(company != null && company.Id != excludeId)
                    return true;
            }
            else if (company != null)
                return true;
            else
                return false;

            return false;
        }

        public bool IsValidISINFormat(string ISIN) 
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