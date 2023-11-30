namespace XPRTZ.Webshop.Models.Product;

public record ProductDetailsResponse(string EAN, string Name, string Description, decimal Price, IEnumerable<string> Reviews);
