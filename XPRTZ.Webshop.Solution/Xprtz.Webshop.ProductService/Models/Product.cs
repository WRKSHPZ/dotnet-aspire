namespace XPRTZ.Webshop.ProductService.Models;

public record Product(string EAN, string Name, string Description, decimal Price, IEnumerable<string> Reviews);
