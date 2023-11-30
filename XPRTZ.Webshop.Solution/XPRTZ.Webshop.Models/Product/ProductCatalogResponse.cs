namespace XPRTZ.Webshop.Models.Product;

using System.Collections.Generic;

public record ProductCatalogResponse(IEnumerable<ProductCatalogItem> Products);
