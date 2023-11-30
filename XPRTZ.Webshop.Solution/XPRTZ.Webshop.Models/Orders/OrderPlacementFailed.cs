namespace XPRTZ.Webshop.Models.Orders;

using System.Collections.Generic;

public record OrderPlacementFailed(IEnumerable<string> Reasons);
