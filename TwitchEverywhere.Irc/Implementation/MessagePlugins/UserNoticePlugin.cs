using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages;
using TwitchEverywhere.Core.Types.Messages.Implementation;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;
using TwitchEverywhere.Irc.Types;

namespace TwitchEverywhere.Irc.Implementation.MessagePlugins; 

public class UserNoticePlugin : IMessagePlugin {

    bool IMessagePlugin.CanHandle(
        MessageType messageType
    ) {
        return messageType == MessageType.UserNotice;
    }

    void IMessagePlugin.ProcessMessage(
        IrcClientSubject subjects,
        RawMessage response
    ) {
        subjects.UserNoticeSubject.OnNext( new UserNoticeMsg( response ) );
    }
}