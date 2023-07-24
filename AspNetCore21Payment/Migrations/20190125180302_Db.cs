using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCore21Payment.Migrations
{
    public partial class Db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Epayments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RetrivalRefNo = table.Column<string>(maxLength: 100, nullable: true),
                    SystemTraceNo = table.Column<string>(nullable: true),
                    ResCode = table.Column<string>(maxLength: 150, nullable: true),
                    Token = table.Column<string>(maxLength: 150, nullable: true),
                    Description = table.Column<string>(maxLength: 150, nullable: true),
                    Rrn = table.Column<long>(nullable: false),
                    PaymentFinished = table.Column<bool>(nullable: false),
                    Amount = table.Column<long>(nullable: false),
                    BankName = table.Column<string>(maxLength: 50, nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    InsertDatetime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Epayments", x => x.PaymentId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Epayments");
        }
    }
}
