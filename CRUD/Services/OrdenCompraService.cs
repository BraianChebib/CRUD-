using CRUD.Data;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Services
{
    public class OrdenCompraService
    {
        private readonly AppDbContext _context;

        public OrdenCompraService(AppDbContext context)
        {
            _context = context;
        }

        public List<OrdenCompra> GetAll()
        {
            return _context.OrdenesCompra
                .Include(orden => orden.Proveedor)
                .Include(orden => orden.Usuario)
                .ToList();
        }

        public OrdenCompra Create(OrdenCompra orden)
        {
            _context.OrdenesCompra.Add(orden);
            _context.SaveChanges();
            return orden;
        }

        public OrdenCompra? Update(int id, OrdenCompra updatedOrden)
        {
            var orden = _context.OrdenesCompra.Find(id);
            if (orden == null) return null;

            orden.ProveedorId = updatedOrden.ProveedorId;
            orden.UsuarioId = updatedOrden.UsuarioId;
            orden.FechaEntregaReal = updatedOrden.FechaEntregaReal;
            orden.Total = updatedOrden.Total;
            orden.NumeroFactura = updatedOrden.NumeroFactura;
            orden.Estado = updatedOrden.Estado;

            _context.SaveChanges();
            return orden;
        }

        public bool Delete(int id)
        {
            var orden = _context.OrdenesCompra.Find(id);
            if (orden == null) return false;

            _context.OrdenesCompra.Remove(orden);
            _context.SaveChanges();
            return true;
        }
    }
}
