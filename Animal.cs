using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    abstract class Animal
    {
        protected string foodID;
        protected string MateID;
        protected MapInformation mapInformation;
        protected IArraySupport helper;

        public float Age { get; set; }
        public int ReproductionAge { get; set; }
        public float[] Location { get; set; } //maybe allow them to move past the edge of the map to the other side of the map. E.g. min/max to the other.
        public float MaxAge { get; set; }
        public char Gender { get; set; }
        public int[] BirthAmount { get; set; }
        public string Species { get; set; } //if going with classes like "Lion" or "Cat" no need for this, execept for you need one for subspecies.
        public float MovementSpeed { get; set; }
        public float Hunger { get; set; }
        public float TimeSinceReproduction { get; set; } //maybe have a cooldown value
        public Point[] Design { get; set; }
        public int[] Colour { get; set; }
        public string ID { get; set; }
        public float Health { get; set; }
        public string[] FoodArray { get; set; } 
        public string[] HuntedBy { get; set; } //the IDs that are after it
        public string Active { get; set; } //dayactive, nightactive or both
        public float NutrienValue { get; set; }

        public Animal(string species, int reproductionAge, float[] location, float maxAge, int[] birthAmount, float movementSpeed, float hunger, Point[] design, int[] colour, string[] foodSource, string active, float nutrienceValue, IArraySupport helper ) : this(helper)
        {
            Species = species; //maybe have all parameters related to the animal as a struct. 
            ReproductionAge = reproductionAge;
            Location = location; //need to deep copy it
            BirthAmount = birthAmount;
            MovementSpeed = movementSpeed;
            Hunger = hunger;
            Design = design;
            Colour = colour;
            FoodArray = foodSource;
            Active = active;
            NutrienValue = nutrienceValue;
        }
        private Animal(IArraySupport helper)
        {
            this.helper = helper;
        }

        public abstract void Control();
        protected abstract void Move();
        protected abstract void FindMate(); //have this as an interface, perhaps //some animals find a mate for line
        protected abstract void Mating(); 
        protected abstract string GenerateID();
        protected abstract char GenerateGender();
        protected abstract void FindFood();
        protected abstract void Eat();
        protected abstract void Death();

        protected virtual void IsPossiblePreyEventHandler()
        { //delegate. Send back location, ID and species. 
            (float[] PreyLocation, string PreyID, string PreySpeices) preyInformation = (Location, ID, Species);
        }
        protected virtual void IsPreyEventHandler()
        { //delegate. Take the ID of the predator and add it to the array. 

        }
        protected virtual void RemovePreyEventHandler()
        { //delegate. The prey has died or is lost to this animal. 

        }
        protected virtual void CanMateEventHandler()
        { //delegate. Check species, if above or is reproduction age, check if it is the corret gender and if it is, send back the ID

        }
        protected virtual void GetMateEventHandler()
        { //delegate. Take the ID of the mate.

        }
        protected virtual void RemoveMateEventHandler() 
        { //delegate. The mate is dead or no longer needing this animal.
            
        }
        protected void DrawEventHandler()
        { //delegate. Transmit location, design and colour back.
            (Point[] Design, int[] Colour, float[] Location) drawInforamtion = (Design, Colour, Location);
        }
        protected virtual void RemoveSubscriptions()
        {

        }

    }
}
