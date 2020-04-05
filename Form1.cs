using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCHWB_Chains
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = textBox1.Text;
            moves = new List<Move>();

            // Initialise variables
            string currentMove = "";
            bool followupOnly = false;
            List<string> followups = new List<string>();
            string input = "";

            bool readingInput = false;
            bool readingFollowup = false;

            if (!File.Exists(path))
                return;

            var lines = File.ReadLines(path);

            // Loop through lines
            foreach (var line in lines)
            {
                if (readingInput)
                {
                    // Set input
                    input = line;
                    readingInput = false;
                    continue;
                }

                if (readingFollowup)
                {
                    // Check if finished
                    if (line == "#FOLLOWUP_END" || line == "#CANCEL_INTO_END")
                    {
                        readingFollowup = false;
                        continue;
                    }

                    // Add to followups
                    followups.Add(line.Split(new char[] { ' ' })[0]);
                }

                if (line.Split(new char[] { ' ' })[0] == "#MOVE")
                {
                    if (!(!followupOnly && followups.Count == 0))
                    {
                        // Add current move to list
                        moves.Add(new Move(currentMove, followups, followupOnly, input));
                    }

                    // Reset variables
                    followups = new List<string>();
                    followupOnly = false;
                    input = "";

                    // Set move name
                    var temp = line.Split(new char[] { ' ' });
                    currentMove = temp[temp.Length - 1];

                    continue;
                }
                else if (line == "#FOLLOWUP_ONLY")
                {
                    // Set followup only bool to true
                    followupOnly = true;
                    continue;
                }
                else if (line == "#INPUT " || line == "#INPUT")
                {
                    // Get the input on the next line
                    readingInput = true;
                    continue;
                }
                else if (line == "#FOLLOWUP" || line == "#CANCEL_INTO")
                {
                    // Read the followups on the next lines
                    readingFollowup = true;
                    continue;
                }
            }

            moves.RemoveAt(0);

            gen = new ComboGenerator(moves);
        }

        public struct Move
        {
            public string Name;
            public List<string> Followups;
            public bool FollowupOnly;
            public string Input;

            public Move(string name, List<string> followups, bool followupOnly, string input)
            {
                Name = name;
                Followups = followups;
                FollowupOnly = followupOnly;
                Input = input;
            }
        }

        List<Move> moves = new List<Move>();
        ComboGenerator gen;
        int numCombos;

        private void button3_Click(object sender, EventArgs e)
        {
            using (var w = new StreamWriter(textBox2.Text))
            {
                for (int i = 0; i < Int32.Parse(textBox3.Text); i++)
                {
                    var combo = gen.GenerateRandomCombo();
                    string moves = "";
                    string inputs = "";

                    foreach (Move m in combo)
                    {
                        moves += " " + m.Name;
                        inputs += " " + m.Input;
                    }

                    moves.Remove(0, 1);

                    var line = String.Format("{0}, {1}, {2}\n", moves, inputs, combo.Count);
                    w.Write(line);
                    w.Flush();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox2.Text = openFileDialog.FileName;
            }
        }
    }
}
