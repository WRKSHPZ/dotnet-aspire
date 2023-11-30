namespace XPRTZ.Webshop.ProductService.Consumers;

using System.Threading.Tasks;
using MassTransit;
using XPRTZ.Webshop.Models.Product;
using XPRTZ.Webshop.ProductService.Models;

internal class ProductCatalogRequestConsumer(IEnumerable<Product> products) : IConsumer<ProductCatalogRequest>
{
    public async Task Consume(ConsumeContext<ProductCatalogRequest> context)
    {
        await Task.Delay(3000);

        var productCatalogItems = products.Select(p => new ProductCatalogItem(p.EAN, p.Name, p.Price));

        await context.RespondAsync(new ProductCatalogResponse(productCatalogItems));
    }
}
