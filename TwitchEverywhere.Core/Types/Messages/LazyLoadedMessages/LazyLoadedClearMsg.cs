using System.Text;
using System.Text.RegularExpressions;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages; 

public class LazyLoadedClearMsg(RawMessage response) : IClearMsg {
    private string m_tags;
    
    public MessageType MessageType => MessageType.ClearMsg;
    
    public string RawMessage => Encoding.UTF8.GetString( response.Data.Span );

    public string Channel => "";

    public string Login {
        get {
            InitializeTags();
            return MessagePluginUtils.GetLastSplitValuesFromResponse(m_tags, MessagePluginUtils.LoginPattern());
        }
    }

    public string RoomId {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.RoomIdPattern());
        }
    }
    
    public string TargetMessageId {
        get {
            InitializeTags();
            return MessagePluginUtils.GetLastSplitValuesFromResponse(m_tags, MessagePluginUtils.TargetMessageIdPattern());
        }
    }
    
    public DateTime Timestamp {
        get {
            InitializeTags();
            long rawTimestamp = Convert.ToInt64(
                MessagePluginUtils.MessageTimestampPattern().Match(m_tags).Value
                    .Split("=")[1]
                    .TrimEnd(';')
            );
            return DateTimeOffset.FromUnixTimeMilliseconds(rawTimestamp).UtcDateTime;
        }
    }

    string IClearMsg.Text {
        get {
            InitializeTags();
            return MessagePluginUtils.GetLastSplitValuesFromResponse(m_tags, new Regex($"CLEARMSG #{Channel} :")).Trim('\n');
        }
    }

    private void InitializeTags() {

        if( !string.IsNullOrEmpty( m_tags ) ) {
            return;
        }

        m_tags = MessagePluginUtils.GetTagsFromMessage( response );
    }
}