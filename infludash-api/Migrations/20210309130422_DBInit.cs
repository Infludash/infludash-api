using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infludash_api.Migrations
{
    public partial class DBInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    email = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    password = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    verified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "socials",
                columns: table => new
                {
                    accessToken = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    SocialId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_socials", x => x.accessToken);
                    table.ForeignKey(
                        name: "FK_socials_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_socials_userId",
                table: "socials",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "socials");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
