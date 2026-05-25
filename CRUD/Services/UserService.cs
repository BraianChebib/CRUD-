using CRUD.Data;
using CRUD.Models;

namespace CRUD.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)  
        {
            _context = context;                  
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();  
        }

        public User Create(User user)
        {
            _context.Users.Add(user);  
            _context.SaveChanges();     
            return user;
        }

        public User? Update(int id, User updatedUser)
        {
            var user = _context.Users.Find(id);
            if (user == null) return null;       

            user.Nombre = updatedUser.Nombre;
            user.Dni = updatedUser.Dni;
            user.Rol = updatedUser.Rol;

            _context.SaveChanges();
            return user;
        }

        public bool Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}
