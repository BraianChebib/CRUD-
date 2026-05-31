namespace CRUD.Models 
{
    public class User  
    {
        public int Id {get; set;}  
        public string Nombre {get; set;} = string.Empty;
        public string Dni {get; set;} = string.Empty;
        public string Rol {get; set;} = string.Empty;
        public string Clave { get; set; } = string.Empty;

        public List<Pedido> Pedidos {get; set;} = new();
        public List<OrdenCompra> OrdenesCompra {get; set;} = new();
    }
}
