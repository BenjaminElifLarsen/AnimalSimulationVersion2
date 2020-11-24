using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    class MapInformation
    {
        private (ushort width, ushort height) mapSize;
        public (ushort width, ushort height) GetSizeOfMap { get => mapSize;  }
        public (ushort width, ushort height) SetSizeOfMap { set => mapSize = value; }
        public float OneAgeInSeconds { get; set; }
        public static MapInformation Instance { get; }
        
        static MapInformation() //consider a way to set the map size and any other possible information
        {
            Instance = new MapInformation();
            Instance.OneAgeInSeconds = 12;
        }
    }
}
