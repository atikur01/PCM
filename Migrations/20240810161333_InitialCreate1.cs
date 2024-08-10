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
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    CollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomString1Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomString2Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomString3Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomInt1Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomInt2Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomInt3Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomMultilineText1Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomMultilineText2Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomMultilineText3Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomBoolean1Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomBoolean2Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomBoolean3Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomDate1Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomDate2Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomDate3Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.CollectionId);
                    table.ForeignKey(
                        name: "FK_Collections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomString1Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomString2Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomString3Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomInt1Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomInt2Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomInt3Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomMultilineText1Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomMultilineText2Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomMultilineText3Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomBoolean1Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomBoolean2Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomBoolean3Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomDate1Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomDate2Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomDate3Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Items_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                    table.ForeignKey(
                        name: "FK_Tags_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collections_UserId",
                table: "Collections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CollectionId",
                table: "Items",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ItemId",
                table: "Tags",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
