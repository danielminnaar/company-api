using api_src.Maps;
using Microsoft.EntityFrameworkCore;
using api_src.Models;
using System.Linq;
using System;
using System.Collections.Generic;

namespace api_src.Models{
    public class CompanyDBService : ICompanyService
    {
        private readonly ApiDbContext _context;
        public CompanyDBService(ApiDbContext context) 
        {
            _context = context;
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