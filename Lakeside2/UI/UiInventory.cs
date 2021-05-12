using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lakeside2.UI
{
    class UiInventory : UiList
    {
        static string[] COMMANDS = new string[3] { "Use", "Examine", "Discard" };
        static Vector2 COMMAND_LOCATION = new Vector2(Tile.TILE_SIZE * 10, Tile.TILE_SIZE);

        ContentManager Content;
        Player player;

        string[] unformatted;

        public UiInventory(ContentManager Content, Player player)
            : base(Content, new string[0])
        {
            this.Content = Content;
            this.player = player;

            resetMenu();

            this.addCallback(element =>
            {
                if (this.selected != -1)
                {
                    if (this.unformatted.Length == 0) return; // "you have no items" placeholder
                    Item selectedItem = Inventory.getItem(unformatted[this.selected]);
                    this.finished = false;

                    system.pushElement(new UiList(Content, COMMANDS).addCallback(command =>
                    {
                        UiList commandList = (UiList)command;
                        switch (commandList.selected)
                        {
                            case 0: break; // TODO Use item
                            case 1: // Examine
                                {
                                    system.pushElement(new UiTextBox(selectedItem.description), Vector2.Zero);
                                    break;
                                }
                            case 2: // Discard
                                {
                                    system.pushElement(new UiOptionBox(Content, "Really discard this?", "No", "Yes").addCallback(o =>
                                    {
                                        UiOptionBox option = (UiOptionBox)o;
                                        if (option.selected == 1)
                                        {
                                            player.setItemCount(selectedItem.name, 0);
                                            this.resetMenu();
                                        }
                                    }), Vector2.Zero);
                                    break;
                                }
                        }
                    }), COMMAND_LOCATION);
                }
            });
        }

        void resetMenu()
        {
            unformatted = getUnformatted(player.getInventory());
            this.setStrings(formatItemList(player, unformatted));
            this.selected = 0;
        }

        string[] getUnformatted(Dictionary<Item, int> inventory)
        {
            return new List<string>(from item in inventory.Keys select item.name).ToArray();
        }

        string[] formatItemList(Player player, string[] itemNames)
        {
            string[] formatted = new string[Math.Max(1, itemNames.Length)];
            if (itemNames.Length == 0)
            {
                formatted[0] = "You have no items.";
            }
            else
            {
                for (int i = 0; i < itemNames.Length; i++)
                {
                    formatted[i] = itemNames[i] + " x" + player.getItemCount(itemNames[i]);
                }
            }

            return formatted;
        } 

    }
}
