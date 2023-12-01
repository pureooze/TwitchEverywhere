using TwitchEverywhere.Implementation.MessagePlugins;
using TwitchEverywhere.Types.Messages.Interfaces;

namespace TwitchEverywhere.Types.Messages.ImmediateLoadedMessages; 

public class ImmediateLoadedJoinMsg : Message, IJoinMsg {
    private readonly string m_channel;
    private readonly string m_user;

    public ImmediateLoadedJoinMsg(
        string channel,
        string user
    ) {
        m_user = user;
        m_channel = channel;
    }

    public override MessageType MessageType => MessageType.Join;
    string IJoinMsg.User => m_user;
    string IJoinMsg.Channel => m_channel;


}