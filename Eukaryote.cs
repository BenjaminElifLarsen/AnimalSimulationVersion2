using System.Drawing;

namespace AnimalSimulationVersion2
{
    /// <summary>
    /// The superclass for all lifeforms. The domain of life. 
    /// </summary>
    abstract class Eukaryote
    { 
        /// <summary>
        /// The amount of seconds this lifeform has been alive.
        /// </summary>
        protected float timeAlive;
        /// <summary>
        /// The amount of time since last update in seconds. Used together with time sensitive variables and properties.
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
        /// An instance of MapInformation.
        /// </summary>
        protected MapInformation mapInformation; //maybe have all variables as abstract, except those that is set in this class, properties so other will know to set them.
        /// <summary>
        /// An instance of IHelper.
        /// </summary>
        protected IHelper helper;
        /// <summary>
        /// An instance of AnimalPublisher.
        /// </summary>
        protected LifeformPublisher lifeformPublisher;
        /// <summary>
        /// An instance of DrawPublisher.
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
        protected Vector Location { get; set; }
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
        /// The maximum possible health of this lifeform.
        /// </summary>
        protected float MaxHealth { get; set; }
        /// <summary>
        /// Predatores of the lifeform.
        /// </summary>
        protected string[] HuntedBy { get; set; } //the IDs that are after it
        /// <summary>
        /// The nutrience value of the lifeform.
        /// </summary>
        protected float NutrientValue { get; set; }
        /// <summary>
        /// The amount of seconds that makes up a single 'year'.
        /// </summary>
        protected float OneAgeInSeconds { get; set; }

        /// <summary>
        /// True if the lifeform has reproduced and is waiting on children.
        /// </summary>
        protected bool HasReproduced { get; set; } //find a better name

