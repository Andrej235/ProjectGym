using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectGym.Migrations
{
    /// <inheritdoc />
    public partial class RemovedSupersets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutSets_Sets_SuperSetId",
                table: "WorkoutSets");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutSets_SuperSetId",
                table: "WorkoutSets");

            migrationBuilder.DropColumn(
                name: "SuperSetId",
                table: "WorkoutSets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SuperSetId",
                table: "WorkoutSets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSets_SuperSetId",
                table: "WorkoutSets",
                column: "SuperSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutSets_Sets_SuperSetId",
                table: "WorkoutSets",
                column: "SuperSetId",
                principalTable: "Sets",
                principalColumn: "Id");
        }
    }
}
