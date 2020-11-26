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
            ReproductionAI();
            GiveOffspringsAI();
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

        protected override bool GiveOffspringsAI()
        {
            if (HasReproduced && periodInReproduction >= lengthOfReproduction)
                Reproduce();
            return true;
        }
        protected override bool ReproductionAI()
        {
            if (TimeToReproductionNeed <= 0)
                Polinate();
            return true;
        }
    }
}
