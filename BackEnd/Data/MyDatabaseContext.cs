using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Data {
  public class MyDatabaseContext : DbContext {
    public MyDatabaseContext (DbContextOptions<MyDatabaseContext> option) : base(option) {

    }

    public DbSet<BackEnd.Models.Branch> Branches { get; set; }
    public DbSet<BackEnd.Models.User> Users { get; set; }
    public DbSet<BackEnd.Models.UserRoleModel> Roles { get; set; }
    public DbSet<BackEnd.Models.CompanyModel> Companies { get; set; }
    public DbSet<BackEnd.Models.UserLogModel> Userlogs { get; set; }
    public DbSet<BackEnd.Models.FiscalModel> Fiscals { get; set; }
    public DbSet<BackEnd.Models.AuthRoleModel> AuthRoles { get; set; }
    public DbSet<BackEnd.Models.ProductGroupModel> ProductGroups { get; set; }
    public DbSet<BackEnd.Models.ProductServiceModel> ProductServices { get; set; }
    public DbSet<BackEnd.Models.ChartOfAccountModel> ChartOfAccounts { get; set; }
    public DbSet<BackEnd.Models.BankModel> Banks { get; set; }
    public DbSet<BackEnd.Models.TransactionModel> Transactions { get; set; }
    public DbSet<BackEnd.Models.CustomerModel> Customers { get; set; }
    public DbSet<BackEnd.Models.SupplierModel> Suppliers { get; set; }
    public DbSet<BackEnd.Models.SaleModel> Sales { get; set; }
    public DbSet<BackEnd.Models.SaleDetailModel> SaleDetails { get; set; }
    public DbSet<BackEnd.Models.PurchaseModel> Purchases { get; set; }
    public DbSet<BackEnd.Models.PurchaseDetailModel> PurchaseDetails { get; set; }
    public DbSet<BackEnd.Models.ProjectModel> Projects{ get; set; }
  }
}
