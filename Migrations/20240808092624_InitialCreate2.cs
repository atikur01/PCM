using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCM.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomFields_Collections_CollectionId",
                table: "CustomFields");

            migrationBuilder.AlterColumn<Guid>(
                name: "CollectionId",
                table: "CustomFields",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomFieldId1",
                table: "CustomFields",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomFields_CustomFieldId1",
                table: "CustomFields",
                column: "CustomFieldId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomFields_Collections_CollectionId",
                table: "CustomFields",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomFields_CustomFields_CustomFieldId1",
                table: "CustomFields",
                column: "CustomFieldId1",
                principalTable: "CustomFields",
                principalColumn: "CustomFieldId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomFields_Collections_CollectionId",
                table: "CustomFields");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomFields_CustomFields_CustomFieldId1",
                table: "CustomFields");

            migrationBuilder.DropIndex(
                name: "IX_CustomFields_CustomFieldId1",
                table: "CustomFields");

            migrationBuilder.DropColumn(
                name: "CustomFieldId1",
                table: "CustomFields");

            migrationBuilder.AlterColumn<Guid>(
                name: "CollectionId",
                table: "CustomFields",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomFields_Collections_CollectionId",
                table: "CustomFields",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "CollectionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
