using BackEnd.Data;
using BackEnd.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models {
  public static class SeedData {
    public static void Initialize (IServiceProvider serviceProvider) {
      using (var context = new MyDatabaseContext(serviceProvider.GetRequiredService<DbContextOptions<MyDatabaseContext>>())) {
        //seed data
        //role
        if (!context.Roles.Any()) {
          context.Roles.AddRange(
            new UserRoleModel {
              Rollname = "admin",
              Status = true
            },
            new UserRoleModel {
              Rollname = "customer",
              Status = true
            },
            new UserRoleModel {
              Rollname = "guest",
              Status = true
            });
          context.SaveChanges();
        }
        //branch
        if (!context.Branches.Any()) {
          context.Branches.AddRange(
            new Branch {
              Branch_Code = "1",
              Branch_Name = "Head Office",
              Created_date = DateTime.Now
            });
          context.SaveChanges();
        }
        //user seed
        UserRoleModel admin = context.Roles.FirstOrDefault(role => role.Rollname == "admin");
        Branch branch = context.Branches.FirstOrDefault();
        if (!context.Users.Any()) {
          context.Users.AddRange(
            new User {
              Branch_Id = branch.Branch_Id,
              Created_by = 0,
              Emp_Id = 0,
              Password = Hashing.HashPassword("admin"),
              Token = "",
              Roll_Id = admin.Roll_Id,
              Useremail = "jml719@outlook.com",
              Username = "Bright Dragon",
              Status = 1
            });
          context.SaveChanges();
        }
        if (!context.AuthRoles.Any()) {
          string[] menus = new string[] {
            "Company",
            "Branch",
            "Fiscal Year",
            "User Role",
            "Users",
            "Role Assignment",
            "Product Group",
            "Product & Service",
            "User Log",
            "Auto Numbering",
            "Import Data",
            "Export Data",
            "Attachment",
            "Bank",
            "Transaction",
            "Customer",
            "Sale",
            "SaleDetail",
            "Supplier",
            "Purchase"
            };
          AuthRoleModel[] models = new AuthRoleModel[menus.Length];
          for (int i = 0; i < menus.Length; i++) {
            models[i] = new AuthRoleModel {
              menu = menus[i],
              viewRole = true,
              saveRole = true,
              updateRole = true,
              deleteRole = true,
              printRole = true,
              created_by = 0,
              created_date = DateTime.Now,
              roll_id = admin.Roll_Id,
              status = true
            };
          }
          context.AuthRoles.AddRange(models);
          context.SaveChanges();
        }

        if (!context.ChartOfAccounts.Any()) {
          string[] createOfAccounts = new string[] {
            "Cash In Hand","Profit & Loss","Sales Of Product Income","Exp.Of Product Sales","Income From Service","Basic Salary","prabhu bank limited"
          };
          int[] groups = new int[] { 1, 18, 12, 16, 12, 17, 1 };
          int[] mast_code = new int[] { 100, 200, 300, 400, 300, 400, 100 };
          ChartOfAccountModel[] models = new ChartOfAccountModel[createOfAccounts.Length];
          for (int i = 0; i < createOfAccounts.Length; i++) {
            models[i] = new ChartOfAccountModel {
              Mast_Code = mast_code[i],
              ledger_Code = i + 1,
              ledger_Group = groups[i],
              Ledger_name = createOfAccounts[i],
              is_ledger = true,
              is_subledger = false,
              Created_by = 0,
              created_date = DateTime.Now
            };
          }
          context.ChartOfAccounts.AddRange(models);
          context.SaveChanges();
        }
      }
    }
  }
}
