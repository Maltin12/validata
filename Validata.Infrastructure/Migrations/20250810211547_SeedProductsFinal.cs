using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Validata.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductsFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("171e7e33-fe1e-4240-bfdf-802aafe0ffb8"), "Apple iPhone 15 Pro", 999.99m },
                    { new Guid("afb87ec7-9290-4a0f-bcb8-0750cc144e11"), "Sony WH-1000XM5 Headphones", 349.99m },
                    { new Guid("c1fffd65-3a2b-4670-9998-9cdf97fd3dd6"), "Samsung 55\" 4K Smart TV", 650.00m },
                    { new Guid("f36ad4d2-3c89-43f2-921d-c552d04206de"), "Dell XPS 13 Laptop", 1200.00m },
                    { new Guid("f95f8f03-ddfd-4a6b-9b9b-1c523f6ba17d"), "Logitech MX Master 3S", 99.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("171e7e33-fe1e-4240-bfdf-802aafe0ffb8"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("afb87ec7-9290-4a0f-bcb8-0750cc144e11"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c1fffd65-3a2b-4670-9998-9cdf97fd3dd6"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f36ad4d2-3c89-43f2-921d-c552d04206de"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("f95f8f03-ddfd-4a6b-9b9b-1c523f6ba17d"));
        }
    }
}
