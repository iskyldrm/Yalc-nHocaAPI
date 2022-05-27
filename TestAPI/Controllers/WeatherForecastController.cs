using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
        [HttpGet]
        [ActionName("1")]
        public List<CustomerDTO> GetCustomerAll()
        {
            SqlConnection sqlConnection = new SqlConnection(@"data source=(localdb)\mssqllocaldb;initial catalog=Northwind;integrated security=True");


            SqlCommand sqlCommand = new SqlCommand("Select CustomerId,CompanyName,Address from Customers");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.Text;
            sqlConnection.Open();




            SqlDataReader reader = sqlCommand.ExecuteReader();
            List<CustomerDTO> dto = new List<CustomerDTO>();
            while (reader.Read())
            {
                CustomerDTO dtoItem = new CustomerDTO();
                dtoItem.CustomerId = reader[0].ToString();
                dtoItem.CustomerName = reader[1].ToString();
                dtoItem.Adress = reader[2].ToString();
                dto.Add(dtoItem);
            }
            sqlConnection.Close();
            return dto;
        }
        [HttpGet]
        [ActionName("2")]
        public List<OrderDTO> GetOrders(string CustomerId)
        {

            SqlConnection sqlConnection = new SqlConnection(@"data source=(localdb)\mssqllocaldb;initial catalog=Northwind;integrated security=True");


            SqlCommand sqlCommand = new SqlCommand($"select o.OrderID,o.OrderDate,o.ShipCountry from orders o where o.CustomerID='{CustomerId}'");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.Text;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            List<OrderDTO> dto = new List<OrderDTO>();
            while (reader.Read())
            {
                OrderDTO dtoItem = new OrderDTO();
                dtoItem.OrderId = Convert.ToInt32(reader[0]);
                dtoItem.OrderDate = Convert.ToDateTime(reader[1]);
                dtoItem.ShipCountry = reader[2].ToString();
                dto.Add(dtoItem);
            }
            sqlConnection.Close();
            return dto;
        }
    }
}
