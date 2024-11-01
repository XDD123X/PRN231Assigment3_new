using System.Collections.Generic;

namespace BusinessObject
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }

        public Category Category { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } 
    }
}
