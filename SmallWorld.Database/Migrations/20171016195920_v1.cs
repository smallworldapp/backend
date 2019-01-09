using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmallWorld.Database.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "Worlds",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Privacy",
                table: "Worlds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 3); // WorldPrivacy.Public

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Pairings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 2); // PairingType.Auto

            migrationBuilder.AddColumn<bool>(
                name: "HasEmailValidation",
                table: "Identity",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasLeft",
                table: "Identity",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasPrivacyValidation",
                table: "Identity",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Identity",
                keyColumn: "Discriminator",
                keyValue: "Member",
                columns: new string[] { "HasEmailValidation", "HasLeft", "HasPrivacyValidation" },
                values: new object[] { false, false, false });

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1); // AccountType.Standard

            migrationBuilder.CreateTable(
                name: "__MfroMigrationsHistory",
                columns: table => new {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Identifier = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___MfroMigrationsHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Body = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Guid = table.Column<Guid>(type: "BLOB", nullable: false),
                    IsSent = table.Column<bool>(type: "INTEGER", nullable: false),
                    Sent = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Subject = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailRecipient",
                columns: table => new {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    EmailId = table.Column<int>(type: "INTEGER", nullable: true),
                    Guid = table.Column<Guid>(type: "BLOB", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailRecipient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailRecipient_Emails_EmailId",
                        column: x => x.EmailId,
                        principalTable: "Emails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailRecipient_EmailId",
                table: "EmailRecipient",
                column: "EmailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__MfroMigrationsHistory");

            migrationBuilder.DropTable(
                name: "EmailRecipient");

            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Worlds");

            migrationBuilder.DropColumn(
                name: "Privacy",
                table: "Worlds");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Pairings");

            migrationBuilder.DropColumn(
                name: "HasEmailValidation",
                table: "Identity");

            migrationBuilder.DropColumn(
                name: "HasLeft",
                table: "Identity");

            migrationBuilder.DropColumn(
                name: "HasPrivacyValidation",
                table: "Identity");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Accounts");
        }
    }
}
