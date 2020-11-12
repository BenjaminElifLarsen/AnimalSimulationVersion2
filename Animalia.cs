using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    abstract class Animalia : Eukaryote
    {      
        
        /// <summary>
        /// The ID of the food that is hunted.
        /// </summary>
        protected string foodID;
        /// <summary>
        /// The ID of the mate.
        /// </summary>
        protected string mateID;
        /// <summary>
        /// The possible genders of the animal and the change for a specific gender.
        /// </summary>
        protected (char Gender, byte Weight)[] genderInformation;

        /// <summary>
        /// The gender of the animal.
        /// </summary>
        protected char Gender { get; set; }
        /// <summary>
        /// The minimum and maximum of children the animal can get in one reproduction.
        /// </summary>
        protected (byte Minimum, byte Maximum) BirthAmount { get; set; }
        /// <summary>
        /// The movement speed per second of the animal.
        /// </summary>
        protected float MovementSpeed { get; set; }
        /// <summary>
        /// The current hunger level of the animal.
        /// </summary>
        protected float Hunger { get; set; }
        /// <summary>
        /// The maximum hunger level of the animal.
        /// </summary>
        protected float MaxHunger { get; set; }
        /// <summary>
        /// The source of food for the animal.
        /// </summary>
        protected string[] FoodSource { get; set; }
        /// <summary>
        /// The end location the animal is moving to.
        /// </summary>
        protected (float X, float Y) MoveTo { get; set; }
        /// <summary>
        /// The current movementspeed of the animal per second. 
        /// </summary>
        protected float CurrentMovementSpeed { get; set; }
        /// <summary>
        /// The current location of the mate.  
        /// </summary>
        protected (float X, float Y) MateLocation { get; set; }
        /// <summary>
        /// The % of hunger in form of 0.n that should get the animal to hunt //rewrite
        /// </summary>
        protected float HungerFoodSeekingLevel { get; set; }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="species"></param>
        /// <param name="location"></param>
        /// <param name="foodSource"></param>
        /// <param name="helper"></param>
        /// <param name="animalPublisher"></param>
        /// <param name="drawPublisher"></param>
        /// <param name="mapInformation"></param>
        public Animalia(string species, (float X, float Y) location, string[] foodSource, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : this(species, location,helper, animalPublisher, drawPublisher, mapInformation)
        {
            FoodSource = foodSource;
            MoveTo = GenerateRandomEndLocation();
        }
        ///// <summary>
        ///// Extra constructor that sets all instances and eventhandlers //rewrite
        ///// </summary>
        ///// <param name="helper">An instance of IHelper.</param>
        ///// <param name="animalPublisher">An instance of AnimalPublisher.</param>
        ///// <param name="drawPublisher">An instance of DrawPublisher.</param>
        ///// <param name="mapInformation">An instance of MapInformation.</param>
        //private Animalia(IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation)
        //{

        //    animalPublisher.RaiseFindPreyEvent += IsPossiblePreyEventHandler;
        //    animalPublisher.RaiseSetPreyEvent += IsPreyEventHandler;
        //    animalPublisher.RaiseRemovePreyEvent += RemovePredatorEventHandler;
        //    animalPublisher.RaisePossibleMatesEvent += CanMateEventHandler;
        //    animalPublisher.RaiseSetMateEvent += GetMateEventHandler;
        //    animalPublisher.RaiseRemoveMateEvent += RemoveMateEventHandler;
        //    animalPublisher.RaiseAIEvent += ControlEventHandler;
        //    animalPublisher.RaiseDied += DeathEventHandler;
        //    animalPublisher.RaiseEaten += EatenEventHandler;
        //    animalPublisher.RaiseGetLocation += LocationEventHandler;
        //    animalPublisher.RaiseInformHunterOfPreyDeath += PreyHasDiedEventHandler;

        //    drawPublisher.RaiseDrawEvent += DrawEventHandler;
        //}
        /// <summary>
        /// Extra constructor that sets all Animalia eventhandlers //rewrite
        /// </summary>
        /// <param name="species"></param>
        /// <param name="location"></param>
        /// <param name="helper">An instance of IHelper.</param>
        /// <param name="animalPublisher">An instance of AnimalPublisher.</param>
        /// <param name="drawPublisher">An instance of DrawPublisher.</param>
        /// <param name="mapInformation">An instance of MapInformation.</param>
        protected Animalia(string species, (float X, float Y) location, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, helper, animalPublisher, drawPublisher, mapInformation)
        {
            animalPublisher.RaisePossibleMatesEvent += CanMateEventHandler;
            animalPublisher.RaiseSetMateEvent += GetMateEventHandler;
            animalPublisher.RaiseRemoveMateEvent += RemoveMateEventHandler;
            animalPublisher.RaiseInformHunterOfPreyDeath += PreyHasDiedEventHandler;

        }

        /// <summary>
        /// Moves the animal.
        /// </summary>
        protected abstract void Move();
        /// <summary>
        /// Updates all time sensitive variables. 
        /// </summary>
        /// <remarks>
        /// By default this is: 
        /// timeAlive, Age Hunger and TimeToProductionNeed
        /// </remarks>
        protected override void TimeUpdate()
        {
            base.TimeUpdate();
            Hunger -= timeSinceLastUpdate;
            if (Hunger < 0)
                Health -= timeSinceLastUpdate;
        }
        /// <summary>
        /// Generates a random end location on the map. X and Y will each be between 0 and the maximum value of their respective maximum possible distance.
        /// </summary>
        /// <returns>Returns a new X and Y coordinate for the animal to move too.</returns>
        protected virtual (float X, float Y) GenerateRandomEndLocation()
        {
            return (helper.GenerateRandomNumber(0,mapInformation.GetSizeOfMap.width-1), helper.GenerateRandomNumber(0, mapInformation.GetSizeOfMap.height - 1));
        }
        /// <summary>
        /// Finds a mate for the animal and informs the other animal that it got a mate.
        /// </summary>
        /// <remarks>It will return null if no mate can be found</remarks>
        /// <returns>The ID of the mate if found. If no mate is found it returns null.</returns>
        protected virtual string FindMate()
        {
            string nearestMate = null;
            float distance = Single.MaxValue;
            List<(string mateID, (float X, float Y) Location)> possibleMates = animalPublisher.PossibleMates(Species, Gender, ID);
            foreach ((string Mate, (float X, float Y) Location) information in possibleMates)
            {
                float distanceTo = Math.Abs((information.Location.X - Location.X)) + Math.Abs((information.Location.Y - Location.Y));
                if (distanceTo < distance)
                {
                    distance = distanceTo;
                    nearestMate = information.Mate;
                }
            }
            if(nearestMate != null)
            animalPublisher.SetMate(ID, nearestMate);
            return nearestMate; 
        }
        /// <summary>
        /// Gets the current location of the Mate.
        /// </summary>
        /// <param name="mateID">The ID of the mate.</param>
        /// <returns>Returns the location of the mate.</returns>
        protected virtual (float X, float Y) GetMateLocation(string mateID)
        {
            return animalPublisher.GetLocation(mateID);
        }
        /// <summary>
        /// Animal mates.
        /// </summary>
        protected abstract void Mate();

        //protected abstract string GenerateID();
        protected virtual char GenerateGender((char Gender, byte Weight)[] genderInfo) //it should take an array of possible genders and a % for each of them.
        {
            ushort totalWeight = 0;
            ushort startLocation;
            List<(char Gender, ushort StartLocation, ushort EndLocation)> genderStartEndLocation = new List<(char, ushort, ushort)>();
            foreach((char Gender, byte Weight) information in genderInfo)
            {
                startLocation = totalWeight;
                totalWeight += information.Weight;
                genderStartEndLocation.Add((information.Gender, startLocation, totalWeight));
            }

            //generate a random number
            ushort rolledWeight = (ushort)helper.GenerateRandomNumber(0, totalWeight); //from zero up to and with the totalWeight
            
            for (int i = 0; i < genderStartEndLocation.Count; i++)
            {
                if (rolledWeight >= genderStartEndLocation[i].StartLocation && rolledWeight < genderStartEndLocation[i].EndLocation)
                    return genderStartEndLocation[i].Gender;
            }
            return genderStartEndLocation[0].Gender; 
        }
        /// <summary>
        /// Finds food and informs the food that it has been found.
        /// </summary>
        /// <remarks>It will return null if no food can be found</remarks>
        /// <returns>The ID of the food if found. If no food is found it returns null.</returns>
        protected virtual string FindFood()//maybe have a property for when the animal should start looking for food that is compared to Hunger (not used in this method but rather as a check to see if this method should be called)
        {
            string nearestFood = null;
            float distance = Single.MaxValue;
            List<((float X, float Y) PreyLocation, string PreyID, string PreySpecies)> possiblePreys = animalPublisher.GetPossiblePreys(ID);
            foreach (((float X, float Y) Location, string PreyID, string Species) information in possiblePreys)
            {
                float distanceTo = Math.Abs((information.Location.X - Location.X)) + Math.Abs((information.Location.Y - Location.Y));
                if (helper.Contains(FoodSource, information.Species))
                    if (distanceTo < distance)
                    {
                        distance = distanceTo;
                        nearestFood = information.PreyID;
                    }
            }
            if(nearestFood != null)
                animalPublisher.SetPrey(ID, nearestFood);
            return nearestFood;
        }
        /// <summary>
        /// Animal eats food
        /// </summary>
        protected virtual void Eat()
        {
            Hunger += animalPublisher.Eat(foodID);
            if (Hunger > MaxHunger)
                Hunger = MaxHunger;
        }

        /// <summary>
        /// Animal is dead.
        /// </summary>
        protected override void Death() 
        {
            if (mateID != null)
            {
                animalPublisher.RemoveMate(ID, mateID);
            }
            if (foodID != null)
            {
                animalPublisher.RemovePrey(ID, foodID);
            }
            base.Death();
            RemoveSubscriptions();
        }
        protected virtual void PreyHasDiedEventHandler(object sender, ControlEvents.InformPredatorOfPreyDeathEventArgs e)
        { //delegate. The prey has died.
            if (e.IDs.ReceiverID == ID)
                foodID = null;
        }
        /// <summary>
        /// Is asked about whether it is a possible mate for another animal or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void CanMateEventHandler(object sender, ControlEvents.PossibleMateEventArgs e)
        { //delegate. Check species, if above or is reproduction age, check if it is the corret gender and if it is, send back the ID
            if(e.SenderID != ID)
                if(Hunger > MaxHunger * HungerFoodSeekingLevel)
                    if (mateID == null)
                        if (e.Information.Species == Species)
                            if (e.Information.Gender != Gender)
                                if(TimeToReproductionNeed <= 0)
                                    if (Age >= ReproductionAge)
                                        e.AddMateInformation((ID, Location));
        }
        /// <summary>
        /// Another animal has chosen this one for its mate. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void GetMateEventHandler(object sender, ControlEvents.SetMateEventArgs e)
        { //delegate. Take the ID of the mate.
                if (e.IDs.receiverID == ID)
                    mateID = e.IDs.senderID;
        }
        /// <summary>
        /// Its mate is dead or no longer needing a mate. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void RemoveMateEventHandler(object sender, ControlEvents.RemoveMateEventArgs e)
        { //delegate. The mate is dead or no longer needing this animal.
                if (e.IDs.receiverID == ID)
                    mateID = null;
        }
        /// <summary>
        /// Remove all subscriptions to ensure the animal can be removed from the memory.
        /// </summary>
        protected override void RemoveSubscriptions() //consider renaming some of the methods to have names that make more sense
        {
            base.RemoveSubscriptions();
            animalPublisher.RaisePossibleMatesEvent -= CanMateEventHandler;
            animalPublisher.RaiseSetMateEvent -= GetMateEventHandler;
            animalPublisher.RaiseRemoveMateEvent -= RemoveMateEventHandler;
            animalPublisher.RaiseInformHunterOfPreyDeath -= PreyHasDiedEventHandler;
        }

    }
}
