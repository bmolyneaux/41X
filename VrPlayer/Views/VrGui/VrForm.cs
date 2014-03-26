using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace VrPlayer.Views.VrGui
{
    class VrForm : UserControl
    {
        public static double CANVAS_WIDTH = 640;          // GUI menu base panel width
        public static double CANVAS_HEIGHT = 400;          // GUI menu base panel height
        private static double CELL_PADDING = 8;            // Pseudo-cell padding for the content grid\

        private static string BASE_DIR = "pack://application:,,,/Medias/VrGui/";

        public VrForm() : base() {
            updateMaterial();
        }

        public void updateMaterial()
        {
            // Main canvas
            Canvas c = new Canvas();
            c.VerticalAlignment = VerticalAlignment.Bottom;
            c.Width = CANVAS_WIDTH;
            c.Height = CANVAS_HEIGHT;
            c.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));
            c.ClipToBounds = true;

            // Main Stack Panel. Vertical stack
            StackPanel mainPanel = new StackPanel();
            mainPanel.Width = CANVAS_WIDTH;
            mainPanel.Height = CANVAS_HEIGHT;
            mainPanel.VerticalAlignment = VerticalAlignment.Bottom;

            //Title block for the menu
            TextBlock title = new TextBlock();
            FontSizeConverter fsc = new FontSizeConverter();
            title.FontSize = (double)fsc.ConvertFrom("22pt");
            title.TextAlignment = TextAlignment.Center;
            title.Text = "MAIN MENU";
            title.Margin = new Thickness(2);
            //title.Foreground = Brushes.Black;
            //title.Background = new SolidColorBrush(Color.FromRgb(0xEC, 0xEC, 0xEC));
            title.Foreground = new SolidColorBrush(Color.FromRgb(0xEC, 0xEC, 0xEC));

            mainPanel.Children.Add(title);

            // Grid Panel for the content selection and content navigation
            Grid content = createContentGrid(CANVAS_WIDTH, CANVAS_HEIGHT / 2);
            mainPanel.Children.Add(content);

            Grid progress = createProgressGrid(CANVAS_WIDTH / 1.5, CANVAS_HEIGHT / 4);
            mainPanel.Children.Add(progress);

            c.Children.Add(mainPanel);

            this.Content = c;
        }

        /// <summary>
        /// Creates a 2x5 grid layout</summary>
        /// <param name="width">Preferred Grid Panel width</param>
        /// <param name="height">Preferred Grid Panel height</param>
        private Grid createGrid(double width, double height)
        {
            Grid grid = new Grid();
            grid.Width = width;
            grid.Height = height;
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;
            grid.ShowGridLines = false;

            // Define the columns and rows for the content selection.
            // This GUI is using 2 rows and 5 columns ( 3 for content and 2 for navigation)
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            ColumnDefinition colDef4 = new ColumnDefinition();
            ColumnDefinition colDef5 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(colDef1);
            grid.ColumnDefinitions.Add(colDef2);
            grid.ColumnDefinitions.Add(colDef3);
            grid.ColumnDefinitions.Add(colDef4);
            grid.ColumnDefinitions.Add(colDef5);

            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            grid.RowDefinitions.Add(rowDef1);
            grid.RowDefinitions.Add(rowDef2);

            return grid;

        }

        /// <summary>
        /// Creates a 2x5 grid layout to display the progress bar</summary>
        /// <param name="width">Preferred Grid Panel width</param>
        /// <param name="height">Preferred Grid Panel height</param>
        private Grid createProgressGrid(double width, double height)
        {
            Grid progress = createGrid(width, height);
            progress.VerticalAlignment = VerticalAlignment.Bottom;

            Rectangle bar = new Rectangle();
            bar.Fill = new SolidColorBrush(Color.FromRgb(0xEC, 0xEC, 0xEC));
            bar.Height = (height / 4) - (CELL_PADDING * 2);
            Grid.SetColumnSpan(bar, 3);
            Grid.SetColumn(bar, 1);
            Grid.SetRow(bar, 0);
            progress.Children.Add(bar);

            //Title block for elapsed time on the video
            TextBlock elapsed = new TextBlock();
            FontSizeConverter fsc = new FontSizeConverter();
            elapsed.FontSize = (double)fsc.ConvertFrom("20pt");
            elapsed.TextAlignment = TextAlignment.Center;
            elapsed.Text = "-:--";
            elapsed.Margin = new Thickness(2);
            elapsed.Foreground = new SolidColorBrush(Color.FromRgb(0xEC, 0xEC, 0xEC));
            Grid.SetColumn(elapsed, 0);
            Grid.SetRow(elapsed, 0);
            progress.Children.Add(elapsed);

            //Title block for the full length of the video
            TextBlock length = new TextBlock();
            length.FontSize = (double)fsc.ConvertFrom("20pt");
            length.TextAlignment = TextAlignment.Center;
            length.Text = "-:--";
            length.Margin = new Thickness(2);
            length.Foreground = new SolidColorBrush(Color.FromRgb(0xEC, 0xEC, 0xEC));
            Grid.SetColumn(length, 4);
            Grid.SetRow(length, 0);
            progress.Children.Add(length);

            return progress;
        }

        /// <summary>
        /// Creates a 2x5 grid layout to display the content (videos/pictures) available for viewing</summary>
        /// <param name="width">Preferred Grid Panel width</param>
        /// <param name="height">Preferred Grid Panel height</param>
        private Grid createContentGrid(double width, double height)
        {
            Grid content = createGrid(width, height);

            // Get all the icons for the content
            // TODO: Differentiate between a video and a photo for the proper icon. Just using video for now
            Image icon1 = createImageIcon("video_icon.png", CANVAS_WIDTH / 5 - CELL_PADDING);
            icon1.Width = (CANVAS_WIDTH / 5) - CELL_PADDING;
            Grid.SetColumn(icon1, 1);
            Grid.SetRow(icon1, 0);
            content.Children.Add(icon1);
            // TODO: This will require some sort of "isLive" logic
            //if (isLive(null)) addLiveBadge(content, 0, 3);

            Image icon2 = createImageIcon("video_icon.png", CANVAS_WIDTH / 5 - CELL_PADDING);
            Grid.SetColumn(icon2, 2);
            Grid.SetRow(icon2, 0);
            content.Children.Add(icon2);
            // TODO: This will require some sort of "isLive" logic
            //if (isLive(null)) addLiveBadge(content, 0, 3);

            Image icon3 = createImageIcon("video_icon.png", CANVAS_WIDTH / 5 - CELL_PADDING);
            Grid.SetColumn(icon3, 3);
            Grid.SetRow(icon3, 0);
            content.Children.Add(icon3);
            // TODO: This will require some sort of "isLive" logic
            if (isLive(null)) addLiveBadge(content, 0, 3);

            Image icon4 = createImageIcon("video_icon.png", CANVAS_WIDTH / 5 - CELL_PADDING);
            Grid.SetColumn(icon4, 1);
            Grid.SetRow(icon4, 1);
            content.Children.Add(icon4);
            // TODO: This will require some sort of "isLive" logic
            //if (isLive(null)) addLiveBadge(content, 0, 3);

            Image icon5 = createImageIcon("video_icon.png", CANVAS_WIDTH / 5 - CELL_PADDING);
            Grid.SetColumn(icon5, 2);
            Grid.SetRow(icon5, 1);
            content.Children.Add(icon5);
            // TODO: This will require some sort of "isLive" logic
            addLiveBadge(content, 1, 2);

            Image icon6 = createImageIcon("video_icon.png", CANVAS_WIDTH / 5 - CELL_PADDING);
            Grid.SetColumn(icon6, 3);
            Grid.SetRow(icon6, 1);
            content.Children.Add(icon6);
            // TODO: This will require some sort of "isLive" logic
            //if (isLive(null)) addLiveBadge(content, 0, 3);

            // These will be the navigation buttons for the content grid
            Image prevIcon = createImageIcon("prev_icon.png", CANVAS_WIDTH / 10 - CELL_PADDING);
            Grid.SetColumn(prevIcon, 0);
            Grid.SetRowSpan(prevIcon, 2);
            content.Children.Add(prevIcon);

            Image nextIcon = createImageIcon("next_icon.png", CANVAS_WIDTH / 10 - CELL_PADDING);
            Grid.SetColumn(nextIcon, 5);
            Grid.SetRowSpan(nextIcon, 2);
            content.Children.Add(nextIcon);

            return content;
        }

        private void addLiveBadge(Grid content, int row, int col)
        {
            Image liveBadge = createImageIcon("live_icon.png", 25);
            liveBadge.HorizontalAlignment = HorizontalAlignment.Left;
            liveBadge.VerticalAlignment = VerticalAlignment.Top;
            liveBadge.Margin = new Thickness(10, 30, 0, 0);

            Grid.SetRow(liveBadge, row);
            Grid.SetColumn(liveBadge, col);
            content.Children.Add(liveBadge);
        }

        /// <summary>
        /// Creates an image of specified width from the URI provided</summary>
        /// <param name="width">Preferred image width</param>
        /// <param name="uri">File location for the image resource</param>
        private Image createImageIcon(String filename, double width)
        {
            Image img = new Image();
            img.Width = width;

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

            //set image source
            img.Source = bmi;

            return img;
        }

        private bool isLive(MediaElement media)
        {
            // TODO: implement logic to determine if media is a stream or a file
            /*Stub*/
            return true;
        }

        // TODO: Implement some sort of handler for mouse clicks
        void MouseUpHandler(Object sender, RoutedEventArgs args)
        {
            if (sender.GetType() == typeof(Canvas))
            {
                Canvas rec = (Canvas)sender;
                SolidColorBrush transparent = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
            }
        }
    }
}
