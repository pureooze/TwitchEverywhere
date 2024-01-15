using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

internal class LazyLoadedPrivMsg( RawMessage response ) : IPrivMsg {

    private string m_tags;

    public MessageType MessageType => MessageType.PrivMsg;

    public string RawMessage => Encoding.UTF8.GetString( response.Data.Span );

    public string Channel => "";

    IImmutableList<Badge> IPrivMsg.Badges {
        get {
            InitializeTags();
            
            string badges = MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.BadgesPattern() );
            return MessagePluginUtils.GetBadges( badges );
        }
    }

    IImmutableList<Badge> IPrivMsg.BadgeInfo {
        get {
            InitializeTags();
            
            string badges = MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.BadgeInfoPattern() );
            return MessagePluginUtils.GetBadges( badges );
        }
    }

    string IPrivMsg.Bits {
        get {
            InitializeTags();
            
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.BitsPattern() );
        }
    }

    string IPrivMsg.Color {
        get {
            InitializeTags();
            
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.ColorPattern() );
        }
    }

    string IPrivMsg.DisplayName {
        get {
            InitializeTags();
            
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.DisplayNamePattern() );
        }
    }

    IImmutableList<Emote>? IPrivMsg.Emotes {
        get {
            InitializeTags();
            
            string emotesText = MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.EmotesPattern() );
            return MessagePluginUtils.GetEmotesFromText( emotesText );
        }
    }

    string IPrivMsg.Id {
        get {
            InitializeTags();
            
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.IdPattern() );
        }
    }

    bool IPrivMsg.Mod {
        get {
            InitializeTags();
            
            return MessagePluginUtils.GetValueIsPresentOrBoolean( m_tags, MessagePluginUtils.ModPattern() );
        }
    }

    long? IPrivMsg.PinnedChatPaidAmount {
        get {
            InitializeTags();
            
            string pinnedChatPaidAmount = MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.PinnedChatPaidAmountPattern() );
            return string.IsNullOrEmpty( pinnedChatPaidAmount ) ? null : long.Parse( pinnedChatPaidAmount );
        }
    }

    string IPrivMsg.PinnedChatPaidCurrency {
        get {
            InitializeTags();
            
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.PinnedChatPaidCurrencyPattern() );
        }
    }

    long? IPrivMsg.PinnedChatPaidExponent {
        get {
            InitializeTags();
            
            string pinnedChatPaidExponent = MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.PinnedChatPaidExponentPattern() );
            return string.IsNullOrEmpty( pinnedChatPaidExponent ) ? null : int.Parse( pinnedChatPaidExponent );
        }
    }

    PinnedChatPaidLevel? IPrivMsg.PinnedChatPaidLevel {
        get {
            InitializeTags();
            
            string pinnedChatPaidLevelText = MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.PinnedChatPaidLevelPattern() );
            return MessagePluginUtils.GetPinnedChatPaidLevelType( pinnedChatPaidLevelText );
        }
    }

    bool IPrivMsg.PinnedChatPaidIsSystemMessage {
        get {
            InitializeTags();
            
            return !string.IsNullOrEmpty( MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.PinnedChatPaidIsSystemMessagePattern() ) );
        }
    }

    string IPrivMsg.ReplyParentMsgId {
        get {
            InitializeTags();
            
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.ReplyParentMsgIdPattern() );
        }
    }

    string IPrivMsg.ReplyParentUserId {
        get {
            InitializeTags();
            
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.ReplyParentUserIdPattern() );
        }
    }

    string IPrivMsg.ReplyParentUserLogin {
        get {
            InitializeTags();
            
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.ReplyParentUserLoginPattern() );
        }
    }

    string IPrivMsg.ReplyParentDisplayName {
        get {
            InitializeTags();
            
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.ReplyParentDisplayNamePattern() );
        }
    }

    string IPrivMsg.ReplyThreadParentMsg {
        get {
            InitializeTags();
            
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.ReplyThreadParentMsgPattern() );
        }
    }

    string IPrivMsg.RoomId {
        get {
            InitializeTags();
            
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.RoomIdPattern() );
        }
    }

    bool IPrivMsg.Subscriber {
        get {
            InitializeTags();
            
            return MessagePluginUtils.GetValueIsPresentOrBoolean( m_tags, MessagePluginUtils.SubscriberPattern() );
        }
    }

    DateTime? IPrivMsg.Timestamp {
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

    bool IPrivMsg.Turbo {
        get {
            InitializeTags();
            
            return MessagePluginUtils.GetValueIsPresentOrBoolean( m_tags, MessagePluginUtils.TurboPattern() );
        }
    }

    string IPrivMsg.UserId {
        get {
            InitializeTags();
            
            return MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.UserIdPattern() );
        }
    }

    UserType IPrivMsg.UserType {
        get {
            InitializeTags();

            return MessagePluginUtils.GetUserType( 
                MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.UserTypePattern() ) 
            );
        }
    }

    bool IPrivMsg.Vip {
        get {
            InitializeTags();

            return !string.IsNullOrEmpty(
                MessagePluginUtils.GetValueFromResponse( m_tags, MessagePluginUtils.VipPattern() )
            );
        }
    }

    private void InitializeTags() {

        if( !string.IsNullOrEmpty( m_tags ) ) {
            return;
        }

        m_tags = MessagePluginUtils.GetTagsFromMessage( response );
    }

    string IPrivMsg.Text {
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
}