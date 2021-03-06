﻿#define DEBUG
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Image = System.Windows.Controls.Image;

namespace AnimalSimulationVersion2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Loaded += OnWindowLoaded;
        }
        /// <summary>
        /// Runs code that depends on the UI components have been initialised.
        /// Also starts two threads up, one for updating the UI and one for running the AI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            MapInformation mapInformation = MapInformation.Instance;
            mapInformation.SetSizeOfMap = ((ushort)ImageBox.Width, (ushort)ImageBox.Height);
            Output output = Output.Instance;

            output.PaintEvent += Output_UpdateVisualEvent;
            output.TextEvent += Output_UpdateInformationEvent;

            output.Map = new Bitmap(mapInformation.GetSizeOfMap.width, mapInformation.GetSizeOfMap.height);
            for(int i = 0; i < 8; i++) //8
                new SleepingCarnivore("Carnis Lupus", new Vector(mapInformation, Helper.Instance), new string[] { "Oryctolagus Cuniculus", "Antidorcas Marsupialis" }, Helper.Instance, Publisher.GetLifeformInstance, Publisher.GetDrawInstance, MapInformation.Instance); //here for testing and nothing else.
            for(int i = 0; i < 20; i++) //20
                new HidingHerbavore("Oryctolagus Cuniculus", new Vector(mapInformation, Helper.Instance), new string[] {"Cucumis Melo", "Carica Papaya" }, Helper.Instance, Publisher.GetLifeformInstance, Publisher.GetDrawInstance, MapInformation.Instance);
            for (int i = 0; i < 20; i++) //20 //find a species that makes more sense
                new SleepingHerbavore("Antidorcas Marsupialis", new Vector(mapInformation, Helper.Instance), new string[] { "Cucumis Melo", "Carica Papaya" }, Helper.Instance, Publisher.GetLifeformInstance, Publisher.GetDrawInstance, MapInformation.Instance);
            for (int i = 0; i < 2; i++) //2
                new MonoeciousPlant("Cucumis Melo", new Vector(mapInformation, Helper.Instance), Helper.Instance, Publisher.GetLifeformInstance, Publisher.GetDrawInstance, MapInformation.Instance);
            for(int i = 0; i < 30; i++) //30
                new DioeciousPlant("Carica Papaya", new Vector(mapInformation, Helper.Instance), Helper.Instance, Publisher.GetLifeformInstance, Publisher.GetDrawInstance, MapInformation.Instance);
            for (int i = 0; i < 6; i++) //6
                new PackCarnivore("Crocuta Crocuta", new Vector(mapInformation, Helper.Instance), new string[] { "Oryctolagus Cuniculus", "Antidorcas Marsupialis" }, Helper.Instance, Publisher.GetLifeformInstance, Publisher.GetDrawInstance, MapInformation.Instance);
            for(int i = 0; i < 6; i++) //6
                new BirdCarnivore("Haliaeetus Albicilla", new Vector(mapInformation, Helper.Instance), new string[] { "Turdus Merula", "Antidorcas Marsupialis","Oryctolagus Cuniculus","Carnis Lupus" }, Helper.Instance, Publisher.GetLifeformInstance, Publisher.GetDrawInstance, MapInformation.Instance);
            for (int i = 0; i < 10; i++) //6
                new BirdHerbavore("Turdus Merula", new Vector(mapInformation, Helper.Instance), new string[] { "Carica Papaya", "Cucumis Melo" }, Helper.Instance, Publisher.GetLifeformInstance, Publisher.GetDrawInstance, MapInformation.Instance);
            output.RunVisualThread();
            output.RunAIThread();

        }
        /// <summary>
        /// Evnethandler for updating the text UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Output_UpdateInformationEvent(object sender, TextEventArgs e)
        {
            string text = "";
            if(e.ListInformation != null)
                foreach ((string species, ushort amount) in e.ListInformation) 
                    text += species + ": " + amount + Environment.NewLine;
            UpdateInformation(text);
        }

        /// <summary>
        /// Updates the text visual part of the UI.
        /// </summary>
        /// <param name="text">The text to display.</param>
        public void UpdateInformation(string text)
        {
            if(LifeformInformation != null)
                Application.Current.Dispatcher.Invoke(() =>
                {
                    LifeformInformation.Text = text;
                });
        }
        /// <summary>
        /// Eventhandler for updating the visual UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Output_UpdateVisualEvent(object sender, ImageEventArgs e)
        {
            BitmapSource MapImage = ConvertBitmapToBitmapImage(e.BitMapImage);
            MapImage.Freeze();
            UpdateVisualImage(MapImage);
        }

        /// <summary>
        /// Updates the image visual part of the UI.
        /// </summary>
        /// <param name="bitmapImage">The image to display.</param>
        public void UpdateVisualImage(BitmapSource bitmapImage) 
        {
            if(ImageBox != null)
                Application.Current.Dispatcher.Invoke(() => //can cause an exception if closing down the window.
                {
                    ImageBox.Source.Freeze();
                    ImageBox.Source = bitmapImage;
                    #if DEBUG
                    if ((int)ImageBox.Width != 783 || (int)ImageBox.Height != 407)
                        Debug.WriteLine(ImageBox.Width + ", " + ImageBox.Height);
                    #endif
                });
        }

        /// <summary>
        /// Converts <paramref name="bitmap"/> to a BitmapImage.
        /// </summary>
        /// <param name="bitmap">The bitmap to convert.</param>
        /// <returns>Returns a BitmapImage of <paramref name="bitmap"/>.</returns>
        private static BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memoryStream = new MemoryStream()) //opens a memory stream, used to save an image in the stream and then load it into another type
            {
                bitmap.Save(memoryStream, ImageFormat.Bmp); //saves the bitmap to the stream
                memoryStream.Position = 0;
                bitmapImage.BeginInit();

                bitmapImage.StreamSource = memoryStream; //loads the bitmap into a bitmapimage
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }
            #if DEBUG
            if ((int)bitmapImage.Width+1 != (int)bitmap.Width || (int)bitmapImage.Height+1 != (int)bitmap.Height)
                Debug.WriteLine("(" + bitmapImage.Width + ", " + bitmapImage.Height + "), (" + bitmap.Width + ", " + bitmap.Height + ")");
            #endif
            return bitmapImage;
        }
    }
}
