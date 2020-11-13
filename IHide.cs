﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface IHide //if it is hunted, for each predator generate a random number and if it is higher than a certian value that predator will "lose" the prey. The prey's movement speed is set to zero.
    { //maybe have a IFlee
        public int StealthLevel { get; set; } 
        public float TimeHidden { get; set; }
        public float MaxHideTime { get; set; }
        public bool IsHiding { get; set; }
        public float TimeThresholdForBeingHuntedAgain { get; set; }
        public (string ID, float TimeSinceEscape)[] LostPredators { get; set; }
        public float CooldownBetweenHiding { get; set; }
        public float MaxCooldownBetweenHiding { get; set; }
        public void HideFromPredator(); //maybe old/young animals got a harder time to hide.
        public void LostPredator();

    }
}
