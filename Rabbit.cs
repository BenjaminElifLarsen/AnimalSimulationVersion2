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
            Design = new Point[] { new Point(0, 0), new Point(6, 0), new Point(6, 6), new Point(0, 6) };
            NutrienValue = 100;
        }

        public int StealthLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AI()
        {
            if (Location == MoveTo)
                MoveTo = GenerateRandomEndLocation();
            CurrentMovementSpeed = MovementSpeed;
            Move();
        }

        public void HideFromPredator()
        {
            throw new NotImplementedException();
        }

        protected override string FindFood()
        {
            throw new NotImplementedException();
        }

        protected override string FindMate()
        {
            throw new NotImplementedException();
        }

        protected override void Mate()
        {
            throw new NotImplementedException();
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
