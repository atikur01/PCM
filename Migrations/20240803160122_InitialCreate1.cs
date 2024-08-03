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
                name: "Collections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_Collections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollectionId = table.Column<int>(type: "int", nullable: false),
                    CustomString1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomString2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomString3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomInt1 = table.Column<int>(type: "int", nullable: true),
                    CustomInt2 = table.Column<int>(type: "int", nullable: true),
                    CustomInt3 = table.Column<int>(type: "int", nullable: true),
                    CustomMultilineText1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomMultilineText2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomMultilineText3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomBoolean1 = table.Column<bool>(type: "bit", nullable: true),
                    CustomBoolean2 = table.Column<bool>(type: "bit", nullable: true),
                    CustomBoolean3 = table.Column<bool>(type: "bit", nullable: true),
                    CustomDate1 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomDate2 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomDate3 = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_CollectionId",
                table: "Items",
                column: "CollectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Collections");
        }
    }
}
