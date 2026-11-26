using CRUD.Data;
using CRUD.Models;
using System.Collections.Generic;
using System.Linq;

namespace CRUD.Services
{
    public class CotizacionService
    {
        private readonly AppDbContext _context;

        public CotizacionService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Cotizacion> GetByPedidoId(int pedidoId)
        {
            return _context.Cotizaciones.Where(c => c.PedidoId == pedidoId).ToList();
        }

        public Cotizacion Create(Cotizacion cotizacion)
        {
            _context.Cotizaciones.Add(cotizacion);
            _context.SaveChanges();
            return cotizacion;
        }
    }
}