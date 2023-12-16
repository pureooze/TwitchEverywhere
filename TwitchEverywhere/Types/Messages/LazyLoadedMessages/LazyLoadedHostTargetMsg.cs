using TwitchEverywhere.Types.Messages.Interfaces;
using MessagePluginUtils = TwitchEverywhere.Implementation.MessagePluginUtils;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages {
    public class LazyLoadedHostTargetMsg(
        string message,
        string channel
    ) : IHostTargetMsg {

        public MessageType MessageType => MessageType.HostTarget;
        
        public string RawMessage => message;

        public string HostingChannel { get; } = channel;

        public string Channel => MessagePluginUtils.HostTargetPattern().Match( message ).Groups[1].Value;

        public int NumberOfViewers => int.Parse( MessagePluginUtils.HostViewerCountPattern().Match( message ).Value );

        public bool IsHostingChannel => !String.Equals( Channel, "-" );
    }
}