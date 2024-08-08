using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCM.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomString3Name",
                table: "Items",
                newName: "CustomString3Value");

            migrationBuilder.RenameColumn(
                name: "CustomString2Name",
                table: "Items",
                newName: "CustomString2Value");

            migrationBuilder.RenameColumn(
                name: "CustomString1Name",
                table: "Items",
                newName: "CustomString1Value");

            migrationBuilder.RenameColumn(
                name: "CustomMultilineText3Name",
                table: "Items",
                newName: "CustomMultilineText3Value");

            migrationBuilder.RenameColumn(
                name: "CustomMultilineText2Name",
                table: "Items",
                newName: "CustomMultilineText2Value");

            migrationBuilder.RenameColumn(
                name: "CustomMultilineText1Name",
                table: "Items",
                newName: "CustomMultilineText1Value");

            migrationBuilder.RenameColumn(
                name: "CustomInt3Name",
                table: "Items",
                newName: "CustomInt3Value");

            migrationBuilder.RenameColumn(
                name: "CustomInt2Name",
                table: "Items",
                newName: "CustomInt2Value");

            migrationBuilder.RenameColumn(
                name: "CustomInt1Name",
                table: "Items",
                newName: "CustomInt1Value");

            migrationBuilder.RenameColumn(
                name: "CustomDate3Name",
                table: "Items",
                newName: "CustomDate3Value");

            migrationBuilder.RenameColumn(
                name: "CustomDate2Name",
                table: "Items",
                newName: "CustomDate2Value");

            migrationBuilder.RenameColumn(
                name: "CustomDate1Name",
                table: "Items",
                newName: "CustomDate1Value");

            migrationBuilder.RenameColumn(
                name: "CustomBoolean3Name",
                table: "Items",
                newName: "CustomBoolean3Value");

            migrationBuilder.RenameColumn(
                name: "CustomBoolean2Name",
                table: "Items",
                newName: "CustomBoolean2Value");

            migrationBuilder.RenameColumn(
                name: "CustomBoolean1Name",
                table: "Items",
                newName: "CustomBoolean1Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomString3Value",
                table: "Items",
                newName: "CustomString3Name");

            migrationBuilder.RenameColumn(
                name: "CustomString2Value",
                table: "Items",
                newName: "CustomString2Name");

            migrationBuilder.RenameColumn(
                name: "CustomString1Value",
                table: "Items",
                newName: "CustomString1Name");

            migrationBuilder.RenameColumn(
                name: "CustomMultilineText3Value",
                table: "Items",
                newName: "CustomMultilineText3Name");

            migrationBuilder.RenameColumn(
                name: "CustomMultilineText2Value",
                table: "Items",
                newName: "CustomMultilineText2Name");

            migrationBuilder.RenameColumn(
                name: "CustomMultilineText1Value",
                table: "Items",
                newName: "CustomMultilineText1Name");

            migrationBuilder.RenameColumn(
                name: "CustomInt3Value",
                table: "Items",
                newName: "CustomInt3Name");

            migrationBuilder.RenameColumn(
                name: "CustomInt2Value",
                table: "Items",
                newName: "CustomInt2Name");

            migrationBuilder.RenameColumn(
                name: "CustomInt1Value",
                table: "Items",
                newName: "CustomInt1Name");

            migrationBuilder.RenameColumn(
                name: "CustomDate3Value",
                table: "Items",
                newName: "CustomDate3Name");

            migrationBuilder.RenameColumn(
                name: "CustomDate2Value",
                table: "Items",
                newName: "CustomDate2Name");

            migrationBuilder.RenameColumn(
                name: "CustomDate1Value",
                table: "Items",
                newName: "CustomDate1Name");

            migrationBuilder.RenameColumn(
                name: "CustomBoolean3Value",
                table: "Items",
                newName: "CustomBoolean3Name");

            migrationBuilder.RenameColumn(
                name: "CustomBoolean2Value",
                table: "Items",
                newName: "CustomBoolean2Name");

            migrationBuilder.RenameColumn(
                name: "CustomBoolean1Value",
                table: "Items",
                newName: "CustomBoolean1Name");
        }
    }
}
