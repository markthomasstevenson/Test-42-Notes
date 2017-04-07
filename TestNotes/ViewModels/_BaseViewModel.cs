using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TestNotes.Commands;
using TestNotes.Libraries;
using TestNotes.Views;

namespace TestNotes.ViewModels
{
    public class _BaseViewModel : INotifyPropertyChanged
    {
        private const string CancelDockText = "Undock the Window";
        private const string DockToText = "Dock the window to the ";

        private bool _isLeftDocked;
        private bool IsLeftDocked
        {
            get { return _isLeftDocked; }
            set
            {
                if (_isLeftDocked != value)
                {
                    _isLeftDocked = value;
                    RaisePropertyChangedEvent("_isLeftDocked");
                    RaisePropertyChangedEvent("LeftToolTipText");
                }
            }
        }

        private bool _isRightDocked;
        private bool IsRightDocked
        {
            get
            {
                return _isRightDocked;
            }
            set
            {
                if (_isRightDocked != value)
                {
                    _isRightDocked = value;
                    RaisePropertyChangedEvent("_isRightDocked");
                    RaisePropertyChangedEvent("RightToolTipText");
                }
            }
        }

        public IView View;

        protected _BaseViewModel()
        {
            IsLeftDocked = false;
            IsRightDocked = false;
        }

        public ICommand LeftDockCommand => new DelegateCommand(LeftDockClick, CanLeftDock);
        public ICommand RightDockCommand => new DelegateCommand(RightDockClick, CanRightDock);

        public string LeftToolTipText
        {
            get
            {
                if (IsLeftDocked)
                {
                    return CancelDockText;
                }
                return DockToText + "Left";
            }
        }
        public string RightToolTipText
        {
            get
            {
                if (IsRightDocked)
                {
                    return CancelDockText;
                }
                return DockToText + "Right";
            }
        }

        private static bool CanLeftDock()
        {
            return true;
        }

        private void LeftDockClick()
        {
            if (IsLeftDocked)
            {
                AppBarFunctions.SetAppBar((Window) View, ABEdge.None);
                IsLeftDocked = false;
                IsRightDocked = false;
                ((MainWindow) View).LockWindow(false);
            }
            else
            {
                AppBarFunctions.SetAppBar((Window)View, ABEdge.Left);
                IsLeftDocked = true;
                IsRightDocked = false;
                ((MainWindow)View).LockWindow(true);
            }
        }

        private static bool CanRightDock()
        {
            return true;
        }

        private void RightDockClick()
        {
            if (IsRightDocked)
            {
                AppBarFunctions.SetAppBar((Window)View, ABEdge.None);
                IsLeftDocked = false;
                IsRightDocked = false;
                ((MainWindow)View).LockWindow(false);
            }
            else
            {
                AppBarFunctions.SetAppBar((Window)View, ABEdge.Right);
                IsRightDocked = true;
                IsLeftDocked = false;
                ((MainWindow)View).LockWindow(true);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
