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
            migrationBuilder.DropTable(
                name: "CustomFields");

            migrationBuilder.AddColumn<string>(
                name: "CustomBoolean1Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomBoolean2Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomBoolean3Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomDate1Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomDate2Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomDate3Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomInt1Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomInt2Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomInt3Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomMultilineText1Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomMultilineText2Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomMultilineText3Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomString1Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomString2Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomString3Name",
                table: "Collections",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "CustomFields",
                columns: table => new
                {
                    CustomFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomFieldId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFields", x => x.CustomFieldId);
                    table.ForeignKey(
                        name: "FK_CustomFields_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "CollectionId");
                    table.ForeignKey(
                        name: "FK_CustomFields_CustomFields_CustomFieldId1",
                        column: x => x.CustomFieldId1,
                        principalTable: "CustomFields",
                        principalColumn: "CustomFieldId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomFields_CollectionId",
                table: "CustomFields",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomFields_CustomFieldId1",
                table: "CustomFields",
                column: "CustomFieldId1");
        }
    }
}
