using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    abstract class Herbavore : Animalia, IEscapePredator
    {
        public Herbavore(string species, (float X, float Y) location, string[] foodSource, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, animalPublisher, drawPublisher, mapInformation)
        {
            LostPredators = new (string[] ID, float TimeSinceEscape)[0];

        }

        public (string[] ID, float TimeSinceEscape)[] LostPredators { get; set; }
        public float EscapeSpeedMultiplier { get; set; }
        public float DiscoverRange { get; set; }
        public byte DiscoverChange { get; set; }
        public string PredatorID { get; set; }
        public float TimeThresholdForBeingHuntedAgain { get; set; }
        public float EscapeSprintTime { get; set; }
        public float TimeSprinted { get; set; }

        public bool DiscoveredPredator(float discoverRange, byte discoverChange)
        {
            if(HuntedBy.Length != 0) 
            { 
                List<(float X, float Y, string ID)> predators = new List<(float X, float Y, string ID)>();
                foreach (string id in HuntedBy)
                {
                    (float X, float Y) location = animalPublisher.GetLocation(id);
                    if (Math.Abs(location.X - Location.X) + Math.Abs(location.Y - Location.Y) <= discoverRange)
                        predators.Add((location.X, location.Y, id));
                }
                foreach((float X, float Y, string ID) predator in predators)
                {
                    short rolledChanged = (short)helper.GenerateRandomNumber(0, discoverChange * 3);
                    if(discoverChange <= rolledChanged)
                    {
                        PredatorID = predator.ID;
                        return true;
                    }    
                }
            }
            return false;
        }

        public (float X, float Y) EscapeLocation(string predatorID)
        {
            throw new NotImplementedException();
        }

        public void LostPredator()
        {
            throw new NotImplementedException();
        }

        public bool TryLosePredator()
        {
            throw new NotImplementedException();
        }
    }
}
