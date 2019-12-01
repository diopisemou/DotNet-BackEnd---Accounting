using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models {
  [Table("tbl_project")]
  public class ProjectModel : BaseModel {
    [Required]
    public string project_name { get; set; }

    [Required]
    public int branch_vode { get; set; }

    [Required]
    public bool status { get; set; }

    [Required]
    public bool closed { get; set; } = false;
  }
}
