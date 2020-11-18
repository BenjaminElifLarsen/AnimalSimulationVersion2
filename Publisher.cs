using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Publisher
    {
        private static LifeformPublisher animalPublisher;
        private static DrawPublisher drawPublisher;
        public static LifeformPublisher GetAnimalInstance { get => animalPublisher; }
        public static DrawPublisher GetDrawInstance { get => drawPublisher; }
        static Publisher()
        {
            animalPublisher = new LifeformPublisher();
            drawPublisher = new DrawPublisher();
        }
    }
}
