using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Publisher
    {
        private static AnimalPublisher animalPublisher;
        public static AnimalPublisher GetAnimalInstance { get => animalPublisher; }
        static Publisher()
        {
            animalPublisher = new AnimalPublisher();
        }
    }
}
