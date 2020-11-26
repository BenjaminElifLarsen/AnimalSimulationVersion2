using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface IEscapePredator
    {
        /// <summary>
        /// Contains predators that have lost this prey and the amount of seconds since so.
        /// </summary>
        public (string ID, float TimeSinceEscape)[] LostPredators { get; set; } //used to prevent a predator from hunting the animal immediately
        /// <summary>
        /// Modifies the movementspeed.
        /// </summary>
        public float EscapeSpeedMultiplier { get; set; }
        /// <summary>
        /// The range at which the prey can detect a predator.
        /// </summary>
        public float DiscoverRange { get; set; }
        /// <summary>
        /// The chance that the prey stops a predator.
        /// </summary>
        public byte DiscoverChance { get; set; }
        /// <summary>
        /// The ID of the predator that was discovered.
        /// </summary>
        public string PredatorID { get; set; }
        /// <summary>
        /// The prey is trying to escape.
        /// </summary>
        public bool IsRunning { get; set; }
        /// <summary>
        /// The amount of seconds that has to pass, before a predator can hunt this prey again.
        /// </summary>
        public float TimeThresholdForBeingHuntedAgain { get; set; }
        /// <summary>
        /// The amount of time the prey can run in.
        /// </summary>
        public float EscapeSprintTime { get; set; }
        /// <summary>
        /// The amount of time the prey has spent running in a row.
        /// </summary>
        public float TimeSprinted { get; set; }
        /// <summary>
        /// The distance to an escape location.
        /// </summary>
        public float EscapeDistance { get; set; }
        /// <summary>
        /// The time that has to pass, before the prey can try to lose a predator.
        /// </summary>
        public float TimeBetweenRolls { get; }
        /// <summary>
        /// The time since the last roll.
        /// </summary>
        public float TimeSinceLastRoll { get; set; }
        /// <summary>
        /// True if the prey has rolled a change to escape a predator.
        /// </summary>
        public bool HasRolled { get; set; }
        /// <summary>
        /// Calculates a random end location.
        /// </summary>
        /// <param name="predatorID">The ID of the predator</param>
        /// <returns></returns>
        public Vector EscapeLocation(string predatorID);
        /// <summary>
        /// Calculates whether the prey has discovered a predator.
        /// </summary>
        /// <param name="discoverRange">The range which the prey can discover.</param>
        /// <param name="discoverChance">The chance for the prey to discover.</param>
        /// <returns>True if the prey discovered a predator.</returns>
        public bool DiscoveredPredator(float discoverRange, byte discoverChance); 
        /// <summary>
        /// The prey tries to lose the predator with the ID value in <paramref name="predatorID"/>.
        /// </summary>
        /// <param name="predatorID">The ID of the predator.</param>
        /// <returns>True if <paramref name="predatorID"/> was lost.</returns>
        public bool TryLosePredator(string predatorID); 
        /// <summary>
        /// Contact the predator, with the ID value of <paramref name="predatorID"/>, that it lost the prey.
        /// </summary>
        /// <param name="predatorID">The ID of the predator.</param>
        public void LostPredator(string predatorID);

    }
}
