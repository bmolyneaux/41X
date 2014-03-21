using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using UserControl = System.Windows.Controls.UserControl;

namespace VrPlayer.Medias.VlcDotNetDual
{
    public partial class VlcDotNetDualPanel : UserControl
    {
        private readonly VlcDotNetDualMedia _media;

        public VlcDotNetDualPanel(VlcDotNetDualMedia media)
        {
            _media = media;

            InitializeComponent();
            try
            {
                DataContext = media;
            }
            catch (Exception exc)
            {
            }
        }

        private void LibVlcDllsPath_Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                _media.LibVlcDllsPath = dialog.SelectedPath;
        }

        private void LibVlcPluginsPath_Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                _media.LibVlcPluginsPath = dialog.SelectedPath;
        }

        private void DownloadVlc_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://www.videolan.org/vlc/");
        }
    }
}
