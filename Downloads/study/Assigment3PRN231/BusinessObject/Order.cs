using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class Order
    {
        public int OrderId { get; set; }
        public string MemberId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal Freight { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } 

        public IdentityUser IdentityUser { get; set; }
    }
}
