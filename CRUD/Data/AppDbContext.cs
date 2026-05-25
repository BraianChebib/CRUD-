using Microsoft.EntityFrameworkCore;
using CRUD.Models;

namespace CRUD.Data
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){} 

        public DbSet<User> Users {get; set;}
        public DbSet<Proveedor> Proveedores {get; set;}
        public DbSet<Articulo> Articulos {get; set;}
        public DbSet<Pedido> Pedidos {get; set;}
        public DbSet<PedidoDetalle> PedidoDetalles {get; set;}
        public DbSet<OrdenCompra> OrdenesCompra {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(user => user.Nombre).HasMaxLength(100);
                entity.Property(user => user.Dni).HasMaxLength(20);
                entity.Property(user => user.Rol).HasMaxLength(50);
                entity.HasIndex(user => user.Dni).IsUnique();
            });

            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.Property(proveedor => proveedor.Nombre).HasMaxLength(100);
                entity.Property(proveedor => proveedor.RazonSocial).HasMaxLength(150);
                entity.Property(proveedor => proveedor.Cuit).HasMaxLength(20);
                entity.Property(proveedor => proveedor.Mail).HasMaxLength(150);
                entity.Property(proveedor => proveedor.Telefono).HasMaxLength(30);
                entity.HasIndex(proveedor => proveedor.Cuit).IsUnique();
            });

            modelBuilder.Entity<Articulo>(entity =>
            {
                entity.Property(articulo => articulo.Nombre).HasMaxLength(100);
                entity.Property(articulo => articulo.UnidadMedida).HasMaxLength(50);
                entity.Property(articulo => articulo.Descripcion).HasMaxLength(500);
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.Property(pedido => pedido.Estado).HasMaxLength(30);
            });

            modelBuilder.Entity<Pedido>()
                .HasOne(pedido => pedido.Usuario)
                .WithMany(usuario => usuario.Pedidos)
                .HasForeignKey(pedido => pedido.UsuarioId);

            modelBuilder.Entity<PedidoDetalle>()
                .HasOne(detalle => detalle.Pedido)
                .WithMany(pedido => pedido.Detalles)
                .HasForeignKey(detalle => detalle.PedidoId);

            modelBuilder.Entity<PedidoDetalle>()
                .HasOne(detalle => detalle.Articulo)
                .WithMany(articulo => articulo.PedidoDetalles)
                .HasForeignKey(detalle => detalle.ArticuloId);

            modelBuilder.Entity<PedidoDetalle>()
                .Property(detalle => detalle.Cantidad)
                .HasPrecision(10, 2);

            modelBuilder.Entity<OrdenCompra>()
                .HasOne(orden => orden.Proveedor)
                .WithMany(proveedor => proveedor.OrdenesCompra)
                .HasForeignKey(orden => orden.ProveedorId);

            modelBuilder.Entity<OrdenCompra>()
                .HasOne(orden => orden.Usuario)
                .WithMany(usuario => usuario.OrdenesCompra)
                .HasForeignKey(orden => orden.UsuarioId);

            modelBuilder.Entity<OrdenCompra>(entity =>
            {
                entity.Property(orden => orden.NumeroFactura).HasMaxLength(50);
                entity.Property(orden => orden.Estado).HasMaxLength(30);
            });

            modelBuilder.Entity<OrdenCompra>()
                .Property(orden => orden.Total)
                .HasPrecision(12, 2);
        }
    }
}
