﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    class PackCarnivore : SleepingCarnivore, IPack
    {
        /// <summary>
        /// The animal has been killed.
        /// </summary>
        private bool isKilled;
        public (IPack.PackRelationship Relationship, string ID, char Gender)[] PackMembers { get; set; }
        public byte MaxPackSize { get; set; }
        public float TimeSinceLastFight { get; set; }
        public float FightCooldown { get; set; }
        public bool CanFightForAlpha { get; set; }
        public IPack.PackRelationship Relationship { get; set; }
        public bool AlphaMatingOnly { get; set; }
        public string[] AttackedBy { get; set; }
        public float StrikeRange { get; }
        public float AttackSpeed { get; }
        public float AttackCooldown { get; set; }
        /// <summary>
        /// The cooldown between looking for a pack.
        /// </summary>
        protected float FindPackCooldown { get; set; }

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
        public PackCarnivore(string species, Vector location, string[] foodSource, IHelper helper, LifeformPublisher lifeformPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, lifeformPublisher, drawPublisher, mapInformation)
        {
            AttackRange = 40;
            AttackSpeedMultiplier = 1.2f; 

            MovementSpeed = 18;

            Colour = new Colour(255,120,0);

            PackMembers = new(IPack.PackRelationship Relationship, string ID, char Gender)[0];
            MaxPackSize = 6;
            FightCooldown = 10;
            CanFightForAlpha = true;
            AlphaMatingOnly = true;
            AttackedBy = new string[0];
            Relationship = IPack.PackRelationship.NonMember;
            AttackSpeed = 1.4f;
            StrikeRange = 10;

            base.lifeformPublisher.RaiseTransmitData += RelationshipEventHandler;
            base.lifeformPublisher.RaisePossibleRelationshipJoiner += RelationshipCandidateEventHandler;
        }

        protected override void AI() 
        { 
            if(!isDead)
                base.AI();
            if (!isDead)
            {
                PackAI();
                Fight(); 
            }
        }
        /// <summary>
        /// Contains the code of the AI related to finding a pack.
        /// </summary>
        /// <returns>True if a pack was found.</returns>
        protected virtual bool PackAI()
        {
            if (FindPackCooldown <= 0)
            {
                GeneratePack();
                FindPackCooldown = ContactCooldownLength;
                return true;
            }
            return false;
        }

        protected override void TimeUpdate()
        {
            base.TimeUpdate();
            if (TimeSinceLastFight > 0)
                TimeSinceLastFight -= timeSinceLastUpdate;
            if (AttackCooldown > 0)
                AttackCooldown -= timeSinceLastUpdate;
            if (FindPackCooldown > 0)
                FindPackCooldown -= timeSinceLastUpdate;
        }

        public void Fight()
        { //maybe allow damage to be in range, but then the damage event needs to use float instead of. Maybe damage should also be calculated as a per second (could go wrong with bad slowdowns)
            if(CanFightForAlpha) 
                if(Relationship != IPack.PackRelationship.Alpha)
                {
                    if(TimeSinceLastFight <= 0)
                    { //can attack the alpha
                        if(TimeToReproductionNeed <= -100 && AttackCooldown <= 0)
                        { //it is something... it is something
                            foreach((IPack.PackRelationship relationship, string id, char gender) in PackMembers)
                                if(relationship == IPack.PackRelationship.Alpha && gender == Gender)
                                {
                                    float distance = Location.DistanceBetweenVectors(GetLifeformLocation(id));
                                    if (distance <= StrikeRange)
                                    {
                                        AttackCooldown = AttackSpeed;
                                        byte damage = (byte)helper.GenerateRandomNumber(0, 16);
                                        bool killed = lifeformPublisher.DamageLifeform(ID, id, damage);
                                        if (killed)
                                        { //update the relationship to alpha and transmit the pack to each member
                                            helper.Replace(PackMembers, (Relationship, ID, Gender), (IPack.PackRelationship.Alpha, ID, Gender));
                                            Relationship = IPack.PackRelationship.Alpha;
                                            foreach ((_, string id_, _) in PackMembers)
                                                if (id_ != ID)
                                                    TransmitPack(id_);
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                }
                else if (AttackedBy.Length > 0 && AttackCooldown <= 0) 
                { //is alpha and being attacked. 
                    AttackCooldown = AttackSpeed;
                    float distance = Location.DistanceBetweenVectors(GetLifeformLocation(AttackedBy[0]));
                    if (distance <= StrikeRange)
                    {
                        byte damage = (byte)helper.GenerateRandomNumber(0, 16);
                        bool killed = lifeformPublisher.DamageLifeform(ID, AttackedBy[0], damage);
                    }
                }
        }

        /// <summary>
        /// Overrides Animalia.Reproduce() to ensure the pack knows of the new members.
        /// </summary>
        protected override void Reproduce()
        {
            object[] dataObject = new object[] { Species, Location, FoodSource, helper, lifeformPublisher, drawPublisher, mapInformation };

            byte childAmount = (byte)helper.GenerateRandomNumber(BirthAmount.Minimum, BirthAmount.Maximum);
            PackCarnivore[] children = new PackCarnivore[childAmount];
            for (int i = 0; i < childAmount; i++)
                children[i] = (PackCarnivore)Activator.CreateInstance(GetType(), dataObject);
            HasReproduced = false;

            (IPack.PackRelationship Relationship, string ID, char Gender)[] pack = PackMembers;
            foreach (PackCarnivore child in children)
            {
                if (pack.Length >= MaxPackSize)
                    break;
                helper.Add(ref pack, (IPack.PackRelationship.Member, child.ID, child.Gender));
            }
            PackMembers = pack;
            foreach ((_, string id, _) in PackMembers)
                TransmitPack(id);
        }

        protected override void Death()
        {
            if (Relationship == IPack.PackRelationship.Alpha && !isKilled)
            {
                bool newAlpha = false;
                (IPack.PackRelationship relationship, string id, char gender)[] pack = PackMembers;
                helper.Remove(ref pack, (Relationship, ID, Gender));
                for(int i = 0; i < pack.Length; i++)
                {
                    if (Gender == pack[i].gender && pack[i].relationship == IPack.PackRelationship.Member) 
                    { 
                        pack[i].relationship = IPack.PackRelationship.Alpha;
                        newAlpha = true;
                        break;
                    }
                }
                if (!newAlpha)
                { //if no new alpha, dissolve the pack
                    PackMembers = new (IPack.PackRelationship Relationship, string ID, char Gender)[0];
                    foreach ((_, string id, _) in pack)
                        TransmitPack(id);
                }
                else
                {
                    PackMembers = pack;
                    foreach((_, string id, _) in PackMembers)
                        TransmitPack(id);
                }
            }
            else if(Relationship == IPack.PackRelationship.Alpha) // != IPack.PackRelationship.NonMember
            {
                (IPack.PackRelationship relationship, string id, char gender)[] pack = PackMembers;
                helper.Remove(ref pack, (Relationship, ID, Gender));
                PackMembers = pack;
                foreach ((_, string id, _) in PackMembers)
                    TransmitPack(id);
            }
            else if(Relationship == IPack.PackRelationship.Member)
            {
                (IPack.PackRelationship relationship, string id, char gender)[] pack = PackMembers;
                helper.Remove(ref pack, (Relationship, ID, Gender));
                PackMembers = pack;
                foreach ((_, string id, _) in PackMembers)
                    TransmitPack(id);
            }
            base.Death();
        }

        protected override string FindMate()
        {
            if( (AlphaMatingOnly && Relationship == IPack.PackRelationship.Alpha) || !AlphaMatingOnly)
                return base.FindMate();
            return null;
        }

        protected override Vector GenerateRandomEndLocation()
        {
            if (PackMembers == null || PackMembers.Length == 1)
                return base.GenerateRandomEndLocation();
            if (Relationship == IPack.PackRelationship.Alpha && Gender == 'f')
                return base.GenerateRandomEndLocation();
            else
            {
                foreach ((IPack.PackRelationship relationship, string id, char gender) in PackMembers)
                    if (relationship == IPack.PackRelationship.Alpha && gender == 'f')
                    {
                        float spreadRange = 20;
                        Vector location = GetLifeformLocation(id);
                        //add some deviation to the location.
                        float xPercentage = (float)(helper.GenerateRandomNumber(0, 100) / 100f);
                        float xMaxDistance = xPercentage * spreadRange;
                        float yMaxDistance = (1 - xPercentage) * spreadRange;
                        float xDistance = helper.GenerateRandomNumber(0, (int)xMaxDistance) - xMaxDistance / 2f;
                        float yDistance = helper.GenerateRandomNumber(0, (int)yMaxDistance) - yMaxDistance / 2f;
                        location.X += xDistance;
                        location.Y += yDistance;
                        if (location.X < 0)
                            location.X = 0;
                        else if (location.X > mapInformation.GetSizeOfMap.width)
                            location.X = mapInformation.GetSizeOfMap.width - 1;
                        if (location.Y < 0)
                            location.Y = 0;
                        else if (location.Y > mapInformation.GetSizeOfMap.height)
                            location.Y = mapInformation.GetSizeOfMap.height - 1;
                        return location;
                    }
                return base.GenerateRandomEndLocation();
            }
        }

        public void TransmitPack(string receiverID)
        { //all but one call to this method is in a forloop scope and the one there is not, is in GeneratePack().
            object data = PackMembers;
            lifeformPublisher.TransmitData(ID, receiverID, data);
        }

        public void GeneratePack() 
        {
            if(PackMembers == null || PackMembers.Length <= 1)
            {
                List<(Vector location, string id, char gender)> possiblePackMembers = lifeformPublisher.PossibleRelationshipJoiner(ID, Species, typeof(IPack));
                if(possiblePackMembers != null)
                { 
                    float distance = float.MaxValue;
                    (string id, char? gender) nearestPackMember = (null,null);
                    foreach((Vector location, string id, char gender) in possiblePackMembers)
                    {
                        if(gender != Gender) 
                        { 
                            float tempDistance = Location.DistanceBetweenVectors(location);
                            if(tempDistance < distance)
                            {
                                nearestPackMember = (id, gender);
                                distance = tempDistance;
                            }
                        }
                    }
                    if(nearestPackMember.id != null)
                    {
                        (IPack.PackRelationship Relationship, string ID, char Gender)[] pack = PackMembers;
                        helper.Add(ref pack, (IPack.PackRelationship.Alpha, ID, Gender));
                        helper.Add(ref pack, (IPack.PackRelationship.Alpha, nearestPackMember.id, (char)nearestPackMember.gender));
                        PackMembers = pack;
                        TransmitPack(nearestPackMember.id);
                        Relationship = IPack.PackRelationship.Alpha;
                    }
                }
            }
        }

        public void RelationshipEventHandler(object sender, ControlEvents.TransmitDataEventArgs e)
        {
            if(e.IDs.ReceiverID == ID)
            {
                if(e.Data is (IPack.PackRelationship, string, char)[] data)
                {
                    TimeSinceLastFight = FightCooldown; //this means that any update to the pack will prevent a fight, even birth
                    string[] attackers = new string[0];
                    PackMembers = data;
                    if(PackMembers.Length == 0)
                        Relationship = IPack.PackRelationship.NonMember;
                    foreach ((IPack.PackRelationship relationship, string id, _) in PackMembers)
                        if (ID == id)
                        {
                            Relationship = relationship;
                        }
                        else
                        {
                            if (helper.Contains(AttackedBy, id))
                            {
                                helper.Add(ref attackers, id);
                            }
                        }
                    AttackedBy = attackers;
                }
            }
        }

        public void RelationshipCandidateEventHandler(object sender, ControlEvents.RelationshipCandidatesEventArgs e)
        { //delegate
            if (e.RelationshipType.IsAssignableFrom(GetType())) //true if the instance implements e.RelationshipType. //reflection, costly. 
                if(PackMembers == null || PackMembers.Length <= 1)
                    if(e.SenderID != ID)
                        if(e.Species == Species)
                            e.Add(Location, ID, Gender);
        }

        protected override void CanMateEventHandler(object sender, ControlEvents.PossibleMateEventArgs e)
        { //delegate. Check species, if above or is reproduction age, check if it is the corret gender and if it is, send back the ID
            if (e.SenderID != ID)
                if (mateID == null)
                    if (e.Information.Species == Species)
                        if (e.Information.Gender != Gender)
                            if ((AlphaMatingOnly && Relationship == IPack.PackRelationship.Alpha) || !AlphaMatingOnly)
                                if (helper.Contains(PackMembers, (IPack.PackRelationship.Alpha, e.SenderID, e.Information.Gender)) || helper.Contains(PackMembers, (IPack.PackRelationship.Member, e.SenderID, e.Information.Gender)))
                                    if (Hunger > MaxHunger * HungerFoodSeekingLevel) 
                                        if (TimeToReproductionNeed <= 0)
                                            if (Age >= ReproductionAge)
                                                e.AddMateInformation((ID, Location));
        }

        protected override void DamageEventHandler(object sender, ControlEvents.DoHealthDamageEventArgs e)
        { //delegate. This lifeform has taken damage.
            if (e.IDs.ReceiverID == ID)
            {
                if (!helper.Contains(AttackedBy, e.IDs.SenderID))
                {
                    string[] attacks = AttackedBy;
                    helper.Add(ref attacks, e.IDs.SenderID);
                    AttackedBy = attacks;
                }
                Health -= e.Damage;
                if (Health <= 0)
                {
                    isKilled = true;
                    Death();
                    e.Died = true;
                }
                else
                    e.Died = false;
            }
        }

        protected override void RemoveSubscriptions()
        {
            lifeformPublisher.RaiseTransmitData -= RelationshipEventHandler;
            lifeformPublisher.RaisePossibleRelationshipJoiner -= RelationshipCandidateEventHandler;
            base.RemoveSubscriptions();
        }
    }
}
