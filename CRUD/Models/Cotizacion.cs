namespace CRUD.Models
{
    public class Cotizacion
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public string NroCotizacion { get; set; } = string.Empty;
        public string RazonSocialProveedor { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public bool TotalidadArticulos { get; set; }
        public string TiempoEstimadoEntrega { get; set; } = string.Empty;

         public Pedido? Pedido { get; set; }
    }
}