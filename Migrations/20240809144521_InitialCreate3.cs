using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCM.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Tags_TagId1",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ItemId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_TagId1",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "TagId1",
                table: "Tags");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ItemId",
                table: "Tags",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_ItemId",
                table: "Tags");

            migrationBuilder.AddColumn<Guid>(
                name: "TagId1",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ItemId",
                table: "Tags",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TagId1",
                table: "Tags",
                column: "TagId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Tags_TagId1",
                table: "Tags",
                column: "TagId1",
                principalTable: "Tags",
                principalColumn: "TagId");
        }
    }
}
