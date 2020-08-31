using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.DataModels
{
    public class TypeAttr
    {
        [Key]
        public int TypeId { get; set; }

        [DisplayName("Kiểu thuộc tính")]
        [Required(ErrorMessage = "Vui lòng nhập kiểu thuộc tính")]
        public string TypeName { get; set; }
        [DisplayName("Vị trí")]
        [Required(ErrorMessage ="Vui lòng nhập số thứ tự")]
        public int OrderBy { get; set; }
        [DisplayName("Trạng thái")]
        [DefaultValue(1)]
        public byte Status { get; set; } = 1;

        public virtual ICollection<Attribute> Attributes { get; set; }
    }
}
