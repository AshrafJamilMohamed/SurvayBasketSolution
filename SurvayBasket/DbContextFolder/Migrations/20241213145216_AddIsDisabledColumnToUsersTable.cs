using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurvayBasket.DbContextFolder.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDisabledColumnToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "D87E70CF-B177-446E-A4AE-87ECEED54A50",
                column: "ConcurrencyStamp",
                value: "224EC06D-1692-4784-9B84-0C388684B013");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "57006781-5610-445B-97D4-235477ADD6E5",
                columns: new[] { "IsDisabled", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAEOJu/7NKc3HRAMpTAEfTZLNtyE44/27tcdtEFN1I5CsUzbjebRkqIzqBVnDwBbkYqQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "D87E70CF-B177-446E-A4AE-87ECEED54A50",
                column: "ConcurrencyStamp",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "57006781-5610-445B-97D4-235477ADD6E5",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGpt4elsB7aQoPijKj0bxI1t41IeJUnxuH9zvO8RRYPi8z9MutUHg9A+WXbcOHvb3w==");
        }
    }
}
