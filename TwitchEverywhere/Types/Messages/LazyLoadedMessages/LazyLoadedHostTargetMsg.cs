using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.LazyLoadedMessages {
    public class LazyLoadedHostTargetMsg : Message, IHostTargetMsg {
        private readonly string m_message;

        public LazyLoadedHostTargetMsg(
            string message,
            string channel
        ) {
            m_message = message;
            HostingChannel = channel;
        }

        public override MessageType MessageType => MessageType.HostTarget;

        public string HostingChannel { get; }

        public string Channel => MessagePluginUtils.HostTargetPattern.Match( m_message ).Groups[1].Value;

        public int NumberOfViewers => int.Parse( MessagePluginUtils.HostViewerCountPattern.Match( m_message ).Value );

        public bool IsHostingChannel => !String.Equals( Channel, "-" );
    }
}