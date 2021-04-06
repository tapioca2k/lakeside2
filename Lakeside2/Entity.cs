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
        public const int ANIM_UP = 1;
        public const int ANIM_DOWN = 0;
        public const int ANIM_LEFT = 2;
        public const int ANIM_RIGHT = 3;

        const string ENTITIES = "entities/";

        Texture2D texture;
        protected Animation animation;
        protected Vector2 location = Vector2.Zero;

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

        public virtual void update(double dt)
        {
            animation.update(dt);
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
