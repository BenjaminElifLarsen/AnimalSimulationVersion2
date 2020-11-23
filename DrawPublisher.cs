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

        public delegate void speciesAndAmountEventHandler(object sender, ControlEvents.SpeciesAndAmountEventArgs args);
        public event speciesAndAmountEventHandler RaiseSpeciesAndAmountEvent;

        public List<(Point[] Design, Colour Colour, Vector Location)> Draw()
        { 
            return OnDraw(new ControlEvents.DrawEventArgs());
        }
        protected List<(Point[] Design, Colour Colour, Vector Location)> OnDraw(ControlEvents.DrawEventArgs e)
        { 
            drawEventHandler eventHandler = RaiseDrawEvent;
            if (eventHandler != null)
            {
                eventHandler.Invoke(this, e);
                return e.DrawInformation;
            }
            return null;
        }
        public List<(string Species, ushort Amount)> SpeciesAndAmount()
        {
            return OnSpeciesAndAmount(new ControlEvents.SpeciesAndAmountEventArgs());
        }

        protected List<(string Species, ushort Amount)> OnSpeciesAndAmount(ControlEvents.SpeciesAndAmountEventArgs e)
        {
            speciesAndAmountEventHandler eventHandler = RaiseSpeciesAndAmountEvent;
            if(eventHandler != null)
            {
                eventHandler.Invoke(this, e);
                return e.GetList;
            }
            return null;
        }
    }
}
