namespace BackendWebApi.Data;

using BackendWebApi.Models.Core;
using BackendWebApi.Models.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Identity Schema
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Ventana> Ventanas { get; set; }
    public DbSet<UsuarioRol> UsuariosRoles { get; set; }
    public DbSet<Permiso> Permisos { get; set; }

    // Core Schema
    public DbSet<Expediente> Expedientes { get; set; }
    public DbSet<Lote> Lotes { get; set; }
    public DbSet<Contenedor> Contenedores { get; set; }
    public DbSet<Palete> Paletes { get; set; }
    public DbSet<Caja> Cajas { get; set; }
    public DbSet<BandejaObs> BandejasObs { get; set; }
    public DbSet<PlantaObs> PlantasObs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Identity Schema
        modelBuilder.Entity<Usuario>().ToTable("Usuarios", "Identity");
        modelBuilder.Entity<Rol>().ToTable("Roles", "Identity");
        modelBuilder.Entity<Ventana>().ToTable("Ventanas", "Identity");
        modelBuilder.Entity<UsuarioRol>().ToTable("UsuariosRoles", "Identity");
        modelBuilder.Entity<Permiso>().ToTable("Permisos", "Identity");

        // Core Schema
        modelBuilder.Entity<Expediente>().ToTable("Expedientes", "Core");
        modelBuilder.Entity<Lote>().ToTable("Lotes", "Core");
        modelBuilder.Entity<Contenedor>().ToTable("Contenedores", "Core");
        modelBuilder.Entity<Palete>().ToTable("Paletes", "Core");
        modelBuilder.Entity<Caja>().ToTable("Cajas", "Core");
        modelBuilder.Entity<BandejaObs>().ToTable("BandejasObs", "Core");
        modelBuilder.Entity<PlantaObs>().ToTable("PlantasObs", "Core");

        base.OnModelCreating(modelBuilder);
    }
}