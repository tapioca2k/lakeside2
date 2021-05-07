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
    public class Player : Entity
    {
        public const string ENTITY_NAME = "player";
        public override string name => ENTITY_NAME;

        World world;
        Lua worldLua;
        Dictionary<Item, int> inventory;


        public Player(ContentManager Content, World world, Lua worldLua)
        {
            loadAnimatedTexture(Content, "greenman");
            this.world = world;
            this.worldLua = worldLua;
            this.inventory = new Dictionary<Item, int>();
        }

        public int addItem(string name, int amnt)
        {
            Item i = Inventory.getItem(name);
            if (inventory.ContainsKey(i)) inventory[i] += amnt;
            else inventory.Add(i, amnt);

            if (inventory[i] < 0)
            {
                inventory.Remove(i);
                return 0;
            }
            else
            {
                return inventory[i];
            }
        }

        public Item getItem(string name)
        {
            Item i = Inventory.getItem(name);
            if (inventory.ContainsKey(i)) return i;
            else return null;
        }

        public int getItemCount(string name)
        {
            return addItem(name, 0);
        }

        public Dictionary<Item, int> getInventory()
        {
            return inventory;
        }
        public void setInventory(Dictionary<Item, int> inventory)
        {
            this.inventory = inventory;
        }

        public void setWorld(World w)
        {
            this.world = w;
            this.worldLua = w.lua;
        }

        public void onInput(InputHandler input)
        {
            if (moving) return;

            // movement
            if (input.isCommandHeld(Bindings.Up)) tryMove(DIREC_UP);
            else if (input.isCommandHeld(Bindings.Left)) tryMove(DIREC_LEFT);
            else if (input.isCommandHeld(Bindings.Down)) tryMove(DIREC_DOWN);
            else if (input.isCommandHeld(Bindings.Right)) tryMove(DIREC_RIGHT);

            // interact
            else if (input.isCommandPressed(Bindings.Interact))
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
            setDirection(getDirection(direction)); // face direction even if collision check fails
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
