﻿using Microsoft.VisualBasic;
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

        public Bitmap Map { get; set; }
        public BitmapImage MapImage { get; set; }
        public float FramesPerSecond { get; set; }
        private float TimeInSecondsBetweenFrames { get; set; }
        private DrawPublisher DrawPublisher { get; set; }
        private LifeformPublisher AnimalPublisher { get; set; }
        public static Output Instance { get; }

        static Output()
        {
            Instance = new Output();
            Instance.DrawPublisher = Publisher.GetDrawInstance;
            Instance.AnimalPublisher = Publisher.GetLifeformInstance; //set this and the one above from MainWindow
            Instance.FramesPerSecond = 30;
            Instance.TimeInSecondsBetweenFrames = 1 / Instance.FramesPerSecond;
        }
        public void RunVisualThread()
        {
            Thread thread = new Thread(VisualThread);
            thread.Start();
        }
        public void RunAIThread()
        {
            Thread thread = new Thread(AIThread);
            thread.Start();
        }

        private void AIThread()
        {
            DateTime lastTime = DateTime.Now;
            while (true)
            {
                double passedTime = (DateTime.Now - lastTime).TotalSeconds;
                if (passedTime >= TimeInSecondsBetweenFrames)
                {
                    lastTime = DateTime.Now;
                    AnimalPublisher.AI((float)passedTime);
                }
            }
        }

        private void VisualThread()
        {
            //Thread.CurrentThread.IsBackground = false; //did not help
            DateTime lastTime = DateTime.Now;
            while (true)
            {
                if ((DateTime.Now - lastTime).TotalSeconds >= TimeInSecondsBetweenFrames) //the part before >= needs also be accessed by the AI thread. 
                {
                    lastTime = DateTime.Now;
                    List<(Point[] Design, Colour Colour, Vector Location)> drawInforamtion = DrawPublisher.Draw();
                    List<(string Species, ushort Amount)> speciesInformation = DrawPublisher.SpeciesAndAmount();
                    Map = Draw(Map, drawInforamtion);
                    PaintEvent?.Invoke(this, new ImageEventArgs { BitMapImage = Map });
                    TextEvent?.Invoke(this, new TextEventArgs { ListInformation = speciesInformation });
                }
            }
        }

        private Bitmap Draw(Bitmap map, List<(Point[] Design, Colour Colour, Vector Location)> drawInforamtion)
        {
            using (Graphics g = Graphics.FromImage(map))
            {
                g.Clear(Color.Black);
                if(drawInforamtion != null)
                    foreach ((Point[] Design, Colour Colour, Vector Location) information in drawInforamtion)
                    {
                        Point[] drawLocations = new Point[information.Design.Length];
                        //for now, consider Location as top left.
                        (byte xCenter,byte yCenter) center = CalculateCenter(information.Design);
                        for (int i = 0; i < information.Design.Length; i++)
                        {
                            drawLocations[i].X = (int)(information.Design[i].X + information.Location.X - center.xCenter); //maybe create a new Point array of same size, call it drawinglocations and use that one instead of information.Design
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
                    heighestHeight = point.X;
                if (point.Y < smallestHeight)
                    smallestHeight = point.Y;
            }
            byte xCenter = (byte)((smallestWidth + widthestWidth) / 2);
            byte yCenter = (byte)((smallestHeight + heighestHeight) / 2);
            return (xCenter, yCenter);
        }
    }

    public class ImageEventArgs : EventArgs
    {
        public Bitmap BitMapImage { get; set; }
    }
    public class TextEventArgs : EventArgs
    {
        public List<(string Species, ushort Amount)> ListInformation { get; set; }
    }
}
