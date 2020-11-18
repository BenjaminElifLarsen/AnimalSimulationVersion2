namespace AnimalSimulationVersion2
{
    class Monoecious : Plantae
    {
        public Monoecious(string species, Vector location, IHelper helper, LifeformPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, helper, animalPublisher, drawPublisher, mapInformation)
        {
        }
        protected override void AI()
        {
            TimeUpdate();
            base.AI();
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
            TimeToReproductionNeed = reproductionCooldown;
            periodInReproduction = 0;
            HasReproduced = true;
        }
        

    }
}
