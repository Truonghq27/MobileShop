using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.DataModels
{
    public class Banner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BannerId { get; set; }
        public string DescriptionBanner { get; set; } = "Không có mô tả";

        [Required(ErrorMessage = "Vui lòng chọn ảnh")]
        public string BannerImage { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập vị trí sắp xếp")]
        public int Orderby { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        public int Status { get; set; }
        public DateTime? Created { get; set; }

    }
}
