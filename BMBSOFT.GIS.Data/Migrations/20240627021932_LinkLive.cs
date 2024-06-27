﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BASE.Data.Migrations
{
    /// <inheritdoc />
    public partial class LinkLive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LinkLive",
                schema: "cms",
                table: "computer",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkLive",
                schema: "cms",
                table: "computer");
        }
    }
}
