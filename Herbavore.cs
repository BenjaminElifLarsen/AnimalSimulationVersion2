﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    abstract class Herbavore : Animalia, IEscapePredator
    {

        public (string ID, float TimeSinceEscape)[] LostPredators { get; set; }
        public float EscapeSpeedMultiplier { get; set; }
        public float DiscoverRange { get; set; }
        public byte DiscoverChance { get; set; }
        public string PredatorID { get; set; }
        public float TimeThresholdForBeingHuntedAgain { get; set; }
        public float EscapeSprintTime { get; set; }
        public float TimeSprinted { get; set; }
        public float EscapeDistance { get; set; }
        public bool IsRunning { get; set; }
        public float TimeBetweenRolls { get; }
        public float TimeSinceLastRoll { get; set; }
        public bool HasRolled { get; set; }
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
        public Herbavore(string species, Vector location, string[] foodSource, IHelper helper, LifeformPublisher lifeformPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, lifeformPublisher, drawPublisher, mapInformation)
        {
            Design = new Point[] { new Point(3, 0), new Point(6, 6), new Point(0, 6) };

            LostPredators = new (string ID, float TimeSinceEscape)[0];
            EscapeSpeedMultiplier = 1.4f;
            DiscoverChance = 10;
            DiscoverRange = 30;
            TimeThresholdForBeingHuntedAgain = 2;
            EscapeSprintTime = 4;
            EscapeDistance = 40;
            HasRolled = false;
            TimeBetweenRolls = 0.25f;
        }

        protected override void TimeUpdate()
        {
            base.TimeUpdate();
            if (IsRunning)
                TimeSprinted += timeSinceLastUpdate;
            else if (TimeSprinted > 0)
                TimeSprinted -= timeSinceLastUpdate;
            if (HasRolled)
                TimeSinceLastRoll += timeSinceLastUpdate;
            if (TimeSinceLastRoll >= TimeBetweenRolls)
            {
                TimeSinceLastRoll = 0;
                HasRolled = false;
            }

            if (LostPredators.Length > 0)
            {
                for (int i = 0; i < LostPredators.Length; i++)
                {
                    LostPredators[i].TimeSinceEscape += timeSinceLastUpdate;
                    if (LostPredators[i].TimeSinceEscape >= TimeThresholdForBeingHuntedAgain)
                    {
                        (string ID, float TimeSinceEscape)[] predators = LostPredators;
                        helper.Remove(ref predators, (LostPredators[i].ID, LostPredators[i].TimeSinceEscape));
                        LostPredators = predators;
                    }
                }
            }
        }

        protected override void AI()
        {
            TimeUpdate();
            if (!DeathCheckAI())
            {
                GiveBirthAI();
                if (!EscapingAI())
                    if(!HungerAI())
                        if(!ReproductionAI())
                            MovementAI();
            }

        }
        protected override bool GiveBirthAI()
        {
            #region Female Reproduction
            if (Gender == 'f')
                if (HasReproduced)
                    if (periodInReproduction >= lengthOfReproduction)
                        Reproduce();
            #endregion
            return true;
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
        protected override bool HungerAI()
        {
            if (Hunger < MaxHunger * HungerFoodSeekingLevel)
            {
                if (mateID != null)
                {
                    lifeformPublisher.RemoveMate(ID, mateID);
                    mateID = null;
                }
                if (foodID == null)
                    foodID = FindFood();
                if (foodID != null)
                {
                    MoveTo = lifeformPublisher.GetLocation(foodID);
                    Move();
                    if (Vector.Compare(Location, MoveTo))
                        Eat();
                    return true;
                }
            }
            return false;
        }
        protected override bool ReproductionAI()
        {
            #region Mating
            if (Age >= ReproductionAge && TimeToReproductionNeed <= 0)
            {
                if (mateID == null)
                    mateID = FindMate();
                if (mateID != null)
                {
                    CurrentMovementSpeed = MovementSpeed;
                    MoveTo = MateLocation = GetLifeformLocation(mateID);
                    Move();
                    Mate();
                    return true;
                }
            }
            return false;
            #endregion
        }
        protected override bool MovementAI()
        {
            if (Vector.Compare(Location, MoveTo))
                MoveTo = GenerateRandomEndLocation();
            CurrentMovementSpeed = MovementSpeed;
            Move();
            return true;
        }
        /// <summary>
        /// Contains the code of the AI related to escaping.
        /// </summary>
        /// <returns>True if the animal is escaping</returns>
        protected virtual bool EscapingAI()
        {
            #region Escaping
            if (!IsRunning && HuntedBy.Length > 0 && TimeSprinted < EscapeSprintTime && Hunger > 0)
            {
                IsRunning = DiscoveredPredator(DiscoverRange, DiscoverChance);
                if (IsRunning)
                {
                    CurrentMovementSpeed *= EscapeSpeedMultiplier;
                    MoveTo = EscapeLocation(PredatorID);
                }
            }
            else if (TimeSprinted > EscapeSprintTime)
            {
                IsRunning = false;
                CurrentMovementSpeed = MovementSpeed;
            }
            if (HuntedBy.Length == 0 || Hunger < 0)
                IsRunning = false;
            if (IsRunning && Hunger > 0)
            {
                Move();
                if (!HasRolled)
                {
                    HasRolled = true;
                    TimeSinceLastRoll = 0;
                    if (TryLosePredator(PredatorID))
                    {
                        LostPredator(PredatorID);
                        IsRunning = false;
                        CurrentMovementSpeed = MovementSpeed;
                    }
                    else
                        if (Vector.Compare(MoveTo, Location))
                        MoveTo = EscapeLocation(PredatorID);
                }
            }
            return IsRunning;
            #endregion
        }

        public bool DiscoveredPredator(float discoverRange, byte discoverChance)
        {
            if(HuntedBy.Length != 0) 
            { 
                List<(float X, float Y, string ID)> predators = new List<(float X, float Y, string ID)>();
                foreach (string id in HuntedBy)
                {
                    Vector location = lifeformPublisher.GetLocation(id);
                    float distance = Location.DistanceBetweenVectors(location);
                    if (distance <= discoverRange)
                        predators.Add((location.X, location.Y, id));
                }
                foreach((float X, float Y, string ID) predator in predators) //make it such they are easier to spot the closer they are
                {
                    short rolledChanced = (short)helper.GenerateRandomNumber(0, discoverChance * 3); //instead of 3, maybe multiply with the (range / something) or (discoverRange + range)
                    if (rolledChanced < discoverChance)
                    {
                        PredatorID = predator.ID;
                        return true;
                    }    
                }
            }
            return false;
        }

        public Vector EscapeLocation(string predatorID)
        { //need to make sure the lifeform does not stop moving toward its target in case it gets hungry, but not starving
            Vector predatorLocation = GetLifeformLocation(predatorID);

            bool predatorIsWest = predatorLocation.X < Location.X;
            bool predatorIsNorth = predatorLocation.Y < Location.Y;

            float distance = Location.DistanceBetweenVectors(predatorLocation);
            float xDifferent = Math.Abs(Location.X - predatorLocation.X);
            float xPercentage = distance != 0 ? 0 : xDifferent / distance;
            float xEscapeDistance = predatorIsWest ? xPercentage * EscapeDistance : -(xPercentage * EscapeDistance);
            float yEscapeDistance = predatorIsNorth ? (1 - xPercentage) * EscapeDistance : -((1 - xPercentage) * EscapeDistance);

            Vector escapeVector = new Vector(Location.X + xEscapeDistance, Location.Y + yEscapeDistance, 0);

            if (escapeVector.X < 0 || escapeVector.X >= mapInformation.GetSizeOfMap.width) //restrictions to the map area
                if (escapeVector.X < 0)
                    escapeVector.X = 0;
                else
                    escapeVector.X = mapInformation.GetSizeOfMap.width - 1;

            if (escapeVector.Y < 0 || escapeVector.Y >= mapInformation.GetSizeOfMap.height) //restrictions to the map area
                if (escapeVector.Y < 0)
                    escapeVector.Y = 0;
                else
                    escapeVector.Y = mapInformation.GetSizeOfMap.height - 1;

            return escapeVector;
        }

        public void LostPredator(string predatorID)
        {
            (string, float)[] lostPredators = LostPredators;
            helper.Add(ref lostPredators, (predatorID, 0));
            LostPredators = lostPredators;
            lifeformPublisher.InformPredatorOfPreyDeath(ID, predatorID);
            string[] huntedBy = HuntedBy;
            helper.Remove(ref huntedBy, predatorID);
            HuntedBy = huntedBy;
            IsRunning = false;

        }

        public bool TryLosePredator(string predatorID)
        {
            float distance = Location.DistanceBetweenVectors(GetLifeformLocation(predatorID));
            float rollOver =  distance / (EscapeDistance*0.2f); //the further away the prey gets the higher the change of ecaping
            float roll = (float)(helper.GenerateRandomNumber(0, (int)EscapeDistance) / EscapeDistance);
            return roll >= rollOver;
        }


        /// <summary>
        /// Is asked for information such that another animal can decided if this animal is food or not.
        /// Overridden to ensure that predators that were lost not to long ago cannot select this prey again for a while.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void IsPossiblePreyEventHandler(object sender, ControlEvents.GetPossiblePreyEventArgs e)
        { //delegate. Send back location, ID and species. 
            string[] hunterIDs = new string[LostPredators.Length];
            for (byte i = 0; i < LostPredators.Length; i++)
                hunterIDs[i] = LostPredators[i].ID;
            if (e.SenderID != ID && !helper.Contains(hunterIDs, e.SenderID))
            {
                (Vector PreyLocation, string PreyID, string PreySpeices) preyInformation = (Location, ID, Species);
                e.AddPreyInformation(preyInformation);
            }
        }
    }
}
