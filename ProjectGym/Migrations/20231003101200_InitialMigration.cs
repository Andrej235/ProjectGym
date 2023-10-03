using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectGym.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Muscles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name_en = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFront = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrlMain = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrlSecondary = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Muscles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UUID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExerciseBaseUUID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercises_ExerciseCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ExerciseCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentExerciseUsage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentExerciseUsage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentExerciseUsage_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipmentExerciseUsage_Exercises_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExerciseAliases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseAliases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseAliases_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseComments_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UUID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    Style = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseImages_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseNotes_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseVideos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UUID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Codec = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodecLong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseVideos_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Variation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Exercise1Id = table.Column<int>(type: "int", nullable: false),
                    Exercise2Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Variation_Exercises_Exercise1Id",
                        column: x => x.Exercise1Id,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Variation_Exercises_Exercise2Id",
                        column: x => x.Exercise2Id,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentExerciseUsage_EquipmentId",
                table: "EquipmentExerciseUsage",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseAliases_ExerciseId",
                table: "ExerciseAliases",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseComments_ExerciseId",
                table: "ExerciseComments",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseImages_ExerciseId",
                table: "ExerciseImages",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseNotes_ExerciseId",
                table: "ExerciseNotes",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_CategoryId",
                table: "Exercises",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseVideos_ExerciseId",
                table: "ExerciseVideos",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_MuscleExerciseConnection_ExerciseId",
                table: "MuscleExerciseConnection",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_MuscleExerciseConnection_MuscleId",
                table: "MuscleExerciseConnection",
                column: "MuscleId");

            migrationBuilder.CreateIndex(
                name: "IX_Variation_Exercise1Id",
                table: "Variation",
                column: "Exercise1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Variation_Exercise2Id",
                table: "Variation",
                column: "Exercise2Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentExerciseUsage");

            migrationBuilder.DropTable(
                name: "ExerciseAliases");

            migrationBuilder.DropTable(
                name: "ExerciseComments");

            migrationBuilder.DropTable(
                name: "ExerciseImages");

            migrationBuilder.DropTable(
                name: "ExerciseNotes");

            migrationBuilder.DropTable(
                name: "ExerciseVideos");

            migrationBuilder.DropTable(
                name: "MuscleExerciseConnection");

            migrationBuilder.DropTable(
                name: "Variation");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Muscles");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "ExerciseCategories");
        }
    }
}
