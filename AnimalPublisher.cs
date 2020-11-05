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

        //AI
        public delegate void aiEvnetHandler(object sender, ControlEvents.AIEventArgs args);
        public event aiEvnetHandler RaiseAIEvent;

        //location
        public delegate void getLocationEventHandler(object sender, ControlEvents.GetOtherLocation args);
        public event getLocationEventHandler RaiseGetLocation;

        //deaths
        public delegate void eatenEventHandler(object sender, ControlEvents.EatenEventArgs args);
        public event eatenEventHandler RaiseEaten;

        public delegate void diedEventHandler(object sender, ControlEvents.DeadEventArgs args);
        public event diedEventHandler RaiseDied;

        public List<((float X, float Y) PreyLocation, string PreyID, string PreySpecies)> GetPossiblePreys(string senderID)
        {
            return OnGetPossiblePreys(new ControlEvents.GetPossiblePreyEventArgs(senderID));
        }
        protected List<((float X, float Y) PreyLocation, string PreyID, string PreySpecies)> OnGetPossiblePreys(ControlEvents.GetPossiblePreyEventArgs e)
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

        public List<(string mateID, (float X, float Y) Location)> PossibleMates(string species, char gender, string senderID)
        {
            return OnPossibleMates(new ControlEvents.PossibleMateEventArgs(species, gender, senderID));
        }
        protected List<(string mateID, (float X, float Y) Location)> OnPossibleMates(ControlEvents.PossibleMateEventArgs e)
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

        public (float X, float Y) GetLocation(string receiverID)
        {
            return OnGetLocation(new ControlEvents.GetOtherLocation(receiverID));
        }
        protected (float X, float Y) OnGetLocation(ControlEvents.GetOtherLocation e)
        {
            getLocationEventHandler eventHandler = RaiseGetLocation;
            if(eventHandler != null)
            {
                eventHandler.Invoke(this, e);
                return e.GetLocation;
            }
            return (-1,-1); //have a custom exception for eventHandler being null when the animal got a food/mate id
        }

        public void Death(string receiverID)
        {
            OnDeath(new ControlEvents.DeadEventArgs(receiverID));
        }
        protected void OnDeath(ControlEvents.DeadEventArgs e)
        {
            diedEventHandler eventHandler = RaiseDied;
            if (eventHandler != null)
                eventHandler.Invoke(this, e);
        }

        public float Eat(string receiverID)
        {
            return OnEat(new ControlEvents.EatenEventArgs(receiverID));
        }
        protected float OnEat(ControlEvents.EatenEventArgs e)
        {
            eatenEventHandler eventHandler = RaiseEaten;
            if(eventHandler != null)
            {
                eventHandler.Invoke(this, e);
                return e.GetNutrience();
            }
            return -1;
        }
    }
}
