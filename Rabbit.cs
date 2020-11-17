﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Rabbit : Herbavore, IHide
    {
        public int StealthLevel { get; set; }
        public float TimeHidden { get; set; }
        public float MaxHideTime { get; set; }
        public bool IsHiding { get; set; }
        public float CooldownBetweenHiding { get; set; }
        public float MaxCooldownBetweenHiding { get; set; }

        //public (float TimeSinceLost, string HunterID)[] LostPredators { get; set; }

        public Rabbit(string species, Vector location, string[] foodSource, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, animalPublisher, drawPublisher, mapInformation)
        {
            MovementSpeed = 10;
            CurrentMovementSpeed = MovementSpeed;
            Colour = (0,120,120);
            Design = new Point[] { new Point(3,0), new Point(6, 6), new Point(0, 6) };
            NutrienValue = 100;

            genderInformation = new (char Gender, byte Weight)[] { ('f', 50), ('m', 50) };
            Gender = GenerateGender(genderInformation);
            reproductionCooldown = 20;
            BirthAmount = (3, 5);
            ReproductionAge = 2;
            lengthOfReproduction = 6;

            MaxHunger = 80;
            Hunger = MaxHunger;
            HungerFoodSeekingLevel = 0.6f;

            MaxAge = 6;
            Health = MaxHealth;

            StealthLevel = 2;
            MaxHideTime = 3;
            TimeThresholdForBeingHuntedAgain = 4;
        }

        /// <summary>
        /// Overridden AI that implements IHide.
        /// </summary>
        protected override void AI()
        {
            TimeUpdate();
            if (Age >= MaxAge || Health <= 0)
                Death();
            else
            {
                #region Female Reproduction
                if (Gender == 'f')
                    if (HasReproduced)
                        if (periodInReproduction >= lengthOfReproduction)
                            Reproduce();
                #endregion
                #region Hiding
                if (HuntedBy.Length > 0 && TimeHidden < MaxHideTime && CooldownBetweenHiding <= 0)
                    IsHiding = true;
                else if (TimeHidden > MaxHideTime)
                    IsHiding = false;
                if (IsHiding)
                    HideFromPredator();
                #endregion
                #region Hunger
                else if (Hunger < MaxHunger * HungerFoodSeekingLevel)
                {
                    if (mateID != null) 
                    { 
                        animalPublisher.RemoveMate(ID, mateID);
                        mateID = null;
                    }
                    if (foodID == null)
                        foodID = FindFood();
                    if (foodID != null)
                    {
                        MoveTo = animalPublisher.GetLocation(foodID);
                        Move();
                        if (Vector.Compare(Location, MoveTo))
                            Eat();
                    }
                    else
                        DefaultMovement();
                }
                #endregion
                #region Mating
                else if (Age >= ReproductionAge && TimeToReproductionNeed <= 0)
                {
                    if (mateID == null)
                        mateID = FindMate();
                    if (mateID != null)
                    {
                        CurrentMovementSpeed = MovementSpeed;
                        MoveTo = MateLocation = GetMateLocation(mateID);
                        Move();
                        Mate();
                    }
                    else
                        DefaultMovement();
                }
                #endregion
                #region Default Movement
                else
                    DefaultMovement();
                #endregion
            }

            void DefaultMovement()
            {
                if (Vector.Compare(Location,MoveTo))
                    MoveTo = GenerateRandomEndLocation();
                CurrentMovementSpeed = MovementSpeed;
                Move();
            }
        }
        /// <summary>
        /// overrides Mate(). Calls the base and contains an extra check to if mateID should be set to null.
        /// </summary>
        protected override void Mate()
        {
            base.Mate();
            if (Vector.Compare(Location, MateLocation))
                mateID = null;
        }

        protected override void TimeUpdate()
        {
            if (HuntedBy.Length > 0 && TimeHidden < MaxHideTime)
                TimeHidden += timeSinceLastUpdate;
            if (HuntedBy.Length == 0 && TimeHidden > 0) //rewrite to look better and less code later
            {
                TimeHidden -= timeSinceLastUpdate;
                if (TimeHidden < 0) 
                    TimeHidden = 0;
            }
            if (CooldownBetweenHiding > 0)
                CooldownBetweenHiding -= timeSinceLastUpdate;
            if (LostPredators.Length > 0)
            {
                for(int i = 0; i < LostPredators.Length; i++)
                {
                    LostPredators[i].TimeSinceEscape += timeSinceLastUpdate;
                    if(LostPredators[i].TimeSinceEscape >= TimeThresholdForBeingHuntedAgain)
                    {
                        (string ID, float TimeSinceEscape)[] predators = LostPredators;
                        helper.Remove(ref predators, (LostPredators[i].ID, LostPredators[i].TimeSinceEscape));
                        LostPredators = predators;
                    }
                }
            }
            base.TimeUpdate();
        }

        /// <summary>
        /// The prey will try and hide for a predator. The more predators the harder it is.
        /// </summary>
        public void HideFromPredator()
        {
            int valueToRollOver = (int)(StealthLevel * 8 + 30 * HuntedBy.Length);
            int rolledNUmber = helper.GenerateRandomNumber(0, valueToRollOver - StealthLevel);
            if (rolledNUmber > valueToRollOver)
                LostPredator();
        }
        /// <summary>
        /// The prey will lose the first predator that is after it.
        /// </summary>
        public new void LostPredator()
        {
            animalPublisher.InformPredatorOfPreyDeath(ID, HuntedBy[0]); //perhaps later change it to remove the nearest one
            string[] array = HuntedBy;
            helper.Remove(ref array, HuntedBy[0]);
            (string ID, float TimeSinceEscape)[] predators = LostPredators;
            helper.Add(ref predators, (HuntedBy[0], 0));
            LostPredators = predators;
            HuntedBy = array;
            CooldownBetweenHiding = MaxCooldownBetweenHiding;
            IsHiding = false;
        }

        protected override void Reproduce()
        {
            byte childAmount = (byte)helper.GenerateRandomNumber(BirthAmount.Minimum, BirthAmount.Maximum); 
            for (int i = 0; i < childAmount; i++)
                new Rabbit(Species, Location, FoodSource, helper, animalPublisher, drawPublisher, mapInformation);
            HasReproduced = false;
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
