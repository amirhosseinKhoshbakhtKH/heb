using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlexIn.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingChanges2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatures_FeatureOptions_OptionId",
                table: "ProductFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatures_Features_FeatureId",
                table: "ProductFeatures");

            migrationBuilder.RenameColumn(
                name: "OptionId",
                table: "ProductFeatures",
                newName: "FeatureOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductFeatures_OptionId",
                table: "ProductFeatures",
                newName: "IX_ProductFeatures_FeatureOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatures_FeatureOptions_FeatureOptionId",
                table: "ProductFeatures",
                column: "FeatureOptionId",
                principalTable: "FeatureOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatures_Features_FeatureId",
                table: "ProductFeatures",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatures_FeatureOptions_FeatureOptionId",
                table: "ProductFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatures_Features_FeatureId",
                table: "ProductFeatures");

            migrationBuilder.RenameColumn(
                name: "FeatureOptionId",
                table: "ProductFeatures",
                newName: "OptionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductFeatures_FeatureOptionId",
                table: "ProductFeatures",
                newName: "IX_ProductFeatures_OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatures_FeatureOptions_OptionId",
                table: "ProductFeatures",
                column: "OptionId",
                principalTable: "FeatureOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatures_Features_FeatureId",
                table: "ProductFeatures",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
