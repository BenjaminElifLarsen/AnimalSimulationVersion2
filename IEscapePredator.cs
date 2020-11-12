using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface IEscapePredator
    {
        public (string ID, float TimeSinceEscape)[] LostPredators { get; set; } //used to prevent a predator from hunting the animal immediately
        public float EscapeSpeedMultiplier { get; set; }
        public float DiscoverRange { get; set; }
        public byte DiscoverChance { get; set; }
        public string PredatorID { get; set; }
        public float TimeThresholdForBeingHuntedAgain { get; set; } //1) rename. 2) time to compare to LostPredators to ensure it does not get hunted immediately by the same animal
        public float EscapeSprintTime { get; set; } //the amount of time the animal can run
        public float TimeSprinted { get; set; }
        public (float X, float Y) EscapeLocation(string predatorID); //calculate a random location the animal will run towards.
        public bool DiscoveredPredator(float discoverRange, byte discoverChange); //calulates if a predator has been discovered.
        public bool TryLosePredator(); //calcule a number and if above a threshold the animal got away
        public void LostPredator(); //transmit to the predator that it have lost the animal

    }
}
