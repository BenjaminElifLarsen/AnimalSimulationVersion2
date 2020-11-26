using System;
using System.Collections.Generic;

namespace AnimalSimulationVersion2
{
    class Dioecious : Plantae
    {
        protected string mateID;
        protected Vector mateLocation;
        protected float mateDistance;
        protected float reproductionExtraTime;
        protected float distanceDivider;
        protected char Gender { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected float FindMateCooldown { get; set; }
        public Dioecious(string species, Vector location, IHelper helper, LifeformPublisher lifeformPublisher, DrawPublisher drawPublisher, MapInformation mapInformation, char gender = (char)0) : base(species, location, helper, lifeformPublisher, drawPublisher, mapInformation)
        {
            if (gender == 0)
                Gender = GenderGenerator();
            else
                Gender = gender;
            this.lifeformPublisher.RaisePossibleMatesEvent += CanMateEventHandler;
        }

        protected override void AI()
        {
            TimeUpdate();
            base.AI();
            GiveOffspringsAI();
            ReproductionAI();
        }
        protected override bool GiveOffspringsAI()
        {

            if (Gender == 'f')
                if (HasReproduced && periodInReproduction >= lengthOfReproduction + reproductionExtraTime)
                    Reproduce();
            return true;
        }
        protected override bool ReproductionAI()
        {
            if (Age >= ReproductionAge && TimeToReproductionNeed <= 0 && !HasReproduced)
            {
                if (mateID == null && FindMateCooldown <= 0)
                {
                    mateID = FindMate();
                    FindMateCooldown = ContactCooldownLength;
                }
                if (mateID != null)
                {
                    mateLocation = GetMateLocation(mateID);
                    reproductionExtraTime = DistanceTime(/*mateLocation*/);
                    Polinate();
                    mateID = null;
                    return true;
                }
            }
            return false;
        }

        protected override void Reproduce()
        {
            byte amountOfOffsprings = (byte)helper.GenerateRandomNumber(offspringAmount.Minimum, offspringAmount.Maximum);
            object[] dataObject = new object[7];
            dataObject[0] = Species;
            dataObject[2] = helper;
            dataObject[3] = lifeformPublisher;
            dataObject[4] = drawPublisher;
            dataObject[5] = mapInformation;
            dataObject[6] = (char)0;
            GenerateOffspring(amountOfOffsprings, dataObject);
            HasReproduced = false;

        }

        /// <summary>
        /// Get the location of the lifeform with the ID of <paramref name="mateID"/>
        /// </summary>
        /// <param name="mateID">The ID of the mate.</param>
        /// <returns>A Vector with the posistion values of the mate.</returns>
        protected virtual Vector GetMateLocation(string mateID)
        {
            return lifeformPublisher.GetLocation(mateID);
        }

        protected override void TimeUpdate()
        {
            timeAlive += timeSinceLastUpdate;
            Age = timeAlive / OneAgeInSeconds;
            TimeToReproductionNeed -= timeSinceLastUpdate;
            if (periodInReproduction < lengthOfReproduction + reproductionExtraTime && HasReproduced)
                periodInReproduction += timeSinceLastUpdate;
            if (FindMateCooldown > 0)
                FindMateCooldown = ContactCooldownLength;
        }

        protected override void Polinate()
        {
            HasReproduced = true;
            if (Gender == 'f')
                periodInReproduction = 0;
            TimeToReproductionNeed = reproductionCooldown + reproductionExtraTime;
        }

        /// <summary>
        /// Generates a gender for the tree, male or female. 
        /// </summary>
        /// <remarks>Can only return 'f' and 'm'.</remarks>
        /// <returns>A char that indicates the gender.</returns>
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
            List<(string mateID, Vector Location)> possibleMates = lifeformPublisher.PossibleMates(Species, Gender, ID);
            foreach ((string Mate, Vector Location) information in possibleMates)
            { //need to catch null
                float distanceTo = information.Location.DistanceBetweenVectors(Location);//Math.Abs((information.Location.X - Location.X)) + Math.Abs((information.Location.Y - Location.Y));
                if (distanceTo < distance)
                {
                    distance = distanceTo;
                    nearestMate = information.Mate;
                }
            }
            if (nearestMate != null)
                lifeformPublisher.SetMate(ID, nearestMate);
            mateDistance = distance;
            return nearestMate;
        }
        /// <summary>
        /// Calculates the time increasement for reproduction based upon distance.
        /// </summary>
        /// <returns></returns>
        protected virtual float DistanceTime(/*Vector locationOfMate*/)
        { //if this class end up using the new pregnacy event, if it is the receiver it should also get the location of the mate and then call this method.
            //float distance = (float)Math.Sqrt(Math.Pow(Math.Abs(Location.X - locationOfMate.X),2) + Math.Pow(Math.Abs(Location.Y - locationOfMate.Y), 2));

            return mateDistance / distanceDivider;
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
                        if (e.Information.Gender != Gender)
                            if (TimeToReproductionNeed <= 0)
                                if (Age >= ReproductionAge)
                                    e.AddMateInformation((ID, Location));
        }

        protected override void RemoveSubscriptions()
        {
            lifeformPublisher.RaisePossibleMatesEvent -= CanMateEventHandler;
            base.RemoveSubscriptions();
        }

    }
}
