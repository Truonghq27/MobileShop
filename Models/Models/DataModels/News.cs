using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Models.DataModels
{
    public class News
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NewsId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập News Title")]
        public string NewsTitle { get; set; }

        [DisplayName("Ảnh minh hoạ")]
        [Required(ErrorMessage = "Vui lòng chọn ảnh minh hoạ")]
        public string FeatureImage { get; set; }

        [DisplayName("Mô tả ngắn")]
        [Required(ErrorMessage = "Vui lòng nhập tóm tắt tin tức")]
        public string ShortDescription { get; set; }

        [DisplayName("Mô tả chi tiết")]
        [Required(ErrorMessage = "Vui lòng viết bài cho tin tức")]
        public string Description { get; set; }

        [DisplayName("Lượt xem")]
        public int? CountView { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime Created { get; set; }

        [DisplayName("Trạng thái")]
        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        public int Status { get; set; }

        public int UserId { get; set; }

    }
}
