using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models {
  [Table("tbl_chartofacc")]
  public class ChartOfAccountModel {
    [Key]
    public int id { get; set; }
    public int Mast_Code { get; set; } = 100;
    public int ledger_Code { get; set; }
    public int ledger_Group { get; set; } = 1;
    public string Ledger_name { get; set; }
    public bool is_ledger { get; set; } = true;
    public bool is_subledger { get; set; } = false;
    public int Created_by { get; set; }
    public bool Status { get; set; } = true;

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime created_date { get; set; } = DateTime.Now;
  }
}
