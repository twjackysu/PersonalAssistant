using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonalAssistant.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountInitialization",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerID = table.Column<string>(maxLength: 50, nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Balance = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountInitialization", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ExpenditureType",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerID = table.Column<string>(maxLength: 50, nullable: true),
                    TypeName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenditureType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StockInitialization",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerID = table.Column<string>(maxLength: 50, nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    StockCode = table.Column<string>(maxLength: 10, nullable: false),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockInitialization", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Income",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerID = table.Column<string>(maxLength: 50, nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    AccountID = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Income", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Income_AccountInitialization_AccountID",
                        column: x => x.AccountID,
                        principalTable: "AccountInitialization",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InternalTransfer",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerID = table.Column<string>(maxLength: 50, nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    AccountID = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Fees = table.Column<int>(nullable: true),
                    TransferIntoAccountID = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalTransfer", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InternalTransfer_AccountInitialization_AccountID",
                        column: x => x.AccountID,
                        principalTable: "AccountInitialization",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InternalTransfer_AccountInitialization_TransferIntoAccountID",
                        column: x => x.TransferIntoAccountID,
                        principalTable: "AccountInitialization",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockTransaction",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerID = table.Column<string>(maxLength: 50, nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    AccountID = table.Column<int>(nullable: false),
                    StockCode = table.Column<string>(maxLength: 10, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    Fees = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransaction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StockTransaction_AccountInitialization_AccountID",
                        column: x => x.AccountID,
                        principalTable: "AccountInitialization",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expenditure",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerID = table.Column<string>(maxLength: 50, nullable: true),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    AccountID = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Fees = table.Column<decimal>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    ExpenditureTypeID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenditure", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Expenditure_AccountInitialization_AccountID",
                        column: x => x.AccountID,
                        principalTable: "AccountInitialization",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenditure_ExpenditureType_ExpenditureTypeID",
                        column: x => x.ExpenditureTypeID,
                        principalTable: "ExpenditureType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenditure_AccountID",
                table: "Expenditure",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Expenditure_ExpenditureTypeID",
                table: "Expenditure",
                column: "ExpenditureTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Income_AccountID",
                table: "Income",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_InternalTransfer_AccountID",
                table: "InternalTransfer",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_InternalTransfer_TransferIntoAccountID",
                table: "InternalTransfer",
                column: "TransferIntoAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransaction_AccountID",
                table: "StockTransaction",
                column: "AccountID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenditure");

            migrationBuilder.DropTable(
                name: "Income");

            migrationBuilder.DropTable(
                name: "InternalTransfer");

            migrationBuilder.DropTable(
                name: "StockInitialization");

            migrationBuilder.DropTable(
                name: "StockTransaction");

            migrationBuilder.DropTable(
                name: "ExpenditureType");

            migrationBuilder.DropTable(
                name: "AccountInitialization");
        }
    }
}
