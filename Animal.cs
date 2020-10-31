﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    abstract class Animal
    {
        /// <summary>
        /// The ID of the food that is hunted.
        /// </summary>
        protected string foodID;
        /// <summary>
        /// The ID of the mate.
        /// </summary>
        protected string mateID;
        protected MapInformation mapInformation;
        protected Publisher publisher;
        protected IHelper helper;

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

        public Animal(string species, int reproductionAge, float[] location, float maxAge, int[] birthAmount, float movementSpeed, float hunger, Point[] design, int[] colour, string[] foodSource, string active, float nutrienceValue, IHelper helper ) : this(helper)
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
        private Animal(IHelper helper)
        {
            this.helper = helper;
        }

        public abstract void AI();
        protected abstract void Move();
        protected abstract void FindMate(); //have this as an interface, perhaps //some animals find a mate for line
        protected abstract void Mating(); 
        protected abstract string GenerateID();
        protected abstract char GenerateGender();
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
        protected virtual void RemovePreyEventHandler(object sender, ControlEvents.RemoveMateEventArgs e)
        { //delegate. The prey has died or is lost to this animal. 
            if (helper.Contains(HuntedBy, e.IDs.senderID))
                helper.Remove(HuntedBy, e.IDs.senderID);
        }
        protected virtual void CanMateEventHandler(object sender, ControlEvents.CanMateEventArgs e)
        { //delegate. Check species, if above or is reproduction age, check if it is the corret gender and if it is, send back the ID
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
        protected virtual void RemoveMateEventHandler() 
        { //delegate. The mate is dead or no longer needing this animal.
            
        }
        protected void DrawEventHandler(object sender, ControlEvents.DrawEventArgs e)
        { //delegate. Transmit location, design and colour back.
            (Point[] Design, int[] Colour, float[] Location) drawInforamtion = (Design, Colour, Location);
            e.AddDrawInformation(drawInforamtion);
        }
        protected void ControlEventHandler()
        {
            AI();
        }
        protected virtual void RemoveSubscriptions()
        {

        }

    }
}
