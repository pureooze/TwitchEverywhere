using System.Runtime.Serialization;

namespace TwitchEverywhere.Core.Types.RestApi;

public enum VideoEntryType {
    [EnumMember(Value = "archive")]
    Archive,
    
    [EnumMember(Value = "highlight")]
    Highlight,
    
    [EnumMember(Value = "upload")]
    Upload
}