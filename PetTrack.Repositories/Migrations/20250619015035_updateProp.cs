using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTrack.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class updateProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "WalletTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankNumber",
                table: "WalletTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankName",
                table: "WalletTransactions");

            migrationBuilder.DropColumn(
                name: "BankNumber",
                table: "WalletTransactions");
        }
    }
}
