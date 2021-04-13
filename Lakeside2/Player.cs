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
        public const string ENTITY_NAME = "player";

        World world;

        Lua worldLua;

        public override string name => ENTITY_NAME;

        public Player(ContentManager Content, World world, Lua worldLua)
        {
            loadAnimatedTexture(Content, "greenman");
            this.world = world;
            this.worldLua = worldLua;
        }

        public void onInput(InputHandler input)
        {
            if (moving) return;

            if (input.isKeyHeld(Keys.W))
            {
                tryMove(DIREC_UP);
                setDirection(Directions.up);
            }
            else if (input.isKeyHeld(Keys.A))
            {
                tryMove(DIREC_LEFT);
                setDirection(Directions.left);
            }
            else if (input.isKeyHeld(Keys.S))
            {
                tryMove(DIREC_DOWN);
                setDirection(Directions.down);
            }
            else if (input.isKeyHeld(Keys.D))
            {
                tryMove(DIREC_RIGHT);
                setDirection(Directions.right);
            }

            else if (input.isKeyPressed(Keys.E))
            {
                NPC interacting = world.map.getNPC(getFacingTile());
                if (interacting != null)
                {
                    interacting.interact(worldLua);
                    interacting.setDirection(getOppositeDirection(facing));
                }
            }
        }

        void tryMove(Vector2 direction)
        {
            if (world.map.checkCollision(getTileLocation() + direction))
            {
                queueMove(direction);
            }
        }

        public override void update(double dt)
        {
            base.update(dt);
            if (step) // a full step was completed this update, check map scripts
            {
                world.map.stepOn(this, getTileLocation(), worldLua);
            }
        }

    }
}
