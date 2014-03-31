using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VrPlayer;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Medias;
using VrPlayer.ViewModels;
using VrPlayer.Services;
using VrPlayer.Models.Plugins;
using NReco;
using System.Linq;

using Application = System.Windows.Application;

public class VrIcon : Image
{
    private String _uri;
    private bool _isLive;
    private static string BASE_DIR = "pack://application:,,,/Medias/VrGui/";
    private static string MOV_DIR = "C:\\Users\\41X\\Videos\\";

    public VrIcon(String uri, double width, bool isLive) : base()
	{
        Source = createImageIcon("video_icon.png", width);
        Width = width;
        MouseUp += MouseUpHandler;
        this._uri = uri;
        this._isLive = isLive;
	}

    void MouseUpHandler(object sender, MouseButtonEventArgs e)
    {
        IPlugin<IMedia> mediaPlugin = App._pluginManager.Medias
                .Where(m => m.GetType().FullName.Contains("VrPlayer.Medias.VlcDotNetDual.VlcDotNetDualPlugin"))
                .DefaultIfEmpty(App._pluginManager.Medias.FirstOrDefault())
                .First();

        App._appState.MediaPlugin = mediaPlugin;
        if (_isLive)
        {
            // open stream at uri
            App._appState.MediaPlugin.Content.OpenStreamCommand.Execute(_uri);
        }
        else
        {
            // open file at uri
            App._appState.MediaPlugin.Content.OpenFileCommand.Execute(_uri);
        }
    }

    public Image getVideoThumbnail(string filename, double height, double width)
    {
        if (!File.Exists(MOV_DIR + filename))
        {
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            ffMpeg.GetVideoThumbnail(_uri, MOV_DIR + filename);
        }
        
        Image thumbnail = new Image();
        thumbnail.Stretch = System.Windows.Media.Stretch.Fill;
        thumbnail.Width = width;
        thumbnail.Height = height;
        thumbnail.MouseUp += this.MouseUpHandler;

        BitmapImage bmi = new BitmapImage();

        // BitmapImage.UriSource must be in a BeginInit/EndInit block
        bmi.BeginInit();
        bmi.UriSource = new Uri(MOV_DIR + filename, UriKind.Absolute);
        // To save significant application memory, set the DecodePixelWidth or   
        // DecodePixelHeight of the BitmapImage value of the image source to the desired  
        // height or width of the rendered image. If you don't do this, the application will  
        // cache the image as though it were rendered as its normal size rather then just  
        // the size that is displayed. 
        // Note: In order to preserve aspect ratio, set DecodePixelWidth 
        // or DecodePixelHeight but not both.
        bmi.DecodePixelWidth = 200;
        bmi.EndInit();

        thumbnail.Source = bmi;
        return thumbnail;
    }

    private BitmapImage createImageIcon(String filename, double width)
    {
        BitmapImage bmi = new BitmapImage();

        // BitmapImage.UriSource must be in a BeginInit/EndInit block
        bmi.BeginInit();
        bmi.UriSource = new Uri(BASE_DIR + filename, UriKind.Absolute);

        // To save significant application memory, set the DecodePixelWidth or   
        // DecodePixelHeight of the BitmapImage value of the image source to the desired  
        // height or width of the rendered image. If you don't do this, the application will  
        // cache the image as though it were rendered as its normal size rather then just  
        // the size that is displayed. 
        // Note: In order to preserve aspect ratio, set DecodePixelWidth 
        // or DecodePixelHeight but not both.
        bmi.DecodePixelWidth = 200;
        bmi.EndInit();

        return bmi;
    }
}
