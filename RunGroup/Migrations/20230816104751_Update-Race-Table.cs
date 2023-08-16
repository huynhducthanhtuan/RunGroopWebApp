using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunGroup.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRaceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_Addresses_AddressId",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "EntryFee",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "Twitter",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Races");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Races",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Races",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Races",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Addresses_AddressId",
                table: "Races",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_Addresses_AddressId",
                table: "Races");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Races",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Races",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Races",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Races",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EntryFee",
                table: "Races",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "Races",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Races",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                table: "Races",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Races",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Addresses_AddressId",
                table: "Races",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
