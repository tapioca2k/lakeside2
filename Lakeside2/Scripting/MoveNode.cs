﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Scripting
{
    public class MoveNode : ScriptNode
    {
        Entity entity;
        Point[] directions;

        public MoveNode(Entity entity, Point[] directions)
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
