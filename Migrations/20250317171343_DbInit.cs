using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Kliker.Migrations
{
    /// <inheritdoc />
    public partial class DbInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "achievements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Primary Key", x => x.Id);
                },
                comment: "Przechowuje informacje o osiągnięciach. Każdy rekord to jedno dostępne do zdobycia osiągnięcie reprezentowane poprzez nazwę oraz opis.");

            migrationBuilder.CreateTable(
                name: "upgrades",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    price = table.Column<decimal>(type: "numeric(1)", precision: 1, nullable: false),
                    levelRequired = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("upgrades_pkey", x => x.ID);
                },
                comment: "Przechowuje informacje o ulepszeniach nabywanych przez użytkowników. Każdy rekord to pojedyncze ulepszenie reprezentowane przez nazwę i opis.");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Password_hash = table.Column<string>(type: "text", nullable: false),
                    Lvl = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Banned = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Points = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.Id);
                },
                comment: "Zawiera informacje o kontach użytkowników. Każdy rekord to reprezentacja jednego użytkownika w postaci loginu, maila, hasła...");

            migrationBuilder.CreateTable(
                name: "PlayersAchievments",
                columns: table => new
                {
                    playerId = table.Column<int>(type: "integer", nullable: false),
                    achievementId = table.Column<int>(type: "integer", nullable: false),
                    dateIfBeingAchieved = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PlayersAchievments_pkey", x => x.playerId);
                    table.ForeignKey(
                        name: "Foreign Key for achievements",
                        column: x => x.achievementId,
                        principalTable: "achievements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Foreign Key for users",
                        column: x => x.playerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                },
                comment: "Tabela pośrednia pomiędzy tabelą użytkowników a tabelą osiągnięć. Przechowuje ona informacje o zdobyciu przez konkretnego użytkownika konkretnego osiągnięcia wraz z datą tego wydarzenia.");

            migrationBuilder.CreateTable(
                name: "PlayersUpgrades",
                columns: table => new
                {
                    playerId = table.Column<int>(type: "integer", nullable: false),
                    upgradeId = table.Column<int>(type: "integer", nullable: false),
                    dateOfBeingUpgraded = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PlayersUpgrades_pkey", x => new { x.playerId, x.upgradeId });
                    table.ForeignKey(
                        name: "PlayerId foreign key",
                        column: x => x.playerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "UpgradeId foreign key",
                        column: x => x.upgradeId,
                        principalTable: "upgrades",
                        principalColumn: "ID");
                },
                comment: "Tabela pomocnicza zawierająca informacje o odblokowaniu ulepszeń przez graczy. Każdy rekord to reprezentacja odblokowania ulepszenia o konkretnym ID przez gracza o konkretnym ID.");

            migrationBuilder.CreateIndex(
                name: "Name",
                table: "achievements",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayersAchievments_achievementId",
                table: "PlayersAchievments",
                column: "achievementId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersUpgrades_upgradeId",
                table: "PlayersUpgrades",
                column: "upgradeId");

            migrationBuilder.CreateIndex(
                name: "name is unique",
                table: "upgrades",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "email_unique",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "username_unique",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayersAchievments");

            migrationBuilder.DropTable(
                name: "PlayersUpgrades");

            migrationBuilder.DropTable(
                name: "achievements");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "upgrades");
        }
    }
}
