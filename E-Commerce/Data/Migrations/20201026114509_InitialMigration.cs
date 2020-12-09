using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Commerce.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RevokedTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    RevokedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevokedTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Role = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Name", "Password", "Role" },
                values: new object[,]
                {
                    { new Guid("65d1e165-f31e-3ac0-194f-82ecbed83b36"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "matt87@yahoo.com", "Matt Krajcik", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "Admin" },
                    { new Guid("ef448a02-14df-0e2f-b5ed-b258d7e9d1a6"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "deborah_mante@yahoo.com", "Deborah Mante", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "Admin" },
                    { new Guid("af12ea5f-7205-02ff-42b9-5a2f5ca84dd3"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "kay39@hotmail.com", "Kay Marquardt", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "User" },
                    { new Guid("0090d917-15a5-d394-da0d-c3b69dc91d1d"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "anita.lind51@hotmail.com", "Anita Lind", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "User" },
                    { new Guid("f65bb821-ef55-a5e1-8d5b-45031f076137"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "alice.davis93@hotmail.com", "Alice Davis", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "User" },
                    { new Guid("841f4f8e-a5cb-d785-1fa2-781436599cb5"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "cedric.huel@hotmail.com", "Cedric Huel", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "User" },
                    { new Guid("27caf944-31c9-67aa-b9cd-5f5fbd2b49e9"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "shane22@yahoo.com", "Shane Upton", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "User" },
                    { new Guid("6eaf0aff-937f-babc-3581-24f58c031544"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "fred_leuschke@yahoo.com", "Fred Leuschke", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "User" },
                    { new Guid("e7678337-3576-596c-392d-f5ed09ac615b"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "olivia_gleason@yahoo.com", "Olivia Gleason", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "Admin" },
                    { new Guid("3f72465c-ddb6-1555-4c3f-da41a8f1b368"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "loretta_goyette78@gmail.com", "Loretta Goyette", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "Admin" },
                    { new Guid("bca9565b-e066-5055-d79a-56bd8a7b6fb9"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "lula_schoen@gmail.com", "Lula Schoen", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "Admin" },
                    { new Guid("89ea143a-99f7-2067-cbfa-a7d517af5b35"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "ethel_cassin52@yahoo.com", "Ethel Cassin", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "User" },
                    { new Guid("b9b82323-7c88-ec3f-6820-a1ee5bbc1187"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "phillip_kuvalis37@gmail.com", "Phillip Kuvalis", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "Admin" },
                    { new Guid("c2f4da08-3b43-d854-a828-6d4bcf26208b"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "oliver81@gmail.com", "Oliver Emmerich", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "Admin" },
                    { new Guid("10d8bdc1-f32d-5916-bc34-998e88e1cf6d"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "lindsay_green31@yahoo.com", "Lindsay Green", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "Admin" },
                    { new Guid("06002bdc-c8f0-b4be-2c04-7cb892fd3837"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "brittany.satterfield96@gmail.com", "Brittany Satterfield", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "User" },
                    { new Guid("6f7f640e-9c15-2538-bb2a-449621ba548c"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "laurence25@yahoo.com", "Laurence Mann", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "Admin" },
                    { new Guid("5803973c-76d1-7e50-93e1-e03619650fdc"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "dawn_schmeler@yahoo.com", "Dawn Schmeler", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "User" },
                    { new Guid("f87e20a0-3745-bd6d-3fd7-f2f38470f4ca"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "olga_dach@yahoo.com", "Olga Dach", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "Admin" },
                    { new Guid("c6d52796-3dfd-e80a-b4fd-9e46a0149236"), new DateTime(2020, 10, 26, 14, 45, 8, 363, DateTimeKind.Local).AddTicks(1731), "darren.tromp@gmail.com", "Darren Tromp", "$2y$11$ZvZR41.QNYU4qAznY.y.ZuvPUmMMSxSnRhxLYRJvhRU0rWYyYffA6", "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderId",
                table: "OrderProducts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProductId",
                table: "OrderProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UserId",
                table: "Products",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.DropTable(
                name: "RevokedTokens");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
