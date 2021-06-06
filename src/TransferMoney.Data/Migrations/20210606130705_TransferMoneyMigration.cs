using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TransferMoney.Data.Migrations
{
    public partial class TransferMoneyMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "char(36)", nullable: false),
                    AccountOrigin = table.Column<string>(type: "longtext", nullable: true),
                    AccountDestination = table.Column<string>(type: "longtext", nullable: true),
                    Value = table.Column<double>(type: "double", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: true),
                    Message = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.TransactionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transfers");
        }
    }
}
