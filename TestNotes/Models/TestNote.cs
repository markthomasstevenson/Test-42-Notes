using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNotes.Views;

namespace TestNotes.Models
{
    [Serializable]
    public class TestNote
    {
        public TestNote()
        {
            Ref = "";
            Title = "";
            Assumptions = new ObservableCollection<Assumption>();
            Observations = new ObservableCollection<Observation>();
            Questions = new ObservableCollection<QuestionAnswer>();
            Bugs = new ObservableCollection<Bug>();
            RegressionScenarios = new ObservableCollection<RegressionScenario>();
        }
        public string Ref { get; set; }
        public string Title { get; set; }
        public ObservableCollection<Assumption> Assumptions { get; set; }
        public ObservableCollection<Observation> Observations { get; set; }
        public ObservableCollection<QuestionAnswer> Questions { get; set; }
        public ObservableCollection<Bug> Bugs { get; set; }
        public ObservableCollection<RegressionScenario> RegressionScenarios { get; set; }
    }
}
