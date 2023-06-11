using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ypf.Xncf.FileCenterModul.Domain.Migrations.Migrations.Oracle
{
    public partial class edit_filecenter_size : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ypf_FileCenterModul_FileCenter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    BucketName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    FileName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    GuidName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Size = table.Column<long>(type: "NUMBER(19)", nullable: false),
                    EXpandedName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Flag = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    AddTime = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    TenantId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    AdminRemark = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true),
                    Remark = table.Column<string>(type: "NVARCHAR2(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ypf_FileCenterModul_FileCenter", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ypf_FileCenterModul_FileCenter");
        }
    }
}
