using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    sealed class MonoeciousPlant : Monoecious //consider what extra stuff a melon can do that would not mate sense to have in Monoecious
    { //after all, some plants reproduce through fire.
        public MonoeciousPlant(string species, Vector location, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, helper, animalPublisher, drawPublisher, mapInformation)
        {
            spreadRange = 140;
            offspringAmount = (1, 3);

            lengthOfReproduction = 12;
            reproductionCooldown = OneAgeInSeconds*2;

            NutrienValue = 14;

            MaxHealth = 100;
            Health = MaxHealth;
            MaxAge = 4;
            ReproductionAge = 2;

            Colour = (100, 200, 0);
            Design = new Point[] { new Point(3,0), new Point(6,3), new Point(3,6), new Point(0,3) };

        }
    }
}
