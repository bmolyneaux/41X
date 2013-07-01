﻿using System;
using System.Linq;
using System.Windows;

namespace VrPlayer.Views.Settings
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            try
            {
                DataContext = ((App)Application.Current).ViewModelFactory.CreateSettingsWindowViewModel();
            }
            catch (Exception exc)
            {
            }
        }

        public static void ShowSingle()
        {
            var window = Application.Current.Windows.Cast<Window>().SingleOrDefault(w => w.GetType() == typeof(SettingsWindow));
            if (window != null)
            {
                window.Activate();
            }
            else
            {
                window = new SettingsWindow();
                window.Show();
            }
        }
    }
}
