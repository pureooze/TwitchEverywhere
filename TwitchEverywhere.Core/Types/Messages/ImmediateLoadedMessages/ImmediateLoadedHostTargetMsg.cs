using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages {
    public class ImmediateLoadedHostTargetMsg : IHostTargetMsg {
        
        public ImmediateLoadedHostTargetMsg(
            string channel,
            string hostingChannel,
            string targetChannel,
            int numberOfViewers
        ) {
            Channel = channel;
            HostingChannel = hostingChannel;
            Channel = targetChannel;
            NumberOfViewers = numberOfViewers;
        }
        
        public MessageType MessageType => MessageType.HostTarget;
        public string RawMessage => GetRawMessage();
        public string HostingChannel { get; }
        public string Channel { get; }
        public int NumberOfViewers { get; }

        public bool IsHostingChannel => !string.IsNullOrEmpty( Channel );
        
        private string GetRawMessage() {
            string message = string.Empty;

            // create if statements that add each property in this class to the message string
            if( !string.IsNullOrEmpty( HostingChannel ) ) {
                message += $":{HostingChannel} ";
            }
            
            if( !string.IsNullOrEmpty( Channel ) ) {
                message += $"Channel: {Channel}";
            }
            
            message += $"NumberOfViewers: {NumberOfViewers}";

            return message;
        }
    }
}