using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models.BaseModels {
  public class InvoiceModel {
    [Key]
    public int id { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime date { get; set; }
    [Column(TypeName = "varchar(50)")]
    public string order_ref { get; set; }
    public int inv_no { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal taxable { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal tax { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal discount { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal netamount { get; set; }
    public int net_dues { get; set; }
    public int created_by { get; set; }
    public int status { get; set; }
    [Column(TypeName = "nchar(10)")]
    public string updated { get; set; }
    public int agent_id { get; set; }
  }
}
