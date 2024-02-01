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

        public DownloadView()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string url = TextBox_SongUrl.Text;
            string name = TextBox_SongName.Text;
            string artistName = TextBox_Artist.Text;

            TextBox_Artist.Text = "";
            TextBox_SongName.Text = "";
            TextBox_SongUrl.Text = "";

            Task.Run(() => {
                resourceManager.downloadSong(url, name, artistName); 
            });
            while (resourceManager.getCurrentStatusMessage() != "Successfully added song to library!")
            {
                if(resourceManager.getCurrentStatusMessage() == "Failed!")
                {
                    return;
                }
                int progress = resourceManager.getCurrentProgress();
                string status = resourceManager.getCurrentStatusMessage();

                Dispatcher.Invoke(() => {
                    Progress.Value = progress;
                    Console.WriteLine("Status is: " + status);
                    Status.Text = status;
                });

                await Task.Delay(100);
            }
        }
    }
}
