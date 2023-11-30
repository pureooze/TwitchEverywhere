using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages; 

public class ImmediateLoadedJoinMsg : Message, IJoinMsg {
    private readonly string m_message;
    private readonly string m_user;

    public ImmediateLoadedJoinMsg(
        string message,
        string channel,
        string user
    ) {
        m_message = message;
        m_user = user;
    }

    public override MessageType MessageType => MessageType.Join;
    string IJoinMsg.User => m_user;



}