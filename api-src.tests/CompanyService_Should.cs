using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
namespace api_src.tests
{
    public class CompanyService_Should
    {
        private readonly api_src.Models.ICompanyService _service;
        private readonly api_src.Controllers.CompanyController _controller;

        public CompanyService_Should()
        {
            _service = new CompanyTestService();
            _controller = new Controllers.CompanyController(_service);
        }

        [Fact]
        public void Get_ShouldReturn200OK()
        {
            var response = _controller.Get();
            Assert.IsType<OkObjectResult>(response);
            
        }

        [Fact]
        public void Get_ShouldReturnTwoCompanies()
        {
            var response = _controller.Get();
            var okResult = response as OkObjectResult;
            var list = okResult.Value as List<Company>;
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public void Post_ShouldCreateCompany()
        {
            var response = _controller.Create(new Company() {Id = 3, Name = "Test 3", Ticker = "Ticker 3", Exchange = "EXCH3", ISIN = "EF1234567890", Website = "test3.com"});
            Assert.IsType<OkObjectResult>(response);
        }
        
        [Fact]
        public void Get_ShouldFindCompanyById() 
        {
            var response = _controller.GetById(1);
            var okResult = response as OkObjectResult;
            var company = okResult.Value as Company;
            Assert.Equal(company.Id, 1);
        }

        [Fact]
        public void Get_ShouldFindCompanyWithISINWith200OK()
        {
            string ISIN = "AB1234567890";
            var response = _controller.GetByISIN(ISIN);
            var okResult = response as OkObjectResult;
            var company = okResult.Value as Company;
            Assert.Equal(company.ISIN, ISIN);
        }

        [Fact]
        public void Util_ShouldNotFindCompanyThatExistsWithISIN()
        {
            var response = _controller.GetById(1);
            var okResult = response as OkObjectResult;
            var company = okResult.Value as Company;
            string ISIN = "AB1234567890";
            // excluding this company and it's ISIN shouldn't find any other matches
            bool exists = _controller.ISINExists(ISIN, 1);
            Assert.False(exists);
        }
    }
}
