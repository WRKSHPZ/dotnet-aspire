// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XPRTZ.Webshop.API.Controllers;

using System.Threading;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using XPRTZ.Webshop.Models.Product;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(
    IRequestClient<ProductCatalogRequest> productCatalogRequestClient,
    IRequestClient<ProductDetailsRequest> productDetailsRequestClient) 
    : ControllerBase
{
    // GET: api/<ProductsController>
    [HttpGet]
    public async Task<IEnumerable<ProductCatalogItem>> Get(CancellationToken cancellationToken)
    {
        var response = await productCatalogRequestClient.GetResponse<ProductCatalogResponse>(new ProductCatalogRequest(), cancellationToken);
        
        return response.Message.Products;
    }

    // GET api/<ProductsController>/5
    [HttpGet("{ean}")]
    public async Task<ProductDetailsResponse?> Get(string ean, CancellationToken cancellationToken)
    {
        var response = await productDetailsRequestClient.GetResponse<ProductDetailsResponse, ProductNotFoundError>(new ProductDetailsRequest(ean), cancellationToken);

        return response.Is(out Response<ProductDetailsResponse>? productDetailsResponse) ? productDetailsResponse.Message : null;
    }
}
