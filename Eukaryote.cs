using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    abstract class Eukaryote
    {
        protected float timeAlive;
        protected float timeSinceLastUpdate;
        protected float reproductionCooldown;
        protected float LengthOfReproduction;

        protected MapInformation mapInformation;
        protected IHelper helper;
        protected AnimalPublisher animalPublisher;
        protected DrawPublisher drawPublisher;

        protected float Age { get; set; }
        protected float ReproductionAge { get; set; }
        protected float MaxAge { get; set; }
        protected string Species { get; set; }
        protected (float X, float Y) Location { get; set; }
        protected float TimeToReproductionNeed { get; set; }
        protected Point[] Design { get; set; }
        protected (byte Red, byte Green, byte Blue) Colour { get; set; }
        protected string ID { get; set; }
        protected float Health { get; set; }
        protected string[] HuntedBy { get; set; }
        protected float NutrienValue { get; set; }
        protected float OneAgeInSeconds { get; set; }

        public Eukaryote(string species, (float X, float Y) location, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : this(helper, animalPublisher, drawPublisher, mapInformation)
        {
            Species = species;
            Location = location;
            HuntedBy = new string[0];
        }

        private Eukaryote(IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation)
        {
            this.helper = helper;
            this.animalPublisher = animalPublisher;
            this.drawPublisher = drawPublisher;
            this.mapInformation = mapInformation;

            OneAgeInSeconds = mapInformation.OneAgeInSeconds;
            ID = helper.GenerateID();

            animalPublisher.RaiseFindPreyEvent += IsPossiblePreyEventHandler;
            animalPublisher.RaiseSetPreyEvent += IsPreyEventHandler;
            animalPublisher.RaiseRemovePreyEvent += RemovePredatorEventHandler;
            animalPublisher.RaiseAIEvent += ControlEventHandler;
            animalPublisher.RaiseDied += DeathEventHandler;
            animalPublisher.RaiseEaten += EatenEventHandler;
            animalPublisher.RaiseGetLocation += LocationEventHandler;

            drawPublisher.RaiseDrawEvent += DrawEventHandler;
        }
        protected abstract void AI();
        protected virtual void TimeUpdate()
        {
            throw new NotImplementedException();
        }
        protected virtual void Death()
        { //things that are shared between Plantae and Animalia could be moved up to an abstract class called Lifeform

        }
        protected virtual void IsPossiblePreyEventHandler(object sender, ControlEvents.GetPossiblePreyEventArgs e)
        {

        }
        protected virtual void IsPreyEventHandler(object sender, ControlEvents.SetPreyEventArgs e)
        {

        }
        protected virtual void RemovePredatorEventHandler(object sender, ControlEvents.RemovePreyEventArgs e)
        {

        }
        protected virtual void LocationEventHandler(object sender, ControlEvents.GetOtherLocationEventArgs e)
        {
            if (e.ReceiverID == ID)
                e.Location = Location;
        }
        protected virtual void EatenEventHandler(object sender, ControlEvents.EatenEventArgs e)
        {
            if (e.ReceiverID == ID)
            {
                e.SetNutrience(NutrienValue);
                Death();
            }
        }
        protected virtual void DeathEventHandler(object sender, ControlEvents.DeadEventArgs e)
        {
            if (e.ReceiverID == ID)
                Death();
        }
        protected virtual void DrawEventHandler(object sender, ControlEvents.DrawEventArgs e)
        {
            if (Design != null)
            {
                (Point[] Design, (byte Red, byte Green, byte Blue), (float X, float Y) Location) drawInforamtion = (helper.DeepCopy(Design), Colour, Location); //(type,type) will ac
                e.AddDrawInformation(drawInforamtion);
            }
        }
        protected virtual void ControlEventHandler(object sender, ControlEvents.AIEventArgs e)
        {
            timeSinceLastUpdate = e.TimeSinceLastUpdate;
            AI();
        }
        protected virtual void RemoveSubscriptions()
        {
            animalPublisher.RaiseFindPreyEvent -= IsPossiblePreyEventHandler;
            animalPublisher.RaiseSetPreyEvent -= IsPreyEventHandler;
            animalPublisher.RaiseRemovePreyEvent -= RemovePredatorEventHandler;
            animalPublisher.RaiseAIEvent -= ControlEventHandler;
            animalPublisher.RaiseDied -= DeathEventHandler;
            animalPublisher.RaiseEaten -= EatenEventHandler;
            animalPublisher.RaiseGetLocation -= LocationEventHandler;

            drawPublisher.RaiseDrawEvent -= DrawEventHandler;
        }
    }
}
