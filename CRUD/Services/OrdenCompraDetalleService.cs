using CRUD.Data;
using CRUD.Models;
using System.Collections.Generic;
using System.Linq;

namespace CRUD.Services
{
    public class OrdenCompraDetalleService
    {
        private readonly AppDbContext _context;

        public OrdenCompraDetalleService(AppDbContext context)
        {
            _context = context;
        }

        public List<OrdenCompraDetalle> GetAll()
        {
            return _context.OrdenCompraDetalles.ToList();
        }

        public OrdenCompraDetalle Create(OrdenCompraDetalle detalle)
        {
            _context.OrdenCompraDetalles.Add(detalle);
            _context.SaveChanges();
            return detalle;
        }

        public OrdenCompraDetalle? Update(int id, OrdenCompraDetalle detalle)
        {
            var existente = _context.OrdenCompraDetalles.Find(id);
            if (existente == null) return null;

            existente.OrdenCompraId = detalle.OrdenCompraId;
            existente.ArticuloId = detalle.ArticuloId;
            existente.Cantidad = detalle.Cantidad;
            existente.PrecioUnitario = detalle.PrecioUnitario;

            _context.SaveChanges();
            return existente;
        }

        public bool Delete(int id)
        {
            var existente = _context.OrdenCompraDetalles.Find(id);
            if (existente == null) return false;

            _context.OrdenCompraDetalles.Remove(existente);
            _context.SaveChanges();
            return true;
        }
    }
}
