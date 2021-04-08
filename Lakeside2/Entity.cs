using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Lakeside2
{
    // Player, NPC, or other object that actually exists in the game world
    abstract class Entity : IDrawable
    {
        public enum Directions
        {
            down, up, left, right 
        };
        public static Vector2 DIREC_UP = new Vector2(0, -1);
        public static Vector2 DIREC_RIGHT = new Vector2(1, 0);
        public static Vector2 DIREC_DOWN = new Vector2(0, 1);
        public static Vector2 DIREC_LEFT = new Vector2(-1, 0);

        const string ENTITIES = "entities/";

        Texture2D texture;
        protected Animation animation;
        protected Vector2 location = Vector2.Zero;
        protected Directions facing = Directions.down;

        protected void loadAnimatedTexture(ContentManager Content, string filename)
        {
            texture = Content.Load<Texture2D>(ENTITIES + filename);
            animation = JsonSerializer.Deserialize<Animation>(File.ReadAllText("Content/entities/entity.json"));
        }

        public Vector2 getLocation()
        {
            return location;
        }

        public void setLocation(Vector2 val)
        {
            location = val;
        }

        public void setTileLocation(Vector2 val)
        {
            setLocation(Vector2.Multiply(val, Tile.TILE_SIZE));
        }

        public Vector2 getTileLocation()
        {
            return new Vector2((int)location.X / Tile.TILE_SIZE, (int)location.Y / Tile.TILE_SIZE);
        }

        public Vector2 getFacingTile()
        {
            switch (facing)
            {
                case Directions.up: return getTileLocation() + DIREC_UP;
                case Directions.right: return getTileLocation() + DIREC_RIGHT;
                case Directions.down: return getTileLocation() + DIREC_DOWN;
                case Directions.left: return getTileLocation() + DIREC_LEFT;
                default: return getTileLocation(); // should never happen
            }
        }

        public virtual void update(double dt)
        {
            animation.update(dt);
        }

        public void setDirection(Directions direction)
        {
            animation.set((int)direction);
            this.facing = direction;
        }

        public virtual void draw(SBWrapper wrapper, TilemapCamera camera)
        {
            draw(wrapper, camera.worldToScreen(location));
        }

        // draw without correcting for camera perspective
        public void draw(SBWrapper wrapper, Vector2 location)
        {
            wrapper.draw(texture, location, animation.getFrame());
        }

    }
}
