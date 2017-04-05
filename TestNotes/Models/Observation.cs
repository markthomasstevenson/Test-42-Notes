using System;

namespace TestNotes.Models
{
    [Serializable]
    public class Observation
    {
        public string Scenario { get; set; }

        public override string ToString()
        {
            return Scenario;
        }
    }
}
