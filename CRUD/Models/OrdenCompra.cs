namespace CRUD.Models
{
    public class OrdenCompra
    {
        public int Id { get; set; }
        public int PedidoId { get; set; } // Enlace al Pedido original
        public int ProveedorId { get; set; }
        public int UsuarioId { get; set; }

        public DateTime FechaEstimadaEntrega { get; set; } // Para la tabla principal
        public DateTime? FechaEntregaReal { get; set; }    // Se llena en la recepción

        public decimal Total { get; set; }
        public string? NumeroFactura { get; set; }
        public string Estado { get; set; } = "Pendiente";

        // Propiedades de navegación
        public Proveedor? Proveedor { get; set; }
        public User? Usuario { get; set; }
        // Si tenés el modelo Pedido, podés agregar: public Pedido? Pedido { get; set; }
        public ICollection<OrdenCompraDetalle> Detalles { get; set; } = new List<OrdenCompraDetalle>();
    }
}