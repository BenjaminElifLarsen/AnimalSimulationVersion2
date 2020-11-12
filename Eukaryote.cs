﻿using System.Drawing;

namespace AnimalSimulationVersion2
{
    abstract class Eukaryote
    { //consider having a vector or posistion class rather than using (X,Y). E.g. a class with 3 floats, X, Y, Z
        /// <summary>
        /// 
        /// </summary>
        protected float timeAlive;
        /// <summary>
        /// 
        /// </summary>
        protected float timeSinceLastUpdate;
        /// <summary>
        /// The amount of seconds since last update.
        /// </summary>
        protected float reproductionCooldown;
        /// <summary>
        /// The length of the reproduction in seconds.
        /// </summary>
        protected float lengthOfReproduction;
        /// <summary>
        /// How long time the current reproduction has lasted in seconds.
        /// </summary>
        protected float periodInReproduction;
        /// <summary>
        /// 
        /// </summary>
        protected MapInformation mapInformation; //maybe have all variables as abstract, except those that is set in this class, properties so other will know to set them.
        /// <summary>
        /// 
        /// </summary>
        protected IHelper helper;
        /// <summary>
        /// 
        /// </summary>
        protected AnimalPublisher animalPublisher;
        /// <summary>
        /// 
        /// </summary>
        protected DrawPublisher drawPublisher;

        /// <summary>
        /// The current age of the lifeform.
        /// </summary>
        protected float Age { get; set; }
        /// <summary>
        /// The age when the animal can reproduce.
        /// </summary>
        protected float ReproductionAge { get; set; }
        /// <summary>
        /// The maximum age of the lifeform.
        /// </summary>
        protected float MaxAge { get; set; }
        /// <summary>
        /// The species of the lifeform.
        /// </summary>
        protected string Species { get; set; }
        /// <summary>
        /// The current location of the lifeform.
        /// </summary>
        protected (float X, float Y) Location { get; set; }
        /// <summary>
        /// The amount of time before the lifeform will feel a need to reproduce in seconds.
        /// </summary>
        protected float TimeToReproductionNeed { get; set; }
        /// <summary>
        /// The design of the lifeform.
        /// </summary>
        protected Point[] Design { get; set; }
        /// <summary>
        /// The RGB colour of the lifeform.
        /// </summary>
        protected (byte Red, byte Green, byte Blue) Colour { get; set; }
        /// <summary>
        /// The unique ID of the lifeform.
        /// </summary>
        protected string ID { get; set; }
        /// <summary>
        /// The health of the lifeform.
        /// </summary>
        protected float Health { get; set; }
        /// <summary>
        /// Predatores of the lifeform.
        /// </summary>
        protected string[] HuntedBy { get; set; } //the IDs that are after it
        /// <summary>
        /// The nutrience value of the lifeform.
        /// </summary>
        protected float NutrienValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected float OneAgeInSeconds { get; set; }

        /// <summary>
        /// True if the lifeform has reproduced and is waiting on children.
        /// </summary>
        protected bool HasReproduced { get; set; } //find a better name

