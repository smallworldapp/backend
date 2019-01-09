using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmallWorld.Database.Migrations
{
    public partial class v0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Application",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Guid = table.Column<Guid>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Application", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Credentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Guid = table.Column<Guid>(type: "BLOB", nullable: false),
                    Hash = table.Column<byte[]>(type: "BLOB", nullable: true),
                    Salt = table.Column<byte[]>(type: "BLOB", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credentials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Description",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Goals = table.Column<string>(type: "TEXT", nullable: false),
                    Guid = table.Column<Guid>(type: "BLOB", nullable: false),
                    Introduction = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Description", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResetToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Expiration = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Guid = table.Column<Guid>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationQuestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Answer = table.Column<string>(type: "TEXT", nullable: true),
                    ApplicationId = table.Column<int>(type: "INTEGER", nullable: true),
                    Guid = table.Column<Guid>(type: "BLOB", nullable: false),
                    Question = table.Column<string>(type: "TEXT", nullable: false),
                    Subtext = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationQuestion_Application_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Application",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FaqItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Answer = table.Column<string>(type: "TEXT", nullable: false),
                    DescriptionId = table.Column<int>(type: "INTEGER", nullable: true),
                    Guid = table.Column<Guid>(type: "BLOB", nullable: false),
                    Question = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaqItem_Description_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "Description",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CredentialsId = table.Column<int>(type: "INTEGER", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Guid = table.Column<Guid>(type: "BLOB", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    ResetTokenId = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Credentials_CredentialsId",
                        column: x => x.CredentialsId,
                        principalTable: "Credentials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_ResetToken_ResetTokenId",
                        column: x => x.ResetTokenId,
                        principalTable: "ResetToken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Worlds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    ApplicationId = table.Column<int>(type: "INTEGER", nullable: true),
                    BackupUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    DescriptionId = table.Column<int>(type: "INTEGER", nullable: true),
                    Guid = table.Column<Guid>(type: "BLOB", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worlds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Worlds_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Worlds_Application_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Application",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Worlds_Description_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "Description",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Guid = table.Column<Guid>(type: "BLOB", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    JoinToken = table.Column<Guid>(type: "BLOB", nullable: true),
                    LeaveToken = table.Column<Guid>(type: "BLOB", nullable: true),
                    OptOut = table.Column<bool>(type: "INTEGER", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: true),
                    WorldId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identity_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pairings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Guid = table.Column<Guid>(type: "BLOB", nullable: false),
                    IsComplete = table.Column<bool>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    WorldId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pairings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pairings_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PairingSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Guid = table.Column<Guid>(type: "BLOB", nullable: false),
                    MostRecent = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Period = table.Column<long>(type: "INTEGER", nullable: false),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PairingSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PairingSettings_Worlds_Id",
                        column: x => x.Id,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Guid = table.Column<Guid>(type: "BLOB", nullable: false),
                    InitiatorId = table.Column<int>(type: "INTEGER", nullable: true),
                    Outcome = table.Column<int>(type: "INTEGER", nullable: false),
                    PairingId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReceiverId = table.Column<int>(type: "INTEGER", nullable: true),
                    WorldId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pairs_Identity_InitiatorId",
                        column: x => x.InitiatorId,
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pairs_Pairings_PairingId",
                        column: x => x.PairingId,
                        principalTable: "Pairings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pairs_Identity_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Identity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pairs_Worlds_WorldId",
                        column: x => x.WorldId,
                        principalTable: "Worlds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CredentialsId",
                table: "Accounts",
                column: "CredentialsId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ResetTokenId",
                table: "Accounts",
                column: "ResetTokenId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationQuestion_ApplicationId",
                table: "ApplicationQuestion",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_FaqItem_DescriptionId",
                table: "FaqItem",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Identity_WorldId",
                table: "Identity",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_Pairings_WorldId",
                table: "Pairings",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_Pairs_InitiatorId",
                table: "Pairs",
                column: "InitiatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pairs_PairingId",
                table: "Pairs",
                column: "PairingId");

            migrationBuilder.CreateIndex(
                name: "IX_Pairs_ReceiverId",
                table: "Pairs",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Pairs_WorldId",
                table: "Pairs",
                column: "WorldId");

            migrationBuilder.CreateIndex(
                name: "IX_Worlds_AccountId",
                table: "Worlds",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Worlds_ApplicationId",
                table: "Worlds",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Worlds_BackupUserId",
                table: "Worlds",
                column: "BackupUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Worlds_DescriptionId",
                table: "Worlds",
                column: "DescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Worlds_Identity_BackupUserId",
                table: "Worlds",
                column: "BackupUserId",
                principalTable: "Identity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Credentials_CredentialsId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_ResetToken_ResetTokenId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Worlds_Application_ApplicationId",
                table: "Worlds");

            migrationBuilder.DropForeignKey(
                name: "FK_Worlds_Description_DescriptionId",
                table: "Worlds");

            migrationBuilder.DropForeignKey(
                name: "FK_Identity_Worlds_WorldId",
                table: "Identity");

            migrationBuilder.DropTable(
                name: "ApplicationQuestion");

            migrationBuilder.DropTable(
                name: "FaqItem");

            migrationBuilder.DropTable(
                name: "PairingSettings");

            migrationBuilder.DropTable(
                name: "Pairs");

            migrationBuilder.DropTable(
                name: "Pairings");

            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "ResetToken");

            migrationBuilder.DropTable(
                name: "Application");

            migrationBuilder.DropTable(
                name: "Description");

            migrationBuilder.DropTable(
                name: "Worlds");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Identity");
        }
    }
}
