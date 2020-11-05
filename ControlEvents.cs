using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class ControlEvents : EventArgs
    {
        public class GetPossiblePreyEventArgs
        {
            private List<((float X, float Y) PreyLocation, string PreyID, string PreySpecies)> preys = new List<((float X, float Y) PreyLocation, string PreyID, string PreySpecies)>();
            public string SenderID { get; set; }
            public GetPossiblePreyEventArgs(string senderID)
            {
                SenderID = senderID;
            }

            public void AddPreyInformation(((float X, float Y) PreyLocation, string PreyID, string PreySpecies) infoToAdd)
            {
                preys.Add(infoToAdd);
            }

            public List<((float X, float Y) PreyLocation, string PreyID, string PreySpecies)> GetPossiblePreys()
            {
                return preys;
            }
        }
        public class SetPreyEventArgs
        {
            public SetPreyEventArgs(string senderID, string receiverID)
            {
                IDs = (senderID, receiverID);
            }
            public (string senderID, string receiverID) IDs { get; }
        }
        public class RemovePreyEventArgs
        {
            public RemovePreyEventArgs(string senderID, string receiverID)
            {
                IDs = (senderID, receiverID);
            }
            public (string senderID, string receiverID) IDs { get; }
        }
        public class SetMateEventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="senderID">The ID of the animal that sends the event.</param>
            /// <param name="receiverID">The ID of the animal that shall receives the event.</param>
            public SetMateEventArgs(string senderID, string receiverID)
            {
                IDs = (senderID, receiverID);
            }
            public (string senderID, string receiverID) IDs { get; }
        }
        public class RemoveMateEventArgs
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="senderID">The ID of the animal that sends the event.</param>
            /// <param name="receiverID">The ID of the animal that shall receives the event.</param>
            public RemoveMateEventArgs(string senderID, string receiverID)
            {
                IDs = (senderID, receiverID);
            }
            public (string senderID, string receiverID) IDs { get; }
        }
        public class PossibleMateEventArgs
        {
            private List<(string mateID, (float X, float Y) Location)> possibleMates = new List<(string mateID, (float X, float Y) Location)>();
            public string SenderID { get; set; }
            public PossibleMateEventArgs(string species, char gender, string senderID)
            {
                Information = (species, gender);
                SenderID = senderID;
            }
            public (string Species, char Gender) Information { get;  }
            public void AddMateInformation((string mateID, (float X, float Y) Location) information)
            {
                possibleMates.Add(information);
            }
            public List<(string mateID, (float X, float Y) Location)> GetPossibleMates()
            {
                return possibleMates;
            }
        }
        public class DrawEventArgs
        {
            private List<(Point[] Design, (int Red, int Green, int Blue) Colour, (float X, float Y) Location)> drawInformation = new List<(Point[] Design, (int Red, int Green, int Blue) Colour, (float X, float Y) Location)>();
            public DrawEventArgs()
            {

            }
            public void AddDrawInformation((Point[] Design, (int Red, int Green, int Blue), (float X, float Y) Location) information)
            {
                drawInformation.Add(information);
            }
            public List<(Point[] Design, (int Red, int Green, int Blue) Colour, (float X, float Y) Location)> DrawInformation()
            {
                return drawInformation;
            }
        }
        public class AIEventArgs
        {
            public float TimeSinceLastUpdate { get; }
            public AIEventArgs(float timeSinceLastUpdate)
            {
                this.TimeSinceLastUpdate = timeSinceLastUpdate;
            }
        }
        public class EatenEventArgs
        {
            private float nutrienceValue;
            public string ReceiverID { get; }
            public EatenEventArgs(string receiverID)
            {
                ReceiverID = receiverID;
            }
            public void SetNutrience(float value) => nutrienceValue = value;
            public float GetNutrience() => nutrienceValue;
        }
        public class DeadEventArgs
        {
            public string ReceiverID { get; }
            public DeadEventArgs(string receiverID)
            {
                ReceiverID = receiverID;
            }
        }
        public class GetOtherLocationEventArgs
        {
            public string ReceiverID { get; }
            public (float X, float Y) Location { get; set; }
            public GetOtherLocationEventArgs(string receiverID)
            {
                ReceiverID = receiverID;
            }
            public (float X, float Y) GetLocation => Location;
        }
        public class InformPredatorOfPreyDeathEventArgs
        {
            public (string SenderID, string ReceiverID) IDs { get; set; }
            public InformPredatorOfPreyDeathEventArgs(string senderID, string receiverID)
            {
                IDs = (senderID,receiverID);
            }
        }
    }
}
