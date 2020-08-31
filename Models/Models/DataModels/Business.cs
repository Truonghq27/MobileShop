using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.DataModels
{
    public class Business
    {
        [Key]
        public string BusinessId { get; set; }
        public string BusinessName { get; set; }
        public byte Status { get; set; }

        public virtual ICollection<GroupRole> GroupRoles { get; set; }
    }
}
