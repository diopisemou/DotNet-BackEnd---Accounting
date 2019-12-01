using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models {
  [Table("tbl_User")]
  public class User {
    [Key]
    public int User_Id { get; set; }
    public int Roll_Id { get; set; }
    public int Branch_Id { get; set; }
    public int Emp_Id { get; set; }
    public string Username { get; set; }
    public string Useremail { get; set; }
    public string Password { get; set; }
    public int Created_by { get; set; } = 0;
    public int Status { get; set; }
    public string Token { get; set; } = "";
  }
}
