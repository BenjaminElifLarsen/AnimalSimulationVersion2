using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class AnimalPublisher
    {
        //prey
        public delegate void getPossiblePreyEventHandler(object sender, ControlEvents.GetPossiblePreyEventArgs args);
        public event getPossiblePreyEventHandler RaiseFindPreyEvent;

        public delegate void setPreyEventHandler(object sender, ControlEvents.SetPreyEventArgs args);
        public event setPreyEventHandler RaiseSetPreyEvent;

        public delegate void removePreyEventHandler(object sender, ControlEvents.RemovePreyEventArgs args);
        public event removePreyEventHandler RaiseRemovePreyEvent;

        //mate
        public delegate void possibleMatesEventHandler(object sender, ControlEvents.PossibleMateEventArgs args);
        public event possibleMatesEventHandler RaisePossibleMatesEvent;

        public delegate void setMateEventHandler(object sende, ControlEvents.SetMateEventArgs args);
        public event setMateEventHandler RaiseSetMateEvent;

        public delegate void removeMateEventHandler(object sender, ControlEvents.RemoveMateEventArgs args);
        public event removeMateEventHandler RaiseRemoveMateEvent;

        //Draw //this and its code should not be here, but in another publisher. 


        //AI
        public delegate void aiEvnetHandler(object sender, ControlEvents.AIEventArgs args);
        public event aiEvnetHandler RaiseAIEvent;

        public List<(float[] PreyLocation, string PreyID, string PreySpecies)> GetPossiblePreys()
        {
            return OnGetPossiblePreys(new ControlEvents.GetPossiblePreyEventArgs());
        }
        protected List<(float[] PreyLocation, string PreyID, string PreySpecies)> OnGetPossiblePreys(ControlEvents.GetPossiblePreyEventArgs e)
        {
            getPossiblePreyEventHandler eventHandler = RaiseFindPreyEvent;
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
            removePreyEventHandler eventHandler = RaiseRemovePreyEvent;
            if (eventHandler != null)
                eventHandler.Invoke(this, e);
        }

        public List<(string mateID, float[] Location)> PossibleMates(string species, char gender)
        {
            return OnPossibleMates(new ControlEvents.PossibleMateEventArgs(species, gender));
        }
        protected List<(string mateID, float[] Location)> OnPossibleMates(ControlEvents.PossibleMateEventArgs e)
        {
            possibleMatesEventHandler eventHandler = RaisePossibleMatesEvent;
            if (eventHandler != null)
            {
                eventHandler.Invoke(this, e);
                return e.GetPossibleMates();
            }
            return null;
        }

        public void SetMate(string senderID, string receiverID)
        {
            OnSetMate(new ControlEvents.SetMateEventArgs(senderID, receiverID));
        }
        protected void OnSetMate(ControlEvents.SetMateEventArgs e)
        {
            setMateEventHandler eventHandler = RaiseSetMateEvent;
            if (eventHandler != null)
                eventHandler.Invoke(this,e);
        }

        public void RemoveMate(string senderID, string receiverID)
        {
            OnRemoveMate(new ControlEvents.RemoveMateEventArgs(senderID, receiverID));
        }
        protected void OnRemoveMate(ControlEvents.RemoveMateEventArgs e)
        {
            removeMateEventHandler eventHandler = RaiseRemoveMateEvent;
            if (eventHandler != null)
                eventHandler.Invoke(this, e);
        }

        public void AI(float timeSinceLastUpdate)
        {
            OnAI(new ControlEvents.AIEventArgs(timeSinceLastUpdate));
        }
        protected void OnAI(ControlEvents.AIEventArgs e)
        {
            aiEvnetHandler evnetHandler = RaiseAIEvent;
            if (evnetHandler != null)
                evnetHandler.Invoke(this, e);
        }
    }
}
