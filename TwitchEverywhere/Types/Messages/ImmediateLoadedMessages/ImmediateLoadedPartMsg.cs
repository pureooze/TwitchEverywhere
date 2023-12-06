using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages; 

public class ImmediateLoadedPartMsg : IPartMsg {
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


    public MessageType MessageType => MessageType.Part;
    public string RawMessage => m_message;
    public string Channel => m_channel;

    string IPartMsg.User => m_user;


}