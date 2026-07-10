using System;
using System.Collections.Generic;

namespace CRUD.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Estado { get; set; } = "Pendiente";
        public DateTime? UltimaModificacion { get; set; }

        public User? Usuario { get; set; }
        public List<PedidoDetalle> Detalles { get; set; } = new();
    }

    // 🔑 AGREGADO: Clases auxiliares para recibir los datos desde el JavaScript (Fetch)
    public class CrearPedidoDTO
    {
        public int UsuarioId { get; set; }
        public List<DetallePedidoDTO> Detalles { get; set; } = new List<DetallePedidoDTO>();
    }

    public class DetallePedidoDTO
    {
        public int ArticuloId { get; set; }
        public decimal Cantidad { get; set; }
    }
}