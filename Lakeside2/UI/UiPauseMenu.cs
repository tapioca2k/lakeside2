﻿using Lakeside2.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using static Lakeside2.InputBindings;

namespace Lakeside2.UI
{
    class UiPauseMenu : UiList
    {
        Game1 game;
        ContentManager Content;
        Player player;

        public UiPauseMenu(Game1 game, ContentManager Content, Player player, string map, bool overworld)
            : base(Content, new string[4] { "Items", "Save", "Options", "Quit"})
        {
            this.game = game;
            this.Content = Content;
            this.player = player;

            addCallback(element =>
            {
                UiList list = (UiList)element;
                switch (list.selected)
                {
                    case 0: // inventory
                        {
                            element.finished = false;
                            system.pushElement(new UiInventory(Content, player), 
                                new Point(Tile.TILE_SIZE, Tile.TILE_SIZE));
                            break;
                        }
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
                            }), new Point(Tile.TILE_SIZE * 10, Tile.TILE_SIZE));
                            break;
                        }
                    case 2: // TODO options
                        {
                            element.finished = false;
                            system.pushElement(new UiOptionsMenu(game, Content),
                                new Point(
                                    Game1.INTERNAL_WIDTH / 2 - UiOptionsMenu.WIDTH / 2,
                                    Game1.INTERNAL_HEIGHT / 2 - UiOptionsMenu.HEIGHT / 2)
                                );
                            break;
                        }
                    case 3: // TODO quit
                        {
                            system.pushElement(new UiOptionBox(Content, "Quit? Unsaved progress will be lost.", "No", "Yes")
                            .addCallback(element =>
                            {
                                UiOptionBox option = (UiOptionBox)element;
                                if (option.selected == 1)
                                {
                                    game.goToTitle();
                                }
                            }), Point.Zero);
                            break;
                        }
                }
            });
        }

        public override void onInput(InputHandler input)
        {
            base.onInput(input);
            if (input.isCommandPressed(Bindings.Start))
            {
                selected = -1;
                finished = true;
            }
        }
    }

}
