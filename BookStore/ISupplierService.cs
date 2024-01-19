namespace BookStore
{
    public interface ISupplierService
    {
        DateOnly OrderCopies(string title, int copiesCount);
    }
}