using System;
using System.ComponentModel;
using System.Windows.Input;
using IRMMAUI.Entity;

namespace IRMMAUI.ViewModels
{
    public class DataViewModel : INotifyPropertyChanged
    {

        public DataViewModel()
        {
            CancelEditCommand = new Command(CmdCancelEdit);
            EditCommand = new Command<TableItem>(CmdEdit);
            RefreshCommand = new Command(CmdRefresh);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private List<TableItem> _items { get; set; }

        private TableItem _teamToEdit;
        private TableItem _selectedItem;
        private bool _isRefreshing;
        private bool _teamColumnVisible = true;
        private bool _wonColumnVisible = true;
        private bool _headerBordersVisible = true;
        private bool _paginationEnabled = true;
        private ushort _teamColumnWidth = 70;

        public TableItem TeamToEdit
        {
            get => _teamToEdit;
            set
            {
                _teamToEdit = value;
                OnPropertyChanged(nameof(TeamToEdit));
            }
        }

        public List<TableItem> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public bool HeaderBordersVisible
        {
            get => _headerBordersVisible;
            set
            {
                _headerBordersVisible = value;
                OnPropertyChanged(nameof(HeaderBordersVisible));
            }
        }

        public bool TeamColumnVisible
        {
            get => _teamColumnVisible;
            set
            {
                _teamColumnVisible = value;
                OnPropertyChanged(nameof(TeamColumnVisible));
            }
        }

        public bool WonColumnVisible
        {
            get => _wonColumnVisible;
            set
            {
                _wonColumnVisible = value;
                OnPropertyChanged(nameof(WonColumnVisible));
            }
        }

        public ushort TeamColumnWidth
        {
            get => _teamColumnWidth;
            set
            {
                _teamColumnWidth = value;
                OnPropertyChanged(nameof(TeamColumnWidth));
            }
        }

        public bool PaginationEnabled
        {
            get => _paginationEnabled;
            set
            {
                _paginationEnabled = value;
                OnPropertyChanged(nameof(PaginationEnabled));
            }
        }

        public TableItem SelectedTeam
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand CancelEditCommand { get; }

        public ICommand EditCommand { get; }

        public ICommand RefreshCommand { get; }

        private void CmdCancelEdit()
        {
            TeamToEdit = null;
        }

        private void CmdEdit(TableItem teamToEdit)
        {
            ArgumentNullException.ThrowIfNull(teamToEdit);

            TeamToEdit = teamToEdit;
        }

        private async void CmdRefresh()
        {
            IsRefreshing = true;
            // wait 3 secs for demo
            await Task.Delay(3000);
            IsRefreshing = false;
        }

        private void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

    }
}

