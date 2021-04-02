using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    // Player, NPC, or other object that actually exists in the game world
    abstract class Entity
    {
        const string ENTITIES = "entities/";

        Texture2D texture;
        protected Vector2 location = Vector2.Zero;

        public void loadAnimatedTexture(ContentManager Content, string filename)
        {
            texture = Content.Load<Texture2D>(ENTITIES + filename);
            // TODO load animation data
        }

        public Vector2 getLocation()
        {
            return location;
        }

        public void setLocation(Vector2 val)
        {
            location = val;
        }

        public Vector2 getTileLocation()
        {
            return new Vector2((int)location.X / Tile.TILE_SIZE, (int)location.Y / Tile.TILE_SIZE);
        }

        public virtual void update(double dt)
        {
        }

        public virtual void draw(SBWrapper wrapper, TilemapCamera camera)
        {
            wrapper.draw(texture, camera.worldToScreen(location));
        }
    }
}
