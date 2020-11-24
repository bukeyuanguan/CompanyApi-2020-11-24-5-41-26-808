﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CompanyApi;
using CompanyApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace CompanyApiTest.Controllers
{
    public class CompaniesControllerTest
    {
        // given
        private TestServer server;
        private HttpClient client;
        public CompaniesControllerTest()
        {
            this.server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            this.client = server.CreateClient();
        }

        [Fact]
        public async Task Should_return_added_company_when_add_company_successful()
        {
            // when
            var company = new Company("1", "Sun");
            var request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Companies", requestBody);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // then
            Company actual = JsonConvert.DeserializeObject<Company>(responseString);
            Assert.Equal(company, actual);
        }
    }
}