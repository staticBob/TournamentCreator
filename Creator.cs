using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;

namespace TournamentCreator
{
    public partial class Creator : Form
    {
        public List<Event> events { get; set; }
        private List<Participant> participants = new List<Participant>();
        private List<Team> teams = new List<Team>();

        private readonly string saveFilePath = "tournament_data.json";

        public Creator()
        {
            InitializeComponent();

            // Load data when form loads
            this.Load += Main_Load;
            this.FormClosing += Main_FormClosing;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveData();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            LoadData();
            UpdateTeamsCmb();
        }

        private void SaveData()
        {
            var data = new
            {
                Participants = participants,
                Teams = teams,
                Events = events
            };

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(saveFilePath, json);
        }

        private void LoadData()
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                var data = JsonConvert.DeserializeObject<dynamic>(json);

                participants = data.Participants.ToObject<List<Participant>>();
                teams = data.Teams.ToObject<List<Team>>();
                events = data.Events.ToObject<List<Event>>();

                UpdateParticipantList();
                UpdateTeams();
                UpdateTeamsCmb();
            }
        }

        private void UpdateParticipantList()
        {
            lstParticipants.Items.Clear();
            foreach (var p in participants)
            {
                lstParticipants.Items.Add(p.IsIndividual ? p.Name : $"{p.TeamName} - {p.Name}");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool isIndividual = cmbParticipantType.SelectedItem.ToString() == "Individual";

            if (!isIndividual && teams.FirstOrDefault(t => t.TeamName == cmbTeam.SelectedItem.ToString()).teamFull)
            {
                MessageBox.Show("The selected team is full, try selecting another.");
                return;
            }

            string name = txtParticipantName.Text;
            string teamName = isIndividual ? null : cmbTeam.Text;

            if (cmbParticipantType.SelectedItem == null || string.IsNullOrWhiteSpace(name) || (!isIndividual && string.IsNullOrWhiteSpace(teamName)))
            {
                MessageBox.Show("Please enter valid participant details.");
                return;
            }

            Team tm = null;
            if (!isIndividual)
            {
                tm = teams.FirstOrDefault(t => t.TeamName == cmbTeam.SelectedItem.ToString());
                tm.Participants.Add(new Participant { Name = name, IsIndividual = isIndividual, TeamName = teamName });
            }

            if (tm == null && !isIndividual)
            {
                MessageBox.Show("Please enter a valid team");
                return;
            }

            participants.Add(new Participant { Name = name, IsIndividual = isIndividual, TeamName = teamName });
            
            UpdateParticipantList();
            UpdateTeams();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = txtTeamName.Text;

            if (!string.IsNullOrWhiteSpace(name))
            {
                teams.Add(new Team {TeamName = name});
            } else
            {
                MessageBox.Show("Please enter a team name");
            }

            UpdateTeams();
            UpdateTeamsCmb();
        }

        private void UpdateTeams()
        {
            lstTeams.Items.Clear();
            foreach (Team team in teams)
            {
                int count = team.Participants.Count;
                if (count < 5)
                {
                    lstTeams.Items.Add($"{team.TeamName} - {count}/5");
                }
                else
                {
                    team.teamFull = true;
                    lstTeams.Items.Add($"{team.TeamName} - team full");
                }
            }
        }

        private void UpdateTeamsCmb()
        {
            cmbTeam.Items.Clear();

            if (cmbParticipantType.SelectedItem != null && cmbParticipantType.SelectedItem.ToString() == "Team")
            {
                foreach (var item in teams)
                {
                    cmbTeam.Items.Add(item.TeamName);
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbParticipantType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbParticipantType.SelectedItem.ToString() == "Individual")
            {
                cmbTeam.SelectedIndex = -1;
                cmbTeam.Enabled = false;
            } else if (cmbParticipantType.SelectedItem.ToString() == "Team")
            {
                UpdateTeamsCmb();
                cmbTeam.Enabled = true;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Scores sc = new Scores(events, participants, teams);
            sc.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            SaveData();
            this.Hide();
            Menu menu = new Menu(participants, events, teams);
            menu.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all participants from the list?", "Clear all participants", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                participants.Clear();
                UpdateParticipantList();
            }
        }

        private void lstParticipants_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = null;
            if (lstParticipants.SelectedItems.Count > 0)
            {
                selectedItem = lstParticipants.SelectedItems[0].ToString();
            }

            DialogResult result = MessageBox.Show($"Delete Participant {selectedItem}?", "Delete Participant", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                participants.Remove(participants[lstParticipants.SelectedIndex]);
            }
            UpdateParticipantList();
        }

        private void lstTeams_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = null;
            if (lstTeams.SelectedItems.Count > 0)
            {
                selectedItem = lstTeams.SelectedItems[0].ToString();
            }

            DialogResult result = MessageBox.Show($"Delete Team {selectedItem}?", "Delete Team", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                teams.Remove(teams[lstTeams.SelectedIndex]);
            }
            UpdateTeamsCmb();
            UpdateTeams();
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all team from the list?", "Clear all teams", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                teams.Clear();
                UpdateTeams();
                UpdateTeamsCmb();
            }
        }
    }
}
