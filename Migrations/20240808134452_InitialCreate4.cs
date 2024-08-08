using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCM.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomBoolean1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomBoolean2",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomBoolean3",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomDate1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomDate2",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomDate3",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomInt1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomInt2",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomInt3",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "CustomString3",
                table: "Items",
                newName: "CustomString3Name");

            migrationBuilder.RenameColumn(
                name: "CustomString2",
                table: "Items",
                newName: "CustomString2Name");

            migrationBuilder.RenameColumn(
                name: "CustomString1",
                table: "Items",
                newName: "CustomString1Name");

            migrationBuilder.RenameColumn(
                name: "CustomMultilineText3",
                table: "Items",
                newName: "CustomMultilineText3Name");

            migrationBuilder.RenameColumn(
                name: "CustomMultilineText2",
                table: "Items",
                newName: "CustomMultilineText2Name");

            migrationBuilder.RenameColumn(
                name: "CustomMultilineText1",
                table: "Items",
                newName: "CustomMultilineText1Name");

            migrationBuilder.AddColumn<string>(
                name: "CustomBoolean1Name",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomBoolean2Name",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomBoolean3Name",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomDate1Name",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomDate2Name",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomDate3Name",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomInt1Name",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomInt2Name",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomInt3Name",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ItemId1",
                table: "Items",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemId1",
                table: "Items",
                column: "ItemId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Items_ItemId1",
                table: "Items",
                column: "ItemId1",
                principalTable: "Items",
                principalColumn: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Items_ItemId1",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemId1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomBoolean1Name",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomBoolean2Name",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomBoolean3Name",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomDate1Name",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomDate2Name",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomDate3Name",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomInt1Name",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomInt2Name",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomInt3Name",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemId1",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "CustomString3Name",
                table: "Items",
                newName: "CustomString3");

            migrationBuilder.RenameColumn(
                name: "CustomString2Name",
                table: "Items",
                newName: "CustomString2");

            migrationBuilder.RenameColumn(
                name: "CustomString1Name",
                table: "Items",
                newName: "CustomString1");

            migrationBuilder.RenameColumn(
                name: "CustomMultilineText3Name",
                table: "Items",
                newName: "CustomMultilineText3");

            migrationBuilder.RenameColumn(
                name: "CustomMultilineText2Name",
                table: "Items",
                newName: "CustomMultilineText2");

            migrationBuilder.RenameColumn(
                name: "CustomMultilineText1Name",
                table: "Items",
                newName: "CustomMultilineText1");

            migrationBuilder.AddColumn<bool>(
                name: "CustomBoolean1",
                table: "Items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CustomBoolean2",
                table: "Items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CustomBoolean3",
                table: "Items",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CustomDate1",
                table: "Items",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CustomDate2",
                table: "Items",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CustomDate3",
                table: "Items",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomInt1",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomInt2",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomInt3",
                table: "Items",
                type: "int",
                nullable: true);
        }
    }
}
