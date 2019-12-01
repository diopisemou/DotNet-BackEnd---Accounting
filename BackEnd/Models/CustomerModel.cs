using BackEnd.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models {
  [Table("tbl_cust_mast")]
  public class CustomerModel :BaseCustomer{
    public int s_agent { get; set; }
  }
}
