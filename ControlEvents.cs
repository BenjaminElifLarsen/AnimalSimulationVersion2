using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class ControlEvents : EventArgs
    {
        /// <summary>
        /// Class that holds event data to get all possible lifeforms.
        /// </summary>
        public class GetPossiblePreyEventArgs
        {
            private List<(Vector PreyLocation, string PreyID, string PreySpecies)> preys = new List<(Vector PreyLocation, string PreyID, string PreySpecies)>();
            /// <summary>
            /// The ID of the sender. Used to prevent the sender from reacting to this event.
            /// </summary>
            public string SenderID { get; set; }
            /// <summary>
            /// Basic constructor.
            /// </summary>
            /// <param name="senderID">The ID of the sender. Used to prevent the sender from reacting to this event.</param>
            public GetPossiblePreyEventArgs(string senderID)
            {
                SenderID = senderID;
            }
            /// <summary>
            /// Adds <paramref name="infoToAdd"/> to the list of possible preys.
            /// </summary>
            /// <param name="infoToAdd">Information about the lifeform.</param>
            public void AddPreyInformation((Vector PreyLocation, string PreyID, string PreySpecies) infoToAdd)
            {
                preys.Add(infoToAdd);
            }
            /// <summary>
            /// Gets the list of all possible preys.
            /// </summary>
            /// <returns>A list with the lcoation, ID and species of all lifeforms.</returns>
            public List<(Vector PreyLocation, string PreyID, string PreySpecies)> GetPossiblePreys()
            {
                return preys;
            }
        }
        /// <summary>
        /// Class that holds event data to set a specific lifeform as a prey for a specific predator.
        /// </summary>
        public class SetPreyEventArgs
        {
            /// <summary>
            /// Basic constructor.
            /// </summary>
            /// <param name="senderID">The ID of the predator that <paramref name="receiverID"/> should know of.</param>
            /// <param name="receiverID">The ID of the lifeform that should react to this event.</param>
            public SetPreyEventArgs(string senderID, string receiverID)
            {
                IDs = (senderID, receiverID);
            }
            /// <summary>
            /// A Tuple holding both IDs.
            /// </summary>
            public (string senderID, string receiverID) IDs { get; }
        }
        /// <summary>
        /// Class that holds event data to remove a specific lifeform as predator for a specific prey.
        /// </summary>
        public class RemovePreyEventArgs
        {
            /// <summary>
            /// Basic constructor.
            /// </summary>
            /// <param name="senderID">The ID of the predator.</param>
            /// <param name="receiverID">The ID of the prey.</param>
            public RemovePreyEventArgs(string senderID, string receiverID)
            {
                IDs = (senderID, receiverID);
            }
            /// <summary>
            /// A Tuple holding both IDs.
            /// </summary>
            public (string senderID, string receiverID) IDs { get; }
        }
        /// <summary>
        /// Class that holds event data to inform a specific lifeform that it got a mate.
        /// </summary>
        public class SetMateEventArgs
        {
            /// <summary>
            /// Basic constructor.
            /// </summary>
            /// <param name="senderID">The ID of the lifeform that sent this event.</param>
            /// <param name="receiverID">The ID of the lifeform that shall react to this event.</param>
            public SetMateEventArgs(string senderID, string receiverID)
            {
                IDs = (senderID, receiverID);
            }
            /// <summary>
            /// A Tuple holding both IDs.
            /// </summary>
            public (string senderID, string receiverID) IDs { get; }
        }
        /// <summary>
        /// Class that holds event data to inform a specific lifeform that it does no longer have a mate.
        /// </summary>
        public class RemoveMateEventArgs
        {
            /// <summary>
            /// Basic constructor.
            /// </summary>
            /// <param name="senderID">The ID of the animal that sent this event.</param>
            /// <param name="receiverID">The ID of the animal that shall receives the event.</param>
            public RemoveMateEventArgs(string senderID, string receiverID)
            {
                IDs = (senderID, receiverID);
            }
            /// <summary>
            /// A Tuple holding both IDs.
            /// </summary>
            public (string senderID, string receiverID) IDs { get; }
        }
        /// <summary>
        /// Class that holds event data regarding possible mates.
        /// </summary>
        public class PossibleMateEventArgs
        {
            private List<(string mateID, Vector Location)> possibleMates = new List<(string mateID, Vector Location)>();
            /// <summary>
            /// The ID of the sender, used to prevent the sender from reacting to this event.
            /// </summary>
            public string SenderID { get; set; }
            /// <summary>
            /// Basic constructor.
            /// </summary>
            /// <param name="species">The species of the sender.</param>
            /// <param name="gender">The gender of the sender.</param>
            /// <param name="senderID">The ID of the sender.</param>
            public PossibleMateEventArgs(string species, char gender, string senderID)
            {
                Information = (species, gender);
                SenderID = senderID;
            }
            /// <summary>
            ///  A Tuple holding the species and gender of the sender.
            /// </summary>
            public (string Species, char Gender) Information { get;  }
            /// <summary>
            /// Adds information about the possible mate to a list.
            /// </summary>
            /// <param name="information">The ID and location of the possible mate.</param>
            public void AddMateInformation((string mateID, Vector Location) information)
            {
                possibleMates.Add(information);
            }
            /// <summary>
            /// Gets the list of possible mates.
            /// </summary>
            /// <returns></returns>
            public List<(string mateID, Vector Location)> GetPossibleMates()
            {
                return possibleMates;
            }
        }
        /// <summary>
        /// Class that holds event data used for drawing.
        /// </summary>
        public class DrawEventArgs
        {
            private List<(Point[] Design, (byte Red, byte Green, byte Blue) Colour, Vector Location)> drawInformation = new List<(Point[] Design, (byte Red, byte Green, byte Blue) Colour, Vector Location)>();
            /// <summary>
            /// Basic constructor
            /// </summary>
            public DrawEventArgs()
            {
            }
            /// <summary>
            /// Adds inforamtion needed for drawing the lifeform.
            /// </summary>
            /// <param name="information">The dsign, colours and location of the lifeform.</param>
            public void AddDrawInformation((Point[] Design, (byte Red, byte Green, byte Blue), Vector Location) information)
            {
                drawInformation.Add(information);
            }
            /// <summary>
            /// Returns the list containing all of the needed information for drawing all lifeforms.
            /// </summary>
            public List<(Point[] Design, (byte Red, byte Green, byte Blue) Colour, Vector Location)> DrawInformation => drawInformation;
            
        }
        /// <summary>
        /// Class that holds event data used for the AI.
        /// </summary>
        public class AIEventArgs
        {
            /// <summary>
            /// Time since the last update.
            /// </summary>
            public float TimeSinceLastUpdate { get; }
            /// <summary>
            /// Basic constructor.
            /// </summary>
            /// <param name="timeSinceLastUpdate">The time that has pasted since the last call.</param>
            public AIEventArgs(float timeSinceLastUpdate)
            {
                TimeSinceLastUpdate = timeSinceLastUpdate;
            }
        }
        /// <summary>
        /// Class that holds event data regarding a lifeform getting eaten.
        /// </summary>
        public class EatenEventArgs
        {
            /// <summary>
            /// The nutrient value of the lifeform.
            /// </summary>
            private float nutrienceValue;
            /// <summary>
            /// The ID of the lifeform that got eaten.
            /// </summary>
            public string ReceiverID { get; }
            /// <summary>
            /// Basic constructor.
            /// </summary>
            /// <param name="receiverID">The ID of the lifeform that eaten.</param>
            public EatenEventArgs(string receiverID)
            {
                ReceiverID = receiverID;
            }
            /// <summary>
            /// Sets the nutrient value.
            /// </summary>
            /// <param name="value">The nutrient value of the lifeform.</param>
            public void SetNutrient(float value) => nutrienceValue = value;
            /// <summary>
            /// Gets the nutrient of the lifeform that was eaten.
            /// </summary>
            public float GetNutrient => nutrienceValue;
        }
        /// <summary>
        /// Class that holds event data regarding a lifeform dying.
        /// </summary>
        public class DeadEventArgs
        {
            /// <summary>
            /// The ID of the lifeform that has died.
            /// </summary>
            public string ReceiverID { get; }
            /// <summary>
            /// Basic constructor.
            /// </summary>
            /// <param name="receiverID">The ID of the lifeform that has died.</param>
            public DeadEventArgs(string receiverID)
            {
                ReceiverID = receiverID;
            }
        }
        /// <summary>
        /// Class that holds event data regarding getting the location of a specific lifeform.
        /// </summary>
        public class GetOtherLocationEventArgs
        {
            /// <summary>
            /// The ID of the lifeform' whoes location was requested.
            /// </summary>
            public string ReceiverID { get; }
            /// <summary>
            /// The location of the lifeform.
            /// </summary>
            public Vector Location { get; set; }
            /// <summary>
            /// Basic constructor
            /// </summary>
            /// <param name="receiverID">The ID of the lifeform' whoes location was requested.</param>
            public GetOtherLocationEventArgs(string receiverID)
            {
                ReceiverID = receiverID;
            }
            /// <summary>
            /// Gets the location of the lifeform.
            /// </summary>
            public Vector GetLocation => Location;
        }
        /// <summary>
        /// Class that holds event data such that a lifeform can now that the food it wanted is no more //rewrite
        /// </summary>
        public class InformPredatorOfPreyDeathEventArgs //rename
        {
            /// <summary>
            /// A Tuple holding both IDs.
            /// </summary>
            public (string SenderID, string ReceiverID) IDs { get; set; }
            /// <summary>
            /// Basic constructor.
            /// </summary>
            /// <param name="senderID">The ID of the lifeform that triggers this event, the food.</param>
            /// <param name="receiverID">The ID of the lifeform that should act on this event.</param>
            public InformPredatorOfPreyDeathEventArgs(string senderID, string receiverID)
            {
                IDs = (senderID,receiverID);
            }
        }
        /// <summary>
        /// Class that holds event data regarding doing damage to a specific lifeform.
        /// </summary>
        public class DoHealthDamageEventArgs
        {
            /// <summary>
            /// A Tuple holding both IDs.
            /// </summary>
            public (string SenderID, string ReceiverID) IDs { get; }
            /// <summary>
            /// The damage done.
            /// </summary>
            public byte Damage { get;}
            /// <summary>
            /// Basic constructor.
            /// </summary>
            /// <param name="senderID">The ID of the lifeform that is attacking.</param>
            /// <param name="receiverID">The ID of the lifeform that is being attacked.</param>
            /// <param name="damage">The amount of damage done.</param>
            public DoHealthDamageEventArgs(string senderID, string receiverID, byte damage)
            {
                IDs = (senderID, receiverID);
                Damage = damage;
            }
        }
        /// <summary>
        /// Class that holds event data regarding getting the location and ID of all lifeforms. 
        /// </summary>
        public class GetAllLocationsEventArgs //maybe also species, so ITerritorial can use it better.
        {
            private List<(Vector Location, string ID)> locationsAndIDs = new List<(Vector Location, string ID)>();
            /// <summary>
            /// The ID of the sender, used to prevent the sender from reacting to the event.
            /// </summary>
            public string SenderID { get; }
            /// <summary>
            /// Basic constructor.
            /// </summary>
            /// <param name="senderID">The ID of the sender.</param>
            public GetAllLocationsEventArgs(string senderID)
            {
                SenderID = senderID;
            }
            /// <summary>
            /// Adds a lifeform' location and id to the list.
            /// </summary>
            /// <param name="location">The location of the lifeform.</param>
            /// <param name="id">The ID of the lifeform.</param>
            public void Add(Vector location, string id) => locationsAndIDs.Add((location, id));
            /// <summary>
            /// Gets the list of locations and IDs.
            /// </summary>
            public List<(Vector Location, string ID)> GetInformation => locationsAndIDs;
        }
        /// <summary>
        /// Class that holds event data regarding of what types of species exist and how many.
        /// </summary>
        public class SpeciesAndAmountEventArgs
        {
            private List<(string Species, ushort Amount)> information = new List<(string, ushort)>();
            /// <summary>
            /// Gets the list of species and amount.
            /// </summary>
            public List<(string Species, ushort Amount)> GetList => information;
            /// <summary>
            /// Basic constructor
            /// </summary>
            public SpeciesAndAmountEventArgs()
            {
            }
            /// <summary>
            /// Adds a lifeform to the list. 
            /// If <paramref name="species"/> does not exist it add it to the list and sets the amount to 1.
            /// Else it add 1 to the total of that species. 
            /// </summary>
            /// <param name="species">The species of the lifeform.</param>
            public void Add(string species)
            {
                for (int i = 0; i <= information.Count; i++)
                {
                    if (i == information.Count)
                    {
                        information.Add((species, 1));
                        break;
                    }
                    else if (information[i].Species == species)
                    {
                        information[i] = (species, (ushort)(information[i].Amount + 1));
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Class that holds event data for transmitting an object to a lifeform.
        /// </summary>
        public class TransmitDataEventArgs//figure out a better name
        {
            /// <summary>
            /// A Tuple holding both IDs.
            /// </summary>
            public (string SenderID, string ReceiverID) IDs { get; }
            /// <summary>
            /// An object of data.
            /// </summary>
            public object Data { get; }
            /// <summary>
            /// Basic constructor.
            /// </summary>
            /// <param name="senderID">The ID of the sender.</param>
            /// <param name="receiverID">The ID of the receiver.</param>
            /// <param name="data">The data to be transmitted.</param>
            public TransmitDataEventArgs(string senderID, string receiverID, object data)
            {
                IDs = (senderID, receiverID);
                Data = data;
            }
        }
        /// <summary>
        /// Class that holds event data for finding possible relationship candidates. 
        /// </summary>
        public class RelationshipCandidatesEventArgs
        {
            private List<(Vector Location, string ID, char Gender)> information = new List<(Vector Location, string ID, char Gender)>();
            /// <summary>
            /// The ID of the sender.
            /// </summary>
            public string SenderID { get; }
            /// <summary>
            /// THe species of the sender.
            /// </summary>
            public string Species { get; }
            public Type RelationshipType { get; }
            /// <summary>
            /// Basic constructor.
            /// </summary>
            /// <param name="senderID">The ID of the sender.</param>
            /// <param name="species">The species of the sender.</param>
            /// <param name="relationshipType">The type of the relationship interface.</param>
            public RelationshipCandidatesEventArgs(string senderID, string species, Type relationshipType)
            {
                SenderID = senderID;
                Species = species;
                RelationshipType = relationshipType;
            }
            /// <summary>
            /// Gets a list of possible relationship candidates.
            /// </summary>
            public List<(Vector Location, string ID, char Gender)> GetList => information;
            /// <summary>
            /// Adds the lifeform as a possible candicate.
            /// </summary>
            /// <param name="location">The location of the lifeform.</param>
            /// <param name="ID">The ID of the lifeform.</param>
            /// <param name="gender">The gender of the lifeform.</param>
            public void Add(Vector location, string ID, char gender) => information.Add((location, ID, gender));
        }
    }
}
