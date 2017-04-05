using System;

namespace TestNotes.Models
{
    [Serializable]
    public class Assumption
    {
        public string Content { get; set; }

        public override string ToString()
        {
            return Content;
        }
    }
}
