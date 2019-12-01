using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BackEnd.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_authrole",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    roll_id = table.Column<int>(nullable: false),
                    menu = table.Column<string>(nullable: true),
                    saveRole = table.Column<bool>(nullable: false),
                    updateRole = table.Column<bool>(nullable: false),
                    deleteRole = table.Column<bool>(nullable: false),
                    viewRole = table.Column<bool>(nullable: false),
                    printRole = table.Column<bool>(nullable: false),
                    status = table.Column<bool>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_authrole", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_bank",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    acc_type = table.Column<string>(nullable: true),
                    acc_number = table.Column<int>(nullable: false),
                    branch = table.Column<int>(nullable: true),
                    swift_code = table.Column<string>(nullable: true),
                    level_value = table.Column<string>(nullable: true),
                    url = table.Column<string>(nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_bank", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Branch",
                columns: table => new
                {
                    Branch_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Branch_Name = table.Column<string>(nullable: true),
                    Branch_Code = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Telephone_No = table.Column<string>(nullable: true),
                    Email_Id = table.Column<string>(nullable: true),
                    Created_date = table.Column<DateTime>(nullable: false),
                    Created_by = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Branch", x => x.Branch_Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_chartofacc",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Mast_Code = table.Column<int>(nullable: false),
                    ledger_Code = table.Column<int>(nullable: false),
                    ledger_Group = table.Column<int>(nullable: false),
                    Ledger_name = table.Column<string>(nullable: true),
                    is_ledger = table.Column<bool>(nullable: false),
                    is_subledger = table.Column<bool>(nullable: false),
                    Created_by = table.Column<int>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_chartofacc", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Company",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Pan = table.Column<int>(nullable: false),
                    Regd = table.Column<string>(nullable: true),
                    Telephone = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    StateCity = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    Createdby = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Company", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Fiscal",
                columns: table => new
                {
                    Fiscal_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Fiscalyear = table.Column<string>(nullable: true),
                    Fromdate = table.Column<DateTime>(nullable: false),
                    Todate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Created_date = table.Column<DateTime>(nullable: false),
                    Created_by = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Fiscal", x => x.Fiscal_Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_pro_group",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    group_name = table.Column<string>(nullable: true),
                    ledger_code = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_pro_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_product",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    p_type = table.Column<int>(nullable: false),
                    p_name = table.Column<string>(nullable: true),
                    d_name = table.Column<string>(nullable: true),
                    p_group = table.Column<int>(nullable: false),
                    qtyonhand = table.Column<int>(nullable: false),
                    p_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    s_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    i_account = table.Column<int>(nullable: false),
                    e_account = table.Column<int>(nullable: false),
                    reorder = table.Column<int>(nullable: false),
                    barcode = table.Column<string>(nullable: true),
                    istaxable = table.Column<bool>(nullable: false),
                    taxtype = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    status = table.Column<bool>(nullable: false),
                    image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_product", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Transaction",
                columns: table => new
                {
                    Tr_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Trans_Id = table.Column<int>(nullable: false),
                    ledger_Code = table.Column<int>(nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Gl_type = table.Column<string>(type: "varchar(10)", nullable: true),
                    Remarks = table.Column<string>(type: "varchar(max)", nullable: true),
                    Narration = table.Column<string>(type: "varchar(max)", nullable: true),
                    Created_by = table.Column<int>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Vouc_No = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Fiscal = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    tran_type = table.Column<string>(type: "varchar(20)", nullable: true),
                    branch_id = table.Column<int>(nullable: false),
                    project_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Transaction", x => x.Tr_Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_User",
                columns: table => new
                {
                    User_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Roll_Id = table.Column<int>(nullable: false),
                    Branch_Id = table.Column<int>(nullable: false),
                    Emp_Id = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Useremail = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Created_by = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Token = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_User", x => x.User_Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_User_Roll",
                columns: table => new
                {
                    Roll_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Rollname = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    Created_by = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_User_Roll", x => x.Roll_Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Userlog",
                columns: table => new
                {
                    SN = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<int>(nullable: false),
                    Datetime = table.Column<DateTime>(nullable: false),
                    Event = table.Column<string>(nullable: true),
                    Activity = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Userlog", x => x.SN);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_authrole");

            migrationBuilder.DropTable(
                name: "tbl_bank");

            migrationBuilder.DropTable(
                name: "tbl_Branch");

            migrationBuilder.DropTable(
                name: "tbl_chartofacc");

            migrationBuilder.DropTable(
                name: "tbl_Company");

            migrationBuilder.DropTable(
                name: "tbl_Fiscal");

            migrationBuilder.DropTable(
                name: "tbl_pro_group");

            migrationBuilder.DropTable(
                name: "tbl_product");

            migrationBuilder.DropTable(
                name: "tbl_Transaction");

            migrationBuilder.DropTable(
                name: "tbl_User");

            migrationBuilder.DropTable(
                name: "tbl_User_Roll");

            migrationBuilder.DropTable(
                name: "tbl_Userlog");
        }
    }
}
