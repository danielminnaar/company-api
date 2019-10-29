using api_src.Maps;
using Microsoft.EntityFrameworkCore;
using api_src.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api_src.Models
{
    public class CompanyDBService : ICompanyService
    {
        private readonly ApiDbContext _context;
        public CompanyDBService(ApiDbContext context) 
        {
            _context = context;
        }

        public string Authenticate(string username, string password)
        {
             if(username == "companyadmin" && password == "password")
            {

                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                // obviously going to keep our secret super-secret in prod
                var key = Encoding.ASCII.GetBytes("my_secret_is_really_just_the_best_and_its_pretty_long_too");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] 
                    {
                        new Claim("Name", username)
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            else
                return "";
        }

        public void Create(Company newCompany)
        {
            _context.Companies.Add(newCompany);
            _context.SaveChanges();
        }

        public List<Company> GetAll()
        {
            return _context.Companies.ToList<Company>();
        }

        public Company GetById(int id)
        {
            return _context.Companies.Where(x => x.Id == id).FirstOrDefault();
        }

        public Company GetByISIN(string isin)
        {
            return _context.Companies.Where(b => b.ISIN == isin).FirstOrDefault();
        }

        public void Update(int id, Company updatedCompany)
        {
            var existingCompany = GetById(id);
            if(existingCompany != null)
            {
                existingCompany.Name = updatedCompany.Name;
                existingCompany.ISIN = updatedCompany.ISIN;
                existingCompany.Website = updatedCompany.Website;
                existingCompany.Ticker = updatedCompany.Ticker;
                existingCompany.Exchange = updatedCompany.Exchange;
                _context.SaveChanges();
            }
            
        }
    }
}