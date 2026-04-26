namespace Inventory.Application.Common.Utils
{
    public static class StockUtil
    {
        public static void ReduceStock(dynamic entity, int quantity)
        {
            if (entity.Stock > quantity)
                entity.Stock -= quantity;
            else
                throw new InvalidOperationException("Insufficient stock for the exit movement.");
        }

        public static void AddStock(dynamic entity, int quantity)
        {
            entity.Stock += quantity;
        }
    }
}
