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
    class Player : IEntity
    {
        const int RUN_SPEED = 75;

        Texture2D texture;
        TileMap map;

        Vector2 queuedMove;
        bool moving
        {
            get
            {
                return queuedMove.X != 0 || queuedMove.Y != 0;
            }
        }

        Vector2 location;
        public Vector2 getLocation()
        {
            return location;
        }

        public Vector2 tileLocation
        {
            get
            {
                return TileMap.worldToTile(location);
            }
            set
            {
                location = new Vector2(
                    value.X * Tile.TILE_SIZE, 
                    value.Y * Tile.TILE_SIZE);
            }
        }

        public Player(ContentManager Content, TileMap map)
        {
            texture = Content.Load<Texture2D>("player");
            this.map = map;
            location = Vector2.Zero;
            queuedMove = Vector2.Zero;
        }

        public void onInput(InputHandler input)
        {
            if (input.isKeyHeld(Keys.W) && !moving) tryMove(new Vector2(0, -1));
            else if (input.isKeyHeld(Keys.A) && !moving) tryMove(new Vector2(-1, 0));
            else if (input.isKeyHeld(Keys.S) && !moving) tryMove(new Vector2(0, 1));
            else if (input.isKeyHeld(Keys.D) && !moving) tryMove(new Vector2(1, 0));
        }

        void tryMove(Vector2 direction)
        {
            if (map.checkCollision(tileLocation + direction))
            queuedMove = Vector2.Multiply(direction, Tile.TILE_SIZE);
        }

        public void update(double dt)
        {
            if (moving)
            {
                float proposed = (float)dt * RUN_SPEED;
                if (queuedMove.X > 0)
                {
                    float trueMove = Math.Min(queuedMove.X, proposed);
                    queuedMove.X -= trueMove;
                    location.X += trueMove;
                }
                else if (queuedMove.X < 0)
                {
                    float trueMove = Math.Max(queuedMove.X, proposed);
                    queuedMove.X += trueMove;
                    location.X -= trueMove;
                }
                if (queuedMove.Y > 0)
                {
                    float trueMove = Math.Min(queuedMove.Y, proposed);
                    queuedMove.Y -= trueMove;
                    location.Y += trueMove;
                }
                else if (queuedMove.Y < 0)
                {
                    float trueMove = Math.Max(queuedMove.Y, proposed);
                    queuedMove.Y += trueMove;
                    location.Y -= trueMove;
                }
                if (!moving)
                {
                    // correct for floating point weirdness, ensure location is on the grid
                    // TODO really need to clean up floating point weirdness!!
                    location = Vector2.Round(location);
                }
            }
        }

        public void draw(SpriteBatch spriteBatch, TilemapCamera camera)
        {
            spriteBatch.Draw(texture, camera.worldToScreen(location), Color.White);
        }
    }
}
