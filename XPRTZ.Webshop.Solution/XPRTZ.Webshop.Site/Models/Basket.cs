namespace XPRTZ.Webshop.Site.Models;

using XPRTZ.Webshop.Models.Product;

public class Basket
{
    public List<ProductDetailsResponse> Products { get; } = new List<ProductDetailsResponse>();
}
