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

        public delegate void informPredatorOfPreyDeathEventHandler(object sender, ControlEvents.InformPredatorOfPreyDeathEventArgs args);
        public event informPredatorOfPreyDeathEventHandler RaiseInformHunterOfPreyDeath;

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
        public delegate void getLocationEventHandler(object sender, ControlEvents.GetOtherLocationEventArgs args);
        public event getLocationEventHandler RaiseGetLocation;

        //deaths
        public delegate void eatenEventHandler(object sender, ControlEvents.EatenEventArgs args);
        public event eatenEventHandler RaiseEaten;

        public delegate void diedEventHandler(object sender, ControlEvents.DeadEventArgs args);
        public event diedEventHandler RaiseDied;

        /// <summary>
        /// Gets the location, ID and species of every single subscriber.
        /// </summary>
        /// <param name="senderID">The ID of the caller.</param>
        /// <returns>Returns a list container the location, ID and species of every single subscriber.</returns>
        public List<(Vector PreyLocation, string PreyID, string PreySpecies)> GetPossiblePreys(string senderID)
        {
            return OnGetPossiblePreys(new ControlEvents.GetPossiblePreyEventArgs(senderID));
        }
        protected List<(Vector PreyLocation, string PreyID, string PreySpecies)> OnGetPossiblePreys(ControlEvents.GetPossiblePreyEventArgs e)
        {
            getPossiblePreyEventHandler eventHandler = RaiseFindPreyEvent;
            if(eventHandler != null)
            {
                eventHandler.Invoke(this, e);
                return e.GetPossiblePreys();
            }
            return null;
        }
        /// <summary>
        /// Informs <paramref name="receiverID"/> that it is a prey of <paramref name="senderID"/>.
        /// </summary>
        /// <param name="senderID">The ID of the caller.</param>
        /// <param name="receiverID">The ID of the receiver.</param>
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
        /// <summary>
        /// Informs <paramref name="receiverID"/> that it is no longer a prey of <paramref name="senderID"/>.
        /// </summary>
        /// <param name="senderID">The ID of the caller.</param>
        /// <param name="receiverID">The ID of the prey</param>
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
        /// <summary>
        /// Gets a list of all possible mates.
        /// </summary>
        /// <param name="species">The species of the caller.</param>
        /// <param name="gender">The gender of the caller.</param>
        /// <param name="senderID">The ID of the caller.</param>
        /// <returns></returns>
        public List<(string mateID, Vector Location)> PossibleMates(string species, char gender, string senderID)
        {
            return OnPossibleMates(new ControlEvents.PossibleMateEventArgs(species, gender, senderID));
        }
        protected List<(string mateID, Vector Location)> OnPossibleMates(ControlEvents.PossibleMateEventArgs e)
        {
            possibleMatesEventHandler eventHandler = RaisePossibleMatesEvent;
            if (eventHandler != null)
            {
                eventHandler.Invoke(this, e);
                return e.GetPossibleMates();
            }
            return null;
        }
        /// <summary>
        /// Informs <paramref name="receiverID"/> that it is mate to <paramref name="senderID"/>.
        /// </summary>
        /// <param name="senderID"></param>
        /// <param name="receiverID"></param>
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
        /// <summary>
        /// Informs <paramref name="receiverID"/> that it is no longer mate to <paramref name="senderID"/>.
        /// </summary>
        /// <param name="senderID"></param>
        /// <param name="receiverID"></param>
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
        /// <summary>
        /// Runs the AI and transmit <paramref name="timeSinceLastUpdate"/> with it for any code that needs to time interval between this call and the last call.
        /// </summary>
        /// <param name="timeSinceLastUpdate"></param>
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
        /// <summary>
        /// Gets the location of <paramref name="receiverID"/>.
        /// </summary>
        /// <param name="receiverID"></param>
        /// <returns></returns>
        public Vector GetLocation(string receiverID)
        {
            return OnGetLocation(new ControlEvents.GetOtherLocationEventArgs(receiverID));
        }
        protected Vector OnGetLocation(ControlEvents.GetOtherLocationEventArgs e)
        {
            getLocationEventHandler eventHandler = RaiseGetLocation;
            if(eventHandler != null)
            {
                eventHandler.Invoke(this, e);
                return e.GetLocation;
            }
            return new Vector(-1,-1,-1); //have a custom exception for eventHandler being null when the animal got a food/mate id
        }
        /// <summary>
        /// Informs <paramref name="receiverID"/> that it has died.
        /// </summary>
        /// <param name="receiverID"></param>
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
        /// <summary>
        /// Informs <paramref name="receiverID"/> that it has been eaten.
        /// </summary>
        /// <param name="receiverID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Informs <paramref name="receiverID"/> of the death of its prey <paramref name="senderID"/>.
        /// </summary>
        /// <param name="senderID"></param>
        /// <param name="receiverID"></param>
        public void InformPredatorOfPreyDeath(string senderID, string receiverID)
        { //rename this and the controlevent so their names also make sense for losing a prey
            OnInformPredatorOfDeath(new ControlEvents.InformPredatorOfPreyDeathEventArgs(senderID, receiverID));
        }
        protected void OnInformPredatorOfDeath(ControlEvents.InformPredatorOfPreyDeathEventArgs e)
        {
            informPredatorOfPreyDeathEventHandler eventHandler = RaiseInformHunterOfPreyDeath;
            if (eventHandler != null)
                eventHandler.Invoke(this, e);
        }
    }
}
