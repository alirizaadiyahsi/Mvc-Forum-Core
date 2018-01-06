using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MvcForumCore.Migrations
{
    public partial class addedentityhistoryentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChangeHistory = table.Column<string>(nullable: true),
                    CreateUserId = table.Column<Guid>(nullable: false),
                    CreationDateTime = table.Column<DateTime>(nullable: false),
                    EntityId = table.Column<Guid>(nullable: false),
                    EntityName = table.Column<string>(nullable: true),
                    EntityState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityHistory", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityHistory");
        }
    }
}
