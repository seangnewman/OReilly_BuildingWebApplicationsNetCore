using SpyStore.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpyStore.Models.Entities
{
    [Table("Orders", Schema = "Store")]
    public  class Order:EntityBase
    {
        public int CustomerId { get; set; }
        

        [DataType(DataType.Date)]
        [Display(Name = "Date Ordered")]
        public DateTime OrderDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name ="Date Shipped")]
        public DateTime ShipDate { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        [InverseProperty("Order")]
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        [Display(Name ="Total")]
        public decimal? OrderTotal { get; set; }
    }
}