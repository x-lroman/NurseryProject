using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendWebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Core");

            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.CreateTable(
                name: "Expedientes",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoExpediente = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Responsable = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TipoCultivo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Variedad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Origen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Certificacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaActualizado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expedientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimoAcceso = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ventanas",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreVentana = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ruta = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Icono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventanas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lotes",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdExpediente = table.Column<int>(type: "int", nullable: false),
                    CodigoLote = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaActualizado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lotes_Expedientes_IdExpediente",
                        column: x => x.IdExpediente,
                        principalSchema: "Core",
                        principalTable: "Expedientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosRoles",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuariosRoles_Roles_IdRol",
                        column: x => x.IdRol,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosRoles_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalSchema: "Identity",
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    IdVentana = table.Column<int>(type: "int", nullable: false),
                    PuedeVer = table.Column<bool>(type: "bit", nullable: false),
                    PuedeCrear = table.Column<bool>(type: "bit", nullable: false),
                    PuedeEditar = table.Column<bool>(type: "bit", nullable: false),
                    PuedeEliminar = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permisos_Roles_IdRol",
                        column: x => x.IdRol,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permisos_Ventanas_IdVentana",
                        column: x => x.IdVentana,
                        principalSchema: "Identity",
                        principalTable: "Ventanas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contenedores",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdLote = table.Column<int>(type: "int", nullable: false),
                    CodigoContenedor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaActualizado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contenedores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contenedores_Lotes_IdLote",
                        column: x => x.IdLote,
                        principalSchema: "Core",
                        principalTable: "Lotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paletes",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdContenedor = table.Column<int>(type: "int", nullable: false),
                    CodigoPalete = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaActualizado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paletes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paletes_Contenedores_IdContenedor",
                        column: x => x.IdContenedor,
                        principalSchema: "Core",
                        principalTable: "Contenedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cajas",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPalete = table.Column<int>(type: "int", nullable: false),
                    CodigoCaja = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaActualizado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cajas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cajas_Paletes_IdPalete",
                        column: x => x.IdPalete,
                        principalSchema: "Core",
                        principalTable: "Paletes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BandejasObs",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCaja = table.Column<int>(type: "int", nullable: false),
                    CodigoBandeja = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaActualizado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BandejasObs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BandejasObs_Cajas_IdCaja",
                        column: x => x.IdCaja,
                        principalSchema: "Core",
                        principalTable: "Cajas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlantasObs",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdBandeja = table.Column<int>(type: "int", nullable: false),
                    CodigoPlanta = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreadoPor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaCreado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualizadoPor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FechaActualizado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantasObs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantasObs_BandejasObs_IdBandeja",
                        column: x => x.IdBandeja,
                        principalSchema: "Core",
                        principalTable: "BandejasObs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BandejasObs_IdCaja",
                schema: "Core",
                table: "BandejasObs",
                column: "IdCaja");

            migrationBuilder.CreateIndex(
                name: "IX_Cajas_IdPalete",
                schema: "Core",
                table: "Cajas",
                column: "IdPalete");

            migrationBuilder.CreateIndex(
                name: "IX_Contenedores_IdLote",
                schema: "Core",
                table: "Contenedores",
                column: "IdLote");

            migrationBuilder.CreateIndex(
                name: "IX_Lotes_IdExpediente",
                schema: "Core",
                table: "Lotes",
                column: "IdExpediente");

            migrationBuilder.CreateIndex(
                name: "IX_Paletes_IdContenedor",
                schema: "Core",
                table: "Paletes",
                column: "IdContenedor");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_IdRol",
                schema: "Identity",
                table: "Permisos",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_IdVentana",
                schema: "Identity",
                table: "Permisos",
                column: "IdVentana");

            migrationBuilder.CreateIndex(
                name: "IX_PlantasObs_IdBandeja",
                schema: "Core",
                table: "PlantasObs",
                column: "IdBandeja");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosRoles_IdRol",
                schema: "Identity",
                table: "UsuariosRoles",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosRoles_IdUsuario",
                schema: "Identity",
                table: "UsuariosRoles",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permisos",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "PlantasObs",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "UsuariosRoles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Ventanas",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "BandejasObs",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Usuarios",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Cajas",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "Paletes",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "Contenedores",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "Lotes",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "Expedientes",
                schema: "Core");
        }
    }
}
