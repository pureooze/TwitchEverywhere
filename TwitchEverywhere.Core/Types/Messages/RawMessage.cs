using TwitchEverywhere.Core.Extensions;

namespace TwitchEverywhere.Core.Types.Messages;

// Inspired by MiniTwitch
// https://github.com/Foretack/MiniTwitch/tree/master/MiniTwitch.Irc
public class RawMessage {
    public ReadOnlyMemory<byte> Data;
    public bool HasMoreMessages = false;
    public int NextMessageStartIndex { get; set; }

    public MessageType Type {
        get {
            return m_type switch {
                IrcCommand.Ping => MessageType.Ping,
                IrcCommand.Join => MessageType.Join,
                // case IrcCommand.PONG:
                //     return MessageType.Pong;
                IrcCommand.CapabilitiesReceived => MessageType.CapReq,
                IrcCommand.Part => MessageType.Part,
                IrcCommand.Notice => MessageType.Notice,
                IrcCommand.Whisper => MessageType.Whisper,
                IrcCommand.Privmsg => MessageType.PrivMsg,
                IrcCommand.ClearMsg => MessageType.ClearMsg,
                IrcCommand.ClearChat => MessageType.ClearChat,
                IrcCommand.Reconnect => MessageType.Reconnect,
                IrcCommand.RoomState => MessageType.RoomState,
                IrcCommand.UserState => MessageType.UserState,
                IrcCommand.UserNotice => MessageType.UserNotice,
                IrcCommand.GlobalUserState => MessageType.GlobalUserState,
                IrcCommand.Unknown => MessageType.Unknown,
                _ => MessageType.Unknown
            };
        }
    }


    public Range? TagsRange;
    public Range? UsernameRange;
    public Range? ChannelRange;
    public Range? MessageContentRange;

    private readonly IrcCommand m_type;

    public RawMessage(
        ReadOnlyMemory<byte> data
    ) {
        Data = data;
        const byte space = (byte)' ';
        const byte exclamation = (byte)'!';
        const byte at = (byte)'@';
        const byte colon = (byte)':';
        const byte asterisk = (byte)'*';
        const byte carriageReturn = (byte)'\r';
        const byte nullCharacter = (byte)'\0';

        ReadOnlySpan<byte> dataSpan = Data.Span;

        switch( dataSpan[0] ) {
            case at:
                TagsRange = 1..dataSpan.IndexOf( space );
                int usernameStart = TagsRange.Value.End.Value + 2;
                
                AfterUsernameStart:
                
                int usernameEnd = dataSpan[usernameStart..].IndexOf( exclamation ) + usernameStart;
                if( usernameEnd - usernameStart is not -1 and <= 25 ) {
                    UsernameRange = usernameStart..usernameEnd;
                }

                int commandStartAddVal = UsernameRange.HasValue ? usernameEnd : usernameStart;
                int commandStart = dataSpan[commandStartAddVal..].IndexOf( space ) + commandStartAddVal + 1;
                int commandEnd = dataSpan[commandStart..].IndexOf( space ) + commandStart;

                if( commandEnd - commandStart == -1 ) {
                    m_type = (IrcCommand)dataSpan[commandStart..dataSpan.Length].Sum();
                } else {
                    m_type = (IrcCommand)dataSpan[commandStart..commandEnd].Sum();
                }

                int contentStart;
                if( dataSpan[commandEnd + 1] == asterisk ) {
                    // IsGlobalChannel = true;
                    contentStart = commandEnd + 4;
                } else {
                    int channelStart = commandEnd + 2;
                    int channelEnd = dataSpan[channelStart..].IndexOfAny( space, carriageReturn ) + channelStart;
                    if( channelEnd - channelStart == -1 ) {
                        ChannelRange = channelStart..dataSpan.Length;
                        return;
                    }

                    ChannelRange = channelStart..channelEnd;
                    if( dataSpan[channelEnd] == carriageReturn ) {
                        // HasMoreMessages = true;
                        NextMessageStartIndex = channelEnd + 2;
                        return;
                    }

                    contentStart = channelEnd + 2;
                }

                // Didn't end at channel so there must be content
                // HasMoreMessages = true;
                int contentEnd = dataSpan[contentStart..].IndexOf( carriageReturn ) + contentStart;
                int nullIndex = dataSpan[contentStart..].IndexOf( nullCharacter ) + contentStart;
                if( nullIndex - contentStart == -1 && contentEnd == nullIndex ) {
                    MessageContentRange = contentStart..dataSpan.Length;
                } else if ( contentEnd - contentStart == -1 ) {
                    MessageContentRange = contentStart..nullIndex;
                } else {
                    MessageContentRange = contentStart..contentEnd;
                }

                if( dataSpan.Length > contentEnd + 1 ) {
                    // HasMoreMessages = true;
                    NextMessageStartIndex = contentEnd + 2;
                }

                break;
            case colon:
                usernameStart = 1;
                goto AfterUsernameStart;
            default:
                commandStart = 0;
                commandEnd = dataSpan.IndexOf( space );
                m_type = (IrcCommand)dataSpan[commandStart..commandEnd].Sum();
                int crIndex = dataSpan[commandEnd..].IndexOf( carriageReturn ) + commandEnd;
                if( crIndex - commandEnd != -1 ) {
                    // HasMoreMessages = true;
                    NextMessageStartIndex = crIndex + 2;
                }

                break;

        }
    }

    private enum IrcCommand {
        Unknown = 0,
        Connected = 145,
        CapabilitiesReceived = 212,
        Ping = 302,
        Join = 304,
        Pong = 308,
        Part = 311,
        Notice = 450,
        Whisper = 546,
        Privmsg = 552,
        ClearMsg = 590,
        ClearChat = 647,
        Reconnect = 673,
        RoomState = 702,
        UserState = 704,
        UserNotice = 769,
        GlobalUserState = 1137
    }
}