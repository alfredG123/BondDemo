using System.Collections.Generic;

public class PartyList
{
    public class PartyMember
    {
        public PartyMember(Spirit spirit)
        {

        }

        public Spirit Spirit { get; private set; } = null;
        public bool IsActive { get; private set; } = false;
    }

    private List<PartyMember> _Party = new List<PartyMember>();

    public PartyList()
    {
    }

    public int PartyMemberCount
    {
        get => _Party.Count;
    }
}
