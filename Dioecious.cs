using System;
using System.Collections.Generic;

namespace AnimalSimulationVersion2
{
    class Dioecious : Plantae
    {
        protected string mateID;
        protected (float X, float Y) mateLocation;
        protected float mateDistance;
        protected float reproductionExtraTime;
        protected float distanceDivider;
        protected char Gender { get; set; }
        public Dioecious(string species, (float X, float Y) location, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, helper, animalPublisher, drawPublisher, mapInformation)
        {
            Gender = GenderGenerator();
            animalPublisher.RaisePossibleMatesEvent += CanMateEventHandler;
        }

        protected override void AI()
        {
            TimeUpdate();
            if (Age >= MaxAge || Health <= 0)
                Death();
            if (Gender == 'f')
                if (HasReproduced && periodInReproduction >= lengthOfReproduction + reproductionExtraTime)
                    Reproduce();
            if(TimeToReproductionNeed <= 0 && !HasReproduced)
            {
                if (mateID == null)
                    mateID = FindMate();
                if(mateID != null)
                {
                    mateLocation = GetMateLocation(mateID);
                    reproductionExtraTime = DistanceTime(mateLocation);
                    Polinate();
                    mateID = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mateID"></param>
        /// <returns></returns>
        protected virtual (float X, float Y) GetMateLocation(string mateID)
        {
            return animalPublisher.GetLocation(mateID);
        }

        protected override void TimeUpdate()
        {
            timeAlive += timeSinceLastUpdate;
            Age = timeAlive / OneAgeInSeconds;
            TimeToReproductionNeed -= timeSinceLastUpdate;
            if (periodInReproduction < lengthOfReproduction + reproductionExtraTime && HasReproduced)
                periodInReproduction += timeSinceLastUpdate;
        }

        protected override void Polinate()
        {
            HasReproduced = true;
            if (Gender == 'f')
                periodInReproduction = 0;
            TimeToReproductionNeed = reproductionCooldown + reproductionExtraTime;
        }

        protected virtual char GenderGenerator()
        {
            byte result = (byte)helper.GenerateRandomNumber(0, 100);
            return result >= 50 ? 'f' : 'm';
        }
        /// <summary>
        /// Finds a mate for the plant and informs the other plant that it got a mate.
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
            if (nearestMate != null)
                animalPublisher.SetMate(ID, nearestMate);
            return nearestMate;
        }
        /// <summary>
        /// Calculates the time increasement for reproduction based upon distance.
        /// </summary>
        /// <returns></returns>
        protected virtual float DistanceTime((float X, float Y) locationOfMate)
        {
            float distance = (float)Math.Sqrt(Math.Pow(Math.Abs(Location.X - locationOfMate.X),2) + Math.Pow(Math.Abs(Location.Y - locationOfMate.Y), 2));

            return distance / distanceDivider;
        }

        /// <summary>
        /// Is asked about whether it is a possible mate for another plant or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void CanMateEventHandler(object sender, ControlEvents.PossibleMateEventArgs e)
        {
            if (e.SenderID != ID)
                if (mateID == null)
                    if (e.Information.Species == Species)
                        if (TimeToReproductionNeed <= 0)
                            if (Age >= ReproductionAge)
                                e.AddMateInformation((ID, Location));
        }

        protected override void RemoveSubscriptions()
        {
            animalPublisher.RaisePossibleMatesEvent -= CanMateEventHandler;
            base.RemoveSubscriptions();
        }
    }
}
