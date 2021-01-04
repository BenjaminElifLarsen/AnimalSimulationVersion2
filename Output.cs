#define DEBUG
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Media.Imaging;

namespace AnimalSimulationVersion2
{
    class Output
    { //draws out to the map image and displays it


        public delegate void paintEvent(object sender, ImageEventArgs args);
        public event paintEvent PaintEvent;

        public delegate void textEvent(object sender, TextEventArgs args);
        public event textEvent TextEvent;

        /// <summary>
        /// The bitmap of the map.
        /// </summary>
        public Bitmap Map { get; set; }
        /// <summary>
        /// The bitmapImage of the map.
        /// </summary>
        public BitmapImage MapImage { get; set; }
        /// <summary>
        /// The amount of frames per second.
        /// </summary>
        public float FramesPerSecond { get; set; }
        /// <summary>
        /// The time between frames.
        /// </summary>
        private float TimeInSecondsBetweenFrames { get; set; }
        /// <summary>
        /// An instance of DrawPublisher.
        /// </summary>
        private DrawPublisher DrawPublisher { get; set; }
        /// <summary>
        /// An instance of LifeformPublisher.
        /// </summary>
        private LifeformPublisher LifeformPublisher { get; set; }
        /// <summary>
        /// The instance of Output.
        /// </summary>
        public static Output Instance { get; }

        /// <summary>
        /// The static constructor. Sets the Instance up for use.
        /// </summary>
        static Output()
        {
            Instance = new Output();
            Instance.DrawPublisher = Publisher.GetDrawInstance;
            Instance.LifeformPublisher = Publisher.GetLifeformInstance; //set this and the one above from MainWindow
            Instance.FramesPerSecond = 30;
            Instance.TimeInSecondsBetweenFrames = 1 / Instance.FramesPerSecond;
        }
        /// <summary>
        /// Starts the visual thread up.
        /// </summary>
        public void RunVisualThread()
        {
            Thread thread = new Thread(VisualThread);
            thread.Start();
        }
        /// <summary>
        /// Setarts the AI thread up.
        /// </summary>
        public void RunAIThread()
        {
            Thread thread = new Thread(AIThread);
            thread.Start();
        }
        /// <summary>
        /// Thread that loops the AI events.
        /// </summary>
        private void AIThread()
        {
            DateTime lastTime = DateTime.Now;
            while (true)
            {
                double passedTime = (DateTime.Now - lastTime).TotalSeconds;
                if (passedTime >= TimeInSecondsBetweenFrames)
                {
                    lastTime = DateTime.Now;
                    LifeformPublisher.AI((float)passedTime);
                }
            }
        }
        /// <summary>
        /// Thread that loops the Draw and speciesAndAmount events.
        /// </summary>
        private void VisualThread()
        {
            DateTime lastTime = DateTime.Now;
            while (true)
            {
                double passedTime = (DateTime.Now - lastTime).TotalSeconds;
                if (passedTime >= TimeInSecondsBetweenFrames)
                {
                    lastTime = DateTime.Now;
                    PaintEvent?.Invoke(this, new ImageEventArgs { BitMapImage = Draw(Map, DrawPublisher.Draw()) });
                    TextEvent?.Invoke(this, new TextEventArgs { ListInformation = DrawPublisher.SpeciesAndAmount() });
                }
            }
        }

        /// <summary>
        /// Draws out a bitmap with all lifeforms.
        /// </summary>
        /// <param name="map">The map to draw on.</param>
        /// <param name="drawInforamtion">The information of all lifeforms.</param>
        /// <returns>A map painted with all lifeforms.</returns>
        private Bitmap Draw(Bitmap map, List<(Point[] Design, Colour Colour, Vector Location)> drawInforamtion)
        {
            using (Graphics g = Graphics.FromImage(map))
            {
                g.Clear(Color.Black);
                if(drawInforamtion != null)
                    foreach ((Point[] Design, Colour Colour, Vector Location) information in drawInforamtion)
                    {
                        Point[] drawLocations = new Point[information.Design.Length];
                        (byte xCenter,byte yCenter) center = CalculateCenter(information.Design);
                        for (int i = 0; i < information.Design.Length; i++)
                        {
                            drawLocations[i].X = (int)(information.Design[i].X + information.Location.X - center.xCenter); 
                            drawLocations[i].Y = (int)(information.Design[i].Y + information.Location.Y - center.yCenter);
                        }
                        using (Pen pen = new Pen(Color.FromArgb(information.Colour.Alpha, information.Colour.Red, information.Colour.Green, information.Colour.Blue), 1))
                        {
                            g.DrawPolygon(pen, drawLocations);
                        }

                    }
            }
            return map;
        }
        /// <summary>
        /// Calculates the center of <paramref name="design"/>.
        /// </summary>
        /// <param name="design">Contains the design of a lifeform.</param>
        /// <returns>The values of the center (x,y)</returns>
        private (byte xCenter,byte yCenter) CalculateCenter(Point[] design)
        {
            int heighestHeight = int.MinValue;
            int widthestWidth = int.MinValue;
            int smallestHeight = int.MaxValue;
            int smallestWidth = int.MaxValue;
            foreach(Point point in design)
            {
                if (point.X > widthestWidth)
                    widthestWidth = point.X;
                if (point.X < smallestWidth)
                    smallestWidth = point.X;
                if (point.Y > heighestHeight)
                    heighestHeight = point.Y;
                if (point.Y < smallestHeight)
                    smallestHeight = point.Y;
            }
            byte xCenter = (byte)((smallestWidth + widthestWidth) / 2);
            byte yCenter = (byte)((smallestHeight + heighestHeight) / 2);
            return (xCenter, yCenter);
        }
    }
    /// <summary>
    /// Contains the information for the image event.
    /// </summary>
    public class ImageEventArgs : EventArgs
    {
        public Bitmap BitMapImage { get; set; }
    }
    /// <summary>
    /// Contains the information for the text event.
    /// </summary>
    public class TextEventArgs : EventArgs
    {
        public List<(string Species, ushort Amount)> ListInformation { get; set; }
    }
}
