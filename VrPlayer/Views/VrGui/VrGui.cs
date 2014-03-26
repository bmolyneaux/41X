using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using VrPlayer.Contracts.Projections;
using System.Windows.Controls;

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
        
        private bool _visible = true;

        private VrForm _form;

        Canvas m;
        Canvas c;

        private const int CURSOR_WIDTH = 10;
        private const int CURSOR_HEIGHT = 10;

        public VrGui()
        {
            _material = new VisualBrush();

            _form = new VrForm();

            c = new Canvas();
            c.Children.Add(_form);

            // Mouse Canvas
            m = new Canvas();
            m.Width = 10;
            m.Height = 10;
            m.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
            Canvas.SetLeft(m, 50);
            Canvas.SetTop(m, 50);
            m.ClipToBounds = true;

            c.Children.Add(m);

            _material.Visual = c;
        }

        public void MouseMove(Point mouseXY, double width, double height)
        {
            Point coords = adjustMouseCoords(mouseXY, width, height);
            Canvas.SetLeft(m, coords.X);
            Canvas.SetTop(m, coords.Y);
        }

        private Point adjustMouseCoords(Point mouseXY, double width, double height)
        {
            double x = mouseXY.X * VrForm.CANVAS_WIDTH / width;
            double y = mouseXY.Y * VrForm.CANVAS_HEIGHT / height;

            x = Math.Min(VrForm.CANVAS_WIDTH - CURSOR_WIDTH, x);
            y = Math.Min(VrForm.CANVAS_HEIGHT - CURSOR_HEIGHT, y);

            return new Point(x, y);
        }
 
        public bool MouseUp()
        {
            _visible = !_visible;

            if (_visible) _material.Visual = c;
            else _material.Visual = null;

            return _visible;
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
