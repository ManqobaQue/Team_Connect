 using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyPhonebook.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedLogIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, "0ab2af2b-7a69-4d97-9b05-d8e405d90108", "user1@example.com", true, false, null, "USER1@EXAMPLE.COM", "USER1@EXAMPLE.COM", "AQAAAAIAAYagAAAAEI0rxhNJULqltz9CCOK1APt0tzhsDUdvvbkLpk8FlMDWlOm01a0aBcul8KLWjOBRCw==", null, false, "ad7566ef-143b-498f-ba3f-be5afdf4a10f", false, "user1@example.com" },
                    { "2", 0, "e6d59999-2b30-4abf-8f91-ca970ad9013d", "user2@example.com", true, false, null, "USER2@EXAMPLE.COM", "USER2@EXAMPLE.COM", "AQAAAAIAAYagAAAAEJhRNjK0QNWPmJ1eGjfVhFisDXVSG06LI2I7NPFW8ehUHI7KVn4WwKaGrwup0a9ZuQ==", null, false, "d6c3d2ee-703b-49a3-94ea-86a153e1e1a2", false, "user2@example.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2");
        }
    }
}
