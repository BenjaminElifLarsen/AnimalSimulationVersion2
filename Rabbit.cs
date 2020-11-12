using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Rabbit : Herbavore, IHide
    {
        public Rabbit(string species, (float X, float Y) location, string[] foodSource, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, animalPublisher, drawPublisher, mapInformation)
        {
            MovementSpeed = 10;
            Colour = (0,120,120);
            Design = new Point[] { new Point(3,0), new Point(6, 6), new Point(0, 6) };
            NutrienValue = 100;

            genderInformation = new (char Gender, byte Weight)[] { ('f', 50), ('m', 50) };
            Gender = GenerateGender(genderInformation);
            reproductionCooldown = 20;
            BirthAmount = (2, 5);
            ReproductionAge = 2;
            lengthOfReproduction = 6;

            MaxHunger = 100;
            Hunger = MaxHunger;
            HungerFoodSeekingLevel = 0.5f;
        }

        public int StealthLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float TimeHidden { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float MaxHideTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected override void AI()
        {
            TimeUpdate();
            if (Age >= MaxAge || Health <= 0)
                Death();
            else
            {
                if (Gender == 'f')
                    if (HasReproduced)
                        if (periodInReproduction >= lengthOfReproduction)
                            Reproduce();
                if (Age >= ReproductionAge && TimeToReproductionNeed <= 0)
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
                else
                    DefaultMovement();
            }
            void DefaultMovement()
            {
                if (Location == MoveTo)
                    MoveTo = GenerateRandomEndLocation();
                CurrentMovementSpeed = MovementSpeed;
                Move();
            }
        }

        public void HideFromPredator()
        {
            throw new NotImplementedException();
        }

        protected override void Reproduce()
        {
            byte childAmount = (byte)helper.GenerateRandomNumber(BirthAmount.Minimum, BirthAmount.Maximum); 
            for (int i = 0; i < childAmount; i++)
                new Rabbit(Species, Location, FoodSource, helper, animalPublisher, drawPublisher, mapInformation);
            //TimeToReproductionNeed = reproductionCooldown - periodInPregnacy; 
            HasReproduced = false;
        }


    }
}
