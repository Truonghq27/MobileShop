using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.DataModels
{
    public class ProductAttr
    {
        [Column(Order = 0), Key]
        public int ProductId { get; set; }
        [Column(Order = 1), Key]
        public int AttrId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Products { get; set; }
        [ForeignKey("AttrId")]
        public virtual Attribute Attributes { get; set; }
    }
}
