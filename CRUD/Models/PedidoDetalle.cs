namespace CRUD.Models
{
    public class PedidoDetalle
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ArticuloId { get; set; }
        public decimal Cantidad { get; set; }

        public Pedido? Pedido { get; set; }
        public Articulo? Articulo { get; set; }
    }
}
