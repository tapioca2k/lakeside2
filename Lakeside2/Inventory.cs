using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.Json;

namespace Lakeside2
{
    public static class Inventory
    {

        static List<Item> items;

        static Inventory()
        {
            string json = File.ReadAllText("Content/items.json");
            items = JsonSerializer.Deserialize<List<Item>>(json);
        }

        public static Item getItem(string name)
        {
            IEnumerable<Item> i = from item in items 
                                  where item.name == name 
                                  select item;
            return i.FirstOrDefault();
        }
    }
}
