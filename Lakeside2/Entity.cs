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
    public abstract class Entity : IDrawable
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

        const int RUN_SPEED = 75;
        Queue<Vector2> moves = new Queue<Vector2>();
        Vector2 move = Vector2.Zero;
        protected bool step = false;

        Entity follower;

        public bool moving
        {
            get
            {
                return move != Vector2.Zero || moves.Count > 0;
            }
        }

        public abstract string name { get; }

        public static Vector2 getFacingVector(Directions direction)
        {
            switch (direction)
            {
                case Directions.up: return DIREC_UP;
                case Directions.right: return DIREC_RIGHT;
                case Directions.down: return DIREC_DOWN;
                case Directions.left: return DIREC_LEFT;
                default: return Vector2.Zero; // should never happen
            }
        }

        public static Directions getDirection(Vector2 v)
        {
            if (v.X < 0) return Directions.left;
            else if (v.X > 0) return Directions.right;
            else if (v.Y > 0) return Directions.down;
            else if (v.Y < 0) return Directions.up;
            else return Directions.down; // should never happen
        }

        public void faceEntity(Entity other)
        {
            Vector2 l = other.getTileLocation();
            if (l.X < getTileLocation().X) setDirection(Directions.left);
            else if (l.X > getTileLocation().X) setDirection(Directions.right);
            else if (l.Y < getTileLocation().Y) setDirection(Directions.up);
            else if (l.Y > getTileLocation().Y) setDirection(Directions.down);
        }

        public static Directions getOppositeDirection(Directions direction)
        {
            switch (direction)
            {
                case Directions.up: return Directions.down;
                case Directions.right: return Directions.left;
                case Directions.down: return Directions.up;
                case Directions.left: return Directions.right;
                default: return Directions.up; // should never happen
            }
        }

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
            return getTileLocation() + getFacingVector(facing);
        }

        public virtual void update(double dt)
        {
            step = false;
            if (!moving) animation.update(dt / 2f); // slow version of the run animation while idle
            else if (moving && move == Vector2.Zero)
            {
                move = moves.Dequeue(); // get next queued move
                setDirection(getDirection(move));
            }

            // work on current move
            if (move != Vector2.Zero)
            {
                animation.update(dt);
                float proposed = (float)dt * RUN_SPEED;
                if (move.X > 0)
                {
                    float trueMove = Math.Min(move.X, proposed);
                    move.X -= trueMove;
                    location.X += trueMove;
                }
                else if (move.X < 0)
                {
                    proposed *= -1;
                    float trueMove = Math.Max(move.X, proposed);
                    move.X -= trueMove;
                    location.X += trueMove;
                }
                if (move.Y > 0)
                {
                    float trueMove = Math.Min(move.Y, proposed);
                    move.Y -= trueMove;
                    location.Y += trueMove;
                }
                else if (move.Y < 0)
                {
                    proposed *= -1;
                    float trueMove = Math.Max(move.Y, proposed);
                    move.Y -= trueMove;
                    location.Y += trueMove;
                }

                if (!moving) // finished with this move
                {
                    location = Vector2.Round(location);
                    step = true;
                }
            }
        }

        public void setDirection(Directions direction)
        {
            animation.set((int)direction);
            this.facing = direction;
        }

        public void queueMove(Vector2 direction)
        {
            moves.Enqueue(Vector2.Multiply(direction, Tile.TILE_SIZE));
            if (follower != null)
            {
                // stand still if entities are overlapping, for now.
                if (follower.getTileLocation() != this.getTileLocation())
                {
                    Vector2 rawFacingDirection = this.getTileLocation() - follower.getTileLocation();
                    follower.queueMove(getFacingVector(getDirection(rawFacingDirection)));
                }
            }
        }

        public void setFollower(Entity follower)
        {
            this.follower = follower;
        }

        public Entity getFollower()
        {
            return follower;
        }

        public void draw(SBWrapper wrapper, TilemapCamera camera)
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
