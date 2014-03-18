using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using VrPlayer.Contracts.Projections;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace VrPlayer.Views.VrGui
{
    public class VrGui : DependencyObject
    {
        private double Width = 2;
        private double Height = 1.5;
        private double Depth = 1;

        Point3D Center = new Point3D(0, 0, 0);
        VisualBrush _material;
        
        private static double cameraSeparation  = 0.01;         // Left and right camera separation
        private static double CANVAS_WIDTH      = 640;          // GUI menu base panel width
        private static double CANVAS_HEIGHT     = 400;          // GUI menu base panel height
        private static double CELL_PADDING      = 8;            // Pseudo-cell padding for the content grid\

#if DEBUG // Base address for the resource files is different in debug mode then it is for release 
        private static string BASE_DIR = @"C:\Users\41X\Documents\41X\VrPlayer";
#else
        private static string BASE_DIR = System.AppDomain.CurrentDomain.BaseDirectory;
#endif


        private Point _mouseXY = new Point(0, 0);
        public Point MouseXY
        {
            get
            {
                return _mouseXY;
            }
            set
            {
                _mouseXY = value;

                updateMaterial();
            }
        }

        private void updateMaterial()
        {
            // Main canvas
            Canvas c = new Canvas();
            c.Width = CANVAS_WIDTH;
            c.Height = CANVAS_HEIGHT;
            c.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));
            c.ClipToBounds = true;

            // Main Stack Panel. Vertical stack
            StackPanel mainPanel = new StackPanel();
            mainPanel.Width = CANVAS_WIDTH;
            mainPanel.Height = CANVAS_HEIGHT;

            //Title block for the menu
            TextBlock title = new TextBlock();
            FontSizeConverter fsc = new FontSizeConverter();
            title.FontSize = (double)fsc.ConvertFrom("22pt");
            title.TextAlignment = TextAlignment.Center;
            title.Text = "MAIN MENU";
            title.Margin = new Thickness(2);
            title.Foreground = Brushes.Black;
            title.Background = new SolidColorBrush(Color.FromRgb(0xEC, 0xEC, 0xEC));

            mainPanel.Children.Add(title);

            // Grid Panel for the content selection and content navigation
            Grid content = createContentGrid(CANVAS_WIDTH, CANVAS_HEIGHT/2);
            mainPanel.Children.Add(content);

            c.Children.Add(mainPanel);

            // Mouse Canvas
            Canvas m = new Canvas();
            m.Width = 10;
            m.Height = 10;
            m.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
            Canvas.SetTop(m, _mouseXY.Y);
            Canvas.SetLeft(m, _mouseXY.X);
            m.ClipToBounds = true;
            m.MouseUp += MouseUpHandler;

            c.Children.Add(m);

            _material.Visual = c;
        }

        /// <summary>
        /// Creates a 2x5 grid layout to display the content (videos/pictures) available for viewing</summary>
        /// <param name="width">Preferred Grid Panel width</param>
        /// <param name="height">Preferred Grid Panel height</param>
        private Grid createContentGrid(double width, double height)
        {
            Grid content = new Grid();
            content.Width = width;
            content.Height = height;
            content.HorizontalAlignment = HorizontalAlignment.Center;
            content.VerticalAlignment = VerticalAlignment.Center;
            content.ShowGridLines = false;

            // Define the columns and rows for the content selection.
            // This GUI is using 2 rows and 5 columns ( 3 for content and 2 for navigation)
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            ColumnDefinition colDef4 = new ColumnDefinition();
            ColumnDefinition colDef5 = new ColumnDefinition();
            content.ColumnDefinitions.Add(colDef1);
            content.ColumnDefinitions.Add(colDef2);
            content.ColumnDefinitions.Add(colDef3);
            content.ColumnDefinitions.Add(colDef4);
            content.ColumnDefinitions.Add(colDef5);

            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            content.RowDefinitions.Add(rowDef1);
            content.RowDefinitions.Add(rowDef2);

            // Get all the icons for the content
            // TODO: Differentiate between a video and a photo for the proper icon. Just using video for now
            Image icon1 = createImageIcon(CANVAS_WIDTH / 5 - CELL_PADDING, BASE_DIR + @"\Medias\VrGui\video_icon.png");
            Grid.SetColumn(icon1, 1);
            Grid.SetRow(icon1, 0);
            content.Children.Add(icon1);

            Image icon2 = createImageIcon(CANVAS_WIDTH / 5 - CELL_PADDING, BASE_DIR + @"\Medias\VrGui\video_icon.png");
            Grid.SetColumn(icon2, 2);
            Grid.SetRow(icon2, 0);
            content.Children.Add(icon2);
            
            Image icon3 = createImageIcon(CANVAS_WIDTH / 5 - CELL_PADDING, BASE_DIR + @"\Medias\VrGui\video_icon.png");
            Grid.SetColumn(icon3, 3);
            Grid.SetRow(icon3, 0);
            content.Children.Add(icon3);

            Image icon4 = createImageIcon(CANVAS_WIDTH / 5 - CELL_PADDING, BASE_DIR + @"\Medias\VrGui\video_icon.png");
            Grid.SetColumn(icon4, 1);
            Grid.SetRow(icon4, 1);
            content.Children.Add(icon4);

            Image icon5 = createImageIcon(CANVAS_WIDTH / 5 - CELL_PADDING, BASE_DIR + @"\Medias\VrGui\video_icon.png");
            Grid.SetColumn(icon5, 2);
            Grid.SetRow(icon5, 1);
            content.Children.Add(icon5);

            Image icon6 = createImageIcon(CANVAS_WIDTH / 5 - CELL_PADDING, BASE_DIR + @"\Medias\VrGui\video_icon.png");
            Grid.SetColumn(icon6, 3);
            Grid.SetRow(icon6, 1);
            content.Children.Add(icon6);

            // These will be the navigation buttons for the content grid
            Image prevIcon = createImageIcon(CANVAS_WIDTH / 10 - CELL_PADDING, BASE_DIR + @"\Medias\VrGui\prev_icon.png");
            Grid.SetColumn(prevIcon, 0);
            Grid.SetRowSpan(prevIcon, 2);
            content.Children.Add(prevIcon);

            Image nextIcon = createImageIcon(CANVAS_WIDTH / 10 - CELL_PADDING, BASE_DIR + @"\Medias\VrGui\next_icon.png");
            Grid.SetColumn(nextIcon, 5);
            Grid.SetRowSpan(nextIcon, 2);
            content.Children.Add(nextIcon);

            return content;
        }

        /// <summary>
        /// Creates an image of specified width from the URI provided</summary>
        /// <param name="width">Preferred image width</param>
        /// <param name="uri">File location for the image resource</param>
        private Image createImageIcon(double width, string uri)
        {
            Image img = new Image();
            img.Width = width;

            // Create source
            BitmapImage bmpImg = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            bmpImg.BeginInit();
            bmpImg.UriSource = new Uri(uri);

            // To save significant application memory, set the DecodePixelWidth or   
            // DecodePixelHeight of the BitmapImage value of the image source to the desired  
            // height or width of the rendered image. If you don't do this, the application will  
            // cache the image as though it were rendered as its normal size rather then just  
            // the size that is displayed. 
            // Note: In order to preserve aspect ratio, set DecodePixelWidth 
            // or DecodePixelHeight but not both.
            bmpImg.DecodePixelWidth = 200;
            bmpImg.EndInit();
            //set image source
            img.Source = bmpImg;

            return img;
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


        public VrGui()
        {
            _material = new VisualBrush();
            updateMaterial();

            /*
            // Create the visual brush's contents.
            StackPanel myStackPanel = new StackPanel();
            Color bg = new Color();
            bg.A = 0xbb;
            bg.R = bg.G = bg.B = 0x00;
            myStackPanel.Background = new SolidColorBrush(bg);

            Rectangle redRectangle = new Rectangle();
            redRectangle.Width = 50;
            redRectangle.Height = 25;
            redRectangle.Fill = Brushes.Red;
            redRectangle.Margin = new Thickness(2);
            myStackPanel.Children.Add(redRectangle);

            TextBlock someText = new TextBlock();
            FontSizeConverter myFontSizeConverter = new FontSizeConverter();
            someText.FontSize = (double)myFontSizeConverter.ConvertFrom("10pt");
            someText.Text = "Hello, World!";
            someText.Margin = new Thickness(2);
            someText.Foreground = Brushes.White;
            myStackPanel.Children.Add(someText);

            Button aButton = new Button();
            aButton.Content = "A Button";
            aButton.Margin = new Thickness(2);
            myStackPanel.Children.Add(aButton);

            // Use myStackPanel as myVisualBrush's content.
            _material.Visual = myStackPanel;
            */
        }

        public static readonly DependencyProperty SlicesProperty =
            DependencyProperty.Register("Slices", typeof(int),
            typeof(VrGui), new FrameworkPropertyMetadata(64));
        [DataMember]
        public int Slices
        {
            get { return (int)GetValue(SlicesProperty); }
            set { SetValue(SlicesProperty, value); }
        }

        public static readonly DependencyProperty StacksProperty =
             DependencyProperty.Register("Stacks", typeof(int),
             typeof(VrGui), new FrameworkPropertyMetadata(32));
        [DataMember]
        public int Stacks
        {
            get { return (int)GetValue(StacksProperty); }
            set { SetValue(StacksProperty, value); }
        }

        public MeshGeometry3D Geometry
        {
            get
            {
                var geometry = new MeshGeometry3D();
                geometry.Positions = Positions;
                geometry.TriangleIndices = TriangleIndices;
                geometry.TextureCoordinates = TextureCoordinates;
                return geometry;
            }
        }

        public Point3D CameraLeftPosition
        {
            get
            {
                return new Point3D(-cameraSeparation, 0, 0);
            }
        }

        public Point3D CameraRightPosition
        {
            get
            {
                return new Point3D(+cameraSeparation, 0, 0);
            }
        }

        public Point3DCollection Positions
        {
            get
            {
                var positions = new Point3DCollection();

                //Left
                positions.Add(new Point3D(-Width / 2, -Height / 2, Depth));
                positions.Add(new Point3D(+Width / 2, -Height / 2, Depth));
                positions.Add(new Point3D(+Width / 2, +Height / 2, Depth));
                positions.Add(new Point3D(-Width / 2, +Height / 2, Depth));

                return positions;
            }
        }

        public Int32Collection TriangleIndices
        {
            get
            {
                var triangleIndices = new Int32Collection();

                //Left
                triangleIndices.Add(0);
                triangleIndices.Add(1);
                triangleIndices.Add(2);
                triangleIndices.Add(2);
                triangleIndices.Add(3);
                triangleIndices.Add(0);

                return triangleIndices;
            }
        }

        public PointCollection TextureCoordinates
        {
            get
            {
                var textureCoordinates = new PointCollection();

                //Left
                textureCoordinates.Add(new Point(1, 1));
                textureCoordinates.Add(new Point(0, 1));
                textureCoordinates.Add(new Point(0, 0));
                textureCoordinates.Add(new Point(1, 0));

                return textureCoordinates;
            }
        }

        public VisualBrush Material
        {
            get
            {
                return _material;
            }
        }
    }
}
