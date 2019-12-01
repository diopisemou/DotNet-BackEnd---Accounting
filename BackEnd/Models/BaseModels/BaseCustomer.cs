using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Models.BaseModels {
  public class BaseCustomer {
    [Key]
    public int cust_id { get; set; }
    [Column(TypeName = "varchar(max)")]
    public string cust_name { get; set; }
    [Column(TypeName = "varchar(20)")]
    public string address { get; set; }
    [Column(TypeName = "varchar(max)")]
    public string email { get; set; }
    [Column(TypeName = "varchar(20)")]
    public string region { get; set; }
    [Column(TypeName = "varchar(15)")]
    public string mob { get; set; }
    [Column(TypeName = "varchar(15)")]
    public string tel { get; set; }
    public int type { get; set; }
    public int netdues { get; set; }
    public int created_by { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime created_date { get; set; }
    public bool status { get; set; }
    public int ledger { get; set; }
  }
}
