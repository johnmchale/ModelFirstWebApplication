using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModelFirstWebApplication.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
