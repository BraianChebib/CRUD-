namespace CRUD.Models
{
    public class OrdenCompraDetalle
    {
        public int Id { get; set; }
        public int OrdenCompraId { get; set; }
        public int ArticuloId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}