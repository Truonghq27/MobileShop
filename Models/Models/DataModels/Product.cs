using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Models.Models.DataModels
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public int ProviderId { get; set; }

        [DisplayName("Giá nhập")]
        [Required(ErrorMessage = "Vui lòng nhập giá nhập vào")]
        public double PriceIn { get; set; }

        [DisplayName("Giá xuất")]
        [Required(ErrorMessage = "Vui lòng nhập giá xuất ra")]
        public double PriceOut { get; set; }

        [DisplayName("Giảm giá")]
        public int? Discount { get; set; }//giảm giá

        [DisplayName("Số lượng")]
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        public int Quantity { get; set; }

        public string FeatureImage { get; set; }

        [DisplayName("Ảnh sản phẩm")]
        public string Images { get; set; }

        [DisplayName("Mô tả")]
        [Required(ErrorMessage = "Vui lòng nhập mô tả sản phẩm")]
        public string Description { get; set; }

        [AllowHtml]
        [DisplayName("Thông số kỹ thuật")]
        [Required(ErrorMessage = "Vui lòng nhập thông số kỹ thuật")]
        public string Specifications { get; set; }

        [AllowHtml]
        [DisplayName("Chi tiết sản phẩm")]
        [Required(ErrorMessage = "Vui lòng nhập chi tiết sản phẩm")]
        public string ProductDetail { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime? CreateDate { get; set; }

        [DisplayName("Lượt mua")]
        public int? CoutBy { get; set; }

        [DisplayName("Trang thái")]
        [Required(ErrorMessage = "Vui lòng chọn trạng thái sản phẩm")]
        public byte Condition { get; set; }

        [DefaultValue(0)]
        public int ProductSaleQuantity { get; set; } = 0; // số sản phẩm đã bán

        [DefaultValue(true)]
        public bool Status { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Categories { get; set; }
        [ForeignKey("ProviderId")]
        public virtual Provider Providers { get; set; }

        public virtual ICollection<ProductAttr> ProductAttrs { get; set; }
    }
}
