using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class BirdCarnivore : Carnivore, IBird
    {
        protected Vector[] circleLocations = new Vector[0];
        protected bool isDiving = false;

        public override float AttackRange { get; set; }
        public override float AttackSpeedMultiplier { get; set; }

        public bool CanDive { get; }

        public float MaximumHeight { get; }

        public float AscendSpeed { get; }

        public float DesendSpeed { get; }

        public float AscendModifier { get; }

        public float DesendModifier { get; }

        public float HoverModifier { get; }

        public float DiveSpeed { get; }

        public float CircleRange { get; }

        public float CurrentModifier { get; set; }

        public BirdCarnivore(string species, Vector location, string[] foodSource, IHelper helper, LifeformPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, animalPublisher, drawPublisher, mapInformation)
        {
            MaximumHeight = 100;
            CanDive = true;
            AscendSpeed = 8f;
            DiveSpeed = 26f;
            DesendSpeed = 12f;
            AscendModifier = 1.3f; //mention in the xml that 1 is 100 %
            DesendModifier = 0.8f;
            HoverModifier = 0.4f;
            CircleRange = 120f;

            MovementSpeed = 20;

            AttackRange = 30;
            AttackSpeedMultiplier = 1.1f;

            reproductionCooldown = 40;
            lengthOfReproduction = 4;

            Design = new Point[] {new Point(0,0), new Point(3,3), new Point(6,0), new Point(3,6) };
            Colour = new Colour(100, 255, 255);
        }

        /// <summary>
        /// Does not use the base' version...
        /// </summary>
        protected override void TimeUpdate()
        {
            timeAlive += timeSinceLastUpdate;
            Age = timeAlive / OneAgeInSeconds;
            TimeToReproductionNeed -= timeSinceLastUpdate;
            if (periodInReproduction < lengthOfReproduction && HasReproduced)
                periodInReproduction += timeSinceLastUpdate;
            if (Health < MaxHealth)
                Health = Health + timeSinceLastUpdate > MaxHealth ? MaxHealth : Health + timeSinceLastUpdate;
            
            //readd the outcommented code when it got a food source and can hunt.
            Hunger -= timeSinceLastUpdate * CurrentModifier;
            if (Hunger < 0)
                Health -= timeSinceLastUpdate;

        }

        protected override void AI()
        {
            UpdateAlpha();
            //do stuff //need a full overriding of all TimeUpdate()s, since currentModifier needs to be used.
            TimeUpdate(); //when the bird does something that is not moving in circle, the circleLocations should set to empty.
            if (Age >= MaxAge || Health <= 0)
                Death();
            else 
            { 
                if (Gender == 'f')
                    if (HasReproduced)
                        if (periodInReproduction >= lengthOfReproduction)
                            Reproduce();
                if(Hunger < MaxHunger * HungerFoodSeekingLevel || Hunger < MaxHunger * 0.1)
                {
                    if (foodID == null)
                        foodID = FindFood();
                    if(foodID != null)
                    {
                        TrackPrey();
                        Move();
                        AttackPrey();
                    }
                    else
                        DefaultMovement();
                }
                else if (Age >= ReproductionAge && TimeToReproductionNeed <= 0 && !HasReproduced)
                {
                    if (mateID == null)
                        mateID = FindMate();
                    if (mateID != null)
                    {
                        circleLocations = new Vector[0];
                        MoveTo = MateLocation = GetLifeformLocation(mateID);
                        Move();
                        Mate();
                    }
                    else
                        DefaultMovement();
                }
                else 
                { 
                    if (circleLocations.Length == 0) 
                        circleLocations = Circle();
                    if (Vector.Compare(Location,MoveTo))
                    {
                        CurrentMovementSpeed = MovementSpeed;
                        MoveTo = circleLocations[0];
                        Vector[] locations = circleLocations;
                        helper.Remove(ref locations, circleLocations[0]);
                        circleLocations = locations;
                    }
                    Move();
                }

            }
            void DefaultMovement()
            {
                if (Vector.Compare(Location, MoveTo))
                    MoveTo = GenerateRandomEndLocation();
                CurrentMovementSpeed = MovementSpeed;
                Move();
            }
            //do stuff
        }

        /// <summary>
        /// ... Overridden to focus on preys that are below the predator and within a specific (x,y) distance
        /// </summary>
        /// <returns></returns>
        protected override string FindNearestFood()
        {
            string nearestFood = null;
            float distance = Single.MaxValue;
            List<(Vector PreyLocation, string PreyID, string PreySpecies)> possiblePreys = lifeformPublisher.GetPossiblePreys(ID);
            foreach ((Vector Location, string PreyID, string Species) information in possiblePreys)
            {
                if (helper.Contains(FoodSource, information.Species))
                {

                    float distanceTo = 0; //if the prey is within a specific distance, the z value should weigh more than the rest 
                    if(!CanDive)
                        distanceTo = information.Location.DistanceBetweenVectors(Location);
                    else
                    {
                        float zWeight = 0;
                        float distanceWithoutZ = (float)Math.Sqrt(Math.Pow(Math.Abs(Location.X - information.Location.X),2)+ Math.Pow(Math.Abs(Location.Y - information.Location.Y),2));
                        if(distanceWithoutZ < 60) //have a property or variable at some point. 
                        {
                            if (distanceWithoutZ != 0)
                                zWeight = (float)Math.Log(distanceWithoutZ / (60)) * 6;
                            else //Log(0.01/60)*6 == -52.197
                                zWeight = (float)Math.Log(0.01 / (60)) * 6;
                        }
                        distanceTo = information.Location.DistanceBetweenVectors(Location) + zWeight; 
                    }
                    if (distanceTo < distance)
                    {
                        distance = distanceTo;
                        nearestFood = information.PreyID;
                    }

                }
            }
            return nearestFood;
        }

        /// <summary>
        /// ... overridden to set the Z value.
        /// </summary>
        /// <returns></returns>
        protected override Vector GenerateRandomEndLocation()
        {
            Vector location =  base.GenerateRandomEndLocation();
            location.Z = helper.GenerateRandomNumber(0, (int)MaximumHeight);
            return location;
        }

        public override void AttackPrey()
        { 
            Vector preyLocation = lifeformPublisher.GetLocation(foodID); 
            float distance = preyLocation.DistanceBetweenVectors(Location);
            if (distance == 0)
            {
                Eat();
                CurrentMovementSpeed = MovementSpeed;
                if (isDiving)
                    isDiving = false;
            }
        }

        protected override void Move()
        {
            float xDistance = Math.Abs(MoveTo.X - Location.X);
            float yDistance = Math.Abs(MoveTo.Y - Location.Y);
            float zDistance = Math.Abs(MoveTo.Z - Location.Z);
            float distanceToEndLocation = (float)Math.Sqrt(Math.Pow(xDistance,2) + Math.Pow(yDistance,2) + Math.Pow(zDistance,2));
            if(distanceToEndLocation != 0)
            {
                float xPercentage = xDistance / distanceToEndLocation;
                float yPercentage = yDistance / distanceToEndLocation;
                float zPercentage = zDistance / distanceToEndLocation;

                float zSpeed;
                //it will switch from hover mode to one of the other modes
                if ((MoveTo.Z - Location.Z) < 0 || (MoveTo.Z - Location.Z) > 0) //let the numbers get modified by the distance, i.e. it require less energy to move 5 up over 200 distance compared to move 5 up over 1 distance 
                { //how to best use the ascend-/desendSpeeds 
                    if ((MoveTo.Z - Location.Z) > 0)
                    {
                        zSpeed = AscendSpeed;
                        CurrentModifier = AscendModifier;
                    }
                    else
                    {
                        if (!isDiving)
                            zSpeed = DesendSpeed;
                        else
                            zSpeed = DiveSpeed;
                        CurrentModifier = DesendModifier;
                    }
                }
                else
                {
                    CurrentModifier = HoverModifier;
                    zSpeed = 0;
                }

                float xCurrentSpeed = xPercentage * CurrentMovementSpeed * timeSinceLastUpdate;
                float yCurrentSpeed = yPercentage * CurrentMovementSpeed * timeSinceLastUpdate;
                float zCurrentSpeed = zPercentage * zSpeed * timeSinceLastUpdate;

                bool moveLeft = (MoveTo.X - Location.X) < 0;
                bool moveUp = (MoveTo.Y - Location.Y) < 0;

                xCurrentSpeed = xCurrentSpeed >= xDistance ? xDistance : xCurrentSpeed;
                yCurrentSpeed = yCurrentSpeed >= yDistance ? yDistance : yCurrentSpeed;
                zCurrentSpeed = zCurrentSpeed >= zDistance ? zDistance : zCurrentSpeed;

                if (CurrentModifier == DesendModifier)
                    zCurrentSpeed = -zCurrentSpeed;

                if (moveLeft)
                    xCurrentSpeed = -xCurrentSpeed;
                if (moveUp)
                    yCurrentSpeed = -yCurrentSpeed;

                Location = new Vector(Location.X + xCurrentSpeed, Location.Y + yCurrentSpeed, Location.Z + zCurrentSpeed);
            }
        }

        public Vector[] Circle()
        {
            string nearestFood = FindNearestFood();
            Vector location;
            if (nearestFood != null)
            {
                location = GetLifeformLocation(nearestFood);
                //add some deviantion here
                float xDeviantionPercentage = helper.GenerateRandomNumber(-100,100)/100f;
                float distance = 30;
                float xDeviantionDistance = xDeviantionPercentage * distance;
                float yDeviantionDistance = (1 - xDeviantionPercentage) * distance;
                location.X += xDeviantionDistance;
                location.Y += yDeviantionDistance;
                if (location.Z == 0)
                    location.Z = helper.GenerateRandomNumber(0, (int)MaximumHeight);
            }
            else
            {
                location = new Vector(mapInformation, helper, MaximumHeight);
            }

            if (location.Z > MaximumHeight)
                location.Z = MaximumHeight;

            //find the 'center' point to circle around
            float xPercent = helper.GenerateRandomNumber(0, 100) / 100f;
            float xMaxDistance = xPercent * MaxFoodDistanceRange;
            float yMaxDistance = (1 - xPercent) * MaxFoodDistanceRange;
            float xDistance = (helper.GenerateRandomNumber(0, (int)xMaxDistance)) - (xMaxDistance / 2);
            float yDistance = (helper.GenerateRandomNumber(0, (int)yMaxDistance)) - (yMaxDistance / 2);
            Vector centerLocation = new Vector(location.X + xDistance, location.Y + yDistance, location.Z);

            if (centerLocation.X - CircleRange < 0 || centerLocation.X + CircleRange >= mapInformation.GetSizeOfMap.width) //constrain end location the be inside of the map and with a distance to the edge that allows the bird to circle.
            {
                if (centerLocation.X - CircleRange < 0)
                    centerLocation.X = helper.GenerateRandomNumber(0,10) + CircleRange; //the helper.GenerateRandomNumber(...,...) is in use to add deviations, else the birds would be to close to eachother.
                else
                    centerLocation.X = mapInformation.GetSizeOfMap.width - helper.GenerateRandomNumber(1, 11) - CircleRange;
            }
            if (centerLocation.Y - CircleRange < 0 || centerLocation.Y + CircleRange >= mapInformation.GetSizeOfMap.height)
            {
                if (centerLocation.Y - CircleRange < 0)
                    centerLocation.Y = helper.GenerateRandomNumber(0, 10) + CircleRange;
                else
                    centerLocation.Y = mapInformation.GetSizeOfMap.height - helper.GenerateRandomNumber(1, 11) - CircleRange;
            }

            Vector[] circlePoints = new Vector[7];
            (float X, float Y)[] rename = new (float X, float Y)[] { (-1, 0), (-0.5f, 1), (0.5f, 1), (1, 0), (0.5f, -1), (-0.5f, -1) };
            //calculate six points the bird has to fly to to move in a circle
            float radius = CircleRange / 2f;
            for (byte i = 0; i < circlePoints.Length - 1; i++) //how to calculate the points
            {
                float x = rename[i].X * radius;
                float y = rename[i].Y * radius;

                float zDeviation = helper.GenerateRandomNumber(-8, 8) / 20f;
                float z = centerLocation.Z * zDeviation;
                if (z < 0 || z > MaximumHeight)
                {
                    if (z < 0)
                        z = 0;
                    else
                        z = MaximumHeight;
                }
                circlePoints[i] = new Vector(centerLocation.X + x, centerLocation.Y + y, centerLocation.Z + z);

            }
            circlePoints[circlePoints.Length - 1] = circlePoints[0];
            return circlePoints;

        }

        /// <summary>
        /// The bird will keep a certain z height different, until it is withit attack range.
        /// </summary>
        /// <remarks>If the bird is within attack range, the bool isDiving will be set to true, else false.</remarks>
        public override void TrackPrey()
        { //should track the prey, but keep a Z difference.
            Vector preyLocation = lifeformPublisher.GetLocation(foodID);
            float xDistance = Math.Abs(preyLocation.X - Location.X);
            float yDistance = Math.Abs(preyLocation.Y - Location.Y);
            //float zDistance = Math.Abs(preyLocation.Z - Location.Z);

            float distanceToPreyWithoutZ = (float)Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2));
            if(distanceToPreyWithoutZ > AttackRange)
            {
                float zLocation = preyLocation.Z + 30; //non-hardcode 30 later
                if (zLocation > MaximumHeight)
                    zLocation = MaximumHeight;
                MoveTo = new Vector(preyLocation.X, preyLocation.Y, zLocation);
                isDiving = false;
            }
            else
            {
                isDiving = true;
                CurrentMovementSpeed = MovementSpeed * AttackSpeedMultiplier;
                MoveTo = preyLocation;
            }
        }

        public void UpdateAlpha()
        {
            float alphaPercentage = 1 - Location.Z / (MaximumHeight+40); //the + 40 is not the best solution as it will affect the final number differently depending on MaximumHeight.
            byte newAlpha = (byte)(byte.MaxValue * alphaPercentage); //maybe (MaximumHeight+MaximumHeight*0.4) or something like that.
            if (Colour.Alpha != newAlpha)
                Colour = new Colour(Colour.Red, Colour.Green, Colour.Blue, newAlpha);
        }
    }
}
