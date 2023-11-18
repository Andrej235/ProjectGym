using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectGym.Migrations
{
    /// <inheritdoc />
    public partial class AddedARelationShipBetweenExerciseAndMuscle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimaryMuscleGroups_Exercises_ExerciseId",
                table: "PrimaryMuscleGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_PrimaryMuscleGroups_MuscleGroups_MuscleGroupId",
                table: "PrimaryMuscleGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_SecondaryMuscleGroups_Exercises_ExerciseId",
                table: "SecondaryMuscleGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_SecondaryMuscleGroups_MuscleGroups_MuscleGroupId",
                table: "SecondaryMuscleGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SecondaryMuscleGroups",
                table: "SecondaryMuscleGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrimaryMuscleGroups",
                table: "PrimaryMuscleGroups");

            migrationBuilder.RenameTable(
                name: "SecondaryMuscleGroups",
                newName: "SecondaryMuscleGroupInExercise");

            migrationBuilder.RenameTable(
                name: "PrimaryMuscleGroups",
                newName: "PrimaryMuscleGroupInExercise");

            migrationBuilder.RenameIndex(
                name: "IX_SecondaryMuscleGroups_MuscleGroupId",
                table: "SecondaryMuscleGroupInExercise",
                newName: "IX_SecondaryMuscleGroupInExercise_MuscleGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_SecondaryMuscleGroups_ExerciseId",
                table: "SecondaryMuscleGroupInExercise",
                newName: "IX_SecondaryMuscleGroupInExercise_ExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_PrimaryMuscleGroups_MuscleGroupId",
                table: "PrimaryMuscleGroupInExercise",
                newName: "IX_PrimaryMuscleGroupInExercise_MuscleGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_PrimaryMuscleGroups_ExerciseId",
                table: "PrimaryMuscleGroupInExercise",
                newName: "IX_PrimaryMuscleGroupInExercise_ExerciseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SecondaryMuscleGroupInExercise",
                table: "SecondaryMuscleGroupInExercise",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrimaryMuscleGroupInExercise",
                table: "PrimaryMuscleGroupInExercise",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PrimaryMuscleInExercise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryMuscleInExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimaryMuscleInExercise_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrimaryMuscleInExercise_Muscles_MuscleId",
                        column: x => x.MuscleId,
                        principalTable: "Muscles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecondaryMuscleInExercise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondaryMuscleInExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecondaryMuscleInExercise_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SecondaryMuscleInExercise_Muscles_MuscleId",
                        column: x => x.MuscleId,
                        principalTable: "Muscles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryMuscleInExercise_ExerciseId",
                table: "PrimaryMuscleInExercise",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryMuscleInExercise_MuscleId",
                table: "PrimaryMuscleInExercise",
                column: "MuscleId");

            migrationBuilder.CreateIndex(
                name: "IX_SecondaryMuscleInExercise_ExerciseId",
                table: "SecondaryMuscleInExercise",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_SecondaryMuscleInExercise_MuscleId",
                table: "SecondaryMuscleInExercise",
                column: "MuscleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrimaryMuscleGroupInExercise_Exercises_ExerciseId",
                table: "PrimaryMuscleGroupInExercise",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrimaryMuscleGroupInExercise_MuscleGroups_MuscleGroupId",
                table: "PrimaryMuscleGroupInExercise",
                column: "MuscleGroupId",
                principalTable: "MuscleGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecondaryMuscleGroupInExercise_Exercises_ExerciseId",
                table: "SecondaryMuscleGroupInExercise",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecondaryMuscleGroupInExercise_MuscleGroups_MuscleGroupId",
                table: "SecondaryMuscleGroupInExercise",
                column: "MuscleGroupId",
                principalTable: "MuscleGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimaryMuscleGroupInExercise_Exercises_ExerciseId",
                table: "PrimaryMuscleGroupInExercise");

            migrationBuilder.DropForeignKey(
                name: "FK_PrimaryMuscleGroupInExercise_MuscleGroups_MuscleGroupId",
                table: "PrimaryMuscleGroupInExercise");

            migrationBuilder.DropForeignKey(
                name: "FK_SecondaryMuscleGroupInExercise_Exercises_ExerciseId",
                table: "SecondaryMuscleGroupInExercise");

            migrationBuilder.DropForeignKey(
                name: "FK_SecondaryMuscleGroupInExercise_MuscleGroups_MuscleGroupId",
                table: "SecondaryMuscleGroupInExercise");

            migrationBuilder.DropTable(
                name: "PrimaryMuscleInExercise");

            migrationBuilder.DropTable(
                name: "SecondaryMuscleInExercise");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SecondaryMuscleGroupInExercise",
                table: "SecondaryMuscleGroupInExercise");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrimaryMuscleGroupInExercise",
                table: "PrimaryMuscleGroupInExercise");

            migrationBuilder.RenameTable(
                name: "SecondaryMuscleGroupInExercise",
                newName: "SecondaryMuscleGroups");

            migrationBuilder.RenameTable(
                name: "PrimaryMuscleGroupInExercise",
                newName: "PrimaryMuscleGroups");

            migrationBuilder.RenameIndex(
                name: "IX_SecondaryMuscleGroupInExercise_MuscleGroupId",
                table: "SecondaryMuscleGroups",
                newName: "IX_SecondaryMuscleGroups_MuscleGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_SecondaryMuscleGroupInExercise_ExerciseId",
                table: "SecondaryMuscleGroups",
                newName: "IX_SecondaryMuscleGroups_ExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_PrimaryMuscleGroupInExercise_MuscleGroupId",
                table: "PrimaryMuscleGroups",
                newName: "IX_PrimaryMuscleGroups_MuscleGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_PrimaryMuscleGroupInExercise_ExerciseId",
                table: "PrimaryMuscleGroups",
                newName: "IX_PrimaryMuscleGroups_ExerciseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SecondaryMuscleGroups",
                table: "SecondaryMuscleGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrimaryMuscleGroups",
                table: "PrimaryMuscleGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrimaryMuscleGroups_Exercises_ExerciseId",
                table: "PrimaryMuscleGroups",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrimaryMuscleGroups_MuscleGroups_MuscleGroupId",
                table: "PrimaryMuscleGroups",
                column: "MuscleGroupId",
                principalTable: "MuscleGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecondaryMuscleGroups_Exercises_ExerciseId",
                table: "SecondaryMuscleGroups",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecondaryMuscleGroups_MuscleGroups_MuscleGroupId",
                table: "SecondaryMuscleGroups",
                column: "MuscleGroupId",
                principalTable: "MuscleGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
