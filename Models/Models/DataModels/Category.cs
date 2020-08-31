using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Models.DataModels
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [DisplayName("Danh mục")]
        [Required(ErrorMessage ="Vui lòng nhập tên danh mục")]
        public string CategoryName { get; set; }

        [DisplayName("Thứ tự")]
        [Required(ErrorMessage = "Vui lòng nhập thứ tự sắp xếp")]
        public int Orderby { get; set; } //thứ tự sắp xếp

        [DisplayName("Trạng thái")]
        public byte Status { get; set; }

        public int? ParentId { get; set; }

        public virtual Category category { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
