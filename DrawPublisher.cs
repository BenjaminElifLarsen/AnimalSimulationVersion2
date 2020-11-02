using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class DrawPublisher
    {
        public delegate void drawEventHandler(object sender, ControlEvents.DrawEventArgs args);
        public event drawEventHandler RaiseDrawEvent;

        public List<(Point[] Design, (int Red, int Green, int Blue) Colour, float[] Location)> Draw()
        { 
            return OnDraw(new ControlEvents.DrawEventArgs());
        }
        protected List<(Point[] Design, (int Red, int Green, int Blue) Colour, float[] Location)> OnDraw(ControlEvents.DrawEventArgs e)
        { 
            drawEventHandler eventHandler = RaiseDrawEvent;
            if (eventHandler != null)
            {
                eventHandler.Invoke(this, e);
                return e.DrawInformation();
            }
            return null;
        }
    }
}
