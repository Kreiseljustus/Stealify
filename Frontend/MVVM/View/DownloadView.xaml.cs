using Frontend.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
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

namespace Frontend.MVVM.View
{
    /// <summary>
    /// Interaction logic for DownloadView.xaml
    /// </summary>
    public partial class DownloadView : UserControl
    {
        static BackendAPI backendAPI = BackendAPI.getBackendAPI();
        Core.ResourceManager resourceManager = backendAPI.createResourceManager();

        string songUrl;
        string songName;
        string artist;

        public DownloadView()
        {
            InitializeComponent();
        }

        //Song url
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            songUrl = TextBox_SongUrl.Text;
            Console.WriteLine("Text is: " + TextBox_SongUrl.Text);
            Console.WriteLine(songUrl);
        }
        
        //Song name
        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            songName = TextBox_SongName.Text;
            Console.WriteLine("Text is: " + TextBox_SongName.Text);
            Console.WriteLine(songName);
        }

        //Artist
        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {
            artist = TextBox_Artist.Text;
            Console.WriteLine("Text is: " + TextBox_Artist.Text);
            Console.WriteLine(artist);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(resourceManager.getCurrentProgress());
        }
    }
}
