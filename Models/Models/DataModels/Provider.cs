using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.DataModels
{
    public class Provider
    {
        [Key]
        public int ProviderId { get; set; }
        [DisplayName("Thương hiệu")]
        public string ProviderName { get; set; }
        public int Orderby { get; set; }
        [DisplayName("Trạng thái")]
        public byte Status { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
