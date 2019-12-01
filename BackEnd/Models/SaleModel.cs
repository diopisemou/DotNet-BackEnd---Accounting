using BackEnd.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models {
  [Table("tbl_salesmast")]
  public class SaleModel: InvoiceModel{
    public int cust_id { get; set; }
  }
}
