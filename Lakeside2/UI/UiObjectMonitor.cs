using System;
using System.Collections.Generic;
using System.Text;

namespace Lakeside2.UI
{
    // Debug UI element for displaying some object info on screen
    class UiObjectMonitor : UiTextDisplay
    {
        Object obj;
        Func<Object, string> lambda;
        public UiObjectMonitor(Object obj, Func<Object, string> lambda) : base(Fonts.get("Arial"), "----------")
        {
            this.obj = obj;
            this.lambda = lambda;
        }

        public override void update(double dt)
        {
            this.text = lambda.Invoke(obj);
        }
    }
}
