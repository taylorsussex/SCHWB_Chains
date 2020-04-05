using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCHWB_Chains
{
    class ComboGenerator
    {
        public ComboGenerator(List<Form1.Move> moves)
        {
            Moves = moves;
            rnd = new Random();

            Starters = new List<Form1.Move>();

            foreach (Form1.Move m in Moves)
            {
                if (!m.FollowupOnly)
                    Starters.Add(m);
            }
        }

        public List<Form1.Move> GenerateRandomCombo(string starterName)
        {
            Combo = new List<Form1.Move>();
            Form1.Move starter;

            // Choose random starter
            // Will be overwritten if a starter is specified
            int r = rnd.Next(Starters.Count);
            starter = Starters[r];

            // Find move that has the specified name
            foreach (Form1.Move s in Starters)
            {
                if (s.Name == starterName)
                {
                    starter = s;
                    break;
                }
            }

            Combo.Add(starter);

            while (true)
            {
                List<Form1.Move> options = getOptions(Combo.Last().Followups);

                if (options.Count == 0)
                    return Combo;

                r = rnd.Next(options.Count);

                Combo.Add(options[r]);
            }
        }

        public List<Form1.Move> GenerateRandomCombo()
        {
            Combo = new List<Form1.Move>();

            // Choose random starter
            int r = rnd.Next(Starters.Count);
            Combo.Add(Starters[r]);

            while (true)
            {
                List<Form1.Move> options = getOptions(Combo.Last().Followups);

                if (options.Count == 0)
                    return Combo;

                r = rnd.Next(options.Count);

                Combo.Add(options[r]);
            }
        }

        private List<Form1.Move> getOptions(List<string> goTo)
        {
            List<Form1.Move> validOptions = new List<Form1.Move>();

            foreach (string m in goTo)
            {
                bool valid = true;

                foreach (Form1.Move n in Combo)
                {
                    if (m == n.Name)
                        valid = false;
                }

                if (valid)
                {
                    foreach (Form1.Move x in Moves)
                    {
                        if (m == x.Name)
                        {
                            validOptions.Add(x);
                            break;
                        }
                    }
                }

            }

            return validOptions;
        }

        private List<Form1.Move> Moves;
        public List<Form1.Move> Starters;

        private List<Form1.Move> Combo;

        private static Random rnd;
    }
}
