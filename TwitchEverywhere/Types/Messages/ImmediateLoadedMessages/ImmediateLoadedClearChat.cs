using TwitchEverywhere.Types.Messages.Interfaces;
using MessagePluginUtils = TwitchEverywhere.Implementation.MessagePluginUtils;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedClearChat : IClearChat {
    private const long PERMANANT_BAN_DURATION = -1;
    
    public ImmediateLoadedClearChat(
        string channel,
        string? targetUserName = null,
        long? banDuration = null,
        string? roomId = null,
        string? targetUserId = null,
        DateTime? timestamp = null,
        string? text = null
    ) {
        Channel = channel;
        TargetUserName = targetUserName ?? string.Empty;
        Duration = banDuration ?? PERMANANT_BAN_DURATION;
        RoomId = roomId ?? string.Empty;
        TargetUserId = targetUserId ?? string.Empty;
        Timestamp = timestamp ?? DateTime.Now;
        Text = text ?? string.Empty;
    }

    public MessageType MessageType => MessageType.ClearChat;
    public string RawMessage => GetRawMessage();

    public string Channel { get; }
    public long? Duration { get; }
    public string RoomId { get; }
    public string TargetUserId { get; }
    public DateTime Timestamp { get; }
    public string Text { get; }
    public string TargetUserName { get; }

    private string GetRawMessage() {
        string message = "@";

        if( Duration != PERMANANT_BAN_DURATION ) {
            message += SerializeProperty( MessagePluginUtils.Properties.BanDuration, () => Duration?.ToString() ?? string.Empty );
        }
        
        if( !string.IsNullOrEmpty( RoomId ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.RoomId, () => RoomId );
        }
        
        if( !string.IsNullOrEmpty( TargetUserId ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.TargetUserId, () => TargetUserId );
        }

        message += SerializeProperty( MessagePluginUtils.Properties.MessageTimestamp, () => new DateTimeOffset(Timestamp).ToUnixTimeMilliseconds().ToString());

        if( !string.IsNullOrEmpty( Text ) ) {
            message += SerializeProperty( MessagePluginUtils.Properties.MsgText, () => Text);
        }
        
        message = message.Substring(0, message.Length - 1);
        
        if( !string.IsNullOrEmpty( TargetUserName ) ) {
            message += $" :tmi.twitch.tv {MessageType.ToString().ToUpper()} #{Channel} :{TargetUserName}";
        } else {
            message += $" :tmi.twitch.tv {MessageType.ToString().ToUpper()} #{Channel}";
        }
        
        return message;
    }
    
    private string SerializeProperty(
        MessagePluginUtils.Properties property,
        Func<string> serializer
    ) {

        return string.Format( MessagePluginUtils.GetPropertyAsString( property ), serializer() );
    }
}