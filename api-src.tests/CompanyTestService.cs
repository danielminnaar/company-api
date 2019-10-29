using System.Collections.Generic;

namespace api_src.tests
{
    public class CompanyTestService : api_src.Models.ICompanyService
    {
        private List<Company> companies;
        public CompanyTestService() 
        {
            companies = new List<Company>();
            companies.Add(new Company() { Id = 1, Name = "Test 1", Ticker = "Ticker 1", Exchange = "EXCH1", ISIN = "AB1234567890", Website = ""});
            companies.Add(new Company() { Id = 2, Name = "Test 2", Ticker = "Ticker 2", Exchange = "EXCH2", ISIN = "CD1234567890", Website = "test.com"});
        }

        public string Authenticate(string username, string password)
        {
            if(username == "test" && password == "test")
                return "OK";
            else
                return "";
        }

        public void Create(Company newCompany)
        {
            companies.Add(newCompany);
        }

        public List<Company> GetAll()
        {
            return companies;
        }

        public Company GetById(int id)
        {
            return companies.Find(x => x.Id == id);
        }

        public Company GetByISIN(string isin)
        {
            return companies.Find(x => x.ISIN == isin); 
        }

        public void Update(int id, Company updatedCompany)
        {
            updatedCompany.Id = id;
            companies[companies.FindIndex(x => x.Id == id)] = updatedCompany;
        }
    }
}