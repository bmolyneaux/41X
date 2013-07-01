﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Projections;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;
using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.State;
using VrPlayer.Models.Config;
using VrPlayer.Views.Dialogs;
using VrPlayer.Views.Settings;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace VrPlayer.ViewModels
{
	public class MenuViewModel: ViewModelBase
	{
        private readonly IApplicationConfig _config;
        public IApplicationConfig Config
        {
            get { return _config; }
        }

        private readonly IApplicationState _state;
        public IApplicationState State
        {
            get { return _state; }
        }

        private readonly IPluginManager _pluginManager;
        public IPluginManager PluginManager
        {
            get { return _pluginManager; }
        }

        //Todo: Create a manager to detect screens activites
	    public bool SupportDualScreen
	    {
            get { return Screen.AllScreens.Count() >= 2; }
	    }

	    #region Commands

        private readonly ICommand _openFileCommand;
        public ICommand OpenFileCommand
        {
            get { return _openFileCommand; }
        }

        private readonly ICommand _openStreamCommand;
        public ICommand OpenStreamCommand
        {
            get { return _openStreamCommand; }
        }

        private readonly ICommand _openDiscCommand;
        public ICommand OpenDiscCommand
        {
            get { return _openDiscCommand; }
        }

        private readonly ICommand _openDeviceCommand;
        public ICommand OpenDeviceCommand
        {
            get { return _openDeviceCommand; }
        }

        private readonly ICommand _openProcessCommand;
        public ICommand OpenProcessCommand
        {
            get { return _openProcessCommand; }
        }        
        
        private readonly ICommand _browseSamplesCommand;
        public ICommand BrowseSamplesCommand
        {
            get { return _browseSamplesCommand; }
        }

        private readonly ICommand _exitCommand;
        public ICommand ExitCommand
        {
            get { return _exitCommand; }
        }

        private readonly ICommand _settingsCommand;
        public ICommand SettingsCommand
        {
            get { return _settingsCommand; }
        }

        private readonly ICommand _pluginsCommand;
        public ICommand PluginsCommand
        {
            get { return _pluginsCommand; }
        }

        private readonly ICommand _launchWebBrowserCommand;
        public ICommand LaunchWebBrowserCommand
        {
            get { return _launchWebBrowserCommand; }
        }

        private readonly ICommand _aboutCommand;
        public ICommand AboutCommand
        {
            get { return _aboutCommand; }
        }

        private readonly ICommand _changeProjectionCommand;
        public ICommand ChangeProjectionCommand
        {
            get { return _changeProjectionCommand; }
        }

        private readonly ICommand _changeEffectCommand;
        public ICommand ChangeEffectCommand
        {
            get { return _changeEffectCommand; }
        }

        private readonly ICommand _changeDistortionCommand;
        public ICommand ChangeDistortionCommand
        {
            get { return _changeDistortionCommand; }
        }

        private readonly ICommand _changeTrackerCommand;
        public ICommand ChangeTrackerCommand
        {
            get { return _changeTrackerCommand; }
        }

        private readonly ICommand _changeFormatCommand;
        public ICommand ChangeFormatCommand
        {
            get { return _changeFormatCommand; }
        }

        private readonly ICommand _changeLayoutCommand;
        public ICommand ChangeLayoutCommand
        {
            get { return _changeLayoutCommand; }
        }

        #endregion

        public MenuViewModel(IApplicationState state, IPluginManager pluginManager, IApplicationConfig config)
        {
            _pluginManager = pluginManager;
            _state = state;
            _config = config;

            //Commands
            _openFileCommand = new DelegateCommand(OpenFile, CanOpenFile);
            _openStreamCommand = new DelegateCommand(OpenStream, CanOpenStream);
            _openDiscCommand = new DelegateCommand(OpenDisc, CanOpenDisc);
            _openDeviceCommand = new DelegateCommand(OpenDevice, CanOpenDevice);
            _openProcessCommand = new DelegateCommand(OpenProcess, CanOpenProcess);
            _browseSamplesCommand = new DelegateCommand(BrowseSamples);
            _exitCommand = new DelegateCommand(Exit);
            _changeFormatCommand = new DelegateCommand(SetStereoInput);
            _changeProjectionCommand = new DelegateCommand(SetProjection);
            _changeEffectCommand = new DelegateCommand(SetEffect);
            _changeLayoutCommand = new DelegateCommand(SetStereoOutput);
            _changeDistortionCommand = new DelegateCommand(SetDistortion);
            _changeTrackerCommand = new DelegateCommand(SetTracker);
            _settingsCommand = new DelegateCommand(ShowSettings);
            _pluginsCommand = new DelegateCommand(ShowPlugins);
            _launchWebBrowserCommand = new DelegateCommand(LaunchWebBrowser);
            _aboutCommand = new DelegateCommand(ShowAbout);
        }

        #region Logic

        private void OpenFile(object o)
        {
            if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return;
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = FileFilterHelper.GetFilter();
            if (openFileDialog.ShowDialog().Value)
            {
                _state.MediaPlugin.Content.OpenFileCommand.Execute(openFileDialog.FileName);
            }
        }

	    private bool CanOpenFile(object o)
	    {
	        if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return false;
	        return _state.MediaPlugin.Content.OpenFileCommand.CanExecute(o);
	    }

	    private void OpenStream(object o)
        {
            if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return;
            var dialog = new StreamInputDialog();
            if (dialog.ShowDialog() == true)
            {
                _state.MediaPlugin.Content.OpenStreamCommand.Execute(dialog.Url);
            }
        }

        private bool CanOpenStream(object o)
        {
            if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return false;
            return _state.MediaPlugin.Content.OpenStreamCommand.CanExecute(o);
        }

        private void OpenDevice(object o)
        {
            if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return;
        }

        private bool CanOpenDevice(object o)
        {
            if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return false;
            return _state.MediaPlugin.Content.OpenDeviceCommand.CanExecute(o);
        }

        private void OpenDisc(object o)
        {
            if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return;
            var dialog = new DiscInputDialog();
            if (dialog.ShowDialog() == true)
            {
                _state.MediaPlugin.Content.OpenDiscCommand.Execute(dialog.Drive);
            }
        }

        private bool CanOpenDisc(object o)
        {
            if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return false;
            return _state.MediaPlugin.Content.OpenDiscCommand.CanExecute(o);
        }

        private void OpenProcess(object o)
        {
            if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return;
            var dialog = new ProcessInputDialog();
            if (dialog.ShowDialog() == true)
            {
                _state.MediaPlugin.Content.OpenProcessCommand.Execute(dialog.Process);
            }
        }

        private bool CanOpenProcess(object o)
        {
            if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return false;
            return _state.MediaPlugin.Content.OpenProcessCommand.CanExecute(o);
        }

		private void Load(object o)
		{
            /*
			var filePath = (string)o;
            
            if (_config.MetaDataReadOnLoad)
            {
                try
                {
                    //Todo: Extract metadata parsing
                    var parser = new MetadataParser(filePath);
                    var metadata = parser.Parse();

                    if (!string.IsNullOrEmpty(metadata.ProjectionType))
                    {
                        _state.ProjectionPlugin = _pluginManager.Projections.FirstOrDefault(
                            plugin => plugin.Content.GetType().Namespace == metadata.ProjectionType);
                    }

                    if (!string.IsNullOrEmpty(metadata.FormatType))
                    {
                        _state.StereoInput = (StereoMode)Enum.Parse(typeof(StereoMode), metadata.FormatType);
                        //Todo: Stereo input should not be assigned to Projection manually
                        if (_state.ProjectionPlugin != null)
                            _state.ProjectionPlugin.Content.StereoMode = _state.StereoInput;
                    }

                    if (!string.IsNullOrEmpty(metadata.Effects))
                    {
                        _state.EffectPlugin = _pluginManager.Effects
                            .Where(plugin => plugin.Content != null)
                            .FirstOrDefault(plugin => plugin.Content.GetType().Namespace == metadata.Effects);
                    }
                }
                catch (Exception exc)
                {
                    var message = String.Format("Unable to parse meta data from file '{0}'.", filePath);
                    Logger.Instance.Warn(message, exc);
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }    
            }
            
            try
            {
                _state.MediaPlayer.Source = new Uri(filePath, UriKind.Absolute);
            }
            catch (Exception exc)
            {
                var message = String.Format("Unable to load '{0}'.", filePath);
                Logger.Instance.Warn(message, exc);
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
             */
        }

        private void BrowseSamples(object o)
		{   
            var dirInfo = new DirectoryInfo(_config.SamplesFolder);
            if (Directory.Exists(dirInfo.FullName))
            {
                Process.Start(dirInfo.FullName);
            }
            else
            {
                MessageBox.Show(string.Format("Invalid samples directory: '{0}'.", _config.SamplesFolder), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LaunchWebBrowser(object o)
		{
            Process.Start("http://www.vrplayer.tv");
        }

        private void Exit(object o)
        {
            Application.Current.Shutdown();
        }

        private void SetEffect(object o)
        {
            _state.EffectPlugin = (IPlugin<EffectBase>)o;
        }

        private void SetStereoInput(object o)
        {
            _state.StereoInput = (StereoMode)o;
            _state.ProjectionPlugin.Content.StereoMode = _state.StereoInput;
        }

        private void SetStereoOutput(object o)
        {
            _state.StereoOutput = (LayoutMode)o;
        }

        private void SetProjection(object o)
        {
            var projectionPlugin = (IPlugin<IProjection>)o;
            projectionPlugin.Content.StereoMode = _state.StereoInput;
            _state.ProjectionPlugin = projectionPlugin;
        }

        private void SetTracker(object o)
        {
            _state.TrackerPlugin = (IPlugin<ITracker>)o;
        }

        private void SetDistortion(object o)
        {
            _state.DistortionPlugin = (IPlugin<DistortionBase>)o;
        }

        private void ShowSettings(object o)
        {
            SettingsWindow.ShowSingle();
        }

        private void ShowPlugins(object o)
        {
            PluginsWindow.ShowSingle();
        }

        private void ShowAbout(object o)
        {
            //Todo: Extract to UI layer
            MessageBox.Show(
                string.Format("VRPlayer ({0})", Assembly.GetExecutingAssembly().GetName().Version) +
                Environment.NewLine +
                Environment.NewLine +
                "(c)Stephane Levesque 2012-2013",    
                "About", 
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        #endregion
    }
}
