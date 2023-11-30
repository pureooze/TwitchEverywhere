using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages {
    public class ImmediateLoadedHostTargetMsg : Message, IHostTargetMsg {
        
        public ImmediateLoadedHostTargetMsg(
            string hostingChannel,
            string targetChannel,
            int numberOfViewers
        ) {
            HostingChannel = hostingChannel;
            Channel = targetChannel;
            NumberOfViewers = numberOfViewers;
        }
        
        public override MessageType MessageType => MessageType.HostTarget;
        public string HostingChannel { get; }
        public string Channel { get; }
        public int NumberOfViewers { get; }

        public bool IsHostingChannel => !string.IsNullOrEmpty( Channel );
    }
}