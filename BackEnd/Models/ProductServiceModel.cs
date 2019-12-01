using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models {
  [Table("tbl_product")]
  public class ProductServiceModel {
    [Key]
    public int id { get; set; }
    public int p_type { get; set; }
    public string p_name { get; set; }
    public string d_name { get; set; }
    public int p_group { get; set; }
    public int qtyonhand { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal p_price { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal s_price { get; set; }
    public int i_account { get; set; }
    public int e_account { get; set; }
    public int reorder { get; set; }
    public string barcode { get; set; }
    public bool istaxable { get; set; }
    public int taxtype { get; set; }
    public int created_by { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime created_date { get; set; }
    public bool status { get; set; }
    public string image { get; set; }

  }
}
