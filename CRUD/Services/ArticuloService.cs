using CRUD.Data;
using CRUD.Models;

namespace CRUD.Services
{
    public class ArticuloService
    {
        private readonly AppDbContext _context;

        public ArticuloService(AppDbContext context)
        {
            _context = context;
        }

        public List<Articulo> GetAll()
        {
            return _context.Articulos.ToList();
        }

        public Articulo Create(Articulo articulo)
        {
            _context.Articulos.Add(articulo);
            _context.SaveChanges();
            return articulo;
        }

        public Articulo? Update(int id, Articulo updatedArticulo)
        {
            var articulo = _context.Articulos.Find(id);
            if (articulo == null) return null;

            articulo.Nombre = updatedArticulo.Nombre;
            articulo.UnidadMedida = updatedArticulo.UnidadMedida;
            articulo.Descripcion = updatedArticulo.Descripcion;

            _context.SaveChanges();
            return articulo;
        }

        public bool Delete(int id)
        {
            var articulo = _context.Articulos.Find(id);
            if (articulo == null) return false;

            _context.Articulos.Remove(articulo);
            _context.SaveChanges();
            return true;
        }
    }
}
