using Microsoft.EntityFrameworkCore;
using Veterinaria.Entidades.Entidades;

namespace Veterinaria.Datos.Context;

public partial class VeterinariaContext(DbContextOptions<VeterinariaContext> options) : DbContext(options)
{
    public virtual DbSet<Mascota> Mascotas { get; set; }
    public virtual DbSet<Propietario> Propietarios { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mascota>(entity =>
        {
            entity.HasKey(e => e.IdMascota);
            entity.HasIndex(e => e.IdPropietario, "IX_Mascotas_IdPropietario");
            entity.Property(e => e.Nombre).HasMaxLength(80);
            entity.Property(e => e.Especie).HasMaxLength(50);
            entity.Property(e => e.Raza).HasMaxLength(80);
            entity.Property(e => e.Sexo).HasMaxLength(1).IsUnicode(false).IsFixedLength();
            entity.Property(e => e.Peso).HasColumnType("decimal(6, 2)");
            entity.HasOne(e => e.Propietario)
                .WithMany(e => e.Mascotas)
                .HasForeignKey(e => e.IdPropietario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mascotas_Propietarios");
        });

        modelBuilder.Entity<Propietario>(entity =>
        {
            entity.HasKey(e => e.IdPropietario);
            entity.HasIndex(e => e.Dni, "UQ_Propietarios_Dni").IsUnique();
            entity.Property(e => e.Nombres).HasMaxLength(80);
            entity.Property(e => e.Apellidos).HasMaxLength(80);
            entity.Property(e => e.Dni).HasMaxLength(8).IsUnicode(false).IsFixedLength();
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.Correo).HasMaxLength(120);
            entity.Property(e => e.Direccion).HasMaxLength(200);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);
            entity.HasIndex(e => e.Correo, "UQ_Usuarios_Correo").IsUnique();
            entity.Property(e => e.Correo).HasMaxLength(120);
            entity.Property(e => e.ClaveHash).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.Rol).HasMaxLength(30);
        });
    }
}
