namespace CRUD.Models
{
    public class OrdenCompra
    {
        public int Id { get; set; }
        public int ProveedorId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime? FechaEntregaReal { get; set; }
        public decimal Total { get; set; }
        public string? NumeroFactura { get; set; }
        public string Estado { get; set; } = "Pendiente";

        public Proveedor? Proveedor { get; set; }
        public User? Usuario { get; set; }
    }
}
