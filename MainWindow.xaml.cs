using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        
        private Image image;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            image = ImageBox;
            Loaded += OnWindowLoaded;
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            MapInformation mapInformation = MapInformation.Instance;
            mapInformation.SetSizeOfMap = ((ushort)image.Width, (ushort)image.Height);
            Output output = Output.Instance;

            output.PaintEvent += Output_UpdateVisualEvent;

            output.Map = new Bitmap(mapInformation.GetSizeOfMap.width, mapInformation.GetSizeOfMap.height);
            new Wolf("Carnis Lupus", (00, 00), new string[] { "Rabbit" }, Helper.Instance, Publisher.GetAnimalInstance, Publisher.GetDrawInstance, MapInformation.Instance); //here for testing and nothing else.
            new Wolf("Carnis Lupus", (130, 20), new string[] { "Rabbit" }, Helper.Instance, Publisher.GetAnimalInstance, Publisher.GetDrawInstance, MapInformation.Instance); //here for testing and nothing else.
            new Wolf("Carnis Lupus", (70, 230), new string[] { "Rabbit" }, Helper.Instance, Publisher.GetAnimalInstance, Publisher.GetDrawInstance, MapInformation.Instance); //here for testing and nothing else.
            for(int i = 0; i < 10; i++)
                new Rabbit("Rabbit", (100,100), null, Helper.Instance, Publisher.GetAnimalInstance, Publisher.GetDrawInstance, MapInformation.Instance);
            output.RunVisualThread();
            output.RunAIThread();

        }

        private void Output_UpdateVisualEvent(object sender, ImageEventArgs eva)
        {
            BitmapSource MapImage = ConvertBitmapToBitmapImage(eva.BitMapImage);
            MapImage.Freeze();
            UpdateVisualImage(MapImage);
        }

        public void UpdateVisualImage(BitmapSource bitmapImage) 
        {
            Application.Current.Dispatcher.Invoke(() => //can cause an exception if closing down the window.
            {
                ImageBox.Source.Freeze();
                ImageBox.Source = bitmapImage;
            });
        }

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
