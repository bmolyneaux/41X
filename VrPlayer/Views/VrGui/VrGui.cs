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
        double cameraSeparation = 0.01;

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
            c.Width = 640;
            c.Height = 400;
            c.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));
            c.ClipToBounds = true;

            // Crap
            StackPanel myStackPanel = new StackPanel();

            Rectangle redRectangle = new Rectangle();
            redRectangle.Width = 640;
            redRectangle.Height = 25;
            redRectangle.Fill = Brushes.Green;
            redRectangle.Margin = new Thickness(2);
            myStackPanel.Children.Add(redRectangle);

            TextBlock someText = new TextBlock();
            FontSizeConverter myFontSizeConverter = new FontSizeConverter();
            someText.FontSize = (double)myFontSizeConverter.ConvertFrom("12pt");
            someText.Text = "Hello, World!";
            someText.Margin = new Thickness(2);
            someText.Foreground = Brushes.White;
            myStackPanel.Children.Add(someText);

            Button aButton = new Button();
            aButton.Content = "A Button";
            aButton.Margin = new Thickness(2);
            myStackPanel.Children.Add(aButton);

            c.Children.Add(myStackPanel);

            // Mouse
            Canvas m = new Canvas();
            m.Width = 10;
            m.Height = 10;
            m.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
            Canvas.SetTop(m, _mouseXY.Y);
            Canvas.SetLeft(m, _mouseXY.X);
            m.ClipToBounds = true;

            c.Children.Add(m);

            _material.Visual = c;
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
