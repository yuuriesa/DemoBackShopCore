namespace DemoBackShopCore.Test;

using Microsoft.AspNetCore.Mvc.Testing;
using DemoBackShopCore.Models;
using DemoBackShopCore.Controllers;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using DemoBackShopCore.DTOs;
using Newtonsoft.Json;
using System.Text;

public class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    // Tenho que Lembrar de Apagar banco de dados com os testes, já que são armazenados lá podendo causar erros ao repetir os mesmos dados.
    public HttpClient _client;

    public IntegrationTest(WebApplicationFactory<Program> factory)
    {
        // Cria um HttpClient para chamar a aplicação real
        _client = factory.CreateClient();
    }

    [Fact(DisplayName = "POST /api/Customers deve retornar 201 Created")]
    [InlineData("/api/Customers")]
    public async Task AddCustomer_ReturnsCreated()
    {
        //Arrange
        CustomerRequestDTO request = new CustomerRequestDTO
        {
            FirstName = "Yuri",
            LastName = "Torres",
            EmailAddress = "yuriA@exemplo.com",
            DateOfBirth = DateTime.UtcNow.AddDays(-20)
        };
            
        string json = JsonConvert.SerializeObject(request);
        StringContent content = new StringContent
        (
            content: json,
            encoding: Encoding.UTF8,
            mediaType: "application/json"
        );

        //Act
        HttpResponseMessage response = await _client.PostAsync
        (
            requestUri: "/api/Customers", 
            content: content
        );

        //Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(expected: System.Net.HttpStatusCode.Created, actual: response.StatusCode);
    }

    [Fact(DisplayName = "POST /api/Customers deve retornar 422 Error por causa da data incorreta.")]
    [InlineData("/api/Customers")]
    public async Task AddCustomers_ReturnsUnprocessableEntity()
    {
        //Arrange
        CustomerRequestDTO request = new CustomerRequestDTO
        {
            FirstName = "Yuri",
            LastName = "Torres",
            EmailAddress = "yuriB@exemplo.com",
            DateOfBirth = DateTime.UtcNow.AddDays(1)
        };
            
        string json = JsonConvert.SerializeObject(request);
        StringContent content = new StringContent
        (
            content: json,
            encoding: Encoding.UTF8,
            mediaType: "application/json"
        );
        //Act
        HttpResponseMessage response = await _client.PostAsync
        (
            requestUri: "/api/Customers", 
            content: content
        );
        //Assert
        Assert.Equal(expected: System.Net.HttpStatusCode.UnprocessableEntity, actual: response.StatusCode);
    }
}
