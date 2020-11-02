using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    abstract class Animalia
    {
        /// <summary>
        /// The ID of the food that is hunted.
        /// </summary>
        protected string foodID;
        /// <summary>
        /// The ID of the mate.
        /// </summary>
        protected string mateID;
        protected float timeSinceLastUpdate;
        protected MapInformation mapInformation;
        protected Publisher publisher;
        protected IHelper helper;
        protected AnimalPublisher animalPublisher;

        public float Age { get; set; }
        public int ReproductionAge { get; set; }
        public float[] Location { get; set; } //(float X, float Y) //maybe allow them to move past the edge of the map to the other side of the map. E.g. min/max to the other.
        public float MaxAge { get; set; }
        public char Gender { get; set; }
        public int[] BirthAmount { get; set; }
        public string Species { get; set; } //if going with classes like "Lion" or "Cat" no need for this, execept for you need one for subspecies.
        public float MovementSpeed { get; set; }
        public float Hunger { get; set; }
        public float TimeSinceReproduction { get; set; } //maybe have a cooldown value
        public Point[] Design { get; set; }
        public (int Red, int Green, int Blue) Colour { get; set; }
        public string ID { get; set; }
        public float Health { get; set; }
        public string[] FoodArray { get; set; } 
        public string[] HuntedBy { get; set; } //the IDs that are after it
        public float NutrienValue { get; set; }

        public Animalia(string species, int reproductionAge, float[] location, float maxAge, int[] birthAmount, float movementSpeed, float hunger, Point[] design, (int Red, int Green, int Blue) colour, string[] foodSource, float nutrienceValue, IHelper helper, AnimalPublisher animalPublisher ) : this(helper, animalPublisher)
        {
            Species = species; //maybe have all parameters related to the animal as a struct. 
            ReproductionAge = reproductionAge;
            Location = helper.DeepCopy(location); //need to deep copy it
            BirthAmount = birthAmount;
            MovementSpeed = movementSpeed;
            Hunger = hunger;
            Design = design;
            Colour = colour;
            FoodArray = foodSource;
            MaxAge = maxAge;
            NutrienValue = nutrienceValue;
            ID = helper.GenerateID();
        }
        private Animalia(IHelper helper, AnimalPublisher animalPublisher)
        {
            this.helper = helper;
            this.animalPublisher = animalPublisher;
            animalPublisher.RaiseFindPreyEvent += IsPossiblePreyEventHandler;
            animalPublisher.RaiseSetPreyEvent += IsPreyEventHandler;
            animalPublisher.RaiseRemovePreyEvent += RemovePreyEventHandler;
            animalPublisher.RaisePossibleMatesEvent += CanMateEventHandler;
            animalPublisher.RaiseSetMateEvent += GetMateEventHandler;
            animalPublisher.RaiseRemoveMateEvent += RemoveMateEventHandler;
            animalPublisher.RaiseAIEvent += ControlEventHandler;
        }

        public abstract void AI();
        protected abstract void Move();
        protected abstract void FindMate(); //have this as an interface, perhaps //some animals find a mate for line
        protected abstract void Mating(); 
        //protected abstract string GenerateID();
        //protected abstract char GenerateGender(); //have this in the IHelper. It should take an array of possible genders and a % for each of them.
        protected abstract void FindFood();
        protected abstract void Eat();
        protected abstract void Death();

        protected virtual void IsPossiblePreyEventHandler(object sender, ControlEvents.GetPossiblePreyEventArgs e)
        { //delegate. Send back location, ID and species. 
            (float[] PreyLocation, string PreyID, string PreySpeices) preyInformation = (Location, ID, Species);
            e.AddPreyInformation(preyInformation);
        }

        protected virtual void IsPreyEventHandler(object sender, ControlEvents.SetPreyEventArgs e)
        { //delegate. Take the ID of the predator and add it to the array. 
            if (e.IDs.receiverID == ID)
                helper.Add(HuntedBy, e.IDs.senderID);
        }

        protected virtual void RemovePreyEventHandler(object sender, ControlEvents.RemovePreyEventArgs e)
        { //delegate. The prey has died or is lost to this animal. 
            if (helper.Contains(HuntedBy, e.IDs.senderID))
                helper.Remove(HuntedBy, e.IDs.senderID);
        }

        protected virtual void CanMateEventHandler(object sender, ControlEvents.PossibleMateEventArgs e)
        { //delegate. Check species, if above or is reproduction age, check if it is the corret gender and if it is, send back the ID
            if(mateID == null)
                if(e.Information.Species == Species)
                    if(e.Information.Gender != Gender)
                        if(Age >= ReproductionAge)
                            e.AddMateInformation((ID, Location));
        }

        protected virtual void GetMateEventHandler(object sender, ControlEvents.SetMateEventArgs e)
        { //delegate. Take the ID of the mate.
            if (e.IDs.receiverID == ID)
                mateID = e.IDs.senderID;
        }

        protected virtual void RemoveMateEventHandler(object sender, ControlEvents.RemoveMateEventArgs e) 
        { //delegate. The mate is dead or no longer needing this animal.
            if (e.IDs.receiverID == ID)
                mateID = null;
        }

        protected void DrawEventHandler(object sender, ControlEvents.DrawEventArgs e)
        { //delegate. Transmit location, design and colour back.
            (Point[] Design, (int Red, int Green, int Blue), float[] Location) drawInforamtion = (Design, Colour, Location);
            e.AddDrawInformation(drawInforamtion);
        }

        protected void ControlEventHandler(object sender, ControlEvents.AIEventArgs e)
        { //delegate
            timeSinceLastUpdate = e.TimeSinceLastUpdate;
            AI();
        }

        protected virtual void RemoveSubscriptions() //consider renaming some of the methods to have names that make more sense
        {
            animalPublisher.RaiseFindPreyEvent -= IsPossiblePreyEventHandler;
            animalPublisher.RaiseSetPreyEvent -= IsPreyEventHandler;
            animalPublisher.RaiseRemovePreyEvent -= RemovePreyEventHandler;
            animalPublisher.RaisePossibleMatesEvent -= CanMateEventHandler;
            animalPublisher.RaiseSetMateEvent -= GetMateEventHandler;
            animalPublisher.RaiseRemoveMateEvent -= RemoveMateEventHandler;
            animalPublisher.RaiseAIEvent -= ControlEventHandler;
        }

    }
}
