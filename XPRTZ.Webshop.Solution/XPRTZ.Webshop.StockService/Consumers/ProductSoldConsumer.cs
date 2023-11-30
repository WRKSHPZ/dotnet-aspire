namespace XPRTZ.Webshop.StockService.Consumers;

using System.Threading.Tasks;
using MassTransit;
using XPRTZ.Webshop.Models.Product;
using XPRTZ.Webshop.StockService.Models;

internal class ProductSoldConsumer(IEnumerable<ProductStock> products) : IConsumer<ProductSold>
{
    public Task Consume(ConsumeContext<ProductSold> context)
    {
        var product = products.FirstOrDefault(p => p.EAN == context.Message.EAN);

        product?.UpdateStock(product.Stock - 1);

        return Task.CompletedTask;
    }
}
