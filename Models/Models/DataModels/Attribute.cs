using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.DataModels
{
    public class Attribute
    {
        [Key]
        public int AttrId { get; set; }
        public int TypeId { get; set; }
        public string AttrName { get; set; }
        public string Value{ get; set; }
        public byte Status { get; set; }

        [ForeignKey("TypeId")]
        public virtual TypeAttr TypeAttrs{ get; set; }
    }
}
