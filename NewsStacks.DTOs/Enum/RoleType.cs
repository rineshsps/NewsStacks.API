using System.Runtime.Serialization;

namespace NewsStacks.DTOs.Enum
{
    public enum RoleType
    {
        Admin = 1,
        [EnumMember(Value = "Reader/User")]
        Reader = 2,
        Reviewer = 3,
        Writer = 4,
        Editor = 5,
        Publisher = 6
    }
}
