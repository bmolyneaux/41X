using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Medias;
using VrPlayer.Helpers;

namespace VrPlayer.Medias.VlcDotNetDual
{
    [Export(typeof(IPlugin<IMedia>))]
    public class VlcDotNetDualPlugin : PluginBase<IMedia>
    {
        public VlcDotNetDualPlugin()
        {
            try
            {
                Name = "VLC Dual";
                var media = new VlcDotNetDualMedia();
                Content = media;
                Panel = new VlcDotNetDualPanel(media);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}