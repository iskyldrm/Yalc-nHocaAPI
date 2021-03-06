using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

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
        internal SqlConnection sqlConnection;
        internal SqlCommand sqlCommand;
        internal SqlDataReader reader;
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            sqlConnection = new SqlConnection(@"data source=(localdb)\mssqllocaldb;initial catalog=Northwind;integrated security=True");
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
        public List<innerjoinDTO> GetCustomerAll(string customerId, int orderId)
        {



            sqlCommand = new SqlCommand($"Select c.CompanyName, o.OrderID,p.ProductName,od.Quantity,od.UnitPrice," +
                "SUM(od.Quantity*od.UnitPrice) as toplam from Orders o inner join Customers c on c.CustomerID = o.CustomerID " +
                "inner join[Order Details] od on od.OrderID = o.OrderID " +
                "inner join Products p on p.ProductID = od.ProductID " +
                $"where c.CustomerID = '{customerId}' AND o.OrderID = '{orderId}'" +
                "group by  c.CompanyName, o.OrderID, p.ProductName, od.Quantity, od.UnitPrice");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.Text;
            sqlConnection.Open();




            SqlDataReader reader = sqlCommand.ExecuteReader();
            List<innerjoinDTO> dto = new List<innerjoinDTO>();
            while (reader.Read())
            {
                innerjoinDTO dtoItem = new innerjoinDTO();
                dtoItem.CustomerName = reader[0].ToString();
                dtoItem.OrderId = Convert.ToInt16(reader[1]);
                dtoItem.ProductName = reader[2].ToString();
                dtoItem.Quantity = Convert.ToInt16(reader[3]);
                dtoItem.UnitPrice = Convert.ToDecimal(reader[4]);
                dtoItem.Toplam = Convert.ToDecimal(reader[5]);
                dto.Add(dtoItem);
            }
            sqlConnection.Close();
            reader.Close();
            return dto;
        }
        [HttpGet]
        [ActionName("2")]
        public List<OrderDTO> GetOrders(string CustomerId)
        {
            sqlCommand = new SqlCommand($"select o.OrderID,o.OrderDate,o.ShipCountry from orders o where o.CustomerID='{CustomerId}'");
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
        [HttpGet]
        [ActionName("3")]
        public List<OrderDTO> GetCustomers(string CustomerId)
        {
            return RunSql<OrderDTO>("Select * from Customers");
        }

        [HttpGet]
        [ActionName("Generic")]
        private List<T> RunSql<T>(string query) where T : class, new()
        {
            sqlCommand = new SqlCommand(query);
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.Text;
            sqlConnection.Open();
            reader = sqlCommand.ExecuteReader();
            List<T> dto = new List<T>();
            while (reader.Read())
            {
                T dtoItem = new T();
                dtoItem = (T)GetPropFromClass(dtoItem);
                dto.Add((T)dtoItem);

            };
            return dto;
        }

        private object GetPropFromClass(object Class)
        {

            Assembly asm = Assembly.GetExecutingAssembly();
            Type t = asm.GetType("OrderDto");
            PropertyInfo pi = t.GetProperty("Id");

            return pi.GetValue(Class, null);

        }
    }
}
