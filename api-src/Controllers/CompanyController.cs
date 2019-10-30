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
        private readonly ICompanyService _service;

        public CompanyController(ICompanyService service)
        {
            _service = service;
        }

        /// <summary>
        /// Just using this to test that my endpoints/routing works
        /// </summary>
        /// <returns>A 200 OK</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("test")]
        public IActionResult test()
        {
            return Ok();
        }

        /// <summary>
        /// Used for initial JWT authentication 
        /// </summary>
        /// <param name="auth"></param>
        /// <returns>A signed token, if successful</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Authenticate([FromBody]Authentication auth)
        {
            
            var token = _service.Authenticate(auth.Username, auth.Password);

            if (token == "")
                return Unauthorized(new { message = "Username or password is incorrect" });

            return Ok(token);
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

        /// <summary>
        /// Update an existing Company entity. We don't expect or want the updated entity from the client to include the Id,
        /// so we strip it out by creating a new entity based on the old one. This also allows us to sanitize each field before just updating it.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="c"></param>
        /// <returns></returns>
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
        /// <summary>
        /// A validation check for ISIN format, defined as per ISO-6166
        /// </summary>
        /// <param name="ISIN"></param>
        /// <returns></returns>
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