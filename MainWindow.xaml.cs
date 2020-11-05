using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
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
        private static Image image;
        public MainWindow()
        {
            InitializeComponent();
            image = ImageBox;
            Loaded += OnWindowLoaded;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            //create and run the visual thread and the AI thread.
            MapInformation mapInformation = MapInformation.Instance;
            mapInformation.SetSizeOfMap = ((ushort)image.Width, (ushort)image.Height);
            Output output = Output.Instance;
            output.Map = new Bitmap(mapInformation.GetSizeOfMap.width, mapInformation.GetSizeOfMap.height);
            new Wolf("Carnis Lupus", (20, 20), null, Helper.Instance, Publisher.GetAnimalInstance, Publisher.GetDrawInstance, MapInformation.Instance); //here for testing and nothing else.
            output.RunVisualThread();
            output.RunAIThread();

        }

        public delegate void UpdateVisualDelegate(BitmapImage image);
        public static UpdateVisualDelegate VisualUpdate = UpdateVisualImage;

        private static void UpdateVisualImage(BitmapImage bitmapImage) //send this one with output using a delegate
        { //this when called causes problems
            //Exception: System.InvalidOperationException - The calling thread cannot access this object because a different thread owns it.
            //this.Dispatcher.Invoke((Action)(() => { image.Source = bitmapImage; })); //does not work since it is a static class
            //System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke((Action)(() => { image.Source = bitmapImage; })); //did not help
            //System.Windows.Application.Current.Dispatcher.Invoke((Action)(() => { image.Source = bitmapImage; })); //nope 
            image.Source = bitmapImage;
        }
        public delegate BitmapImage ConvertBitMapToBitMapIamge(Bitmap bitmap);
        public static ConvertBitMapToBitMapIamge GenerateBitMapImage = ConvertBitmapToBitmapImage;
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
            return bitmapImage;
        }
    }
}
