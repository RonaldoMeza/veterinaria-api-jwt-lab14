using Microsoft.EntityFrameworkCore;
using Veterinaria.Entidades;
namespace Veterinaria.Datos;
public partial class VeterinariaContext(DbContextOptions<VeterinariaContext> options) : DbContext(options)
{
 public virtual DbSet<Mascota> Mascotas { get; set; }
 public virtual DbSet<Propietario> Propietarios { get; set; }
 protected override void OnModelCreating(ModelBuilder b) { b.Entity<Mascota>(e=>{e.HasKey(x=>x.IdMascota);e.HasIndex(x=>x.IdPropietario,"IX_Mascotas_IdPropietario");e.Property(x=>x.Nombre).HasMaxLength(80);e.Property(x=>x.Especie).HasMaxLength(50);e.Property(x=>x.Raza).HasMaxLength(80);e.Property(x=>x.Sexo).HasMaxLength(1).IsUnicode(false).IsFixedLength();e.Property(x=>x.Peso).HasColumnType("decimal(6, 2)");e.HasOne(x=>x.Propietario).WithMany(x=>x.Mascotas).HasForeignKey(x=>x.IdPropietario).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Mascotas_Propietarios");}); b.Entity<Propietario>(e=>{e.HasKey(x=>x.IdPropietario);e.HasIndex(x=>x.Dni,"UQ_Propietarios_Dni").IsUnique();e.Property(x=>x.Nombres).HasMaxLength(80);e.Property(x=>x.Apellidos).HasMaxLength(80);e.Property(x=>x.Dni).HasMaxLength(8).IsUnicode(false).IsFixedLength();e.Property(x=>x.Telefono).HasMaxLength(20);e.Property(x=>x.Correo).HasMaxLength(120);e.Property(x=>x.Direccion).HasMaxLength(200);}); }
}
