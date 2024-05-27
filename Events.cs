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
    public partial class Events : Form
    {
        private List<Event> events = new List<Event>();
        private List<Participant> participants = new List<Participant>();
        private List<Team> teams = new List<Team>();

        public Events(List<Event> _events, List<Team> _teams)
        {
            events = _events;
            teams = _teams;
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            UpdateEventList();
        }

        private void UpdateEventList()
        {
            lstEvents.Items.Clear();
            foreach (var e in events)
            {
                lstEvents.Items.Add($"{e.EventName} ({(e.IsTeamEvent ? "Team" : "Individual")}) - {e.Participants.Count}{(e.IsTeamEvent ? "/5" : "/20")}");
            }
        }

        private void UpdateEventTypeReg()
        {
            cmbEventTypeReg.Items.Clear();
            foreach (Event item in events)
            {
                cmbEventTypeReg.Items.Add(item.EventName);
            }
        }

        private void lstEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = null;
            if (lstEvents.SelectedItems.Count > 0)
            {
                selectedItem = lstEvents.SelectedItems[0].ToString();
            }

            DialogResult result = MessageBox.Show($"Delete Event {selectedItem}? This will also delete all participants in this event", "Delete Event", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                events.Remove(events[lstEvents.SelectedIndex]);
            }
            UpdateEventList();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Menu menu = new Menu(participants, events, teams);
            menu.ShowDialog();
        }

        private void txtEventName_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string eventName = txtEventName.Text;
            bool isTeamEvent = cmbEventType.SelectedItem.ToString() == "Team";

            if (string.IsNullOrWhiteSpace(eventName))
            {
                MessageBox.Show("Please enter a valid event name.");
                return;
            }

            events.Add(new Event { EventName = eventName, IsTeamEvent = isTeamEvent});
            UpdateEventList();
            UpdateEventTypeReg();
        }

        private void Events_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbEventTypeReg_SelectedIndexChanged(object sender, EventArgs e)
        {
            Event ev = events.FirstOrDefault(eve => eve.EventName == cmbEventTypeReg.Text);
            if (ev.IsTeamEvent)
            {
                lblRegParticipant.Text = "Team";
                cmbParticipantNameReg.Items.Clear();
                foreach(Team tm in teams)
                {
                    cmbParticipantNameReg.Items.Add(tm.TeamName);
                }
            }
            else
            {
                lblRegParticipant.Text = "Participant";
                cmbParticipantNameReg.Items.Clear();
                foreach (Participant pc in participants)
                {
                    cmbParticipantNameReg.Items.Add(pc.Name);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // register participant for chosen event
            Event ev = events.FirstOrDefault(eve => eve.EventName == cmbEventTypeReg.Text);
            if (ev.IsTeamEvent)
            {
                foreach (Participant pc in teams.FirstOrDefault(t => t.TeamName == cmbParticipantNameReg.Text).Participants)
                {
                    events[cmbEventTypeReg.SelectedIndex].Participants.Add(pc);
                }
            }
            else
            {
                events[cmbEventTypeReg.SelectedIndex].Participants.Add(participants.FirstOrDefault(p => p.Name == cmbParticipantNameReg.Text));
            }

            UpdateEventList();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
