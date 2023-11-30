namespace XPRTZ.Webshop.OrderService.Consumers;

using System.Threading.Tasks;
using MassTransit;
using XPRTZ.Webshop.Models.Orders;
using XPRTZ.Webshop.Models.Product;
using XPRTZ.Webshop.Models.Stock;

internal class OrderPlacementConsumer(
    IRequestClient<ProductStockRequest> productStockRequestClient) 
    : IConsumer<OrderPlacement>
{
    public async Task Consume(ConsumeContext<OrderPlacement> context)
    {
        var responses = await productStockRequestClient.GetResponse<ProductStockResponse>(new ProductStockRequest(context.Message.ProductEANs));

        var outOfStockProducts = responses.Message.StockInformation.Where(x => x.Stock <= 0);

        if (outOfStockProducts.Any())
        {
            await context.RespondAsync(new OrderPlacementFailed(outOfStockProducts.Select(x => $"{x.EAN} has a stock of {x.Stock}")));
        }
        else
        {
            await Task.WhenAll(context.Message.ProductEANs.Select(ean => context.Publish(new ProductSold(ean))));

            await context.RespondAsync(new OrderConfirmation());
        }
    }
}
