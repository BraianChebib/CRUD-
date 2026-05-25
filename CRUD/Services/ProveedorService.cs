using CRUD.Data;
using CRUD.Models;

namespace CRUD.Services
{
    public class ProveedorService
    {
        private readonly AppDbContext _context;

        public ProveedorService(AppDbContext context)
        {
            _context = context;
        }

        public List<Proveedor> GetAll()
        {
            return _context.Proveedores.ToList();
        }

        public Proveedor Create(Proveedor proveedor)
        {
            _context.Proveedores.Add(proveedor);
            _context.SaveChanges();
            return proveedor;
        }

        public Proveedor? Update(int id, Proveedor updatedProveedor)
        {
            var proveedor = _context.Proveedores.Find(id);
            if (proveedor == null) return null;

            proveedor.Nombre = updatedProveedor.Nombre;
            proveedor.RazonSocial = updatedProveedor.RazonSocial;
            proveedor.Cuit = updatedProveedor.Cuit;
            proveedor.Mail = updatedProveedor.Mail;
            proveedor.Telefono = updatedProveedor.Telefono;

            _context.SaveChanges();
            return proveedor;
        }

        public bool Delete(int id)
        {
            var proveedor = _context.Proveedores.Find(id);
            if (proveedor == null) return false;

            _context.Proveedores.Remove(proveedor);
            _context.SaveChanges();
            return true;
        }
    }
}
