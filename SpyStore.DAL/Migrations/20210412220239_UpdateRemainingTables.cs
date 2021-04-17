using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpyStore.DAL.Migrations
{
    public partial class UpdateRemainingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "Store",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "Store",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(3800)", maxLength: 3800, nullable: true),
                    ModelName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    ModelNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProductImage = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ProductImageLarge = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ProductImageThumb = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    UnitCost = table.Column<decimal>(type: "money", nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "money", nullable: false),
                    UnitsInStock = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Store",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "Store",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    ShipDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Store",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartRecords",
                schema: "Store",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "getdate()"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartRecords_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Store",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartRecords_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Store",
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                schema: "Store",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitCost = table.Column<decimal>(type: "money", nullable: false),
                    LineItemTotal = table.Column<decimal>(type: "money", nullable: true, computedColumnSql: "[Quantity] * [UnitCost]"),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Store",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Store",
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers",
                schema: "Store",
                table: "Customers",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                schema: "Store",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                schema: "Store",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                schema: "Store",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "Store",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart",
                schema: "Store",
                table: "ShoppingCartRecords",
                columns: new[] { "Id", "ProductId", "CustomerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartRecords_CustomerId",
                schema: "Store",
                table: "ShoppingCartRecords",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartRecords_ProductId",
                schema: "Store",
                table: "ShoppingCartRecords",
                column: "ProductId");

            string sql = "CREATE FUNCTION Store.GetOrderTotal ( @OrderId INT ) " +
                "RETURNS MONEY WITH SCHEMABINDING " +
                "BEGIN " +
                "DECLARE @Result MONEY; " +
                "SELECT @Result = SUM([Quantity]*[UnitCost]) FROM Store.OrderDetails " +
                " WHERE OrderId = @OrderId; RETURN @Result END";
            migrationBuilder.Sql(sql);

            sql = "CREATE PROCEDURE [Store].[PurchaseItemsInCart] " +
                " (@customerId INT = 0, @orderId INT OUTPUT) AS BEGIN " +
                " SET NOCOUNT ON; " +
                " INSERT INTO Store.Orders (CustomerId, OrderDate, ShipDate) " +
                "    VALUES(@customerId, GETDATE(), GETDATE()); " +
                " SET @orderId = SCOPE_IDENTITY(); " +
                " DECLARE @TranName VARCHAR(20);SELECT @TranName = 'CommitOrder'; " +
                "   BEGIN TRANSACTION @TranName; " +
                "   BEGIN TRY " +
                "       INSERT INTO Store.OrderDetails (OrderId, ProductId, Quantity, UnitCost) " +
                "       SELECT @orderId, ProductId, Quantity, p.CurrentPrice " +
                "       FROM Store.ShoppingCartRecords scr " +
                "          INNER JOIN Store.Products p ON p.Id = scr.ProductId " +
                "       WHERE CustomerId = @customerId; " +
                "       DELETE FROM Store.ShoppingCartRecords WHERE CustomerId = @customerId; " +
                "       COMMIT TRANSACTION @TranName; " +
                "   END TRY " +
                "   BEGIN CATCH " +
                "       ROLLBACK TRANSACTION @TranName; " +
                "       SET @orderId = -1; " +
                "   END CATCH; " +
                 "END;";
            
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "ShoppingCartRecords",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "Store");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "Store");


            //Drop function and procedure
            migrationBuilder.Sql("DROP FUNCTION [Store].[GetOrderTotal]");
            migrationBuilder.Sql("DROP PROCEDURE [Store].[PurchaseItemsInCart]");

        }
    }
}
