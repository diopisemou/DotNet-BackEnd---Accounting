using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models.BaseModels {
  public class ItemDetailModel {
    [Key]
    public int id { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime date { get; set; }
    public int inv_no { get; set; }
    public int item_id { get; set; }
    public int unit_id { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal price { get; set; }
    public int qty { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal total { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal discount { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal tax { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal net_amount { get; set; }
    public int created_by { get; set; }
    public bool status { get; set; }
  }
}
