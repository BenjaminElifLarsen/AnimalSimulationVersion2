using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    /// <summary>
    /// Contains instances of LifeformPublisher and DrawPublisher.
    /// </summary>
    class Publisher
    {
        private static LifeformPublisher lifeformPublisher;
        private static DrawPublisher drawPublisher;
        /// <summary>
        /// Get the instance of LifeformPublisher
        /// </summary>
        public static LifeformPublisher GetLifeformInstance { get => lifeformPublisher; }
        /// <summary>
        /// Get the instance of DrawPublisher
        /// </summary>
        public static DrawPublisher GetDrawInstance { get => drawPublisher; }
        /// <summary>
        /// Static constructor that sets GetLifeformInstance and GetDrawInstance.
        /// </summary>
        static Publisher()
        {
            lifeformPublisher = new LifeformPublisher();
            drawPublisher = new DrawPublisher();
        }
    }
}
