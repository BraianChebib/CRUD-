using CRUD.Data;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Services
{
    public class PedidoDetalleService
    {
        private readonly AppDbContext _context;

        public PedidoDetalleService(AppDbContext context)
        {
            _context = context;
        }

        public List<PedidoDetalle> GetAll()
        {
            return _context.PedidoDetalles
                .Include(detalle => detalle.Pedido)
                .Include(detalle => detalle.Articulo)
                .ToList();
        }

        public PedidoDetalle Create(PedidoDetalle detalle)
        {
            _context.PedidoDetalles.Add(detalle);
            _context.SaveChanges();
            return detalle;
        }

        public PedidoDetalle? Update(int id, PedidoDetalle updatedDetalle)
        {
            var detalle = _context.PedidoDetalles.Find(id);
            if (detalle == null) return null;

            detalle.PedidoId = updatedDetalle.PedidoId;
            detalle.ArticuloId = updatedDetalle.ArticuloId;
            detalle.Cantidad = updatedDetalle.Cantidad;

            _context.SaveChanges();
            return detalle;
        }

        public bool Delete(int id)
        {
            var detalle = _context.PedidoDetalles.Find(id);
            if (detalle == null) return false;

            _context.PedidoDetalles.Remove(detalle);
            _context.SaveChanges();
            return true;
        }
    }
}
