namespace XPRTZ.Webshop.ProductService.Models;

public record Product(string EAN, string Name, string Description, decimal Price, int Stock, IEnumerable<string> Reviews);
