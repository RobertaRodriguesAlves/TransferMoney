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
                    AccountOrigin = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false),
                    AccountDestination = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false),
                    Value = table.Column<double>(type: "double", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: true),
                    Message = table.Column<string>(type: "longtext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.TransactionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_TransactionId",
                table: "Transfers",
                column: "TransactionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transfers");
        }
    }
}
