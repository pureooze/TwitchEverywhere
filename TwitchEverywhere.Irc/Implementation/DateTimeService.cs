namespace TwitchEverywhere.Irc.Implementation; 

public class DateTimeService : IDateTimeService {

    DateTime IDateTimeService.GetStartTime() {
        return DateTime.UtcNow;
    }
}