using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    class AnimalPublisher
    {
        public delegate void findPreyEventHandler(object sender, ControlEvents.GetPossiblePreyEventArgs args);
        public event findPreyEventHandler RaiseFindPreyEvent;

        public delegate void setPreyEventHandler(object sender, ControlEvents.SetPreyEventArgs args);
        public event setPreyEventHandler RaiseSetPreyEvent;

        public delegate void removePreyEventHaandler(object sender, ControlEvents.RemovePreyEventArgs args);
        public event removePreyEventHaandler RaiseRemovePreyEvent;

        public List<(float[] PreyLocation, string PreyID, string PreySpecies)> FindPrey()
        {
            return OnFindPrey(new ControlEvents.GetPossiblePreyEventArgs());
        }
        protected List<(float[] PreyLocation, string PreyID, string PreySpecies)> OnFindPrey(ControlEvents.GetPossiblePreyEventArgs e)
        {
            findPreyEventHandler eventHandler = RaiseFindPreyEvent;
            if(eventHandler != null)
            {
                eventHandler.Invoke(this, e);
                return e.GetPossiblePreys();
            }
            return null;
        }

        public void SetPrey(string senderID, string receiverID)
        {
            OnSetPrey(new ControlEvents.SetPreyEventArgs(senderID, receiverID));
        }
        protected void OnSetPrey(ControlEvents.SetPreyEventArgs e)
        {
            setPreyEventHandler eventHandler = RaiseSetPreyEvent;
            if (eventHandler != null)
                eventHandler.Invoke(this, e);
        }

        public void RemovePrey(string senderID, string receiverID)
        {
            OnRemovePrey(new ControlEvents.RemovePreyEventArgs(senderID, receiverID));
        }
        protected void OnRemovePrey(ControlEvents.RemovePreyEventArgs e)
        {
            removePreyEventHaandler eventHandler = RaiseRemovePreyEvent;
            if (eventHandler != null)
                eventHandler.Invoke(this, e);
        }

    }
}
