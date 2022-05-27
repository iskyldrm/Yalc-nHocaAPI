using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
    }
}
