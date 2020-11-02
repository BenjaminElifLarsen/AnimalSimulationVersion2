using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Publisher
    {
        private static AnimalPublisher animalPublisher;
        private static DrawPublisher drawPublisher;
        public static AnimalPublisher GetAnimalInstance { get => animalPublisher; }
        public static DrawPublisher GetDrawInstance { get => drawPublisher; }
        static Publisher()
        {
            animalPublisher = new AnimalPublisher();
            drawPublisher = new DrawPublisher();
        }
    }
}
