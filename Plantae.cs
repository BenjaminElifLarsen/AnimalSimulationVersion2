using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    abstract class Plantae : Eukaryote
    {
        protected float lengthOfReproduction;
        protected float spreadRange;
        protected bool HasPolinated { get; set; }

        public Plantae(string species, (float X, float Y) location, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, helper, animalPublisher, drawPublisher, mapInformation)
        {
        }
        protected override void AI()
        {

        }

        protected virtual void Polinate()
        {
            throw new NotImplementedException();
        }
        protected virtual void Reproduce()
        {
            throw new NotImplementedException();
        }
    }

}
