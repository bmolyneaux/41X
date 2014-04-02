using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using VrPlayer.Contracts.Projections;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VrPlayer.Views.VrGui
{
    public class VrGui : DependencyObject
    {
        private static string BASE_DIR = "pack://application:,,,/Medias/VrGui/";

        private double Width = 2;
        private double Height = 1.5;
        private double Depth = 1;

        Point3D Center = new Point3D(0, 0, 0);
        VisualBrush _material;
        
        private static double cameraSeparation  = 0.01;         // Left and right camera separation
        
        private bool _visible = true;

        private VrForm _form;

        static Canvas mouse;
        Panel panel;

        private const int CURSOR_WIDTH = 10;
        private const int CURSOR_HEIGHT = 10;

        public VrGui()
        {
            _material = new VisualBrush();

            _form = new VrForm();
        }

        public void setVisual(Panel p)
        {

            panel = p;
            
            // Mouse Canvas
            mouse = new Canvas();
            mouse.Width = 48;
            mouse.Height = 48;
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(BASE_DIR+"hand-icon.png"));
            //mouse.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
            mouse.Background = ib;
            Canvas.SetLeft(mouse, 50);
            Canvas.SetTop(mouse, 50);
            mouse.ClipToBounds = true;

            panel.Children.Add(_form);
            panel.Children.Add(mouse);

            _material.Visual = panel;
        }

        public void setUiMasks(Rectangle mask1, Rectangle mask2)
        {
            _form.UiMask1 = mask1;
            _form.UiMask2 = mask2;
        }

        public static void setMouseVisibility(Visibility visibility)
        {
            mouse.Visibility = visibility;
        }

        public void MouseMove(Point actualXY, double viewportWidth, double viewportHeight)
        {
            Point adjustedXY = adjustMouseCoords(actualXY, viewportWidth, viewportHeight);
            Canvas.SetLeft(mouse, adjustedXY.X);
            Canvas.SetTop(mouse, adjustedXY.Y);
        }

        private Point adjustMouseCoords(Point actualXY, double viewportWidth, double viewportHeight)
        {
            double x = actualXY.X * VrForm.CANVAS_WIDTH / viewportWidth;
            double y = actualXY.Y * VrForm.CANVAS_HEIGHT / viewportHeight;

            x = Math.Min(VrForm.CANVAS_WIDTH - CURSOR_WIDTH, x);
            y = Math.Min(VrForm.CANVAS_HEIGHT - CURSOR_HEIGHT, y);

            return new Point(x, y);
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
                triangleIndices.Add(2);
                triangleIndices.Add(1);
                triangleIndices.Add(0);
                triangleIndices.Add(0);
                triangleIndices.Add(3);
                triangleIndices.Add(2);

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
