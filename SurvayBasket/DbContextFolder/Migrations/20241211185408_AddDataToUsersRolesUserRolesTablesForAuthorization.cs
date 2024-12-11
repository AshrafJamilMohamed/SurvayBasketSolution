using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurvayBasket.DbContextFolder.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToUsersRolesUserRolesTablesForAuthorization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VoteAnswers_VoteId",
                table: "VoteAnswers");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "26B4DBD9-C25D-4CB5-BD72-DE4B793226BA", "584EC06D-1692-4784-9B84-0C320684B064", true, false, "Member", "MEMBER" },
                    { "D87E70CF-B177-446E-A4AE-87ECEED54A50", "224EC06D-1692-4784-9B84-0C388684B013", false, false, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FristName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "57006781-5610-445B-97D4-235477ADD6E5", 0, "FC84E7FD-A166-4648-A417-CD7DA283DD68", "Admin@Survay-Basket.com", true, "Admin", "Demo", false, null, "ADMIN@SURVAY-BASKET.COM", "ADMIN", "AQAAAAIAAYagAAAAEGpt4elsB7aQoPijKj0bxI1t41IeJUnxuH9zvO8RRYPi8z9MutUHg9A+WXbcOHvb3w==", null, false, "B5A50E65C79749EABCDE5C88F8D07705", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "D87E70CF-B177-446E-A4AE-87ECEED54A50", "57006781-5610-445B-97D4-235477ADD6E5" },
                    { "26B4DBD9-C25D-4CB5-BD72-DE4B793226BA", "5c490b1e-6fe9-4aaa-83e8-72a3e68ea280" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoteAnswers_VoteId",
                table: "VoteAnswers",
                column: "VoteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VoteAnswers_VoteId",
                table: "VoteAnswers");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "D87E70CF-B177-446E-A4AE-87ECEED54A50", "57006781-5610-445B-97D4-235477ADD6E5" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "26B4DBD9-C25D-4CB5-BD72-DE4B793226BA", "5c490b1e-6fe9-4aaa-83e8-72a3e68ea280" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "26B4DBD9-C25D-4CB5-BD72-DE4B793226BA");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "D87E70CF-B177-446E-A4AE-87ECEED54A50");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "57006781-5610-445B-97D4-235477ADD6E5");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "IX_VoteAnswers_VoteId",
                table: "VoteAnswers",
                column: "VoteId",
                unique: true);
        }
    }
}
