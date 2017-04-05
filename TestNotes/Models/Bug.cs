using System;

namespace TestNotes.Models
{
    [Serializable]
    public class Bug
    {
        public string Ref { get; set; }
        public string Summary { get; set; }

        public override string ToString()
        {
            return Ref + " : " + Summary;
        }
    }
}
