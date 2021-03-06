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

using System.Collections.Generic;
using System.Net;

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
        public async Task Should_return_added_company_when_add_company_successfully()
        {
            await client.DeleteAsync("Companies/clear");
            var company = new Company("Sun");
            string request = JsonConvert.SerializeObject(company);
            StringContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");

            //when
            var response = await client.PostAsync("/Companies", requestBody);

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Company actual = JsonConvert.DeserializeObject<Company>(responseString);
            //Assert.Equal(company, actual);
        }

        [Fact]
        public async Task Should_return_all_companies_when_get()
        {
            await client.DeleteAsync("Companies/clear");
            var company1 = new Company("Sun");
            var company2 = new Company("Moon");
            string request1 = JsonConvert.SerializeObject(company1);
            string request2 = JsonConvert.SerializeObject(company2);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");

            //when
            await client.PostAsync("/Companies", requestBody1);
            await client.PostAsync("/Companies", requestBody2);
            var response = await client.GetAsync("/Companies");

            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            List<Company> actual = JsonConvert.DeserializeObject<List<Company>>(responseString);
            Assert.Equal(new List<Company> { company1, company2 }, actual);
        }

        [Fact]
        public async Task Should_return_company_when_get_company_by_id()
        {
            await client.DeleteAsync("Companies/clear");
            var company1 = new Company("Sun");
            var company2 = new Company("Moon");
            string request1 = JsonConvert.SerializeObject(company1);
            string request2 = JsonConvert.SerializeObject(company2);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");

            //when
            var responseWithId = await client.PostAsync("/Companies", requestBody1);
            responseWithId.EnsureSuccessStatusCode();
            var responseStringWithId = await responseWithId.Content.ReadAsStringAsync();
            Company actualWithId = JsonConvert.DeserializeObject<Company>(responseStringWithId);
            var id = actualWithId.Id;
            var response = await client.GetAsync($"/Companies/{id}");
            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Company actual = JsonConvert.DeserializeObject<Company>(responseString);
            Assert.Equal(company1, actual);
        }

        [Fact]
        public async Task Should_return_updated_company_when_update_company_with_certain_id()
        {
            await client.DeleteAsync("Companies/clear");
            var company1 = new Company("Sun");
            var company2 = new Company("Moon");
            var updateCompany = new UpdateCompany("star");
            string request1 = JsonConvert.SerializeObject(company1);
            string request2 = JsonConvert.SerializeObject(company2);
            string updateRequest = JsonConvert.SerializeObject(updateCompany);
            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            StringContent updateRequestBody = new StringContent(updateRequest, Encoding.UTF8, "application/json");

            //when
            var responseWithId = await client.PostAsync("/Companies", requestBody1);
            await client.PostAsync("/Companies", requestBody2);
            responseWithId.EnsureSuccessStatusCode();
            var responseStringWithId = await responseWithId.Content.ReadAsStringAsync();
            Company actualWithId = JsonConvert.DeserializeObject<Company>(responseStringWithId);
            var id = actualWithId.Id;
            var response = await client.PatchAsync($"/Companies/{id}", updateRequestBody);
            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Company actual = JsonConvert.DeserializeObject<Company>(responseString);
            company1.Name = "star";
            Assert.Equal(company1, actual);
        }

        [Fact]
        public async Task Should_return_employee_when_add_eployee_to_company_with_certain_id()
        {
            await client.DeleteAsync("Companies/clear");
            var company1 = new Company("Sun");
            var company2 = new Company("Moon");
            string request1 = JsonConvert.SerializeObject(company1);
            string request2 = JsonConvert.SerializeObject(company2);

            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            //when
            var responseWithId = await client.PostAsync("/Companies", requestBody1);
            await client.PostAsync("/Companies", requestBody2);
            responseWithId.EnsureSuccessStatusCode();
            var responseStringWithId = await responseWithId.Content.ReadAsStringAsync();
            Company actualWithId = JsonConvert.DeserializeObject<Company>(responseStringWithId);
            var id = actualWithId.Id;

            var employee = new Employee("1", "Mike", "6000");
            string request3 = JsonConvert.SerializeObject(employee);
            StringContent requestBody3 = new StringContent(request3, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/Companies/{id}/Employees", requestBody3);
            //then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Employee actual = JsonConvert.DeserializeObject<Employee>(responseString);
            //Assert.Equal(employee, actual);
        }

        [Fact]
        public async Task Should_return_all_employees_when_get_from_company_with_certain_id()
        {
            await client.DeleteAsync("Companies/clear");
            var company1 = new Company("Sun");
            var company2 = new Company("Moon");
            string request1 = JsonConvert.SerializeObject(company1);
            string request2 = JsonConvert.SerializeObject(company2);

            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            //when
            var responseWithId = await client.PostAsync("/Companies", requestBody1);
            await client.PostAsync("/Companies", requestBody2);
            responseWithId.EnsureSuccessStatusCode();
            var responseStringWithId = await responseWithId.Content.ReadAsStringAsync();
            Company actualWithId = JsonConvert.DeserializeObject<Company>(responseStringWithId);
            var id = actualWithId.Id;

            var employee1 = new Employee("1", "Mike", "6000");
            var employee2 = new Employee("2", "Jane", "8000");
            string employeeRequest1 = JsonConvert.SerializeObject(employee1);
            string employeeRequest2 = JsonConvert.SerializeObject(employee2);
            StringContent employeeRequestBody1 = new StringContent(employeeRequest1, Encoding.UTF8, "application/json");
            StringContent employeeRequestBody2 = new StringContent(employeeRequest2, Encoding.UTF8, "application/json");
            var employeeResponse1 = await client.PostAsync($"/Companies/{id}/Employees", employeeRequestBody1);
            var employeeResponse2 = await client.PostAsync($"/Companies/{id}/Employees", employeeRequestBody2);
            var employeeResponse = await client.GetAsync($"/Companies/{id}/Employees");
            //then
            employeeResponse.EnsureSuccessStatusCode();
            var responseString = await employeeResponse.Content.ReadAsStringAsync();
            List<Employee> actual = JsonConvert.DeserializeObject<List<Employee>>(responseString);
            Assert.Equal(new List<Employee> { employee1, employee2 }, actual);
        }

        [Fact]
        public async Task Should_return_updated_employee_when_update_employee_in_certain_company_with_certain_id()
        {
            await client.DeleteAsync("Companies/clear");
            var company1 = new Company("Sun");
            var company2 = new Company("Moon");
            string request1 = JsonConvert.SerializeObject(company1);
            string request2 = JsonConvert.SerializeObject(company2);

            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            //when
            var responseWithId = await client.PostAsync("/Companies", requestBody1);
            await client.PostAsync("/Companies", requestBody2);
            responseWithId.EnsureSuccessStatusCode();
            var responseStringWithId = await responseWithId.Content.ReadAsStringAsync();
            Company actualWithId = JsonConvert.DeserializeObject<Company>(responseStringWithId);
            var id = actualWithId.Id;

            var employee1 = new Employee("1", "Mike", "6000");
            var employee2 = new Employee("2", "Jane", "8000");
            var updateEmploy = new UpdateEmployee("Mike", "9000");
            string employeeRequest1 = JsonConvert.SerializeObject(employee1);
            string employeeRequest2 = JsonConvert.SerializeObject(employee2);
            string updateEmployRequest = JsonConvert.SerializeObject(updateEmploy);
            StringContent employeeRequestBody1 = new StringContent(employeeRequest1, Encoding.UTF8, "application/json");
            StringContent employeeRequestBody2 = new StringContent(employeeRequest2, Encoding.UTF8, "application/json");
            StringContent updateEmployeeRequestBody = new StringContent(updateEmployRequest, Encoding.UTF8, "application/json");
            var employeeResponse1 = await client.PostAsync($"/Companies/{id}/Employees", employeeRequestBody1);
            var employeeResponse2 = await client.PostAsync($"/Companies/{id}/Employees", employeeRequestBody2);
            var updatedEmployeeResponse = await client.PatchAsync($"/Companies/{id}/Employees/1", updateEmployeeRequestBody);
            //then
            updatedEmployeeResponse.EnsureSuccessStatusCode();
            var responseString = await updatedEmployeeResponse.Content.ReadAsStringAsync();
            Employee actual = JsonConvert.DeserializeObject<Employee>(responseString);
            Assert.Equal(new Employee("1", "Mike", "9000"), actual);
        }

        [Fact]
        public async Task Should_delete_employee_when_delete_employee_with_company_Id_and_employee_Id()
        {
            await client.DeleteAsync("Companies/clear");
            var company1 = new Company("Sun");
            var company2 = new Company("Moon");
            string request1 = JsonConvert.SerializeObject(company1);
            string request2 = JsonConvert.SerializeObject(company2);

            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            //when
            var responseWithId = await client.PostAsync("/Companies", requestBody1);
            await client.PostAsync("/Companies", requestBody2);
            responseWithId.EnsureSuccessStatusCode();
            var responseStringWithId = await responseWithId.Content.ReadAsStringAsync();
            Company actualWithId = JsonConvert.DeserializeObject<Company>(responseStringWithId);
            var id = actualWithId.Id;

            var employee1 = new Employee("1", "Mike", "6000");
            var employee2 = new Employee("2", "Jane", "8000");
            string employeeRequest1 = JsonConvert.SerializeObject(employee1);
            string employeeRequest2 = JsonConvert.SerializeObject(employee2);
            StringContent employeeRequestBody1 = new StringContent(employeeRequest1, Encoding.UTF8, "application/json");
            StringContent employeeRequestBody2 = new StringContent(employeeRequest2, Encoding.UTF8, "application/json");
            var employeeResponse1 = await client.PostAsync($"/Companies/{id}/Employees", employeeRequestBody1);
            var employeeResponse2 = await client.PostAsync($"/Companies/{id}/Employees", employeeRequestBody2);
            var deleteEmployeeResponse = await client.DeleteAsync($"/Companies/{id}/Employees/1");
            //then
            deleteEmployeeResponse.EnsureSuccessStatusCode();
            var responseString = await deleteEmployeeResponse.Content.ReadAsStringAsync();
            Employee actual = JsonConvert.DeserializeObject<Employee>(responseString);
            Assert.Equal(employee1, actual);
        }

        [Fact]
        public async Task Should_delete_company_when_delete_company_by_id()
        {
            await client.DeleteAsync("Companies/clear");
            var company1 = new Company("Sun");
            var company2 = new Company("Moon");
            string request1 = JsonConvert.SerializeObject(company1);
            string request2 = JsonConvert.SerializeObject(company2);

            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            //when
            var responseWithId = await client.PostAsync("/Companies", requestBody1);
            await client.PostAsync("/Companies", requestBody2);
            responseWithId.EnsureSuccessStatusCode();
            var responseStringWithId = await responseWithId.Content.ReadAsStringAsync();
            Company actualWithId = JsonConvert.DeserializeObject<Company>(responseStringWithId);
            var id = actualWithId.Id;

            var employee1 = new Employee("1", "Mike", "6000");
            var employee2 = new Employee("2", "Jane", "8000");
            company1.Employees.Add(employee1);
            company1.Employees.Add(employee2);
            string employeeRequest1 = JsonConvert.SerializeObject(employee1);
            string employeeRequest2 = JsonConvert.SerializeObject(employee2);
            StringContent employeeRequestBody1 = new StringContent(employeeRequest1, Encoding.UTF8, "application/json");
            StringContent employeeRequestBody2 = new StringContent(employeeRequest2, Encoding.UTF8, "application/json");
            await client.PostAsync($"/Companies/{id}/Employees", employeeRequestBody1);
            await client.PostAsync($"/Companies/{id}/Employees", employeeRequestBody2);
            var deleteCompanyResponse = await client.DeleteAsync($"/Companies/{id}");
            var restAllEmployeeResponse = await client.GetAsync($"/Companies/{id}");
            //then
            deleteCompanyResponse.EnsureSuccessStatusCode();
            var responseString = await deleteCompanyResponse.Content.ReadAsStringAsync();
            Company actual = JsonConvert.DeserializeObject<Company>(responseString);
            Assert.Equal(company1, actual);
            Assert.Equal(HttpStatusCode.NotFound, restAllEmployeeResponse.StatusCode);
        }

        [Fact]
        public async Task Should_return_x_companies_when_get_companies_by_index_y()
        {
            await client.DeleteAsync("Companies/clear");
            var company1 = new Company("Sun");
            var company2 = new Company("Moon");
            var company3 = new Company("Star");
            string request1 = JsonConvert.SerializeObject(company1);
            string request2 = JsonConvert.SerializeObject(company2);
            string request3 = JsonConvert.SerializeObject(company3);

            StringContent requestBody1 = new StringContent(request1, Encoding.UTF8, "application/json");
            StringContent requestBody2 = new StringContent(request2, Encoding.UTF8, "application/json");
            StringContent requestBody3 = new StringContent(request3, Encoding.UTF8, "application/json");
            //when
            var responseWithId = await client.PostAsync("/Companies", requestBody1);
            var responseWithId2 = await client.PostAsync("/Companies", requestBody2);
            var responseWithId3 = await client.PostAsync("/Companies", requestBody3);
            await client.PostAsync("/Companies", requestBody2);
            responseWithId.EnsureSuccessStatusCode();
            var responseStringWithId = await responseWithId.Content.ReadAsStringAsync();
            Company actualWithId = JsonConvert.DeserializeObject<Company>(responseStringWithId);
            var id = actualWithId.Id;

            var restAllEmployeeResponse = await client.GetAsync("/Companies?pageSize=1&pageIndex=1");
            //then
            restAllEmployeeResponse.EnsureSuccessStatusCode();
            var responseString = await restAllEmployeeResponse.Content.ReadAsStringAsync();
            List<Company> actual = JsonConvert.DeserializeObject<List<Company>>(responseString);
            Assert.Equal(new List<Company> { company1 }, actual);
        }
    }
}
