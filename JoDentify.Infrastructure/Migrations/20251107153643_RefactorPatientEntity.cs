using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoDentify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorPatientEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BloodPressure",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "BloodSugar",
                table: "Patients",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BloodType",
                table: "Patients",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChronicDiseases",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrentMedications",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DentalHistory",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DentalInsuranceType",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DentistNotes",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactName",
                table: "Patients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactPhone",
                table: "Patients",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasBraces",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "HeartRate",
                table: "Patients",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Height",
                table: "Patients",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsuranceExpiryDate",
                table: "Patients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceNumber",
                table: "Patients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InsuranceProvider",
                table: "Patients",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsReferredByFriend",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSmoker",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastVisitDate",
                table: "Patients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastVisitReason",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastXRayDate",
                table: "Patients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                table: "Patients",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MembershipStatus",
                table: "Patients",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "Patients",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NotesFromDoctor",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "Patients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OralCareRoutine",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "OxygenLevel",
                table: "Patients",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatientFeedback",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "Patients",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredDoctor",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReferralSource",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryPhone",
                table: "Patients",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Temperature",
                table: "Patients",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TreatmentPlan",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "Patients",
                type: "real",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PatientFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ClinicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientFiles_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientFiles_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientFiles_ClinicId",
                table: "PatientFiles",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientFiles_PatientId",
                table: "PatientFiles",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientFiles");

            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "BloodPressure",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "BloodSugar",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "BloodType",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ChronicDiseases",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "CurrentMedications",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DentalHistory",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DentalInsuranceType",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DentistNotes",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "EmergencyContactName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "EmergencyContactPhone",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "HasBraces",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "HeartRate",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "InsuranceExpiryDate",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "InsuranceNumber",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "InsuranceProvider",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "IsReferredByFriend",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "IsSmoker",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "LastVisitDate",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "LastVisitReason",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "LastXRayDate",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MembershipStatus",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "NotesFromDoctor",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "OralCareRoutine",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "OxygenLevel",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PatientFeedback",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PreferredDoctor",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ReferralSource",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "SecondaryPhone",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Temperature",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "TreatmentPlan",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Patients");
        }
    }
}
