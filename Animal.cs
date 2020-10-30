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

        public float Age { get; set; }
        public int ReproductionAge { get; set; }
        public float[] Location { get; set; } //maybe allow them to move past the edge of the map to the other side of the map. E.g. min/max to the other.
        public float MaxAge { get; set; }
        public char Gender { get; set; }
        public int[] BirthAmount { get; set; }
        public string Species { get; set; }
        public float MovementSpeed { get; set; }
        public float Hunger { get; set; }
        public float TimeSinceReproduction { get; set; }
        public List<Point> Design { get; set; }
        public int[] Colour { get; set; }
        public string ID { get; set; }
        public float Health { get; set; }
        public string[] FoodArray { get; set; } 
        public string[] HuntedBy { get; set; } //the IDs that are after it
        public string Active { get; set; } //dayactive, nightactive or both
        public float NutrienValue { get; set; }

        public Animal(string species)
        {
            Species = species;
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

        protected void IsPossiblePreyEventHandler()
        { //delegate. Send back location, ID and species. 

        }
        protected void IsPreyEventHandler()
        { //delegate. Take the ID of the predator and add it to the array. 

        }
        protected void RemovePreyEventHandler()
        { //delegate. The prey has died or is lost to this animal. 

        }
        protected void CanMateEventHandler()
        { //delegate. Check species, if above or is reproduction age, check if it is the corret gender and if it is, send back the ID

        }
        protected void GetMateEventHandler()
        { //delegate. Take the ID of the mate.

        }
        protected void RemoveMateEventHandler() 
        { //delegate. The mate is dead or no longer needing this animal.
            
        }
        protected void DrawEventHandler()
        { //delegate. Transmit location, design and colour back.

        }
        protected void RemoveSubscriptions()
        {

        }

    }
}
