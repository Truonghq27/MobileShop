using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.DataModels
{
    public class AddToCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }
        public virtual Product product { get; set; }
        public string AttrId{ get; set; }
        public string AttrName{ get; set; }
        public double Price{ get; set; }
        public int Quantity{ get; set; }
        public int? CustomerId { get; set; }

    }
}
