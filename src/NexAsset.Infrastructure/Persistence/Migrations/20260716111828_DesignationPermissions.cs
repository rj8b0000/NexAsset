using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexAsset.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DesignationPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DesignationPermissions",
                columns: table => new
                {
                    DesignationId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignationPermissions", x => new { x.DesignationId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_DesignationPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DesignationPermissions_PermissionId",
                table: "DesignationPermissions",
                column: "PermissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DesignationPermissions");
        }
    }
}
