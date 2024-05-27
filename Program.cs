using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace TournamentCreator
{
    static class Program
    {
        private static List<Participant> participants = new List<Participant>();
        private static List<Team> teams = new List<Team>();
        private static List<Event> events = new List<Event>();

        private readonly static string saveFilePath = "tournament_data.json";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                var data = JsonConvert.DeserializeObject<dynamic>(json);

                participants = data.Participants.ToObject<List<Participant>>();
                teams = data.Teams.ToObject<List<Team>>();
                events = data.Events.ToObject<List<Event>>();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Menu(participants, events, teams));
        }
    }
}
