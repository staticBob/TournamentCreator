using System.Collections.Generic;

public class Event
{
    public string EventName { get; set; }
    public bool IsTeamEvent { get; set; }
    public List<Participant> Participants { get; set; } = new List<Participant>();
}
