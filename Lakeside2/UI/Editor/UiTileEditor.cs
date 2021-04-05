using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2.UI.Editor
{
    class UiTileEditor : UiElement
    {
        public override Vector2 size
        {
            get
            {
                return new Vector2(160, 160);
            }
        }

        ContentManager Content;
        public Tile tile;
        public NPC npc;

        public UiTileEditor(ContentManager Content, Tile tile, NPC npc)
        {
            this.Content = Content;
            this.tile = tile;
            this.npc = npc;
            setBackground(Color.White);
        }

        public override void onInput(InputHandler input)
        {
            if (input.isAnyKeyPressed(Keys.E, Keys.Enter, Keys.Escape))
            {
                finished = true;
            }
            else if (input.isKeyPressed(Keys.T))
            {
                system.pushElement(new UiTilePicker(Content).addCallback(element =>
                {
                    UiTilePicker picker = (UiTilePicker)element;
                    Tile picked = (Tile)picker.selected;
                    this.tile.setTexture(Content, picked.filename);

                }), Vector2.Zero);
            }
            else if (input.isKeyPressed(Keys.W))
            {
                this.tile.collision = !this.tile.collision;
            }
            else if (input.isKeyPressed(Keys.N))
            {
                system.pushElement(new UiNpcEditor(Content, null).addCallback(element =>
                {
                    UiNpcEditor editor = (UiNpcEditor)element;
                    if (editor.delete) this.npc = null;
                    else this.npc = editor.npc;
                }), Vector2.Zero);
            }
            else if (input.isKeyPressed(Keys.S))
            {
                system.pushElement(new UiTextInput("Script: ").addCallback(element =>
                {
                    UiTextInput input = (UiTextInput)element;
                    if (input.text != "") this.tile.setScript(input.text);

                }), Vector2.Zero);
            }
        }

        public override void draw(SBWrapper wrapper)
        {
            drawBackground(wrapper);
            tile.draw(wrapper, new Vector2(5, 5));

            wrapper.drawString("(T)exture: " + tile.filename, new Vector2(25, 5));
            wrapper.drawString("(W)alkable: " + UiTextDisplay.YesOrNo(tile.collision), new Vector2(5, 25));
            wrapper.drawString("(N)PC: " + "(None)", new Vector2(5, 45));
            wrapper.drawString("(S)cript: " + UiTextDisplay.TextOrNull(tile.script), new Vector2(5, 65));
        }

    }
}
