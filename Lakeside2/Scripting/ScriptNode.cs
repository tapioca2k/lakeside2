﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.Scripting
{
    abstract class ScriptNode
    {
        public ScriptNode next { get; set; }
        public bool finished { get; set; }

        public virtual void update(double dt)
        {
        }

    }
}
