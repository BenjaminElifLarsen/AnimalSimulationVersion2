using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Dioecious : Plantae
    {
        protected string mateID;
        protected (float X, float Y) mateLocation;
        protected float mateDistance;
        protected char Gender { get; set; }
        public Dioecious(string species, (float X, float Y) location, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, helper, animalPublisher, drawPublisher, mapInformation)
        {
            Gender = GenderGenerator();
            animalPublisher.RaisePossibleMatesEvent += CanMateEventHandler;
        }

        protected override void AI()
        {
            if(TimeToReproductionNeed <= 0 && !HasReproduced)
            {
                if (mateID == null)
                    mateID = FindMate();
                if(mateID != null)
                {
                    mateLocation = GetMateLocation(mateID);

                    mateID = null;
                }
            }
            throw new NotImplementedException();
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

        protected override void Polinate()
        {
            HasReproduced = true;
            if (Gender == 'f')
                periodInReproduction = 0;
            
            throw new NotImplementedException();
        }

        protected override void Reproduce()
        {
            throw new NotImplementedException();
        }
        protected virtual char GenderGenerator()
        {
            byte result = (byte)helper.GenerateRandomNumber(0, 100);
            return result >= 50 ? 'f' : 'm';
        }
        protected virtual string FindMate()
        {
            throw new NotImplementedException();
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
