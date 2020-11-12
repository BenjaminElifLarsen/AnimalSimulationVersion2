using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    /// <summary>
    /// Abstract class for all plants, by default methods assume any plants inheriting are monoecious.
    /// </summary>
    /// <remarks>
    /// This means functions like void Polinate() and void Reproduce() needs to be overwritten for non-monoecious plants.
    /// </remarks>
    abstract class Plantae : Eukaryote
    {
        protected float spreadRange;
        protected (byte Minimum, byte Maximum) offspringAmount;

        public Plantae(string species, (float X, float Y) location, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, helper, animalPublisher, drawPublisher, mapInformation)
        {
        }

        /// <summary>
        /// Baisc implementation of polinating the plant.
        /// </summary>
        protected abstract void Polinate();
    }
    //perhaps for non-monoecious species they should find the nearest mate and add the distance to the reproduction time
    //e.g. distance/20 (non-hard coding number in the end, but a property) 
}
