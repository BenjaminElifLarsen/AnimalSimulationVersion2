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
            private List<(float[] PreyLocation, string PreyID, string PreySpecies)> preys = new List<(float[] PreyLocation, string PreyID, string PreySpecies)>();
            public GetPossiblePreyEventArgs()
            {

            }

            public void AddPreyInformation((float[] PreyLocation, string PreyID, string PreySpecies) infoToAdd)
            {
                preys.Add(infoToAdd);
            }

            public List<(float[] PreyLocation, string PreyID, string PreySpecies)> GetPossiblePreys()
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
        public class CanMateEventArgs
        {
            private List<(string mateID, float[] Location)> possibleMates = new List<(string mateID, float[] Location)>();
            public CanMateEventArgs(string species, char gender)
            {
                Information = (species, gender);
            }
            public (string Species, char Gender) Information { get;  }
            public void AddMateInformation((string mateID, float[] Location) information)
            {
                possibleMates.Add(information);
            }
            public List<(string mateID, float[] Location)> GetPossibleMates()
            {
                return possibleMates;
            }
        }
        public class DrawEventArgs
        {
            private List<(Point[] Design, int[] Colour, float[] Location)> drawInformation = new List<(Point[] Design, int[] Colour, float[] Location)>();
            public DrawEventArgs()
            {

            }
            public void AddDrawInformation((Point[] Design, int[] Colour, float[] Location) information)
            {
                drawInformation.Add(information);
            }
            public List<(Point[] Design, int[] Colour, float[] Location)> DrawInformation()
            {
                return drawInformation;
            }
        }
    }
}
