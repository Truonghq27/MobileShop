using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.DataModels
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string CodeOrder { get; set; }
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public double totalPrice { get; set; }
        public string City { get; set; }//thành phố
        public string District { get; set; }// quận huyện
        public string Commune { get; set; }//phường xã
        public string HouseNumber { get; set; }//số nhà
        public string Address { get; set; } // save all as Address
        public string Description { get; set; } //Ghi chú đơn hàng

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime Created { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime TimeExpires { get; set; }
        public int Status { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
