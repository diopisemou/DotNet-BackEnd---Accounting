using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models {
    [Table("tbl_Userlog")]
    public class UserLogModel {
        [Key]
        public int SN { get; set; }
        public int Username { get; set; }
        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datetime { get; set; }
        public string Event { get; set; }
        public string Activity { get; set; }
        public string Type { get; set; }
    }
}
