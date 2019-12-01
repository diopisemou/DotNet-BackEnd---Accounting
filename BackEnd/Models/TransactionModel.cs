using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models {
  [Table("tbl_Transaction")]
  public class TransactionModel {
    [Key]
    public int Tr_Id { get; set; }
    public int Trans_Id { get; set; }
    public int ledger_Code { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Debit { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Credit { get; set; }
    [Column(TypeName = "varchar(10)")]
    public string Gl_type{ get; set; }
    [Column(TypeName = "varchar(max)")]
    public string Remarks { get; set; }
    [Column(TypeName = "varchar(max)")]
    public string Narration { get; set; }
    public int Created_by { get; set; }
    public bool Status { get; set; }
    [Column(TypeName = "nvarchar(50)")]
    public string Vouc_No { get; set; }
    [Column(TypeName = "nvarchar(10)")]
    public string Fiscal { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; }
    [Column(TypeName = "varchar(20)")]
    public string tran_type { get; set; }
    public int branch_id { get; set; }
    public int project_id { get; set; }

  }
}
