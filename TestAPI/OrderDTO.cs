using System;

namespace TestAPI
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipCountry { get; set; }


    }
}
