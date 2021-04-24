using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lakeside2
{
    public class TilemapCamera
    {

        TileMap map;
        Vector2 location;

        Entity centeredEntity;
        bool centeringEntity => centeredEntity != null;

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
                rawMove(new Vector2(targetLocation.X - location.X, targetLocation.Y - location.Y));
                if (Vector2.Distance(location, targetLocation) < 1)
                {
                    location = targetLocation;
                    inMove = false;
                }
            }

            // update camera to center to entity, bounded by the edges of the map
            if (centeringEntity)
            {
                Vector2 centerLoc = centeredEntity.getLocation();
                if (centerLoc.X >= World.HALF_PORTAL_WIDTH && centerLoc.X <= (map.width * Tile.TILE_SIZE) - World.HALF_PORTAL_WIDTH)
                {
                    location.X = centeredEntity.getLocation().X - World.HALF_PORTAL_WIDTH;
                }
                if (centerLoc.Y >= World.HALF_PORTAL_HEIGHT && centerLoc.Y <= (map.height * Tile.TILE_SIZE) - World.HALF_PORTAL_HEIGHT)
                {
                    location.Y = centeredEntity.getLocation().Y - World.HALF_PORTAL_HEIGHT;
                }
            }
        }

        // force the camera to the correct position around the centered entity
        public void forceUpdate()
        {
            if (centeringEntity && map != null)
            {
                this.location = new Vector2(
                    Math.Max(0, Math.Min(centeredEntity.getLocation().X - World.HALF_PORTAL_WIDTH, (map.width * Tile.TILE_SIZE) - World.PORTAL_WIDTH)),
                    Math.Max(0, Math.Min(centeredEntity.getLocation().Y - World.HALF_PORTAL_HEIGHT, (map.height * Tile.TILE_SIZE) - World.PORTAL_HEIGHT)));
                if (map.width < Game1.TILE_WIDTH)
                {
                    int extraTiles = (Game1.TILE_WIDTH - map.width) / 2;
                    this.location.X = -Tile.TILE_SIZE * extraTiles;
                }
                if (map.width < Game1.TILE_HEIGHT)
                {
                    int extraTiles = (Game1.TILE_HEIGHT - map.height) / 2;
                    this.location.Y = -Tile.TILE_SIZE * extraTiles;
                }
            }
        }

        public void setCenteringEntity(Entity entity)
        {
            this.centeredEntity = entity;
            forceUpdate();
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
