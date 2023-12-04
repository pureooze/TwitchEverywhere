using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages; 

public class ImmediateLoadedPartMsg : Message, IPartMsg {
    private readonly string m_message;
    private string m_channel;
    private string m_user;

    public ImmediateLoadedPartMsg(
        string message,
        string channel,
        string user
    ) {
        m_message = message;
        m_channel = channel;
        m_user = user;
    }


    public override MessageType MessageType => MessageType.Part;
    public override string RawMessage => m_message;

    string IPartMsg.User => m_user;

    string IPartMsg.Channel => m_channel;

}