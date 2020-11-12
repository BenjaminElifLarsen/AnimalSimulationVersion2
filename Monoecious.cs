namespace AnimalSimulationVersion2
{
    class Monoecious : Plantae
    {
        public Monoecious(string species, (float X, float Y) location, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, helper, animalPublisher, drawPublisher, mapInformation)
        {
        }
        protected override void AI()
        {
            TimeUpdate();
            if (Age >= MaxAge || Health <= 0)
                Death();
            if (TimeToReproductionNeed <= 0)
                Polinate();
            if (HasReproduced && periodInReproduction >= lengthOfReproduction)
                Reproduce();
        }

        /// <summary>
        /// Baisc monoecious implementation of polinating the plant.
        /// </summary>
        protected override void Polinate()
        {
            TimeToReproductionNeed = reproductionCooldown + lengthOfReproduction;
            periodInReproduction = 0;
            HasReproduced = true;
        }
        

    }
}
