using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models {
  [Table("tbl_authrole")]
  public class AuthRoleModel {
    [Key]
    public int id { get; set; }

    public int  roll_id { get; set; }
    public string  menu { get; set; }
    public bool saveRole { get; set; }
    public bool updateRole { get; set; }
    public bool deleteRole { get; set; }
    public bool viewRole { get; set; }
    public bool printRole { get; set; }
    public bool status { get; set; }
    public int created_by { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime created_date { get; set; }
  }
}
