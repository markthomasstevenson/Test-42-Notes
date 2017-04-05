using System;

namespace TestNotes.Models
{
    [Serializable]
    public class QuestionAnswer
    {
        public string Question { get; set; }
        public string Answer { get; set; }

        public override string ToString()
        {
            return "'" + Question + "' -> '" + (string.IsNullOrWhiteSpace(Answer) ? "No Answer" : Answer) + "'";
        }
    }
}
