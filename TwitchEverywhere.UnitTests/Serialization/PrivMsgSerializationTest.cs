using System.Collections.Immutable;
using TwitchEverywhere.Core.Types;
using TwitchEverywhere.Core.Types.Messages.ImmediateLoadedMessages;
using TwitchEverywhere.Core.Types.Messages.LazyLoadedMessages;

namespace TwitchEverywhere.UnitTests.Serialization;

[TestFixture]
public class PrivMsgSerializationTest {

    [Test]
    public void LazyLoadedPrivMsgSerialization() {
        const string channel = "channel";
        const string message =
            "@badge-info=;badges=turbo/1;color=#0D4200;display-name=ronni;emotes=25:0-4,12-16/1902:6-10;id=b34ccfc7-4977-403a-8a94-33c6bac34fb8;mod=0;room-id=1337;subscriber=0;tmi-sent-ts=1507246572675;turbo=1;user-id=1337;user-type=global_mod :ronni!ronni@ronni.tmi.twitch.tv PRIVMSG #channel :Kappa Keepo Kappa";
        LazyLoadedPrivMsg lazyLoadedPrivMsg = new( channel: channel, message: message );

        Assert.That( lazyLoadedPrivMsg.RawMessage, Is.EqualTo( message ) );
    }

    [Test]
    public void NonSubscriber_ImmediateLoadedPrivMsg_SerializationToIRCMessage() {

        ImmediateLoadedPrivMsg immediateLoadedPrivMsg = new(
            channel: "ronni",
            badges: new List<Badge>() {
                new( "turbo", "1" )
            }.ToImmutableList(),
            emotes: new List<Emote>() {
                new( "25", 0, 4 ),
                new( "25", 12, 16 ),
                new( "1902", 6, 10 )
            }.ToImmutableList(),
            displayName: "ronni",
            bits: null,
            color: "#0D4200",
            subscriber: false,
            text: "Kappa Keepo Kappa",
            id: "b34ccfc7-4977-403a-8a94-33c6bac34fb8",
            timestamp: new DateTime( 2017, 10, 5, 23, 36, 12, 675, DateTimeKind.Utc ),
            roomId: "1337",
            turbo: true,
            userId: "1337",
            userType: UserType.GlobalMod
        );

        Assert.That(
            immediateLoadedPrivMsg.RawMessage,
            Is.EqualTo(
                "@badge-info=;badges=turbo/1;color=#0D4200;display-name=ronni;mod=0;emotes=25:0-4,12-16/1902:6-10;id=b34ccfc7-4977-403a-8a94-33c6bac34fb8;room-id=1337;subscriber=0;turbo=1;user-id=1337;user-type=global_mod;tmi-sent-ts=1507246572675 :ronni!ronni@ronni.tmi.twitch.tv PRIVMSG #ronni :Kappa Keepo Kappa"
            )
        );
    }

    [Test]
    public void Cheer100Bits_ImmediateLoadedPrivMsg_SerializationToIRCMessage() {

        ImmediateLoadedPrivMsg immediateLoadedPrivMsg = new(
            channel: "ronni",
            badges: new List<Badge>() {
                new( "staff", "1" ),
                new( "bits", "1000" )
            }.ToImmutableList(),
            displayName: "ronni",
            bits: "100",
            subscriber: false,
            text: "cheer100",
            id: "b34ccfc7-4977-403a-8a94-33c6bac34fb8",
            timestamp: new DateTime( 2017, 10, 5, 23, 36, 12, 675, DateTimeKind.Utc ),
            roomId: "12345678",
            turbo: true,
            userId: "12345678",
            userType: UserType.Staff
        );

        Assert.That(
            immediateLoadedPrivMsg.RawMessage,
            Is.EqualTo(
                "@badge-info=;badges=staff/1,bits/1000;bits=100;display-name=ronni;mod=0;id=b34ccfc7-4977-403a-8a94-33c6bac34fb8;room-id=12345678;subscriber=0;turbo=1;user-id=12345678;user-type=staff;tmi-sent-ts=1507246572675 :ronni!ronni@ronni.tmi.twitch.tv PRIVMSG #ronni :cheer100"
            )
        );
    }

    [Test]
    public void VipChatter_ImmediateLoadedPrivMsg_SerializationToIRCMessage() {

        ImmediateLoadedPrivMsg immediateLoadedPrivMsg = new(
            channel: "ronni",
            badges: new List<Badge>() {
                new( "vip", "1" ),
                new( "partner", "1" )
            }.ToImmutableList(),
            displayName: "fun2bfun",
            subscriber: false,
            id: "1fd20412-965f-4c96-beb3-52266448f564",
            timestamp: new DateTime( 2022, 8, 24, 20, 14, 12, 425, DateTimeKind.Utc ),
            roomId: "102336968",
            turbo: false,
            userId: "12345678",
            userType: UserType.Normal,
            vip: true
        );

        Assert.That(
            immediateLoadedPrivMsg.RawMessage,
            Is.EqualTo(
                "@badge-info=;badges=vip/1,partner/1;display-name=fun2bfun;mod=0;id=1fd20412-965f-4c96-beb3-52266448f564;room-id=102336968;subscriber=0;turbo=0;user-id=12345678;tmi-sent-ts=1661372052425;vip=1"
            )
        );
    }

    [Test]
    public void HypeChat_ImmediateLoadedPrivMsg_SerializationToIRCMessage() {

        ImmediateLoadedPrivMsg immediateLoadedPrivMsg = new(
            channel: "ronni",
            badges: new List<Badge>() {
                new( "glhf-pledge", "1" )
            }.ToImmutableList(),
            subscriber: false,
            id: "f6fb34f8-562f-4b4d-b628-32113d0ef4b0",
            timestamp: new DateTime( 2023, 6, 22, 22, 13, 04, 306, DateTimeKind.Utc ),
            roomId: "12345678",
            turbo: false,
            userId: "12345678",
            userType: UserType.Normal,
            pinnedChatPaidAmount: 200,
            pinnedChatPaidExponent: 2,
            pinnedChatPaidCurrency: "USD",
            pinnedChatPaidLevel: PinnedChatPaidLevel.One
        );

        Assert.That(
            immediateLoadedPrivMsg.RawMessage,
            Is.EqualTo(
                "@badge-info=;badges=glhf-pledge/1;mod=0;id=f6fb34f8-562f-4b4d-b628-32113d0ef4b0;pinned-chat-paid-amount=200;pinned-chat-paid-canonical-amount=200;pinned-chat-paid-currency=USD;pinned-chat-paid-exponent=2;pinned-chat-paid-is-system-message=0;pinned-chat-paid-level=ONE;room-id=12345678;subscriber=0;turbo=0;user-id=12345678;tmi-sent-ts=1687471984306"
            )
        );
    }
}