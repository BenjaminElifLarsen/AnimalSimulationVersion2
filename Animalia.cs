﻿using Microsoft.Windows.Themes;
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
        protected DrawPublisher drawPublisher;

        /// <summary>
        /// The current age of the animal.
        /// </summary>
        public float Age { get; set; }
        /// <summary>
        /// The age when the animal can reproduce.
        /// </summary>
        public int ReproductionAge { get; set; }
        /// <summary>
        /// The current location of the animal.
        /// </summary>
        public (float X, float Y) Location { get; set; } //(float X, float Y) //maybe allow them to move past the edge of the map to the other side of the map. E.g. min/max to the other.
        /// <summary>
        /// The maximum age of the animal.
        /// </summary>
        public float MaxAge { get; set; }
        /// <summary>
        /// The gender of the animal.
        /// </summary>
        public char Gender { get; set; }
        /// <summary>
        /// The minimum and maximum of children the animal can get in one reproduction.
        /// </summary>
        public (byte Minimum, byte Maximum) BirthAmount { get; set; }
        /// <summary>
        /// The species of the animal.
        /// </summary>
        public string Species { get; set; } //if going with classes like "Lion" or "Cat" no need for this, execept for you need one for subspecies.
        /// <summary>
        /// The movement speed per second of the animal.
        /// </summary>
        public float MovementSpeed { get; set; }
        /// <summary>
        /// The current hunger level of the animal.
        /// </summary>
        public float Hunger { get; set; }
        /// <summary>
        /// The amount of time since last reproduction in seconds.
        /// </summary>
        public float TimeSinceReproduction { get; set; } //maybe have a cooldown value
        /// <summary>
        /// The design of the animal.
        /// </summary>
        public Point[] Design { get; set; }
        /// <summary>
        /// The RGB colour of the animal.
        /// </summary>
        public (int Red, int Green, int Blue) Colour { get; set; }
        /// <summary>
        /// The unique ID of the animal.
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// The health of the animal.
        /// </summary>
        public float Health { get; set; }
        /// <summary>
        /// The source of food for the animal.
        /// </summary>
        public string[] FoodSource { get; set; }
        /// <summary>
        /// Predatores of the animal.
        /// </summary>
        public string[] HuntedBy { get; set; } //the IDs that are after it
        /// <summary>
        /// The nutrience value of the animal.
        /// </summary>
        public float NutrienValue { get; set; }

        public Animalia(string species, int reproductionAge, (float X, float Y) location, float maxAge, (byte Minimum, byte Maximum) birthAmount, float movementSpeed, float hunger, Point[] design, (int Red, int Green, int Blue) colour, string[] foodSource, float nutrienceValue, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher ) : this(helper, animalPublisher, drawPublisher)
        {
            Species = species; //maybe have all parameters related to the animal as a struct. 
            ReproductionAge = reproductionAge;
            Location = location; //need to deep copy it
            BirthAmount = birthAmount;
            MovementSpeed = movementSpeed;
            Hunger = hunger;
            Design = design;
            Colour = colour;
            FoodSource = foodSource;
            MaxAge = maxAge;
            NutrienValue = nutrienceValue;
            ID = helper.GenerateID();
        }
        private Animalia(IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher)
        {
            this.helper = helper;
            this.animalPublisher = animalPublisher;
            this.drawPublisher = drawPublisher;

            animalPublisher.RaiseFindPreyEvent += IsPossiblePreyEventHandler;
            animalPublisher.RaiseSetPreyEvent += IsPreyEventHandler;
            animalPublisher.RaiseRemovePreyEvent += RemovePreyEventHandler;
            animalPublisher.RaisePossibleMatesEvent += CanMateEventHandler;
            animalPublisher.RaiseSetMateEvent += GetMateEventHandler;
            animalPublisher.RaiseRemoveMateEvent += RemoveMateEventHandler;
            animalPublisher.RaiseAIEvent += ControlEventHandler;

            drawPublisher.RaiseDrawEvent += DrawEventHandler;
        }
        /// <summary>
        /// The 'AI' of the animal.
        /// </summary>
        public abstract void AI();
        /// <summary>
        /// Moves the animal.
        /// </summary>
        protected abstract void Move();
        /// <summary>
        /// Finds a mate for the animal.
        /// </summary>
        protected abstract void FindMate(); //have this as an interface, perhaps //some animals find a mate for line
        /// <summary>
        /// Animal mates.
        /// </summary>
        protected abstract void Mating(); 
        //protected abstract string GenerateID();
        //protected abstract char GenerateGender(); //have this in the IHelper. It should take an array of possible genders and a % for each of them. Actually, maybe it is better that each species contains a function and the values needed written in each class
        /// <summary>
        /// Finds food
        /// </summary>
        protected abstract void FindFood(); //maybe have a property for when the animal should start looking for food that is compared to Hunger
        /// <summary>
        /// Animal eats food
        /// </summary>
        protected abstract void Eat();
        /// <summary>
        /// Animal is dead.
        /// </summary>
        protected abstract void Death();
        /// <summary>
        /// Is asked for information such that another animal can decided if this animal is food or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void IsPossiblePreyEventHandler(object sender, ControlEvents.GetPossiblePreyEventArgs e)
        { //delegate. Send back location, ID and species. 
            ((float X, float Y) PreyLocation, string PreyID, string PreySpeices) preyInformation = (Location, ID, Species);
            e.AddPreyInformation(preyInformation);
        }
        /// <summary>
        /// Is informed that another animal is considering it food.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void IsPreyEventHandler(object sender, ControlEvents.SetPreyEventArgs e)
        { //delegate. Take the ID of the predator and add it to the array. 
            if (e.IDs.receiverID == ID)
                helper.Add(HuntedBy, e.IDs.senderID);
        }
        /// <summary>
        /// Its predator is dead or have lost this animal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void RemovePreyEventHandler(object sender, ControlEvents.RemovePreyEventArgs e)
        { //delegate. The prey has died or is lost to this animal. 
            if (helper.Contains(HuntedBy, e.IDs.senderID))
                helper.Remove(HuntedBy, e.IDs.senderID);
        }
        /// <summary>
        /// Is asked about whether it is a possible mate for another animal or not.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void CanMateEventHandler(object sender, ControlEvents.PossibleMateEventArgs e)
        { //delegate. Check species, if above or is reproduction age, check if it is the corret gender and if it is, send back the ID
            if(mateID == null)
                if(e.Information.Species == Species)
                    if(e.Information.Gender != Gender)
                        if(Age >= ReproductionAge)
                            e.AddMateInformation((ID, Location));
        }
        /// <summary>
        /// Another animal has chosen this one for its mate. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void GetMateEventHandler(object sender, ControlEvents.SetMateEventArgs e)
        { //delegate. Take the ID of the mate.
            if (e.IDs.receiverID == ID)
                mateID = e.IDs.senderID;
        }
        /// <summary>
        /// Its mate is dead or no longer of need a mate. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void RemoveMateEventHandler(object sender, ControlEvents.RemoveMateEventArgs e) 
        { //delegate. The mate is dead or no longer needing this animal.
            if (e.IDs.receiverID == ID)
                mateID = null;
        }
        /// <summary>
        /// Asked to return information that permits the animal to be drawned.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void DrawEventHandler(object sender, ControlEvents.DrawEventArgs e)
        { //delegate. Transmit location, design and colour back.
            (Point[] Design, (int Red, int Green, int Blue), (float X, float Y) Location) drawInforamtion = (Design, Colour, Location);
            e.AddDrawInformation(drawInforamtion);
        }
        /// <summary>
        /// Asked to run a sequence of its AI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ControlEventHandler(object sender, ControlEvents.AIEventArgs e)
        { //delegate
            timeSinceLastUpdate = e.TimeSinceLastUpdate;
            AI();
        }
        /// <summary>
        /// Remove all subscriptions to ensure the animal can be removed from the memory.
        /// </summary>
        protected virtual void RemoveSubscriptions() //consider renaming some of the methods to have names that make more sense
        {
            animalPublisher.RaiseFindPreyEvent -= IsPossiblePreyEventHandler;
            animalPublisher.RaiseSetPreyEvent -= IsPreyEventHandler;
            animalPublisher.RaiseRemovePreyEvent -= RemovePreyEventHandler;
            animalPublisher.RaisePossibleMatesEvent -= CanMateEventHandler;
            animalPublisher.RaiseSetMateEvent -= GetMateEventHandler;
            animalPublisher.RaiseRemoveMateEvent -= RemoveMateEventHandler;
            animalPublisher.RaiseAIEvent -= ControlEventHandler;

            drawPublisher.RaiseDrawEvent -= DrawEventHandler;
        }

    }
}