        /// <summary>
        /// Sets the Species and Location and call another constructor. //rewrite
        /// </summary>
        /// <param name="species"></param>
        /// <param name="location"></param>
        /// <param name="helper"></param>
        /// <param name="animalPublisher"></param>
        /// <param name="drawPublisher"></param>
        /// <param name="mapInformation"></param>
        public Eukaryote(string species, Vector location, IHelper helper, LifeformPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : this(helper, animalPublisher, drawPublisher, mapInformation)
        {
            Species = species;
            Location = location;
            HuntedBy = new string[0];
            MaxHealth = 100;
        }
        /// <summary>
        /// Sets all instances of classes and interfaces, sets properties that depends on these and subscribes the event handlers to the events.
        /// </summary>
        /// <param name="helper">The instance of IHelper.</param>
        /// <param name="lifeformPublisher">The instance of AnimalPublisher.</param>
        /// <param name="drawPublisher">The instance of DrawPublisher.</param>
        /// <param name="mapInformation">The instance of MapInformation.</param>
        private Eukaryote(IHelper helper, LifeformPublisher lifeformPublisher, DrawPublisher drawPublisher, MapInformation mapInformation)
        {
            this.helper = helper;
            this.lifeformPublisher = lifeformPublisher;
            this.drawPublisher = drawPublisher;
            this.mapInformation = mapInformation;

            OneAgeInSeconds = mapInformation.OneAgeInSeconds;
            ID = helper.GenerateID();

            this.lifeformPublisher.RaiseFindPreyEvent += IsPossiblePreyEventHandler;
            this.lifeformPublisher.RaiseSetPreyEvent += IsPreyEventHandler;
            this.lifeformPublisher.RaiseRemovePreyEvent += RemovePredatorEventHandler;
            this.lifeformPublisher.RaiseAIEvent += ControlEventHandler;
            this.lifeformPublisher.RaiseDied += DeathEventHandler;
            this.lifeformPublisher.RaiseEaten += EatenEventHandler;
            this.lifeformPublisher.RaiseGetLocation += LocationEventHandler;
            this.lifeformPublisher.RaiseDamage += DamageEventHandler;
            this.lifeformPublisher.RaiseGetAllLocations += GetAllLocationsEventHandler;

            this.drawPublisher.RaiseDrawEvent += DrawEventHandler;
            this.drawPublisher.RaiseSpeciesAndAmountEvent += SpeciesAmountEventHandler;
        }
        /// <summary>
        /// The 'AI' of the lifeform.
        /// </summary>
        protected abstract void AI();
        /// <summary>
        /// Updates the variables and properties that depends on time.
        /// </summary>
        protected virtual void TimeUpdate()
        {
            timeAlive += timeSinceLastUpdate;
            Age = timeAlive / OneAgeInSeconds;
            TimeToReproductionNeed -= timeSinceLastUpdate;
            if (periodInReproduction < lengthOfReproduction && HasReproduced)
                periodInReproduction += timeSinceLastUpdate;
            if (Health < MaxHealth)
                Health = Health + timeSinceLastUpdate > MaxHealth ? MaxHealth : Health + timeSinceLastUpdate;
        }
        /// <summary>
        /// Lifeform produces offsprings.
        /// </summary>
        protected abstract void Reproduce();
        /// <summary>
        /// Death of the lifeform.
        /// </summary>
        protected virtual void Death()
        { 

            if (HuntedBy.Length != 0)
            {
                foreach (string hunterID in HuntedBy)
                    lifeformPublisher.InformPredatorOfPreyDeath(ID, hunterID);
            }
            RemoveSubscriptions();
        }
        /// <summary>
        /// Is asked for information such that another lifeform can decided if this lifeform is food or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void IsPossiblePreyEventHandler(object sender, ControlEvents.GetPossiblePreyEventArgs e)
        { //delegate. Send back location, ID and species. 
            if (e.SenderID != ID)
            {
                (Vector PreyLocation, string PreyID, string PreySpeices) preyInformation = (Location, ID, Species);
                e.AddPreyInformation(preyInformation);
            }
        }
        /// <summary>
        /// Is informed that another lifeform is considering it food.
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
        /// Its predator is dead or have lost this lifeform.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void RemovePredatorEventHandler(object sender, ControlEvents.RemovePreyEventArgs e)
        { //delegate. The predator has died or is lost to this lifeform. 
            if (e.IDs.senderID != ID)
                if (helper.Contains(HuntedBy, e.IDs.senderID)) //this should always be true. If it is not, then something has gone wrong.
                {
                    string[] array = HuntedBy;
                    helper.Remove(ref array, e.IDs.senderID);
                    HuntedBy = array;
                }
        }
        /// <summary>
        /// Transmit the location back of this lifeform if <paramref name="e"/> contains the ID of this lifeform.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void LocationEventHandler(object sender, ControlEvents.GetOtherLocationEventArgs e)
        { //delegate. Someone needs this one's location.
            if (e.ReceiverID == ID)
                e.Location = Vector.Copy(Location);
        }
        /// <summary>
        /// Transmit back the location and ID of this lifeform.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void GetAllLocationsEventHandler(object sender, ControlEvents.GetAllLocationsEventArgs e)
        { //delegate. Someone needs this one's location.
            if (e.SenderID != ID)
                e.Add(Location, ID);
        }
        /// <summary>
        /// This lifeform has been eaten if <paramref name="e"/> contains its ID.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void EatenEventHandler(object sender, ControlEvents.EatenEventArgs e)
        { //delegate. This lifeform has been eaten.
            if (e.ReceiverID == ID)
            {
                e.SetNutrient(NutrientValue);
                Death();
            }
        }
        /// <summary>
        /// This lifeform has died if <paramref name="e"/> contains its ID.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DeathEventHandler(object sender, ControlEvents.DeadEventArgs e)
        { //delegate. This lifeform has died. E.g. fought to death.
            if (e.ReceiverID == ID)
                Death();
        }
        /// <summary>
        /// The lifeform has taken damage.
        /// </summary>
        /// <remarks><paramref name="e"/> contains the ID of the sender, of the receiver and the amount of damage.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DamageEventHandler(object sender, ControlEvents.DoHealthDamageEventArgs e)
        { //delegate. This lifeform has taken damage.
            if(e.IDs.ReceiverID == ID)
            {
                Health -= e.Damage;
                if (Health <= 0)
                    Death();
            }
        }
        /// <summary>
        /// Asked to return information that permits the lifeform to be drawned.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DrawEventHandler(object sender, ControlEvents.DrawEventArgs e)
        { //delegate. Transmit location, design and colour back.
            if (Design != null)
            {
                (Point[] Design, (byte Red, byte Green, byte Blue), Vector Location) drawInforamtion = (helper.DeepCopy(Design), Colour, Location); //(type,type) will ac
                e.AddDrawInformation(drawInforamtion);
            }
        }
        /// <summary>
        /// Adds it species to a list in <paramref name="e"/>.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">Contains an Add(string:Species) method...</param>
        protected virtual void SpeciesAmountEventHandler(object sender, ControlEvents.SpeciesAndAmountEventArgs e)
        { //delegate. Transmit species back.
            e.Add(Species);
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
        /// <summary>
        /// Removes all subscriptions. This needs to be called when the lifeform dies else the instance will not be collected by the garbage collector.
        /// </summary>
        protected virtual void RemoveSubscriptions()
        {
            lifeformPublisher.RaiseFindPreyEvent -= IsPossiblePreyEventHandler;
            lifeformPublisher.RaiseSetPreyEvent -= IsPreyEventHandler;
            lifeformPublisher.RaiseRemovePreyEvent -= RemovePredatorEventHandler;
            lifeformPublisher.RaiseAIEvent -= ControlEventHandler;
            lifeformPublisher.RaiseDied -= DeathEventHandler;
            lifeformPublisher.RaiseEaten -= EatenEventHandler;
            lifeformPublisher.RaiseGetLocation -= LocationEventHandler;
            lifeformPublisher.RaiseDamage -= DamageEventHandler;
            lifeformPublisher.RaiseGetAllLocations -= GetAllLocationsEventHandler;

            drawPublisher.RaiseDrawEvent -= DrawEventHandler;
            drawPublisher.RaiseSpeciesAndAmountEvent -= SpeciesAmountEventHandler;
        }

        //~Eukaryote() //only here to ensure that all references to the object have been removed.
        //{
        //    System.Diagnostics.Debug.WriteLine($"{ID} of {Species} has been removed");
        //}
    }
}
