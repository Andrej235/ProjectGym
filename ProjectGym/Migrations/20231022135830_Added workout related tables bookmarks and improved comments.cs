using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectGym.Migrations
{
    /// <inheritdoc />
    public partial class Addedworkoutrelatedtablesbookmarksandimprovedcomments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentExerciseUsage");

            migrationBuilder.DropTable(
                name: "PrimaryMuscleExerciseConnection");

            migrationBuilder.DropTable(
                name: "SecondaryMuscleExerciseConnection");

            migrationBuilder.DropTable(
                name: "Variation");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "ExerciseComments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CommentUserDownvotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentUserDownvotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentUserDownvotes_ExerciseComments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "ExerciseComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentUserDownvotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommentUserUpvotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentUserUpvotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentUserUpvotes_ExerciseComments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "ExerciseComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentUserUpvotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentExerciseUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentExerciseUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentExerciseUsages_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EquipmentExerciseUsages_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExerciseVariations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Exercise1Id = table.Column<int>(type: "int", nullable: false),
                    Exercise2Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseVariations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseVariations_Exercises_Exercise1Id",
                        column: x => x.Exercise1Id,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExerciseVariations_Exercises_Exercise2Id",
                        column: x => x.Exercise2Id,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PrimaryMuscleExerciseConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryMuscleExerciseConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimaryMuscleExerciseConnections_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrimaryMuscleExerciseConnections_Muscles_MuscleId",
                        column: x => x.MuscleId,
                        principalTable: "Muscles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SecondaryMuscleExerciseConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondaryMuscleExerciseConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecondaryMuscleExerciseConnections_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SecondaryMuscleExerciseConnections_Muscles_MuscleId",
                        column: x => x.MuscleId,
                        principalTable: "Muscles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RepRange_Bottom = table.Column<int>(type: "int", nullable: false),
                    RepRange_Top = table.Column<int>(type: "int", nullable: false),
                    Partials = table.Column<bool>(type: "bit", nullable: false),
                    ToFaliure = table.Column<bool>(type: "bit", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sets_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sets_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExerciseBookmarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExerciseBookmarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExerciseBookmarks_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserExerciseBookmarks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserExerciseWeights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    DateOfAchieving = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExerciseWeights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExerciseWeights_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserExerciseWeights_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Workouts_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Supersets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetSets = table.Column<int>(type: "int", nullable: false),
                    DropSets = table.Column<bool>(type: "bit", nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supersets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supersets_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetSets = table.Column<int>(type: "int", nullable: false),
                    DropSets = table.Column<bool>(type: "bit", nullable: false),
                    WorkoutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SuperSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutSets_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutSets_Supersets_SuperSetId",
                        column: x => x.SuperSetId,
                        principalTable: "Supersets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutSets_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseComments_CreatorId",
                table: "ExerciseComments",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentUserDownvotes_CommentId",
                table: "CommentUserDownvotes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentUserDownvotes_UserId",
                table: "CommentUserDownvotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentUserUpvotes_CommentId",
                table: "CommentUserUpvotes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentUserUpvotes_UserId",
                table: "CommentUserUpvotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentExerciseUsages_EquipmentId",
                table: "EquipmentExerciseUsages",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentExerciseUsages_ExerciseId",
                table: "EquipmentExerciseUsages",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseVariations_Exercise1Id",
                table: "ExerciseVariations",
                column: "Exercise1Id");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseVariations_Exercise2Id",
                table: "ExerciseVariations",
                column: "Exercise2Id");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryMuscleExerciseConnections_ExerciseId",
                table: "PrimaryMuscleExerciseConnections",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryMuscleExerciseConnections_MuscleId",
                table: "PrimaryMuscleExerciseConnections",
                column: "MuscleId");

            migrationBuilder.CreateIndex(
                name: "IX_SecondaryMuscleExerciseConnections_ExerciseId",
                table: "SecondaryMuscleExerciseConnections",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_SecondaryMuscleExerciseConnections_MuscleId",
                table: "SecondaryMuscleExerciseConnections",
                column: "MuscleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_CreatorId",
                table: "Sets",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_ExerciseId",
                table: "Sets",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Supersets_SetId",
                table: "Supersets",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExerciseBookmarks_ExerciseId",
                table: "UserExerciseBookmarks",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExerciseBookmarks_UserId",
                table: "UserExerciseBookmarks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExerciseWeights_ExerciseId",
                table: "UserExerciseWeights",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExerciseWeights_UserId",
                table: "UserExerciseWeights",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_CreatorId",
                table: "Workouts",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_SetId",
                table: "WorkoutSets",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_SuperSetId",
                table: "WorkoutSets",
                column: "SuperSetId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_WorkoutId",
                table: "WorkoutSets",
                column: "WorkoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseComments_Users_CreatorId",
                table: "ExerciseComments",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseComments_Users_CreatorId",
                table: "ExerciseComments");

            migrationBuilder.DropTable(
                name: "CommentUserDownvotes");

            migrationBuilder.DropTable(
                name: "CommentUserUpvotes");

            migrationBuilder.DropTable(
                name: "EquipmentExerciseUsages");

            migrationBuilder.DropTable(
                name: "ExerciseVariations");

            migrationBuilder.DropTable(
                name: "PrimaryMuscleExerciseConnections");

            migrationBuilder.DropTable(
                name: "SecondaryMuscleExerciseConnections");

            migrationBuilder.DropTable(
                name: "UserExerciseBookmarks");

            migrationBuilder.DropTable(
                name: "UserExerciseWeights");

            migrationBuilder.DropTable(
                name: "WorkoutSets");

            migrationBuilder.DropTable(
                name: "Supersets");

            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseComments_CreatorId",
                table: "ExerciseComments");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ExerciseComments");

            migrationBuilder.CreateTable(
                name: "EquipmentExerciseUsage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_EquipmentExerciseUsage_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id");
                });

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
                name: "IX_EquipmentExerciseUsage_ExerciseId",
                table: "EquipmentExerciseUsage",
                column: "ExerciseId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Variation_Exercise1Id",
                table: "Variation",
                column: "Exercise1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Variation_Exercise2Id",
                table: "Variation",
                column: "Exercise2Id");
        }
    }
}
