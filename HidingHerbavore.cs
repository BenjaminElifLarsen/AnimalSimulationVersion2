using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class HidingHerbavore : Herbavore, IHide
    {
        public int StealthLevel { get; set; }
        public float TimeHidden { get; set; }
        public float MaxHideTime { get; set; }
        public bool IsHiding { get; set; }
        public float CooldownBetweenHiding { get; set; }
        public float MaxCooldownBetweenHiding { get; set; }
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
        public HidingHerbavore(string species, Vector location, string[] foodSource, IHelper helper, LifeformPublisher lifeformPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, lifeformPublisher, drawPublisher, mapInformation)
        {
            MovementSpeed = 10;
            CurrentMovementSpeed = MovementSpeed;
            Colour = new Colour(0,120,120);
            NutrientValue = 100;

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
            if (!DeathCheckAI()) 
            {
                GiveBirthAI();
                if (!HidingAI())
                    if (!EscapingAI())
                        if (!HungerAI())
                            if (!ReproductionAI())
                                MovementAI();

            }
        }
        /// <summary>
        /// Contains the code of the AI related to hiding.
        /// </summary>
        /// <returns>True if hiding.</returns>
        protected virtual bool HidingAI()
        {
            #region Hiding
            if (!IsHiding && HuntedBy.Length > 0 && TimeHidden < MaxHideTime && CooldownBetweenHiding <= 0)
                IsHiding = true; //maybe make it such that if there are to many predators after it, it will run instead of hiding.
            else if (TimeHidden > MaxHideTime)
                IsHiding = false;
            if (IsHiding)
                HideFromPredator();
            #endregion
            return IsHiding;
        }

        /// <summary>
        /// Overrides Mate(). Sets mateID to null.
        /// </summary>
        protected override void Mate()
        {
            if (Vector.Compare(Location, MateLocation) && !HasReproduced)
            {
                lifeformPublisher.Pregnacy(ID, mateID, Gender != 'f');
                TimeToReproductionNeed = reproductionCooldown;
                if (Gender == 'f')
                {
                    HasReproduced = true;
                    periodInReproduction = 0;
                }
                mateID = null;
            }
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
            base.TimeUpdate();
        }

        /// <summary>
        /// The prey will try and hide for a predator. The more predators, the harder it is.
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
        public void LostPredator()
        {
            lifeformPublisher.InformPredatorOfPreyDeath(ID, HuntedBy[0]); //perhaps later change it to remove the nearest one
            string[] array = HuntedBy;
            helper.Remove(ref array, HuntedBy[0]);
            (string ID, float TimeSinceEscape)[] predators = LostPredators;
            helper.Add(ref predators, (HuntedBy[0], 0));
            LostPredators = predators;
            HuntedBy = array;
            CooldownBetweenHiding = MaxCooldownBetweenHiding;
            IsHiding = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void PregnacyEventHandler(object sender, ControlEvents.PregnacyEventArgs e)
        {
            if (e.IDs.ReceiverID == ID)
            {
                mateID = null;
                periodInReproduction = 0; 
                TimeToReproductionNeed = reproductionCooldown;
                if (e.IsPregnant)
                    HasReproduced = true;
            }
        }

    }
}
