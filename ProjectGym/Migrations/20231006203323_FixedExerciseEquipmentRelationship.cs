using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectGym.Migrations
{
    /// <inheritdoc />
    public partial class FixedExerciseEquipmentRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentExerciseUsage_Exercises_EquipmentId",
                table: "EquipmentExerciseUsage");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentExerciseUsage_ExerciseId",
                table: "EquipmentExerciseUsage",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentExerciseUsage_Exercises_ExerciseId",
                table: "EquipmentExerciseUsage",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentExerciseUsage_Exercises_ExerciseId",
                table: "EquipmentExerciseUsage");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentExerciseUsage_ExerciseId",
                table: "EquipmentExerciseUsage");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentExerciseUsage_Exercises_EquipmentId",
                table: "EquipmentExerciseUsage",
                column: "EquipmentId",
                principalTable: "Exercises",
                principalColumn: "Id");
        }
    }
}
