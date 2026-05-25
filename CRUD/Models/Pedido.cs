namespace CRUD.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Estado { get; set; } = "Pendiente";

        public User? Usuario { get; set; }
        public List<PedidoDetalle> Detalles { get; set; } = new();
    }
}
