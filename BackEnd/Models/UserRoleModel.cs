using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models {
    [Table("tbl_User_Roll")]
    public class UserRoleModel {
        [Key]
        public int Roll_Id { get; set; }
        public string Rollname { get; set; }
        public bool Status { get; set; }
        public int Created_by { get; set; }
    }

}
