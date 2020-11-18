using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class LifeformPublisher
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

        public delegate void getAllLocationsEventHandler(object sender, ControlEvents.GetAllLocationsEventArgs args);
        public event getAllLocationsEventHandler RaiseGetAllLocations;

        //deaths
        public delegate void eatenEventHandler(object sender, ControlEvents.EatenEventArgs args);
        public event eatenEventHandler RaiseEaten;

        public delegate void diedEventHandler(object sender, ControlEvents.DeadEventArgs args);
        public event diedEventHandler RaiseDied;

        //damage
        public delegate void damageEventHandler(object sender, ControlEvents.DoHealthDamageEventArgs args);
        public event damageEventHandler RaiseDamage;

        //relationship
        public delegate void transmitDataEventHandler(object sender, ControlEvents.TransmitDataEventArgs args);
        public event transmitDataEventHandler RaiseTransmitData;

        public delegate void possibleRelationshipJoinerEvnetHandler(object sender, ControlEvents.RelationshipCandidatesEventArgs args);
        public event possibleRelationshipJoinerEvnetHandler RaisePossibleRelationshipJoiner;




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
        /// <param name="senderID">The ID of the caller.</param>
        /// <param name="receiverID">The ID of the prey</param>
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
        /// <param name="senderID">The ID of the caller.</param>
        /// <param name="receiverID">The ID of the prey</param>
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
        /// <param name="timeSinceLastUpdate">The amount of time that has passed since the last time the AI was called.</param>
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
        /// <param name="receiverID">The ID of the lifeform whoes location is wanted.</param>
        /// <returns>A Vector with the location of <paramref name="receiverID"/>.</returns>
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
        /// <param name="receiverID">The ID of the lifeform that have died.</param>
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
        /// <param name="receiverID">The ID of the lifeform that has been eaten</param>
        /// <returns>The nutrient value of <paramref name="receiverID"/>.</returns>
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
                return e.GetNutrient;
            }
            return -1;
        }
        /// <summary>
        /// Informs <paramref name="receiverID"/> of the death of its prey <paramref name="senderID"/>.
        /// </summary>
        /// <param name="senderID">The ID of the prey.</param>
        /// <param name="receiverID">The ID of the predator.</param>
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
        /// <summary>
        /// Informs <paramref name="receiverID"/> that <paramref name="senderID"/> attacked it for <paramref name="damage"/>.
        /// </summary>
        /// <param name="senderID">The ID of the attacker.</param>
        /// <param name="receiverID">The ID of the attacked.</param>
        /// <param name="damage">The amount of damage done.</param>
        public void DamageLifeform(string senderID, string receiverID, byte damage)
        {
            OnDamageLifeform(new ControlEvents.DoHealthDamageEventArgs(senderID, receiverID, damage));
        }
        protected void OnDamageLifeform(ControlEvents.DoHealthDamageEventArgs e)
        {
            damageEventHandler eventHandler = RaiseDamage;
            if (eventHandler != null)
                eventHandler.Invoke(this, e);
        }
        /// <summary>
        /// Gets the location of every lifeform.
        /// </summary>
        /// <param name="senderID">The ID of the sender, used to ensure the sender does not react to the event.</param>
        public void GetAllLocations(string senderID)
        {
            OnGetAllLocations(new ControlEvents.GetAllLocationsEventArgs(senderID));
        }
        protected List<(Vector Location, string ID)> OnGetAllLocations(ControlEvents.GetAllLocationsEventArgs e)
        {
            getAllLocationsEventHandler eventHandler = RaiseGetAllLocations;
            if(eventHandler != null)
            {
                eventHandler.Invoke(this, e);
                return e.GetInformation;
            }
            return null;
        }
        /// <summary>
        /// Transmit <paramref name="data"/> from <paramref name="senderID"/> to <paramref name="receivierID"/>.
        /// </summary>
        /// <remarks>This event can be used to tranmit any form of data from one lifeform to another as long time the receiver knows how to handle the data.</remarks>
        /// <param name="senderID">The ID of the data sender.</param>
        /// <param name="receivierID">The ID of the receiver of the data.</param>
        /// <param name="data">An object of data.</param>
        public void TransmitData(string senderID, string receivierID, object data)
        {
            OnTransmitData(new ControlEvents.TransmitDataEventArgs(senderID, receivierID, data));
        }
        protected void OnTransmitData(ControlEvents.TransmitDataEventArgs e)
        {
            transmitDataEventHandler eventHandler = RaiseTransmitData;
            if (eventHandler != null)
                eventHandler.Invoke(this, e);
        }
        /// <summary>
        /// Allows a lifefrom to contact other lifeforms if they implement an interface of type <paramref name="type"/>. The receiver(s) can look at <paramref name="species"/> to help decide if they should response.
        /// </summary>
        /// <param name="senderID">The sender of this event, used to prevent them reacting to the event.</param>
        /// <param name="species">The species of <paramref name="senderID"/>.</param>
        /// <param name="type">A Type, should be used to check for interface implementation.</param>
        /// <returns>A list of possible candidates for a pack.</returns>
        public List<(Vector Location, string ID, char Gender)> PossibleRelationshipJoiner(string senderID, string species, Type type)
        {
            return OnPossibleRelationshipJoiner(new ControlEvents.RelationshipCandidatesEventArgs(senderID, species, type));
        }
        protected List<(Vector Location, string ID, char Gender)> OnPossibleRelationshipJoiner(ControlEvents.RelationshipCandidatesEventArgs e)
        {
            possibleRelationshipJoinerEvnetHandler evnetHandler = RaisePossibleRelationshipJoiner;
            if(evnetHandler != null)
            {
                evnetHandler.Invoke(this, e);
                return e.GetList;
            }
            return null;
        }
    }
}
