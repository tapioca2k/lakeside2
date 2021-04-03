using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2
{
    class Player : Entity
    {
        const int RUN_SPEED = 75;

        World world;

        Lua worldLua;

        Vector2 queuedMove;
        bool moving
        {
            get
            {
                return queuedMove.X != 0 || queuedMove.Y != 0;
            }
        }

        public Player(ContentManager Content, World world, Lua worldLua)
        {
            loadAnimatedTexture(Content, "player");
            this.world = world;
            this.worldLua = worldLua;
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
            if (world.map.checkCollision(getTileLocation() + direction))
            {
                queuedMove = Vector2.Multiply(direction, Tile.TILE_SIZE);
            }
        }

        public override void update(double dt)
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
                    proposed *= -1;
                    float trueMove = Math.Max(queuedMove.X, proposed);
                    queuedMove.X -= trueMove;
                    location.X += trueMove;
                }
                if (queuedMove.Y > 0)
                {
                    float trueMove = Math.Min(queuedMove.Y, proposed);
                    queuedMove.Y -= trueMove;
                    location.Y += trueMove;
                }
                else if (queuedMove.Y < 0)
                {
                    proposed *= -1;
                    float trueMove = Math.Max(queuedMove.Y, proposed);
                    queuedMove.Y -= trueMove;
                    location.Y += trueMove;
                }
                if (!moving)
                {
                    location = Vector2.Round(location);
                    queuedMove = Vector2.Zero;
                    world.map.stepOn(getTileLocation(), worldLua);
                }
            }
            base.update(dt);
        }
    }
}
