using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models {
  [Table("tbl_Branch")]
  public class Branch {
    [Key]
    public int Branch_Id { get; set; }
    public string Branch_Name { get; set; }
    public string Branch_Code { get; set; }
    public string Address { get; set; }
    public string Telephone_No { get; set; }
    public string Email_Id { get; set; }
    [Display(Name = "Created Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Created_date { get; set; }
    public int Created_by { get; set; }

  }
}
