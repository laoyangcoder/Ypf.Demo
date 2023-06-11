using Microsoft.EntityFrameworkCore.Migrations;

namespace Ypf.Xncf.FileCenterModul.Migrations.Migrations.SqlServer
{
    public partial class AddTenantId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Ypf_FileCenterModul_Color",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Ypf_FileCenterModul_Color");
        }
    }
}
