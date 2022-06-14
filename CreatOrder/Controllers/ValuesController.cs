using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CreatOrder.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> logger;
        internal SqlConnection sqlConnection;
        internal SqlCommand sqlCommand;
        internal SqlDataReader reader;
        public ValuesController(ILogger<ValuesController> logger)
        {
            this.logger = logger;
            sqlConnection = new SqlConnection(@"data source=(localdb)\mssqllocaldb;initial catalog=Northwind;integrated security=True");
        }


        [HttpGet]
        [ActionName("GetEmployee")]
        public List<EmployeeDTO> GetEmployee()
        {
            sqlCommand = new SqlCommand($"select e.EmployeeID, (e.FirstName + ' ' + e.LastName) as Adsoyad from Employees e");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.Text;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            List<EmployeeDTO> dto = new List<EmployeeDTO>();
            while (reader.Read())
            {
                EmployeeDTO dtoItem = new EmployeeDTO();
                dtoItem.Id = Convert.ToInt16(reader[0]);
                dtoItem.FirstLastName = reader[1].ToString();

                dto.Add(dtoItem);


            }
            reader.Close();
            sqlConnection.Close();
            return dto;
        }
        [HttpGet]
        [ActionName("GetCustomer")]
        public List<CustomerDTO> GetCustomer()
        {
            sqlCommand = new SqlCommand($"select c.CustomerID,c.CompanyName from Customers c");
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

                dto.Add(dtoItem);


            }
            reader.Close();
            sqlConnection.Close();
            return dto;
        }
        [HttpGet]
        [ActionName("GetProduct")]
        public List<ProductDTO> GetProducts()
        {
            sqlCommand = new SqlCommand($"SELECT P.ProductID,ProductName,P.UnitPrice FROM Products P");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.Text;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            List<ProductDTO> dto = new List<ProductDTO>();
            while (reader.Read())
            {
                ProductDTO dtoItem = new ProductDTO();
                dtoItem.ProductId = Convert.ToInt16(reader[0]);
                dtoItem.ProductName = reader[1].ToString();
                dtoItem.Unitprice = Convert.ToDecimal(reader[2]);

                dto.Add(dtoItem);


            }
            reader.Close();
            sqlConnection.Close();
            return dto;
        }
        [HttpGet]
        [ActionName("GetShipper")]
        public List<ShipperDTO> GetShipper()
        {
            sqlCommand = new SqlCommand($"select * from Shippers");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.Text;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            List<ShipperDTO> dto = new List<ShipperDTO>();
            while (reader.Read())
            {
                ShipperDTO dtoItem = new ShipperDTO();
                dtoItem.ShipperId = Convert.ToInt16(reader[0]);
                dtoItem.ShipperName = reader[1].ToString();

                dto.Add(dtoItem);


            }
            reader.Close();
            sqlConnection.Close();
            return dto;
        }

        [HttpGet]
        [ActionName("GetLastOrderId")]
        public List<LastOrderIdDTO> GetLastOrderId(string customerId)
        {
            sqlCommand = new SqlCommand($"Select TOP 1 OrderID from Orders where CustomerID = '{customerId}' order by OrderID desc");
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.Text;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            List<LastOrderIdDTO> dto = new List<LastOrderIdDTO>();
            while (reader.Read())
            {
                LastOrderIdDTO dtoItem = new LastOrderIdDTO();
                dtoItem.orderId = Convert.ToInt16(reader[0]);

                dto.Add(dtoItem);


            }
            reader.Close();
            sqlConnection.Close();
            return dto;
        }

        [HttpGet]
        [ActionName("CreatOrder")]
        public IActionResult CreatOrder(int employEeID, string customerId, int shipVia, string shipName)
        {
            sqlConnection.Open();
            sqlCommand = new SqlCommand($"INSERT INTO Orders(CustomerID,EmployeeID,ShipVia,ShipName) " +
                $"VALUES ('{customerId}','{employEeID}','{shipVia}','{shipName}')");
            sqlCommand.Connection = sqlConnection;
            int result = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return Ok(result);
        }

        [HttpGet]
        [ActionName("CreatOrderDetail")]
        public int CreatOrderDetail(int orderId, int productId, decimal unitPrice, int quantity, decimal discount)
        {
            sqlConnection.Open();
            sqlCommand = new SqlCommand("INSERT INTO [Order Details] (OrderID,ProductID,UnitPrice,Quantity,Discount) " +
                "VALUES (" + orderId + ", " + productId + ", " + unitPrice.ToString().Replace(",", ".") + ", " + quantity + ", " + discount.ToString().Replace(",", ".") + ")");
            sqlCommand.Connection = sqlConnection;
            var result = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return result;
        }

        [HttpPost]
        [ActionName("CreatOrderPost")]
        public IActionResult CreatOrderwithPost([FromBody] CreatOrder creatOrder)
        {
            sqlConnection.Open();
            sqlCommand = new SqlCommand($"INSERT INTO Orders(CustomerID,EmployeeID,ShipVia,ShipName) " +
                $"VALUES ('{creatOrder.customerId}','{creatOrder.employeeId}','{creatOrder.shipVia}','{creatOrder.shipName}')");
            sqlCommand.Connection = sqlConnection;
            int result = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return Ok(creatOrder);
        }
    }
}
