namespace XPRTZ.Webshop.StockService.Models;

internal class ProductStock(string ean, int initialStock)
{
    public string EAN => ean;

    public int Stock
    {
        get => initialStock;
        private set => initialStock = value;
    }

    public void UpdateStock(int newStock)
    {
        Stock = newStock;
    }
}
