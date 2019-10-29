using api_src.Maps;
using Microsoft.EntityFrameworkCore;
using api_src.Models;
using System;
using System.Collections.Generic;

namespace api_src.Models{
    public interface ICompanyService
    {
        Company GetById(int id);
        List<Company> GetAll();
        Company GetByISIN(string isin);
        void Update(int id, Company updatedCompany);
        void Create(Company newCompany);
    } 
}