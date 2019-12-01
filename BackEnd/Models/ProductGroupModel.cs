using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models {
  [Table("tbl_pro_group")]
  public class ProductGroupModel {
    [Key]
    public int id { get; set; }
    public string group_name { get; set; }
    public int ledger_code { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime created_date { get; set; }
    public int created_by { get; set; }
    public bool status { get; set; }
  }
}
