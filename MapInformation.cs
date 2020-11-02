using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    class MapInformation
    {
        public static (ushort length, ushort width) GetSizeOfMap { get;  }
        public static MapInformation Instance { get; }
        static MapInformation() //consider a way to set the map size and any other possible information
        {
            Instance = new MapInformation();
        }
    }
}
