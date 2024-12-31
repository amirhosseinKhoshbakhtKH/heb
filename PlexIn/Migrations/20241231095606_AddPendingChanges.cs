using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlexIn.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingChanges : Migration
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

            migrationBuilder.DropForeignKey(
                name: "FK_Products_FeatureOptions_OptionId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Features_FeatureId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FeatureId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_OptionId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FeatureId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "BusinessId",
                table: "Categories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_BusinessId",
                table: "Categories",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Businesses_BusinessId",
                table: "Categories",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Businesses_BusinessId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatures_FeatureOptions_OptionId",
                table: "ProductFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatures_Features_FeatureId",
                table: "ProductFeatures");

            migrationBuilder.DropIndex(
                name: "IX_Categories_BusinessId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "FeatureId",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OptionId",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FeatureId",
                table: "Products",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OptionId",
                table: "Products",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatures_FeatureOptions_OptionId",
                table: "ProductFeatures",
                column: "OptionId",
                principalTable: "FeatureOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatures_Features_FeatureId",
                table: "ProductFeatures",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_FeatureOptions_OptionId",
                table: "Products",
                column: "OptionId",
                principalTable: "FeatureOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Features_FeatureId",
                table: "Products",
                column: "FeatureId",
                principalTable: "Features",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
