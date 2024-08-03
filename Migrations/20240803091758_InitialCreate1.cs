using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCM.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomFields");

            migrationBuilder.RenameColumn(
                name: "Tags1",
                table: "Items",
                newName: "CustomString3");

            migrationBuilder.RenameColumn(
                name: "CustomFieldValues",
                table: "Items",
                newName: "CustomString2");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Collections",
                newName: "UserId");

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

            migrationBuilder.AddColumn<string>(
                name: "CustomMultilineText1",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomMultilineText2",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomMultilineText3",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomString1",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Collections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CustomBoolean1Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomBoolean2Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomBoolean3Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomDate1Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomDate2Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomDate3Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomInt1Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomInt2Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomInt3Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomMultilineText1Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomMultilineText2Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomMultilineText3Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomString1Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomString2Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomString3Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "CustomMultilineText1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomMultilineText2",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomMultilineText3",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CustomString1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CustomBoolean1Name",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CustomBoolean2Name",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CustomBoolean3Name",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CustomDate1Name",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CustomDate2Name",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CustomDate3Name",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CustomInt1Name",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CustomInt2Name",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CustomInt3Name",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CustomMultilineText1Name",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CustomMultilineText2Name",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CustomMultilineText3Name",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CustomString1Name",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CustomString2Name",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CustomString3Name",
                table: "Collections");

            migrationBuilder.RenameColumn(
                name: "CustomString3",
                table: "Items",
                newName: "Tags1");

            migrationBuilder.RenameColumn(
                name: "CustomString2",
                table: "Items",
                newName: "CustomFieldValues");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Collections",
                newName: "Category");

            migrationBuilder.CreateTable(
                name: "CustomFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollectionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomFields_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomFields_CollectionId",
                table: "CustomFields",
                column: "CollectionId");
        }
    }
}
