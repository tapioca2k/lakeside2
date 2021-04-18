using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.Json;

namespace Lakeside2
{
    static class Inventory
    {
        class Cookbook
        {
            public List<Item> items { get; set; }
        }

        static Cookbook items;

        static Inventory()
        {
            string json = File.ReadAllText("Content/items.json");
            items = JsonSerializer.Deserialize<Cookbook>(json);
        }

        public static Item getItem(string name)
        {
            IEnumerable<Item> i = from item in items.items 
                                  where item.name == name 
                                  select item;
            return i.FirstOrDefault();
        }
    }
}
