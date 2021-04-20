using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2
{
    public class TilemapCamera
    {

        TileMap map;
        Vector2 location;

        Entity centeredEntity;
        bool centeringEntity;

        bool inMove;
        Vector2 targetLocation;

        public Vector2 screenToWorld(Vector2 coordinates)
        {
            return location + coordinates;
        }
        public Vector2 worldToScreen(Vector2 coordinates)
        {
            return coordinates - location;
        }

        public TilemapCamera(TileMap map)
        {
            this.map = map;
            location = Vector2.Zero;

            inMove = false;
            targetLocation = Vector2.Zero;
        }

        public void setMap(TileMap newMap)
        {
            this.map = newMap;
        }

        public TileMap getMap()
        {
            return map;
        }

        public void rawMove(Vector2 amnt)
        {
            location += amnt;
        }

        public void tileMoveX(int amnt)
        {
            tileMove(amnt, 0);
        }

        public void tileMoveY(int amnt)
        {
            tileMove(0, amnt);
        }

        public void tileMove(int x, int y)
        {
            if (inMove) return;
            targetLocation = new Vector2(location.X + (x * Tile.TILE_SIZE), location.Y + (y * Tile.TILE_SIZE));
            inMove = true;
        }

        public void update(double dt)
        {
            if (inMove)
            {
                // TODO smoothness
                rawMove(new Vector2(targetLocation.X - location.X, targetLocation.Y - location.Y));
                if (Vector2.Distance(location, targetLocation) < 1)
                {
                    location = targetLocation;
                    inMove = false;
                }
            }

            if (centeringEntity)
            {
                location = new Vector2(
                    centeredEntity.getLocation().X - World.HALF_PORTAL_WIDTH,
                    centeredEntity.getLocation().Y - World.HALF_PORTAL_HEIGHT);
            }
        }

        public void centerEntity(bool center)
        {
            this.centeringEntity = center;
        }
        public void setCenteringEntity(Entity entity)
        {
            this.centeredEntity = entity;
        }

        public void draw(SBWrapper wrapper, List<Entity> entities)
        {
            SBWrapper cameraSpace = new SBWrapper(wrapper, -location);
            map.draw(cameraSpace);
            foreach (Entity entity in entities) {
                entity.draw(wrapper, this);
            }

        }

    }
}
