using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TournamentCreator
{
    public partial class Scores : Form
    {
        private List<Event> events;
        private List<Participant> participants;
        private List<Team> teams;

        public Scores(List<Event> _events, List<Participant> _participants, List<Team> _teams)
        {
            teams = _teams;
            events = _events;
            participants = _participants;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string participantName = cmbName.Text;
            string eventName = txtScoreEvent.Text;
            if (!int.TryParse(txtScorePoints.Text, out int points))
            {
                MessageBox.Show("Please enter a valid score.");
                return;
            }

            var participant = participants.FirstOrDefault(p => p.Name == participantName || p.TeamName == participantName);
            if (participant == null)
            {
                MessageBox.Show("Participant not found.");
                return;
            }

            var evnt = events.FirstOrDefault(ev => ev.EventName == eventName);
            if (evnt == null)
            {
                MessageBox.Show("Event not found.");
                return;
            }

            //evnt.Scores.Add(new Score { Participant = participant, Points = points });
            UpdateScoreList();
        }

        private void txtScorePoints_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpdateScoreList()
        {
            //lstScores.Items.Clear();
            //foreach (var e in events)
            //{
            //    lstScores.Items.Add($"Event: {e.EventName}");
            //    foreach (var s in e.Scores.OrderByDescending(s => s.Points))
            //    {
            //        string participantName = e.IsTeamEvent ? s.Participant.TeamName : s.Participant.Name;
            //        lstScores.Items.Add($"    {participantName}: {s.Points} points");
            //    }
            //}
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Menu menu = new Menu(participants, events, teams);
            menu.ShowDialog();
        }

        private void lstEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSelectedEvent.Text = $"Selected Event: {lstEvents.SelectedItem}";
        }
    }
}
