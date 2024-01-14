using System.Text;
using System.Text.RegularExpressions;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

public class LazyLoadedClearChatMsg : IClearChatMsg {
    private readonly string m_channel;
    private readonly string m_message;
    private readonly RawMessage m_response;
    private string m_tags;

    public LazyLoadedClearChatMsg(
        RawMessage response
    ) {
        m_response = response;
        m_message = Encoding.UTF8.GetString( response.Data.Span );
        m_channel = "";
    }
    
    public MessageType MessageType => MessageType.ClearChat;
    
    public string RawMessage => m_message;

    public string Channel => m_channel;

    public long? Duration {
        get {
            InitializeTags();
            string duration = MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.BanDurationPattern());
            return string.IsNullOrEmpty(duration) ? null : long.Parse(duration);
        }
    }

    public string RoomId {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.RoomIdPattern());
        }
    }

    public string TargetUserId {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.TargetUserIdPattern());
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

    public string Text {
        get {
            if( !m_response.MessageContentRange.HasValue ) {
                return "";
            }
            
            return Encoding.UTF8.GetString(
                m_response.Data.Span[
                    m_response.MessageContentRange.Value.Start
                        ..m_response.MessageContentRange.Value.End
                ]
            ).TrimEnd();
        }
    }

    public string TargetUserName {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse(m_tags, MessagePluginUtils.MsgTextPattern());
        }
    }
    
    private void InitializeTags() {

        if( !string.IsNullOrEmpty( m_tags ) ) {
            return;
        }

        m_tags = MessagePluginUtils.GetTagsFromMessage( m_response );
    }
}