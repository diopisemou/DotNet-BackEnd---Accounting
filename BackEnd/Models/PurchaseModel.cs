﻿using BackEnd.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models {
  [Table("tbl_purchasemast")]
  public class PurchaseModel : InvoiceModel {
    public int supp_id { get; set; }

  }
}
