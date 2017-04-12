using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using TestNotes.Commands;
using TestNotes.IO;
using TestNotes.Models;
using TestNotes.Views;
using Clipboard = System.Windows.Forms.Clipboard;
using TestNote = TestNotes.Models.TestNote;

namespace TestNotes.ViewModels
{
    public class TestNoteViewModel : _BaseViewModel
    {
        private TestNote _testNote = new TestNote();

        public TestNoteViewModel() : base()
        {
            Load();
        }

        private void Load()
        {
            RaisePropertyChangedEvent("Ref");
            RaisePropertyChangedEvent("Title");
            RaisePropertyChangedEvent("Assumptions");
            RaisePropertyChangedEvent("Observations");
            RaisePropertyChangedEvent("Questions");
            RaisePropertyChangedEvent("Bugs");
            RaisePropertyChangedEvent("RegressionScenarios");
        }

        public ICommand SaveCommand => new DelegateCommand(SaveCurrentTestNote, CanSave);
        public ICommand LoadCommand => new DelegateCommand(LoadTestNote, CanLoad);
        public ICommand CopyToClipboardCommand => new DelegateCommand(CopyToClipboard, CanCopy);

        #region Public Parameters
        public string Ref
        {
            get { return _testNote.Ref; }
            set
            {
                if (_testNote.Ref != value)
                {
                    _testNote.Ref = value;
                    RaisePropertyChangedEvent("Ref");
                }
            }
        }
        
        public string Title
        {
            get { return _testNote.Title; }
            set
            {
                if (_testNote.Title != value)
                {
                    _testNote.Title = value;
                    RaisePropertyChangedEvent("Title");
                }
            }
        }

        public ObservableCollection<Assumption> Assumptions
        {
            get { return _testNote.Assumptions; }
            set
            {
                if (_testNote.Assumptions != value)
                {
                    _testNote.Assumptions = value;
                    RaisePropertyChangedEvent("Assumptions");
                }
            }
        }

        public ObservableCollection<Observation> Observations
        {
            get { return _testNote.Observations; }
            set
            {
                if (_testNote.Observations != value)
                {
                    _testNote.Observations = value;
                    RaisePropertyChangedEvent("Observations");
                }
            }
        }

        public ObservableCollection<QuestionAnswer> Questions
        {
            get { return _testNote.Questions; }
            set
            {
                if (_testNote.Questions != value)
                {
                    _testNote.Questions = value;
                    RaisePropertyChangedEvent("Questions");
                }
            }
        }

        public ObservableCollection<Bug> Bugs
        {
            get { return _testNote.Bugs; }
            set
            {
                if (_testNote.Bugs != value)
                {
                    _testNote.Bugs = value;
                    RaisePropertyChangedEvent("Bugs");
                }
            }
        }

        public ObservableCollection<RegressionScenario> RegressionScenarios
        {
            get { return _testNote.RegressionScenarios; }
            set
            {
                if (_testNote.RegressionScenarios != value)
                {
                    _testNote.RegressionScenarios = value;
                    RaisePropertyChangedEvent("RegressionScenarios");
                }
            }
        }
        #endregion

        #region Command Parameters
        private static bool CanSave()
        {
            return true;
        }

        private void SaveCurrentTestNote()
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dialog.Filter = "Test Note File (*.tnote)|*.tnote|All files (*.*)|*.*";
                dialog.FileName = Ref + "_" + Title;
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK || result == DialogResult.Yes)
                {
                    Binary.WriteToBinaryFile(dialog.FileName, _testNote);
                }
            }
        }

        private static bool CanLoad()
        {
            return true;
        }

        private void LoadTestNote()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dialog.Multiselect = false;
                dialog.Filter = "Test Note File (*.tnote)|*.tnote|All files (*.*)|*.*";
                var result = dialog.ShowDialog();
                if (result != DialogResult.OK && result != DialogResult.Yes)
                {
                    return;
                }
                LoadTestNote(dialog.FileName);
            }
        }
        public void LoadTestNote(string filePath)
        {
            _testNote = Binary.ReadFromBinaryFile<TestNote>(filePath);
            Load();
        }

        private static bool CanCopy()
        {
            return true;
        }

        private void CopyToClipboard()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Ref: '" + _testNote.Ref + "'\n\n");
            stringBuilder.Append("Title: '" + _testNote.Title + "'\n\n");
            stringBuilder.Append("Assumptions:\n");
            stringBuilder.Append(" - '" + string.Join("'\n - '", _testNote.Assumptions) + "'\n\n");
            stringBuilder.Append("Observations:\n");
            stringBuilder.Append(" - '" + string.Join("'\n - '", _testNote.Observations) + "'\n\n");
            stringBuilder.Append("Questions -> Answers:\n");
            stringBuilder.Append(" - " + string.Join("\n - ", _testNote.Questions) + "\n\n");
            stringBuilder.Append("Bugs:\n");
            stringBuilder.Append(" - '" + string.Join("'\n - '", _testNote.Bugs) + "'\n\n");
            stringBuilder.Append("Regression Scenarios:\n");
            stringBuilder.Append(" - '" + string.Join("'\n - '", _testNote.RegressionScenarios));

            Clipboard.SetText(stringBuilder.ToString());
        }
        #endregion
    }
}
