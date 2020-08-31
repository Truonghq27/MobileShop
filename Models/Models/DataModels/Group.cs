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
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage ="Vui lòng nhập RoleId")]
        public string GroupId { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập tên Role")]
        public string GroupName { get; set; }
        public string Background { get; set; }

        [DefaultValue(1)]
        public byte Status { get; set; } = 1;

        [DefaultValue(false)]
        public bool isAdmin { get; set; } = false;

        public virtual ICollection<GroupRole> GroupRoles { get; set; }
    }
}
