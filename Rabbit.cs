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
            lengthOfPregnacy = 6;

            MaxHunger = 100;
            Hunger = MaxHunger;
            HungerFoodSeekingLevel = 0.5f;
        }

        public int StealthLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AI()
        {
            TimeUpdate();
            if (Gender == 'f')
                if (HasMated)
                    if (periodInPregnacy >= lengthOfPregnacy)
                        GiveBirth();
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

        protected override void GiveBirth()
        {
            byte childAmount = (byte)helper.GenerateRandomNumber(BirthAmount.Minimum, BirthAmount.Maximum); 
            for (int i = 0; i < childAmount; i++)
                new Rabbit(Species, Location, FoodSource, helper, animalPublisher, drawPublisher, mapInformation);
            //TimeToReproductionNeed = reproductionCooldown - periodInPregnacy; 
            HasMated = false;
        }

        protected override void Mate()
        {
            if (MateLocation == Location && !HasMated)
            {
                periodInPregnacy = 0;
                if (Gender == 'f')
                {
                    HasMated = true;
                    TimeToReproductionNeed = reproductionCooldown - periodInPregnacy;
                }
                else
                {
                    TimeToReproductionNeed = reproductionCooldown;
                }
                mateID = null;
            }
        }

        protected override void Move()
        {
            float xDistance = Math.Abs(MoveTo.X - Location.X);
            float yDistance = Math.Abs(MoveTo.Y - Location.Y);
            float distanceToEndLocation = xDistance + yDistance;
            if (distanceToEndLocation != 0)
            {
                //calculates the %s of the move distance that belong to x and y and then multiply those numbers with the current movement speed. 
                float xPercentage = Math.Abs(MoveTo.X - Location.X) / distanceToEndLocation;
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
                Location = (Location.X + xCurrentSpeed, Location.Y + yCurrentSpeed);
            }
        }
    }
}
