using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayFab.ClientModels;


class ItemCatalog
{
    public bool isLoaded { get { return catalog.Count > 0; }  }

    private static Dictionary<string, CatalogItem> catalog;

    public ItemCatalog()
    {
        catalog = new Dictionary<string, CatalogItem>();
    }

    public void ClearItems()
    {
        catalog.Clear();
    }
    
    public void LoadItems(List<CatalogItem> catalogItems)
    {
        catalog = catalogItems.ToDictionary((item) => item.ItemId);
    }

    public CatalogItem GetCatalogItem(string itemId)
    {
        return catalog[itemId];
    }
}

