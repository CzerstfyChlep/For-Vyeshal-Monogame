using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForVyeshal___Monogame
{
    public class Modifier
    {
        public string Title = "";
        public string Description = "";
        public List<Variable> Variables = new List<Variable>();
        public int Duration = -1;
        public Modifier(string title, List<Variable> variables, int duration = -1, string description = "")
        {
            Title = title;
            Description = description;
            Variables = new List<Variable>(variables);
            Duration = duration;
        }
        public class Variable
        {
            public string Name = "";
            public decimal Value = 0;
            public Variable(string name, decimal value)
            {
                Name = name;
                Value = value;
            }
        }
    }
}
