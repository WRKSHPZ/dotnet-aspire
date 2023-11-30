// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XPRTZ.Webshop.API.Controllers;

using System.Threading;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using XPRTZ.Webshop.API.Models;
using XPRTZ.Webshop.Models.Product;
using XPRTZ.Webshop.Models.Stock;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(
    IRequestClient<ProductCatalogRequest> productCatalogRequestClient,
    IRequestClient<ProductDetailsRequest> productDetailsRequestClient,
    IRequestClient<ProductStockRequest> productStockRequestClient) 
    : ControllerBase
{
    // GET: api/<ProductsController>
    [HttpGet]
    public async Task<IEnumerable<ProductInformation>> Get(CancellationToken cancellationToken)
    {
        var catalogResponse = await productCatalogRequestClient.GetResponse<ProductCatalogResponse>(new ProductCatalogRequest(), cancellationToken);

        var stockResponse = await productStockRequestClient.GetResponse<ProductStockResponse>(new ProductStockRequest(catalogResponse.Message.Products.Select(p => p.EAN)), cancellationToken);

        var productInformation = from catalogItem in catalogResponse.Message.Products
                                 join stockInformation in stockResponse.Message.StockInformation
                                 on catalogItem.EAN equals stockInformation.EAN
                                 select new ProductInformation(catalogItem.EAN, catalogItem.Name, catalogItem.Price, stockInformation.Stock);

        return productInformation;
    }

    // GET api/<ProductsController>/5
    [HttpGet("{ean}")]
    public async Task<ProductDetailsResponse?> Get(string ean, CancellationToken cancellationToken)
    {
        var response = await productDetailsRequestClient.GetResponse<ProductDetailsResponse, ProductNotFoundError>(new ProductDetailsRequest(ean), cancellationToken);

        return response.Is(out Response<ProductDetailsResponse>? productDetailsResponse) ? productDetailsResponse.Message : null;
    }
}
