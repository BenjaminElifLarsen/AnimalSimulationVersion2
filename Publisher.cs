using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Publisher
    {
        private static LifeformPublisher lifeformPublisher;
        private static DrawPublisher drawPublisher;
        public static LifeformPublisher GetLifeformInstance { get => lifeformPublisher; }
        public static DrawPublisher GetDrawInstance { get => drawPublisher; }
        static Publisher()
        {
            lifeformPublisher = new LifeformPublisher();
            drawPublisher = new DrawPublisher();
        }
    }
}
