namespace XPRTZ.Webshop.ProductService.Consumers;

using System.Threading.Tasks;
using MassTransit;
using XPRTZ.Webshop.Models.Product;
using XPRTZ.Webshop.ProductService.Models;

internal class ProductDetailsRequestConsumer(IEnumerable<Product> products) : IConsumer<ProductDetailsRequest>
{
    public async Task Consume(ConsumeContext<ProductDetailsRequest> context)
    {
        var product = products.FirstOrDefault(p => p.EAN == context.Message.ProductEAN);

        if (product is null)
        {
            await context.RespondAsync(new ProductNotFoundError(context.Message.ProductEAN));
        }
        else
        {
            await context.RespondAsync(new ProductDetailsResponse(
                product.EAN,
                product.Name,
                product.Description,
                product.Price,
                product.Reviews));
        }
    }
}
