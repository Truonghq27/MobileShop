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
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên nhân viên")]
        public string FullName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@" + "[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression("^[0][1-9][0-9]{8}$", ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string Phone { get; set; }
        public string Avatar { get; set; }

        [DefaultValue(false)]
        public bool IsAdmin { get; set; } = false;

        [DefaultValue(1)]
        public byte Status { get; set; } = 1;

        /// <summary>
        /// Status for Users and delete Users with ,Status = 10
        /// </summary>
        public byte mStatus { get; set; }

        public bool isEmailVerified { get; set; }
        public System.Guid ActiveCode { get; set; }
        public string ResetPasswordCode { get; set; }
        public string GroupId { get; set; }
    }
}
