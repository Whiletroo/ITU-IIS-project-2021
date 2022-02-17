using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Charity.DAL.Migrations
{
    public partial class Initial01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShelterAdmins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShelterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShelterAdmins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shelters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shelters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shelters_ShelterAdmins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "ShelterAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: true),
                    Goal = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShelterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donations_Shelters_ShelterId",
                        column: x => x.ShelterId,
                        principalTable: "Shelters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Volunteerings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequiredCount = table.Column<int>(type: "int", nullable: true),
                    ShelterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteerings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Volunteerings_Shelters_ShelterId",
                        column: x => x.ShelterId,
                        principalTable: "Shelters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sum = table.Column<int>(type: "int", nullable: false),
                    DonationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VolunteerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Donations_DonationId",
                        column: x => x.DonationId,
                        principalTable: "Donations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Volunteers_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "Volunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VolunteerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VolunteeringId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollments_Volunteerings_VolunteeringId",
                        column: x => x.VolunteeringId,
                        principalTable: "Volunteerings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Volunteers_VolunteerId",
                        column: x => x.VolunteerId,
                        principalTable: "Volunteers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Phone", "PhotoURL", "Role" },
                values: new object[] { new Guid("35bfc454-ff3c-4079-92ab-52c3060be8a1"), "admin@shelterio.com", "Guy", "Super", "verysecurepass", "+420777111222", "", "admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f3099cd-47e9-403c-9919-686da947ff5f", "4adff49a-d4ac-437a-a3bc-77323e109e44", "Volunteer", "VOLUNTEER" },
                    { "6293c9c0-a0dc-4567-9da1-f5b420d89712", "490dae8c-1325-4ee9-9e7a-af7c706d8633", "ShelterAdministrator", "SHELTERADMINISTRATOR" },
                    { "f7eea789-ae98-40d3-843e-ef44deb82ba3", "81fced2a-5848-49fe-b336-920bbe296cbc", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "ShelterAdmins",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Phone", "PhotoURL", "Role", "ShelterId" },
                values: new object[] { new Guid("2ea469b7-70e9-4810-b679-0a60ab205f16"), "johndoe@superdoggies.com", "John", "Doe", "doggiepass", "+420666555444", "", "shelter-admin", new Guid("7600763f-6a2e-482c-9ded-fa9a824376e5") });

            migrationBuilder.InsertData(
                table: "Volunteers",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Phone", "PhotoURL", "Role" },
                values: new object[] { new Guid("952f40f0-8181-4cc6-aff8-d932e002d98f"), "janedoe@mail.com", "Jane", "Sue", "suepass", "+420555444222", "https://scontent-prg1-1.xx.fbcdn.net/v/t39.30808-6/214664454_3054028748220135_5232530489626250445_n.jpg?_nc_cat=110&ccb=1-5&_nc_sid=09cbfe&_nc_ohc=jvZz0yhGArwAX-FAACl&tn=mRRjwhJuYUsf6LX7&_nc_ht=scontent-prg1-1.xx&oh=a1ff46a61f59ff521bcc67f9083cd5f2&oe=61A900D7", "volunteer" });

            migrationBuilder.InsertData(
                table: "Shelters",
                columns: new[] { "Id", "Address", "AdminId", "Description", "PhotoURL", "Title" },
                values: new object[] { new Guid("7600763f-6a2e-482c-9ded-fa9a824376e5"), null, new Guid("2ea469b7-70e9-4810-b679-0a60ab205f16"), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer sit amet lectus ligula. Aliquam tincidunt a ex et viverra. Nam dignissim egestas urna, et lobortis erat aliquet vitae. Aenean bibendum magna est, eu iaculis tortor blandit quis. Integer vestibulum sapien at velit pretium, sed sollicitudin mi scelerisque. Donec vitae auctor urna. In faucibus turpis turpis, et finibus purus imperdiet et. Nulla sed magna orci. Cras tortor urna, dictum nec ligula et, vehicula consectetur lorem. Aenean quis pharetra ipsum, eget malesuada ligula. Donec sed consectetur eros. Integer aliquam lacinia nisl, nec condimentum arcu pellentesque a. Aliquam vel odio lectus. Sed eget est nisl. Curabitur at sapien in tellus dapibus feugiat. Maecenas dolor sem, volutpat id tempus ut, iaculis a dolor.", "https://www.logolynx.com/images/logolynx/3b/3b4e5f16f0ccd5f02f4c3f5fa68031e9.jpeg", "Super Doggies" });

            migrationBuilder.InsertData(
                table: "Donations",
                columns: new[] { "Id", "DateTime", "Description", "Goal", "PhotoURL", "ShelterId", "State", "Title" },
                values: new object[] { new Guid("5af6bbb5-a2ae-433a-b999-d7b3891eb51b"), new DateTime(2021, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Donate cute dog lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean malesuada lacus tortor, at rutrum ante dictum nec. Proin sodales porttitor lorem ut dictum. Pellentesque fringilla vulputate luctus. Nam sollicitudin risus id libero commodo aliquam. Duis ut feugiat neque. In elementum turpis quis odio ultrices, eget fermentum elit malesuada. Ut sit amet imperdiet massa. Suspendisse urna nisl, cursus eu pharetra eget, tincidunt ut neque.", 1000, "https://www.expats.cz/images/publishing/articles/2019/11/1280_650/go-cuddle-in-kadlin-handipet-rescue-animal-shelter-is-seeking-volunteers-jpg-xpmac.jpg", new Guid("7600763f-6a2e-482c-9ded-fa9a824376e5"), 200, "Doggie donation" });

            migrationBuilder.InsertData(
                table: "Volunteerings",
                columns: new[] { "Id", "DateTime", "Description", "PhotoURL", "RequiredCount", "ShelterId", "Title" },
                values: new object[] { new Guid("9a2625c7-9008-403f-8d0e-c5257a5e9af5"), new DateTime(2020, 4, 12, 10, 34, 42, 0, DateTimeKind.Unspecified), "Help as volunteer", "https://i.ebayimg.com/images/g/hXoAAOSwQnpgblRI/s-l300.jpg", 4, new Guid("7600763f-6a2e-482c-9ded-fa9a824376e5"), "Volunteer Super Doggies" });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "Id", "DateTime", "VolunteerId", "VolunteeringId" },
                values: new object[] { new Guid("b9f0731b-3b02-48a2-88ce-7d7fd48a4d2a"), new DateTime(2021, 11, 20, 17, 26, 50, 0, DateTimeKind.Unspecified), new Guid("952f40f0-8181-4cc6-aff8-d932e002d98f"), new Guid("9a2625c7-9008-403f-8d0e-c5257a5e9af5") });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "DateTime", "DonationId", "Sum", "VolunteerId" },
                values: new object[] { new Guid("7600763f-6a2e-482c-9ded-fa9a824376e5"), new DateTime(2021, 10, 21, 11, 20, 15, 0, DateTimeKind.Unspecified), new Guid("5af6bbb5-a2ae-433a-b999-d7b3891eb51b"), 100, new Guid("952f40f0-8181-4cc6-aff8-d932e002d98f") });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_ShelterId",
                table: "Donations",
                column: "ShelterId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_VolunteerId",
                table: "Enrollments",
                column: "VolunteerId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_VolunteeringId",
                table: "Enrollments",
                column: "VolunteeringId");

            migrationBuilder.CreateIndex(
                name: "IX_Shelters_AdminId",
                table: "Shelters",
                column: "AdminId",
                unique: true,
                filter: "[AdminId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DonationId",
                table: "Transactions",
                column: "DonationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_VolunteerId",
                table: "Transactions",
                column: "VolunteerId");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteerings_ShelterId",
                table: "Volunteerings",
                column: "ShelterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Volunteerings");

            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropTable(
                name: "Volunteers");

            migrationBuilder.DropTable(
                name: "Shelters");

            migrationBuilder.DropTable(
                name: "ShelterAdmins");
        }
    }
}
