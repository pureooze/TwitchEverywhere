using System.Text;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

public class LazyLoadedHostTargetMsg( RawMessage response ) : IHostTargetMsg {

    private string m_tags;

    MessageType IMessage.MessageType => MessageType.HostTarget;

    string IMessage.RawMessage => Encoding.UTF8.GetString( response.Data.Span );

    string IHostTargetMsg.HostingChannel => MessagePluginUtils.HostTargetPattern().Match(((IMessage)this).RawMessage).Groups[1].Value;

    string IMessage.Channel => MessagePluginUtils.GetChannelFromMessage( response );

    int IHostTargetMsg.NumberOfViewers {
        get {
            InitializeTags();
            return int.Parse(MessagePluginUtils.HostViewerCountPattern().Match(((IMessage)this).RawMessage).Value);
        }
    }

    bool IHostTargetMsg.IsHostingChannel {
        get {
            InitializeTags();
            return !string.Equals(((IHostTargetMsg) this).HostingChannel, "-");
        }
    }
    
    private void InitializeTags() {

        if( !string.IsNullOrEmpty( m_tags ) ) {
            return;
        }

        m_tags = MessagePluginUtils.GetTagsFromMessage( response );
    }
}