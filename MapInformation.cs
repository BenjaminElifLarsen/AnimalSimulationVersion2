using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    /// <summary>
    /// Contains information of the map.
    /// </summary>
    class MapInformation
    {
        /// <summary>
        /// The width and height of the map.
        /// </summary>
        private (ushort width, ushort height) mapSize;
        /// <summary>
        /// Get the width and height of the map.
        /// </summary>
        public (ushort width, ushort height) GetSizeOfMap { get => mapSize;  }
        /// <summary>
        /// Set the width and height of the map.
        /// </summary>
        public (ushort width, ushort height) SetSizeOfMap { set => mapSize = value; }
        /// <summary>
        /// The amount of seconds that makes up one year on the map.
        /// </summary>
        public float OneAgeInSeconds { get; set; }
        /// <summary>
        /// Get the instance of MapInformation.
        /// </summary>
        public static MapInformation Instance { get; }
        /// <summary>
        /// The static constructor. Creates an instance of MapInformation and sets <c>OneAgeInSeconds</c>
        /// </summary>
        static MapInformation() 
        {
            Instance = new MapInformation();
            Instance.OneAgeInSeconds = 12;
        }
    }
}
