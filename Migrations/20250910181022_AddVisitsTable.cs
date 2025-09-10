using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedicalAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddVisitsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visit_Doctor_DoctorId",
                table: "Visit");

            migrationBuilder.DropForeignKey(
                name: "FK_Visit_Users_UserId",
                table: "Visit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Visit",
                table: "Visit");

            migrationBuilder.RenameTable(
                name: "Visit",
                newName: "Visits");

            migrationBuilder.RenameIndex(
                name: "IX_Visit_UserId",
                table: "Visits",
                newName: "IX_Visits_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Visit_DoctorId",
                table: "Visits",
                newName: "IX_Visits_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Visits",
                table: "Visits",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Doctor_DoctorId",
                table: "Visits",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Users_UserId",
                table: "Visits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Doctor_DoctorId",
                table: "Visits");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Users_UserId",
                table: "Visits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Visits",
                table: "Visits");

            migrationBuilder.RenameTable(
                name: "Visits",
                newName: "Visit");

            migrationBuilder.RenameIndex(
                name: "IX_Visits_UserId",
                table: "Visit",
                newName: "IX_Visit_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Visits_DoctorId",
                table: "Visit",
                newName: "IX_Visit_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Visit",
                table: "Visit",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Visit_Doctor_DoctorId",
                table: "Visit",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visit_Users_UserId",
                table: "Visit",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
