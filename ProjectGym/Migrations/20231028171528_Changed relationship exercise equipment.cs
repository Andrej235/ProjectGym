using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectGym.Migrations
{
    /// <inheritdoc />
    public partial class Changedrelationshipexerciseequipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentExerciseUsages_Equipment_EquipmentId",
                table: "EquipmentExerciseUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentExerciseUsages_Exercises_ExerciseId",
                table: "EquipmentExerciseUsages");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentExerciseUsages_Equipment_EquipmentId",
                table: "EquipmentExerciseUsages",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentExerciseUsages_Exercises_ExerciseId",
                table: "EquipmentExerciseUsages",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentExerciseUsages_Equipment_EquipmentId",
                table: "EquipmentExerciseUsages");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentExerciseUsages_Exercises_ExerciseId",
                table: "EquipmentExerciseUsages");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentExerciseUsages_Equipment_EquipmentId",
                table: "EquipmentExerciseUsages",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentExerciseUsages_Exercises_ExerciseId",
                table: "EquipmentExerciseUsages",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id");
        }
    }
}
