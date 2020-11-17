using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface IHide //if it is hunted, for each predator generate a random number and if it is higher than a certian value that predator will "lose" the prey. The prey's movement speed is set to zero.
    { //maybe have a IFlee
        /// <summary>
        /// THe stealth level of the lifeform. 
        /// </summary>
        public int StealthLevel { get; set; } 
        /// <summary>
        /// The amount of seconds spent hidden in a row.
        /// </summary>
        public float TimeHidden { get; set; }
        /// <summary>
        /// The maximum amount of time the lifeform can stay hidden in a row.
        /// </summary>
        public float MaxHideTime { get; set; }
        /// <summary>
        /// True if the lifeform is hidden.
        /// </summary>
        public bool IsHiding { get; set; }
        /// <summary>
        /// The amount of seconds that have to passed before a predator can hunt this lifeform again.
        /// </summary>
        public float TimeThresholdForBeingHuntedAgain { get; set; }
        /// <summary>
        /// Contains the ID and time since escape of all predators this lifeform have escaped from.
        /// </summary>
        public (string ID, float TimeSinceEscape)[] LostPredators { get; set; }
        /// <summary>
        /// The amount of seconds since the last hide.
        /// </summary>
        public float CooldownBetweenHiding { get; set; }
        /// <summary>
        /// The maximum amount of seconds that can be between hides.
        /// </summary>
        public float MaxCooldownBetweenHiding { get; set; }
        /// <summary>
        /// Lifeform will hide from predators and try to lose them.
        /// </summary>
        public void HideFromPredator(); //maybe old/young animals got a harder time to hide.
        /// <summary>
        /// Lifeform lost a predator and it will select one of them and inform that predator of the lost prey.
        /// </summary>
        public void LostPredator();

    }
}
