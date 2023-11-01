using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectGym.Migrations
{
    /// <inheritdoc />
    public partial class Changedallmanytomanyrelationshipstodeletecascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimaryMuscleExerciseConnections_Exercises_ExerciseId",
                table: "PrimaryMuscleExerciseConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_PrimaryMuscleExerciseConnections_Muscles_MuscleId",
                table: "PrimaryMuscleExerciseConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_SecondaryMuscleExerciseConnections_Exercises_ExerciseId",
                table: "SecondaryMuscleExerciseConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_SecondaryMuscleExerciseConnections_Muscles_MuscleId",
                table: "SecondaryMuscleExerciseConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExerciseBookmarks_Exercises_ExerciseId",
                table: "UserExerciseBookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExerciseBookmarks_Users_UserId",
                table: "UserExerciseBookmarks");

            migrationBuilder.AddForeignKey(
                name: "FK_PrimaryMuscleExerciseConnections_Exercises_ExerciseId",
                table: "PrimaryMuscleExerciseConnections",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrimaryMuscleExerciseConnections_Muscles_MuscleId",
                table: "PrimaryMuscleExerciseConnections",
                column: "MuscleId",
                principalTable: "Muscles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecondaryMuscleExerciseConnections_Exercises_ExerciseId",
                table: "SecondaryMuscleExerciseConnections",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecondaryMuscleExerciseConnections_Muscles_MuscleId",
                table: "SecondaryMuscleExerciseConnections",
                column: "MuscleId",
                principalTable: "Muscles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExerciseBookmarks_Exercises_ExerciseId",
                table: "UserExerciseBookmarks",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExerciseBookmarks_Users_UserId",
                table: "UserExerciseBookmarks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimaryMuscleExerciseConnections_Exercises_ExerciseId",
                table: "PrimaryMuscleExerciseConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_PrimaryMuscleExerciseConnections_Muscles_MuscleId",
                table: "PrimaryMuscleExerciseConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_SecondaryMuscleExerciseConnections_Exercises_ExerciseId",
                table: "SecondaryMuscleExerciseConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_SecondaryMuscleExerciseConnections_Muscles_MuscleId",
                table: "SecondaryMuscleExerciseConnections");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExerciseBookmarks_Exercises_ExerciseId",
                table: "UserExerciseBookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExerciseBookmarks_Users_UserId",
                table: "UserExerciseBookmarks");

            migrationBuilder.AddForeignKey(
                name: "FK_PrimaryMuscleExerciseConnections_Exercises_ExerciseId",
                table: "PrimaryMuscleExerciseConnections",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrimaryMuscleExerciseConnections_Muscles_MuscleId",
                table: "PrimaryMuscleExerciseConnections",
                column: "MuscleId",
                principalTable: "Muscles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SecondaryMuscleExerciseConnections_Exercises_ExerciseId",
                table: "SecondaryMuscleExerciseConnections",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SecondaryMuscleExerciseConnections_Muscles_MuscleId",
                table: "SecondaryMuscleExerciseConnections",
                column: "MuscleId",
                principalTable: "Muscles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExerciseBookmarks_Exercises_ExerciseId",
                table: "UserExerciseBookmarks",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExerciseBookmarks_Users_UserId",
                table: "UserExerciseBookmarks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
