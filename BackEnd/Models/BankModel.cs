using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models {
  [Table("tbl_bank")]
  public class BankModel {
    [Key]
    public int id { get; set; }
    public string name { get; set; }
    public string acc_type { get; set; }
    public int acc_number { get; set; }
    public int? branch { get; set; }
    public string swift_code { get; set; }
    public string level_value { get; set; }
    public string url { get; set; }
    public int created_by { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime created_date { get; set; }
    public bool status { get; set; } = true;
  }
}
