using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelManagment.Migrations
{
    /// <inheritdoc />
    public partial class updateImgages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Camera",
                keyColumn: "CameraId",
                keyValue: new Guid("12a8bcc1-0804-43e4-9107-5218c30f505f"));

            migrationBuilder.DeleteData(
                table: "Camera",
                keyColumn: "CameraId",
                keyValue: new Guid("15add54e-18a5-46f2-b81a-0d8169428090"));

            migrationBuilder.DeleteData(
                table: "Camera",
                keyColumn: "CameraId",
                keyValue: new Guid("2ecfd0e4-3b58-4895-aff3-83f8ccd4c2d6"));

            migrationBuilder.DeleteData(
                table: "Camera",
                keyColumn: "CameraId",
                keyValue: new Guid("31a81649-6d34-41ef-9e66-17fb6b31a964"));

            migrationBuilder.DeleteData(
                table: "Camera",
                keyColumn: "CameraId",
                keyValue: new Guid("4db911f2-ae85-4d08-9432-0af59b2ff731"));

            migrationBuilder.DeleteData(
                table: "Camera",
                keyColumn: "CameraId",
                keyValue: new Guid("588ae193-380e-48e8-860d-e5ad4a1c632e"));

            migrationBuilder.DeleteData(
                table: "Camera",
                keyColumn: "CameraId",
                keyValue: new Guid("6703cf65-76ae-4b76-b3f3-4e37df562905"));

            migrationBuilder.DeleteData(
                table: "Camera",
                keyColumn: "CameraId",
                keyValue: new Guid("9ae082cf-5a63-4819-ba9a-647369dc401e"));

            migrationBuilder.DeleteData(
                table: "Camera",
                keyColumn: "CameraId",
                keyValue: new Guid("bbb09be5-ac19-401c-bce5-d52f369901fb"));

            migrationBuilder.DeleteData(
                table: "Camera",
                keyColumn: "CameraId",
                keyValue: new Guid("edb1338f-9aea-4c61-80d8-606b71621553"));

            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "Camera",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "Camera");

            migrationBuilder.InsertData(
                table: "Camera",
                columns: new[] { "CameraId", "IsLoan", "Numero", "Prezzo", "Tipo" },
                values: new object[,]
                {
                    { new Guid("12a8bcc1-0804-43e4-9107-5218c30f505f"), false, "101", 50.00m, "Singola" },
                    { new Guid("15add54e-18a5-46f2-b81a-0d8169428090"), false, "205", 80.00m, "Doppia" },
                    { new Guid("2ecfd0e4-3b58-4895-aff3-83f8ccd4c2d6"), false, "202", 100.00m, "Tripla" },
                    { new Guid("31a81649-6d34-41ef-9e66-17fb6b31a964"), false, "105", 150.00m, "Suite" },
                    { new Guid("4db911f2-ae85-4d08-9432-0af59b2ff731"), false, "204", 50.00m, "Singola" },
                    { new Guid("588ae193-380e-48e8-860d-e5ad4a1c632e"), false, "203", 100.00m, "Tripla" },
                    { new Guid("6703cf65-76ae-4b76-b3f3-4e37df562905"), false, "102", 50.00m, "Singola" },
                    { new Guid("9ae082cf-5a63-4819-ba9a-647369dc401e"), false, "104", 80.00m, "Doppia" },
                    { new Guid("bbb09be5-ac19-401c-bce5-d52f369901fb"), false, "201", 150.00m, "Suite" },
                    { new Guid("edb1338f-9aea-4c61-80d8-606b71621553"), false, "103", 80.00m, "Doppia" }
                });
        }
    }
}
