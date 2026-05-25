using CRUD.Data;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Services
{
    public class PedidoService
    {
        private readonly AppDbContext _context;

        public PedidoService(AppDbContext context)
        {
            _context = context;
        }

        public List<Pedido> GetAll()
        {
            return _context.Pedidos
                .Include(pedido => pedido.Usuario)
                .Include(pedido => pedido.Detalles)
                .ThenInclude(detalle => detalle.Articulo)
                .ToList();
        }

        public Pedido? GetById(int id)
        {
            return _context.Pedidos
                .Include(pedido => pedido.Usuario)
                .Include(pedido => pedido.Detalles)
                .ThenInclude(detalle => detalle.Articulo)
                .FirstOrDefault(pedido => pedido.Id == id);
        }

        public Pedido Create(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            _context.SaveChanges();
            return pedido;
        }

        public Pedido? Update(int id, Pedido updatedPedido)
        {
            var pedido = _context.Pedidos.Find(id);
            if (pedido == null) return null;

            pedido.UsuarioId = updatedPedido.UsuarioId;
            pedido.Fecha = updatedPedido.Fecha;
            pedido.Estado = updatedPedido.Estado;

            _context.SaveChanges();
            return pedido;
        }

        public bool Delete(int id)
        {
            var pedido = _context.Pedidos.Find(id);
            if (pedido == null) return false;

            _context.Pedidos.Remove(pedido);
            _context.SaveChanges();
            return true;
        }
    }
}
