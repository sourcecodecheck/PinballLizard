using System.Collections.Generic;
using System.Linq;
using PlayFab.ClientModels;


/// <summary>
/// Class for storing contents of catalog
/// </summary>
public static class ItemCatalog
{
    public static bool isLoaded { get { return catalog.Count > 0; }  }

    private static Dictionary<string, CatalogItem> catalog = new Dictionary<string, CatalogItem>();

    public static void ClearItems()
    {
        catalog.Clear();
    }
    
    public static void LoadItems(List<CatalogItem> catalogItems)
    {
        catalog = catalogItems.ToDictionary((item) => item.ItemId);
    }

    public static CatalogItem GetCatalogItem(string itemId)
    {
        return catalog[itemId];
    }

    public static CatalogItem SearchCatalog(string term)
    {
        return catalog.Where((pair) => pair.Key.ToLower().Contains(term)).FirstOrDefault().Value;
    }
}

