using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectGym.Migrations
{
    /// <inheritdoc />
    public partial class Fixedprimaryandsecondarymusclereferencesmerging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MuscleExerciseConnection");

            migrationBuilder.DropColumn(
                name: "ExerciseBaseUUID",
                table: "Exercises");

            migrationBuilder.CreateTable(
                name: "PrimaryMuscleExerciseConnection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryMuscleExerciseConnection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimaryMuscleExerciseConnection_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrimaryMuscleExerciseConnection_Muscles_MuscleId",
                        column: x => x.MuscleId,
                        principalTable: "Muscles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SecondaryMuscleExerciseConnection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondaryMuscleExerciseConnection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecondaryMuscleExerciseConnection_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SecondaryMuscleExerciseConnection_Muscles_MuscleId",
                        column: x => x.MuscleId,
                        principalTable: "Muscles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryMuscleExerciseConnection_ExerciseId",
                table: "PrimaryMuscleExerciseConnection",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryMuscleExerciseConnection_MuscleId",
                table: "PrimaryMuscleExerciseConnection",
                column: "MuscleId");

            migrationBuilder.CreateIndex(
                name: "IX_SecondaryMuscleExerciseConnection_ExerciseId",
                table: "SecondaryMuscleExerciseConnection",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_SecondaryMuscleExerciseConnection_MuscleId",
                table: "SecondaryMuscleExerciseConnection",
                column: "MuscleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrimaryMuscleExerciseConnection");

            migrationBuilder.DropTable(
                name: "SecondaryMuscleExerciseConnection");

            migrationBuilder.AddColumn<string>(
                name: "ExerciseBaseUUID",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MuscleExerciseConnection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuscleExerciseConnection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MuscleExerciseConnection_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MuscleExerciseConnection_Muscles_MuscleId",
                        column: x => x.MuscleId,
                        principalTable: "Muscles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MuscleExerciseConnection_ExerciseId",
                table: "MuscleExerciseConnection",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_MuscleExerciseConnection_MuscleId",
                table: "MuscleExerciseConnection",
                column: "MuscleId");
        }
    }
}
