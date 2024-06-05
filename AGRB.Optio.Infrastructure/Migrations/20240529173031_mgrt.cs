using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AGRB.Optio.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mgrt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryOfTransactions_TypeOfTransactions_TransactionTypeID",
                table: "CategoryOfTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationToMerchants_Locations_LocatrionId",
                table: "LocationToMerchants");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationToMerchants_Merchants_merchantId",
                table: "LocationToMerchants");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Curencies_CurrencyId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "ValutesCourses");

            migrationBuilder.DropIndex(
                name: "IX_CategoryOfTransactions_TransactionTypeID",
                table: "CategoryOfTransactions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Personal_Number",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Curencies",
                table: "Curencies");

            migrationBuilder.RenameTable(
                name: "Curencies",
                newName: "Currencies");

            migrationBuilder.RenameColumn(
                name: "merchantId",
                table: "LocationToMerchants",
                newName: "MerchantId");

            migrationBuilder.RenameColumn(
                name: "LocatrionId",
                table: "LocationToMerchants",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_LocationToMerchants_merchantId",
                table: "LocationToMerchants",
                newName: "IX_LocationToMerchants_MerchantId");

            migrationBuilder.RenameIndex(
                name: "IX_LocationToMerchants_LocatrionId",
                table: "LocationToMerchants",
                newName: "IX_LocationToMerchants_LocationId");

            migrationBuilder.RenameColumn(
                name: "TransactionTypeID",
                table: "CategoryOfTransactions",
                newName: "TransactionTypeId");

            migrationBuilder.RenameColumn(
                name: "Name_Of_Valute",
                table: "Currencies",
                newName: "Name_Of_Currency");

            migrationBuilder.RenameIndex(
                name: "IX_Curencies_Name_Of_Valute",
                table: "Currencies",
                newName: "IX_Currencies_Name_Of_Currency");

            migrationBuilder.RenameIndex(
                name: "IX_Curencies_Currency_Code",
                table: "Currencies",
                newName: "IX_Currencies_Currency_Code");

            migrationBuilder.AddColumn<long>(
                name: "TypeOfTransactionId",
                table: "CategoryOfTransactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Last_Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeRates_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryOfTransactions_TypeOfTransactionId",
                table: "CategoryOfTransactions",
                column: "TypeOfTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Personal_Number",
                table: "AspNetUsers",
                column: "Personal_Number",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_CurrencyId",
                table: "ExchangeRates",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_Last_Updated",
                table: "ExchangeRates",
                column: "Last_Updated",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_Rate",
                table: "ExchangeRates",
                column: "Rate",
                descending: new bool[0]);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryOfTransactions_TypeOfTransactions_TypeOfTransactionId",
                table: "CategoryOfTransactions",
                column: "TypeOfTransactionId",
                principalTable: "TypeOfTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationToMerchants_Locations_LocationId",
                table: "LocationToMerchants",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationToMerchants_Merchants_MerchantId",
                table: "LocationToMerchants",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Currencies_CurrencyId",
                table: "Transactions",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryOfTransactions_TypeOfTransactions_TypeOfTransactionId",
                table: "CategoryOfTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationToMerchants_Locations_LocationId",
                table: "LocationToMerchants");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationToMerchants_Merchants_MerchantId",
                table: "LocationToMerchants");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Currencies_CurrencyId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "ExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_CategoryOfTransactions_TypeOfTransactionId",
                table: "CategoryOfTransactions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Personal_Number",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "TypeOfTransactionId",
                table: "CategoryOfTransactions");

            migrationBuilder.RenameTable(
                name: "Currencies",
                newName: "Curencies");

            migrationBuilder.RenameColumn(
                name: "MerchantId",
                table: "LocationToMerchants",
                newName: "merchantId");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "LocationToMerchants",
                newName: "LocatrionId");

            migrationBuilder.RenameIndex(
                name: "IX_LocationToMerchants_MerchantId",
                table: "LocationToMerchants",
                newName: "IX_LocationToMerchants_merchantId");

            migrationBuilder.RenameIndex(
                name: "IX_LocationToMerchants_LocationId",
                table: "LocationToMerchants",
                newName: "IX_LocationToMerchants_LocatrionId");

            migrationBuilder.RenameColumn(
                name: "TransactionTypeId",
                table: "CategoryOfTransactions",
                newName: "TransactionTypeID");

            migrationBuilder.RenameColumn(
                name: "Name_Of_Currency",
                table: "Curencies",
                newName: "Name_Of_Valute");

            migrationBuilder.RenameIndex(
                name: "IX_Currencies_Name_Of_Currency",
                table: "Curencies",
                newName: "IX_Curencies_Name_Of_Valute");

            migrationBuilder.RenameIndex(
                name: "IX_Currencies_Currency_Code",
                table: "Curencies",
                newName: "IX_Curencies_Currency_Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Curencies",
                table: "Curencies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ValutesCourses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyID = table.Column<int>(type: "int", nullable: false),
                    Last_Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Exchange_Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status_Of_Valute = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValutesCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValutesCourses_Curencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Curencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryOfTransactions_TransactionTypeID",
                table: "CategoryOfTransactions",
                column: "TransactionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Personal_Number",
                table: "AspNetUsers",
                column: "Personal_Number",
                unique: true,
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_ValutesCourses_CurrencyID",
                table: "ValutesCourses",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_ValutesCourses_Exchange_Rate",
                table: "ValutesCourses",
                column: "Exchange_Rate",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_ValutesCourses_Last_Updated",
                table: "ValutesCourses",
                column: "Last_Updated",
                descending: new bool[0]);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryOfTransactions_TypeOfTransactions_TransactionTypeID",
                table: "CategoryOfTransactions",
                column: "TransactionTypeID",
                principalTable: "TypeOfTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationToMerchants_Locations_LocatrionId",
                table: "LocationToMerchants",
                column: "LocatrionId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationToMerchants_Merchants_merchantId",
                table: "LocationToMerchants",
                column: "merchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Curencies_CurrencyId",
                table: "Transactions",
                column: "CurrencyId",
                principalTable: "Curencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
