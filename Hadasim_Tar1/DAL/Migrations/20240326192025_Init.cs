using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    CellPhone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VaccinsManufacturers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinsManufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Illnesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PositiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NegativeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(9)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Illnesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Illnesses_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vaccins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VaccinsManufacturerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientID = table.Column<string>(type: "nvarchar(9)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vaccins_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vaccins_VaccinsManufacturers_VaccinsManufacturerID",
                        column: x => x.VaccinsManufacturerID,
                        principalTable: "VaccinsManufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Illnesses_PatientId",
                table: "Illnesses",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Vaccins_PatientID",
                table: "Vaccins",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Vaccins_VaccinsManufacturerID",
                table: "Vaccins",
                column: "VaccinsManufacturerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Illnesses");

            migrationBuilder.DropTable(
                name: "Vaccins");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "VaccinsManufacturers");
        }
    }
}
