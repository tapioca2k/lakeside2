using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Scripting
{
    class MoveNode : ScriptNode
    {
        Entity entity;
        Vector2[] directions;

        public MoveNode(Entity entity, Vector2[] directions)
        {
            this.entity = entity;
            this.directions = directions;
        }

        public override void start()
        {
            for (int i = 0; i < directions.Length; i++)
            {
                entity.queueMove(directions[i]);
            }
            base.start();
        }

        public override void update(double dt)
        {
            finished = !entity.moving;
        }
    }
}
