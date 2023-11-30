namespace XPRTZ.Webshop.StockService.Consumers;

using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;
using XPRTZ.Webshop.Models.Stock;
using XPRTZ.Webshop.StockService.Models;

internal class ProductStockRequestConsumer(IEnumerable<ProductStock> products) : IConsumer<ProductStockRequest>
{
    public async Task Consume(ConsumeContext<ProductStockRequest> context)
    {
        var stockInformation =
            from ean in context.Message.ProductEANCodes
            join product in products
            on ean equals product.EAN
            into g
            from productStock in g.DefaultIfEmpty()
            select new StockInformation(ean, productStock?.Stock ?? 0);

        await context.RespondAsync(new ProductStockResponse(stockInformation));
    }
}
