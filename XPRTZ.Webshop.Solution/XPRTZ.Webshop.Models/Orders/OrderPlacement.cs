namespace XPRTZ.Webshop.Models.Orders;

using System.Collections.Generic;

public record OrderPlacement(IEnumerable<string> ProductEANs);