        public Eukaryote(string species, (float X, float Y) location, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : this(helper, animalPublisher, drawPublisher, mapInformation)
        {
            Species = species;
            Location = location;
            HuntedBy = new string[0];
        }

        private Eukaryote(IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation)
        {
            this.helper = helper;
            this.animalPublisher = animalPublisher;
            this.drawPublisher = drawPublisher;
            this.mapInformation = mapInformation;

            OneAgeInSeconds = mapInformation.OneAgeInSeconds;
            ID = helper.GenerateID();

            animalPublisher.RaiseFindPreyEvent += IsPossiblePreyEventHandler;
            animalPublisher.RaiseSetPreyEvent += IsPreyEventHandler;
            animalPublisher.RaiseRemovePreyEvent += RemovePredatorEventHandler;
            animalPublisher.RaiseAIEvent += ControlEventHandler;
            animalPublisher.RaiseDied += DeathEventHandler;
            animalPublisher.RaiseEaten += EatenEventHandler;
            animalPublisher.RaiseGetLocation += LocationEventHandler;

            drawPublisher.RaiseDrawEvent += DrawEventHandler;
        }
        /// <summary>
        /// The 'AI' of the lifeform.
        /// </summary>
        protected abstract void AI();
        protected virtual void TimeUpdate()
        {
            timeAlive += timeSinceLastUpdate;
            Age = timeAlive / OneAgeInSeconds;
            TimeToReproductionNeed -= timeSinceLastUpdate;
            if (periodInReproduction < lengthOfReproduction && HasReproduced)
                periodInReproduction += timeSinceLastUpdate;
        }
        /// <summary>
        /// Lifeform produces offsprings.
        /// </summary>
        protected abstract void Reproduce();

        protected virtual void Death()
        { 

            if (HuntedBy.Length != 0)
            {
                foreach (string hunterID in HuntedBy)
                    animalPublisher.InformPredatorOfDeath(ID, hunterID);
            }
            RemoveSubscriptions();
        }
        /// <summary>
        /// Is asked for information such that another animal can decided if this animal is food or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void IsPossiblePreyEventHandler(object sender, ControlEvents.GetPossiblePreyEventArgs e)
        { //delegate. Send back location, ID and species. 
            if (e.SenderID != ID)
            {
                ((float X, float Y) PreyLocation, string PreyID, string PreySpeices) preyInformation = (Location, ID, Species);
                e.AddPreyInformation(preyInformation);
            }
        }
        /// <summary>
        /// Is informed that another animal is considering it food.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void IsPreyEventHandler(object sender, ControlEvents.SetPreyEventArgs e)
        { //delegate. Take the ID of the predator and add it to the array. 
            if (e.IDs.receiverID == ID)
            {
                string[] array = HuntedBy;
                helper.Add(ref array, e.IDs.senderID);
                HuntedBy = array;
            }
        }
        /// <summary>
        /// Its predator is dead or have lost this animal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void RemovePredatorEventHandler(object sender, ControlEvents.RemovePreyEventArgs e)
        { //delegate. The predator has died or is lost to this animal. 
            if (e.IDs.senderID != ID)
                if (helper.Contains(HuntedBy, e.IDs.senderID))
                    helper.Remove(HuntedBy, e.IDs.senderID);
        }
        protected virtual void LocationEventHandler(object sender, ControlEvents.GetOtherLocationEventArgs e)
        { //delegate. Someone needs this one's location.
            if (e.ReceiverID == ID)
                e.Location = Location;
        }
        protected virtual void EatenEventHandler(object sender, ControlEvents.EatenEventArgs e)
        { //delegate. This animal has been eaten.
            if (e.ReceiverID == ID)
            {
                e.SetNutrience(NutrienValue);
                Death();
            }
        }
        protected virtual void DeathEventHandler(object sender, ControlEvents.DeadEventArgs e)
        { //delegate. This animal has died. E.g. fought to death.
            if (e.ReceiverID == ID)
                Death();
        }
        /// <summary>
        /// Asked to return information that permits the animal to be drawned.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DrawEventHandler(object sender, ControlEvents.DrawEventArgs e)
        { //delegate. Transmit location, design and colour back.
            if (Design != null)
            {
                (Point[] Design, (byte Red, byte Green, byte Blue), (float X, float Y) Location) drawInforamtion = (helper.DeepCopy(Design), Colour, Location); //(type,type) will ac
                e.AddDrawInformation(drawInforamtion);
            }
        }
        /// <summary>
        /// Asked to run a sequence of its AI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ControlEventHandler(object sender, ControlEvents.AIEventArgs e)
        { //delgate
            timeSinceLastUpdate = e.TimeSinceLastUpdate;
            AI();
        }
        protected virtual void RemoveSubscriptions()
        {
            animalPublisher.RaiseFindPreyEvent -= IsPossiblePreyEventHandler;
            animalPublisher.RaiseSetPreyEvent -= IsPreyEventHandler;
            animalPublisher.RaiseRemovePreyEvent -= RemovePredatorEventHandler;
            animalPublisher.RaiseAIEvent -= ControlEventHandler;
            animalPublisher.RaiseDied -= DeathEventHandler;
            animalPublisher.RaiseEaten -= EatenEventHandler;
            animalPublisher.RaiseGetLocation -= LocationEventHandler;

            drawPublisher.RaiseDrawEvent -= DrawEventHandler;
        }
    }
}
