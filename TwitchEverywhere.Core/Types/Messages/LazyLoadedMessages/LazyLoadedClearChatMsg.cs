using System.Text;
using System.Text.RegularExpressions;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

public class LazyLoadedClearChatMsg(
    RawMessage response
) : IClearChatMsg {
    private readonly string m_channel = "";
    private string m_tags;

    public MessageType MessageType => MessageType.ClearChat;

    public string RawMessage => Encoding.UTF8.GetString( response.Data.Span );

    public string Channel => m_channel;

    public long? Duration {
        get {
            InitializeTags();
            string duration = MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.BanDurationPattern() );
            return string.IsNullOrEmpty( duration ) ? null : long.Parse( duration );
        }
    }

    public string RoomId {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.RoomIdPattern() );
        }
    }

    public string TargetUserId {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.TargetUserIdPattern() );
        }
    }

    public DateTime Timestamp {
        get {
            InitializeTags();
            long rawTimestamp = Convert.ToInt64(
                MessagePluginUtils.MessageTimestampPattern().Match( m_tags ).Value
                    .Split( "=" )[1]
                    .TrimEnd( ';' )
            );
            return DateTimeOffset.FromUnixTimeMilliseconds( rawTimestamp ).UtcDateTime;
        }
    }

    string IClearChatMsg.Text {
        get {

            if( !response.MessageContentRange.HasValue ) {
                return "";
            }

            return Encoding.UTF8.GetString(
                response.Data.Span[
                    response.MessageContentRange.Value.Start
                        ..response.MessageContentRange.Value.End
                ]
            ).TrimEnd();
        }
    }

    public string TargetUserName {
        get {
            InitializeTags();
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.MsgTextPattern() );
        }
    }

    private void InitializeTags() {

        if( !string.IsNullOrEmpty( m_tags ) ) {
            return;
        }

        m_tags = MessagePluginUtils.GetTagsFromMessage( response );
    }
}