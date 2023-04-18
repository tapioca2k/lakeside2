using Lakeside2.Editor;
using Lakeside2.Serialization;
using Lakeside2.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2
{
    class TitleScreen : IGameState
    {
        Game1 game;
        ContentManager Content;
        Texture2D bg;
        UiSystem ui;
        IGameState editor;
        bool editing => editor != null;

        UiList mainList;

        public Color background => Game1.BG_COLOR;

        public TitleScreen(Game1 game, ContentManager Content)
        {
            this.game = game;
            this.Content = Content;
            bg = Content.Load<Texture2D>(GameInfo.titleBackground);
            ui = new UiSystem(false);

            mainList = new UiList(Content, new string[4] { "New Game", "Load Game", "Options", "Quit" });
            mainList.addCallback(element =>
            {
                element.finished = false; // cancel removing the main list
                UiList list = (UiList)element;
                int selected = list.selected;
                switch (selected)
                {
                    case -1: // thwart attempt to close the main menu
                        {
                            list.selected = 0;
                            break;
                        }
                    case 0: // new game
                        {
                            TimeOfDay.restart();
                            Flags.reset();
                            if (GameInfo.startOverworld)
                            {
                                game.goToOverworld(new Player(Content, null, null), GameInfo.startMap);
                            }
                            else
                            {
                                game.goToWorld(new Player(Content, null, null), GameInfo.startMap);
                            }
                            break;
                        }
                    case 1: // load game
                        {
                            ui.pushElement(new UiSavePicker(Content, false).addCallback(element =>
                            {
                                UiSavePicker saves = (UiSavePicker)element;
                                string chosen = saves.selectedString;
                                if (chosen == UiSavePicker.NO_FILES || chosen == null) return;
                                SaveGame game = SaveGame.Load(Content, chosen);
                                TimeOfDay.restart();
                                Flags.reset();

                                // restore data from save
                                Player player = new Player(Content, null, null);
                                foreach (string s in game.inventory.Keys)
                                {
                                    player.addItem(s, game.inventory[s]);
                                }
                                Flags.setAllFlags(game.flags, game.strings);
                                TimeOfDay.addMillis(game.time);
                                player.setTileLocation(game.location);

                                // go to overworld or map
                                if (game.overworld) this.game.goToOverworld(player, game.map);
                                else this.game.goToWorld(player, game.map, false);

                            }), new Vector2(Tile.TILE_SIZE, Tile.TILE_SIZE));
                            break;
                        }
                    case 2: // options
                        {
                            ui.pushElement(new UiOptionsMenu(game, Content),
                                new Vector2(
                                    Game1.INTERNAL_WIDTH / 2 - UiOptionsMenu.WIDTH / 2,
                                    Game1.INTERNAL_HEIGHT / 2 - UiOptionsMenu.HEIGHT / 2)
                                );
                            break;
                        }
                    case 3: // quit
                        {
                            game.Exit();
                            break;
                        }
                }
            });

            Vector2 center = Vector2.Round(new Vector2(
                Game1.INTERNAL_WIDTH / 2 - mainList.size.X / 2, 
                (Game1.INTERNAL_HEIGHT / 2 - mainList.size.Y / 2) + 40));

            ui.pushElement(mainList, center);
        }

        public void updateBackground()
        {
            bg = Content.Load<Texture2D>(GameInfo.titleBackground);
        }


        public void onInput(InputHandler input)
        {
            if (editing) editor.onInput(input);
            else ui.onInput(input);

            if (input.isKeyPressed(Keys.F1))
            {
                if (editing)
                {
                    editor = null;
                }
                else editor = new GameEditor(game, Content, this);
            }
        }

        public void update(double dt)
        {
            if (editing) editor.update(dt);
            else ui.update(dt);
        }

        public void draw(SBWrapper wrapper)
        {
            wrapper.draw(bg);
            ui.draw(wrapper);
            if (editing) editor.draw(wrapper);
        }
    }
}
