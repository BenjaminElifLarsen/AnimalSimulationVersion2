#undef DEBUG

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
        /// The possible genders of the animal with weights.
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
        protected Vector MoveTo { get; set; }
        /// <summary>
        /// The current movementspeed of the animal per second. 
        /// </summary>
        protected float CurrentMovementSpeed { get; set; }
        /// <summary>
        /// The current location of the mate.  
        /// </summary>
        protected Vector MateLocation { get; set; }
        /// <summary>
        /// The % of hunger in form of 0.n that should get the animal to hunt //rewrite
        /// </summary>
        protected float HungerFoodSeekingLevel { get; set; }
        /// <summary>
        /// The maximum distance the animal can be from the nearest food source. 
        /// </summary>
        protected float MaxFoodDistanceRange { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected float FindMateCooldown { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected float FindFoodCooldown { get; set; }
        /// <summary>
        /// Default constructor. Initialises properites and variables to 'default' values.
        /// </summary>
        /// <param name="species">The species of this animal.</param>
        /// <param name="location">The start location of this animal.</param>
        /// <param name="foodSource">The food source of this animal.</param>
        /// <param name="helper">An instance of IHelper.</param>
        /// <param name="lifeformPublisher">An instance of AnimalPublisher.</param>
        /// <param name="drawPublisher">An instance of DrawPublisher.</param>
        /// <param name="mapInformation">An instance of MapInformation.</param>
        public Animalia(string species, Vector location, string[] foodSource, IHelper helper, LifeformPublisher lifeformPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : this(species, location,helper, lifeformPublisher, drawPublisher, mapInformation)
        {
            MateLocation = Vector.Copy(location);
            FoodSource = foodSource;
            MoveTo = Vector.Copy(location);
            MovementSpeed = 10;
            CurrentMovementSpeed = MovementSpeed;

            MaxFoodDistanceRange = 100;

            MaxHunger = 80;
            HungerFoodSeekingLevel = 0.5f;
            Hunger = MaxHunger;

            ReproductionAge = MaxAge * 0.25f;
            lengthOfReproduction = 9;
            reproductionCooldown = 20;
            BirthAmount = (1, 3);
            genderInformation = new (char Gender, byte Weight)[] { ('f', 50), ('m', 50) };
            Gender = GenerateGender(genderInformation);

        }
        /// <summary>
        /// Extra constructor that sets all Animalia eventhandlers //rewrite
        /// </summary>
        /// <param name="species">The species of this animal.</param>
        /// <param name="location">The start location of this animal.</param>
        /// <param name="helper">An instance of IHelper.</param>
        /// <param name="lifeformPublisher">An instance of AnimalPublisher.</param>
        /// <param name="drawPublisher">An instance of DrawPublisher.</param>
        /// <param name="mapInformation">An instance of MapInformation.</param>
        protected Animalia(string species, Vector location, IHelper helper, LifeformPublisher lifeformPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, helper, lifeformPublisher, drawPublisher, mapInformation)
        {
            this.lifeformPublisher.RaisePossibleMatesEvent += CanMateEventHandler;
            this.lifeformPublisher.RaiseSetMateEvent += GetMateEventHandler;
            this.lifeformPublisher.RaiseRemoveMateEvent += RemoveMateEventHandler;
            this.lifeformPublisher.RaiseInformHunterOfPreyDeath += PreyHasDiedEventHandler;
            this.lifeformPublisher.RaisePregnacy += PregnacyEventHandler;
        }

        protected override bool DeathCheckAI()
        {
            if (Age >= MaxAge || Health <= 0)
            {
                Death();
                return true;
            }
            return false;
        }
        protected abstract bool GiveBirthAI();
        protected abstract bool HungerAI();
        protected abstract bool ReproductionAI();
        protected abstract bool MovementAI();

        /// <summary>
        /// Moves the animal.
        /// </summary>
        protected virtual void Move() 
        {
            float xDistance = Math.Abs(MoveTo.X - Location.X);
            float yDistance = Math.Abs(MoveTo.Y - Location.Y);
            float distanceToEndLocation = (float)Math.Sqrt(Math.Pow(xDistance,2) + Math.Pow(yDistance,2));
            if (distanceToEndLocation != 0)
            {
                //calculates the %s of the move distance that belong to x and y and then multiply those numbers with the current movement speed. 
                float xPercentage = Math.Abs(MoveTo.X - Location.X) / distanceToEndLocation; //could just use xDistance to optimise the code a little bit
                float xCurrentSpeed = xPercentage * CurrentMovementSpeed * timeSinceLastUpdate; //multiply with the amount of seconds that have gone.
                float yCurrentSpeed = (1 - xPercentage) * CurrentMovementSpeed * timeSinceLastUpdate;
                //calculates the direction to move in for each axel. 

                bool moveLeft = (MoveTo.X - Location.X) < 0;
                bool moveUp = (MoveTo.Y - Location.Y) < 0;

                xCurrentSpeed = xCurrentSpeed >= xDistance ? xDistance : xCurrentSpeed;
                yCurrentSpeed = yCurrentSpeed >= yDistance ? yDistance : yCurrentSpeed;

                if (moveLeft)
                    xCurrentSpeed = -xCurrentSpeed;
                if (moveUp)
                    yCurrentSpeed = -yCurrentSpeed;

                //set the new location
                Location = new Vector(Location.X + xCurrentSpeed, Location.Y + yCurrentSpeed,0);
            }
        }
        /// <summary>
        /// Updates all time sensitive variables. 
        /// </summary>
        /// <remarks>
        /// By default this is: 
        /// timeAlive, Age, Hunger and TimeToProductionNeed
        /// </remarks>
        protected override void TimeUpdate()
        {
            base.TimeUpdate();
            Hunger -= timeSinceLastUpdate;
            if (Hunger < 0)
                Health -= timeSinceLastUpdate;
            if (FindMateCooldown > 0)
                FindMateCooldown -= timeSinceLastUpdate;
            if (FindFoodCooldown > 0)
                FindFoodCooldown -= timeSinceLastUpdate;
        }
        /// <summary>
        /// Animal produces offsprings at the same location as it.
        /// Sets HasReproduced to false. 
        /// </summary>
        ///<remarks>If any class that inhiret from Animalia got a different constructor, this method needs to be overwritten.</remarks>
        protected override void Reproduce()
        {
            object[] dataObject = new object[] { Species, Location, FoodSource, helper, lifeformPublisher, drawPublisher, mapInformation };

            byte childAmount = (byte)helper.GenerateRandomNumber(BirthAmount.Minimum, BirthAmount.Maximum); //seems like wolves mate for life, but if losing a mate, they will quickly find another one.
            for (int i = 0; i < childAmount; i++) //perhaps use the Activator.CreateInstance(...) and this method takes a Type argument, then this implementation could be moved up to Animalia
                Activator.CreateInstance(GetType(), dataObject);//new SleepingCarnivore(Species, Location, FoodSource, helper, animalPublisher, drawPublisher, mapInformation);
            HasReproduced = false;
        }
        /// <summary>
        /// Generates a random end location on the map. X and Y will each be between 0 and the maximum value of their respective maximum possible distance.
        /// Z will be 0.
        /// </summary>
        /// <returns>Returns a new X and Y coordinate for the animal to move too.</returns>
        protected virtual Vector GenerateRandomEndLocation()
        {
            string food = FindNearestFood(); //find food and its location
            if(food != null) 
            { 
                Vector foodLocation = GetLifeformLocation(food);

                float xPercent = helper.GenerateRandomNumber(0, 100) / 100f; //calculate the new end location
                float xMaxDistance = xPercent * MaxFoodDistanceRange;
                float yMaxDistance = (1 - xPercent) * MaxFoodDistanceRange;
                float xDistance = (helper.GenerateRandomNumber(0, (int)xMaxDistance)) - (xMaxDistance / 2);
                float yDistance = (helper.GenerateRandomNumber(0, (int)yMaxDistance)) - (yMaxDistance / 2);
                Vector newEndLocation = new Vector(foodLocation.X + xDistance, foodLocation.Y + yDistance, foodLocation.Z);

                if(newEndLocation.X < 0 || newEndLocation.X >= mapInformation.GetSizeOfMap.width) //constrain end location the be inside of the map.
                {
                    if (newEndLocation.X < 0)
                        newEndLocation.X = 0;
                    else
                        newEndLocation.X = mapInformation.GetSizeOfMap.width - 1;
                }
                if (newEndLocation.Y < 0 || newEndLocation.Y >= mapInformation.GetSizeOfMap.height)
                {
                    if (newEndLocation.Y < 0)
                        newEndLocation.Y = 0;
                    else
                        newEndLocation.Y = mapInformation.GetSizeOfMap.height - 1;
                }
                return newEndLocation;
            }
            return new Vector(helper.GenerateRandomNumber(0, mapInformation.GetSizeOfMap.width - 1), helper.GenerateRandomNumber(0, mapInformation.GetSizeOfMap.height - 1), 0);
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
            List<(string mateID, Vector Location)> possibleMates = lifeformPublisher.PossibleMates(Species, Gender, ID);
            foreach ((string Mate, Vector Location) information in possibleMates)
            {
                float distanceTo = Math.Abs((information.Location.X - Location.X)) + Math.Abs((information.Location.Y - Location.Y));
                if (distanceTo < distance)
                {
                    distance = distanceTo;
                    nearestMate = information.Mate;
                }
            }
            if(nearestMate != null)
            lifeformPublisher.SetMate(ID, nearestMate);
            return nearestMate; 
        }
        /// <summary>
        /// Gets the current location of the lifeform with ID of <paramref name="lifeformID"/>.
        /// </summary>
        /// <param name="lifeformID">The ID of the lifeform.</param>
        /// <returns>Returns the location of the lifeform.</returns>
        protected virtual Vector GetLifeformLocation(string lifeformID)
        {
            return lifeformPublisher.GetLocation(lifeformID);
        }
        /// <summary>
        /// Animal mates if they are on the same posistion. By default the method assumes the child caring member got the gender of 'f'.
        /// </summary>
        /// <remarks>By default the gender, 'f', will be pregnant.
        /// If a specific gender needs to be pregant, this method needs to be overwritten.
        /// Also, by default it will not set mateID to null.</remarks>
        protected virtual void Mate()
        {
            if (Vector.Compare(Location, MateLocation) && !HasReproduced)
            {
                lifeformPublisher.Pregnacy(ID, mateID, Gender != 'f');
                TimeToReproductionNeed = reproductionCooldown;
                if(Gender == 'f')
                {
                    HasReproduced = true;
                    periodInReproduction = 0;
                }
            }
        }

        /// <summary>
        /// Generates a gender for the animal  out from the data in <paramref name="genderInfo"/>.
        /// </summary>
        /// <param name="genderInfo">Contains a set of possible genders and gender weights</param>
        /// <returns></returns>
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
        protected virtual string FindFood()
        {
            string nearestFood = FindNearestFood();
            if(nearestFood != null)
                lifeformPublisher.SetPrey(ID, nearestFood);
            return nearestFood;
        }
        /// <summary>
        /// Used to find the nearest food and returns its ID.
        /// </summary>
        /// <returns>The ID of the nearest food or null if no food is present.</returns>
        protected virtual string FindNearestFood()
        {
            string nearestFood = null;
            float distance = Single.MaxValue;
            List<(Vector PreyLocation, string PreyID, string PreySpecies)> possiblePreys = lifeformPublisher.GetPossiblePreys(ID);
            foreach ((Vector Location, string PreyID, string Species) information in possiblePreys)
            { //move distanceTo calculation into the if-statement  scope with the check to minimize computational cost
                float distanceTo = information.Location.DistanceBetweenVectors(Location);
                if (helper.Contains(FoodSource, information.Species))
                    if (distanceTo < distance)
                    {
                        distance = distanceTo;
                        nearestFood = information.PreyID;
                    }
            }
            return nearestFood;
        }

        /// <summary>
        /// Animal eats food
        /// </summary>
        protected virtual void Eat()
        {
            Hunger += lifeformPublisher.Eat(foodID);
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
                lifeformPublisher.RemoveMate(ID, mateID);
            }
            if (foodID != null)
            {
                lifeformPublisher.RemovePrey(ID, foodID);
            }
            base.Death();
        }
        protected virtual void PreyHasDiedEventHandler(object sender, ControlEvents.InformPredatorOfPreyDeathEventArgs e)
        { //delegate. The prey has died. //rename this event handler since it is also used for losing a prey
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
        /// 
        /// </summary>
        /// <remarks>By default it will not set mateID to null.</remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void PregnacyEventHandler(object sender, ControlEvents.PregnacyEventArgs e)
        { //Delegate. The mate has mated with this animal.
            if(e.IDs.ReceiverID == ID)
            {
                periodInReproduction = 0;
                TimeToReproductionNeed = reproductionCooldown;
                if(e.IsPregnant)
                    HasReproduced = true;
            }
        }
        /// <summary>
        /// Remove all subscriptions to ensure the animal can be removed from the memory.
        /// </summary>
        protected override void RemoveSubscriptions() //consider renaming some of the methods to have names that make more sense
        {
            base.RemoveSubscriptions();
            lifeformPublisher.RaisePossibleMatesEvent -= CanMateEventHandler;
            lifeformPublisher.RaiseSetMateEvent -= GetMateEventHandler;
            lifeformPublisher.RaiseRemoveMateEvent -= RemoveMateEventHandler;
            lifeformPublisher.RaiseInformHunterOfPreyDeath -= PreyHasDiedEventHandler;
            lifeformPublisher.RaisePregnacy -= PregnacyEventHandler;
        }

        ~Animalia()
        {
            #if DEBUG
            System.Diagnostics.Debug.WriteLine($"{ID} of {Species} had mate {mateID} and food {foodID}");
            #endif
        }
    }
}
