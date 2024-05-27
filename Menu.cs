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
    public partial class Menu : Form
    {
        private List<Participant> participants = new List<Participant>();
        private List<Event> events = new List<Event>();
        private List<Team> teams = new List<Team>();

        public Menu(List<Participant> _participants, List<Event> _events, List<Team> _teams)
        {
            teams = _teams;
            events = _events;
            participants = _participants;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Creator form1 = new Creator();
            form1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Scores sc = new Scores(events, participants, teams);
            sc.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Events ev = new Events(events, teams);
            ev.ShowDialog();
        }
    }
}
