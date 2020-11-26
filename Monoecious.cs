namespace AnimalSimulationVersion2
{
    class Monoecious : Plantae
    {
        /// <summary>
        /// Default constructor. Initialises properites and variables to 'default' values.
        /// </summary>
        /// <param name="species">The species of this animal.</param>
        /// <param name="location">The start location of this animal.</param>
        /// <param name="foodSource">The food source of this animal.</param>
        /// <param name="helper">An instance of IHelper.</param>
        /// <param name="lifeformPublisher">An instance of AnimalPublisher.</param>
        /// <param name="drawPublisher">An instance of DrawPublisher.</param>
        /// <param name="mapInformation">An instance of MapInformation.</param>
        public Monoecious(string species, Vector location, IHelper helper, LifeformPublisher lifeformPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, helper, lifeformPublisher, drawPublisher, mapInformation)
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
