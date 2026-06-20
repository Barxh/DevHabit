using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevHabit.Api.Migrations.Application
{
    /// <inheritdoc />
    public partial class Fix_HabitTags_Relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tags_habits_habit_id",
                schema: "dev_habit",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "ix_tags_habit_id",
                schema: "dev_habit",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "habit_id",
                schema: "dev_habit",
                table: "tags");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "habit_id",
                schema: "dev_habit",
                table: "tags",
                type: "character varying(500)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_tags_habit_id",
                schema: "dev_habit",
                table: "tags",
                column: "habit_id");

            migrationBuilder.AddForeignKey(
                name: "fk_tags_habits_habit_id",
                schema: "dev_habit",
                table: "tags",
                column: "habit_id",
                principalSchema: "dev_habit",
                principalTable: "habits",
                principalColumn: "id");
        }
    }
}
