using System.Collections.Immutable;
using TwitchEverywhere.Core.Types.Messages.Interfaces;

namespace TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages;

public class ImmediateLoadedWhisperMsg : IWhisperMsg {
    private readonly IImmutableList<Badge> m_badges;
    private readonly string m_color;
    private readonly string m_displayName;
    private readonly IImmutableList<Emote>? m_emotes;
    private readonly string m_msgId;
    private readonly string m_threadId;
    private readonly bool m_turbo;
    private readonly string m_userId;
    private readonly UserType m_userType;
    private readonly string m_fromUser;
    private readonly string m_toUser;
    private readonly string m_text;


    public ImmediateLoadedWhisperMsg(
        string channel,
        IImmutableList<Badge> badges = null,
        string color = null,
        string displayName = null,
        IImmutableList<Emote>? emotes = null,
        string msgId = null,
        string threadId = null,
        bool turbo = default,
        string userId = null,
        UserType userType = default,
        string fromUser = null,
        string toUser = null,
        string text = null
    ) {
        Channel = channel;
        m_badges = badges;
        m_color = color;
        m_displayName = displayName;
        m_emotes = emotes;
        m_msgId = msgId;
        m_threadId = threadId;
        m_turbo = turbo;
        m_userId = userId;
        m_userType = userType;
        m_fromUser = fromUser;
        m_toUser = toUser;
        m_text = text;
    }

    public MessageType MessageType => MessageType.Whisper;
    
    public string RawMessage => "";
    
    public string Channel { get; }
    
    IImmutableList<Badge> IWhisperMsg.Badges => m_badges;

    string IWhisperMsg.Color => m_color;

    string IWhisperMsg.DisplayName => m_displayName;

    IImmutableList<Emote>? IWhisperMsg.Emotes => m_emotes;

    string IWhisperMsg.MsgId => m_msgId;

    string IWhisperMsg.ThreadId => m_threadId;

    bool IWhisperMsg.Turbo => m_turbo;

    string IWhisperMsg.UserId => m_userId;

    UserType IWhisperMsg.UserType => m_userType;

    string IWhisperMsg.FromUser => m_fromUser;

    string IWhisperMsg.ToUser => m_toUser;

    string IWhisperMsg.Text => m_text;
}