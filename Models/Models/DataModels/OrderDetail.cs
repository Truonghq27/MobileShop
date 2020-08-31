using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.DataModels
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public string CategoryName { get; set; }

        public string ProviderName { get; set; }

        public string AttrName { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int? Discount { get; set; }

        public string FeatureImage { get; set; }

        public int Quantity { get; set; }

        public double PriceOut { get; set; }

        public double Price { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Orders { get; set; }
    }
}
