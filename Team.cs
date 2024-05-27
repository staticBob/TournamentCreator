using System.Collections.Generic;

public class Team
{
    public string TeamName { get; set; }
    public bool teamFull { get; set; }
    public List<Participant> Participants { get; set; } = new List<Participant>();
}