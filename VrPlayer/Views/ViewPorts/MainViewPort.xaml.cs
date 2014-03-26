using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using VrPlayer.Helpers;
using VrPlayer.Models.State;
using VrPlayer.ViewModels;
using System.Windows.Input;
using Application = System.Windows.Application;

namespace VrPlayer.Views.ViewPorts
{
    public partial class MainViewPort
    {
        private readonly ViewPortViewModel _viewModel;
        private readonly ExternalViewPort _externalViewPort;

        public MainViewPort()
        {
            InitializeComponent();
            try
            {
                _viewModel = ((App) Application.Current).ViewModelFactory.CreateViewPortViewModel();
                DataContext = _viewModel;

                //Todo: Extract to view model
                _externalViewPort = new ExternalViewPort(Resources["Geometry"] as GeometryModel3D);
                _externalViewPort.Closing += ExternalViewPortOnClosing;
                _viewModel.State.PropertyChanged += StateOnPropertyChanged;
                _viewModel.State.StereoOutput = _viewModel.State.StereoOutput;

                Cursor = System.Windows.Input.Cursors.None;
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while initilizing MainViewPort view.", exc);
            }
        }

        private void Border_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _viewModel.ToggleNavigationCommand.Execute(null);

            byte alpha = (_viewModel.Gui.MouseUp()) ? (byte)0xBB : (byte)0x00;
            this.UiMask1.Fill = this.UiMask2.Fill = new SolidColorBrush(Color.FromArgb(alpha, 0x00, 0x00, 0x00));
        }

        private void Border_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _viewModel.Gui.MouseXY = e.GetPosition(this);
        }

        private void ExternalViewPortOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (!(sender is ExternalViewPort)) return;
            if (_viewModel.State.StereoOutput != LayoutMode.DualScreen) return;

            cancelEventArgs.Cancel = true;
            ((Window)sender).Hide();
            _viewModel.State.StereoOutput = LayoutMode.MonoLeft;
        }

        private void StateOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "StereoOutput") return;

            ViewPortLeft.Visibility = Visibility.Visible;
            if (_viewModel.State.StereoOutput == LayoutMode.MonoRight)
            {
                ViewPortLeft.Visibility = Visibility.Hidden;
            }

            ViewPortRight.Visibility = Visibility.Visible;
            if (_viewModel.State.StereoOutput == LayoutMode.MonoLeft ||
                _viewModel.State.StereoOutput == LayoutMode.DualScreen)
            {
                ViewPortRight.Visibility = Visibility.Hidden;
            }
            
            if (_viewModel.State.StereoOutput == LayoutMode.DualScreen)
            {
                var secondScreenIndex = (SystemInformation.MonitorCount >= 2) ? 1 : 0;
                PositionWindowToScreen(Application.Current.MainWindow, Screen.AllScreens[0]);
                PositionWindowToScreen(_externalViewPort, Screen.AllScreens[secondScreenIndex]);
            }
            else
            {
                if(_externalViewPort != null)
                    _externalViewPort.Hide();
            }
        }

        private void PositionWindowToScreen(Window window, Screen screen)
        {
            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.Left = screen.WorkingArea.Left;
            window.Top = screen.WorkingArea.Top;
            window.Width = screen.WorkingArea.Width;
            window.Height = screen.WorkingArea.Height;
            window.Show();
            window.WindowState = WindowState.Maximized;
            window.Focus();
        }
    }
}
