﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class BirdHerbavore : HidingHerbavore, IBird
    {
        /// <summary>
        /// The bird needs to lower its height, so it can hide.
        /// </summary>
        protected bool needToGoLower = false;
        /// <summary>
        /// Contains the end points of the circle.
        /// </summary>
        protected Vector[] circleLocations = new Vector[0];

        public bool CanDive { get; }

        public float MaximumHeight { get; }

        public float AscendSpeed { get; }

        public float DiveSpeed { get; }

        public float DesendSpeed { get; }

        public float AscendModifier { get; }

        public float DesendModifier { get; }

        public float HoverModifier { get; }

        public float CurrentModifier { get; set; }

        public float CircleRange { get; }

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
        public BirdHerbavore(string species, Vector location, string[] foodSource, IHelper helper, LifeformPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, animalPublisher, drawPublisher, mapInformation)
        {
            CanDive = false;
            MaximumHeight = 80;
            AscendSpeed = 10f;
            DiveSpeed = 0f;
            DesendSpeed = 14f;
            AscendModifier = 1.3f;
            DesendModifier = 0.7f;
            HoverModifier = 0.8f;
            CircleRange = 0;

            MovementSpeed = 18;
            BirthAmount = (1, 1);
            reproductionCooldown = 40;
            lengthOfReproduction = 4;

            Design = new Point[] { new Point(0, 0), new Point(3, 3), new Point(6, 0), new Point(3, 6) };
            Colour = new Colour(255, 255, 255);
        }

        protected override void AI()
        {
            TimeUpdate();
            if (!DeathCheckAI())
            {
                GiveBirthAI();
                if(!HidingAI())
                    if(!EscapingAI())
                        if (!HungerAI())
                            if (!ReproductionAI())
                                if (!CircleAI())
                                    MovementAI();
                UpdateAlpha();
            }
        }

        protected override Vector GenerateRandomEndLocation()
        {
            Vector location = base.GenerateRandomEndLocation();
            location.Z = helper.GenerateRandomNumber(0, (int)MaximumHeight);
            return location;
        }

        protected override void Move()
        {
            float xDistance = Math.Abs(MoveTo.X - Location.X);
            float yDistance = Math.Abs(MoveTo.Y - Location.Y);
            float zDistance = Math.Abs(MoveTo.Z - Location.Z);
            float distanceToEndLocation = (float)Math.Sqrt(Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2) + Math.Pow(zDistance, 2));
            if (distanceToEndLocation != 0)
            {
                float xPercentage = xDistance / distanceToEndLocation;
                float yPercentage = yDistance / distanceToEndLocation;
                float zPercentage = zDistance / distanceToEndLocation;

                float zSpeed;
                //it will switch from hover mode to one of the other modes
                if ((MoveTo.Z - Location.Z) < 0 || (MoveTo.Z - Location.Z) > 0)
                { 
                    if ((MoveTo.Z - Location.Z) > 0)
                    {
                        zSpeed = AscendSpeed;
                        CurrentModifier = AscendModifier;
                    }
                    else
                    {
                        zSpeed = DesendSpeed;
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
        /// <summary>
        /// Overridden to force the bird down to a low Z value, before it can hide.
        /// </summary>
        /// <returns>True if the bird is hiding.</returns>
        protected override bool HidingAI()
        {
            #region Hiding
            if (!IsHiding && HuntedBy.Length > 0 && TimeHidden < MaxHideTime && CooldownBetweenHiding <= 0)
                IsHiding = true; 
            else if (TimeHidden > MaxHideTime)
                IsHiding = false;
            if (IsHiding && Location.Z > 10)
                needToGoLower = true;
            if (needToGoLower)
            {
                Location.Z = helper.GenerateRandomNumber(0, 10);
                MoveTo = Location;
                Move();
            }
            if (IsHiding && Location.Z <= 10)
            {
                HideFromPredator();
                needToGoLower = false;
            }
            #endregion
            return IsHiding;
        }

        /// <summary>
        /// Contains the code of the AI related to circling.
        /// </summary>
        /// <returns>True if the bird is circling.</returns>
        protected virtual bool CircleAI()
        {
            if (CircleRange == 0)
                return false;
            if (circleLocations.Length == 0)
                circleLocations = Circle();
            if (Vector.Compare(Location, MoveTo))
            {
                CurrentMovementSpeed = MovementSpeed;
                MoveTo = circleLocations[0];
                Vector[] locations = circleLocations;
                helper.Remove(ref locations, circleLocations[0]);
                circleLocations = locations;
                Move();
                return true;
            }
            return false;
        }

        public Vector[] Circle()
        {

            string nearestFood = FindNearestFood();
            Vector location;
            if (nearestFood != null)
            {
                location = GetLifeformLocation(nearestFood);
                //add some deviantion here
                float xDeviantionPercentage = helper.GenerateRandomNumber(-100, 100) / 100f;
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
                    centerLocation.X = helper.GenerateRandomNumber(0, 10) + CircleRange; //the helper.GenerateRandomNumber(...,...) is in use to add deviations, else the birds would be to close to eachother.
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
            for (byte i = 0; i < circlePoints.Length - 1; i++) 
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

        public void UpdateAlpha()
        {
            float alphaPercentage = 1 - Location.Z / (MaximumHeight + MaximumHeight*0.3f); //the + 40 is not the best solution as it will affect the final number differently depending on MaximumHeight.
            byte newAlpha = (byte)(byte.MaxValue * alphaPercentage); //maybe (MaximumHeight+MaximumHeight*0.4) or something like that.
            if (Colour.Alpha != newAlpha)
                Colour = new Colour(Colour.Red, Colour.Green, Colour.Blue, newAlpha);
        }
    }
}
