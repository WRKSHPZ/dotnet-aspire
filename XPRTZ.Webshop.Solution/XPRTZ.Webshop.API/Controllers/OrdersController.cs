// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XPRTZ.Webshop.API.Controllers;

using MassTransit;
using Microsoft.AspNetCore.Mvc;
using XPRTZ.Webshop.Models.Orders;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(IRequestClient<OrderPlacement> orderPlacementRequestClient) : ControllerBase
{
    [HttpPost]
    public async Task<IEnumerable<string>> Post([FromBody] IEnumerable<string> productEANs)
    {
        var result = await orderPlacementRequestClient.GetResponse<OrderConfirmation, OrderPlacementFailed>(new OrderPlacement(productEANs));

        if (result.Is(out Response<OrderConfirmation>? orderConfirmation ))
        {
            return ["Order placed successfully"];
        }

        if (result.Is(out Response<OrderPlacementFailed>? orderPlacementFailed))
        {
            return orderPlacementFailed.Message.Reasons;
        }

        return Array.Empty<string>();
    }
}
