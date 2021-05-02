using Lakeside2.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    class UiPauseMenu : UiList
    {
        public UiPauseMenu(ContentManager Content, Player player, string map, bool overworld)
            : base(Content, new string[4] { "Resume", "Save", "Options", "Quit"})
        {
            addCallback(element =>
            {
                UiList list = (UiList)element;
                switch (list.selected)
                {
                    case 1: // save
                        {
                            element.finished = false; // don't close the menu yet
                            system.pushElement(new UiSavePicker(Content, true).addCallback(element2 =>
                            {
                                UiSavePicker savePicker = (UiSavePicker)element2;
                                string filename = savePicker.selectedString;
                                if (filename == null) return;
                                else if (filename == UiSavePicker.CREATE_NEW_FILE)
                                    filename = "save" + savePicker.GetHashCode(); // create new file name
                                SaveGame.Save(filename + ".json", player, map, overworld);
                                element.finished = true; // actually, do the close menu
                            }), new Vector2(Tile.TILE_SIZE * 10, Tile.TILE_SIZE));
                            break;
                        }
                    case 2: // TODO options
                        {
                            element.finished = false;
                            break;
                        }
                    case 3: // TODO quit
                        {
                            break;
                        }
                }
            });
        }
    }

}
