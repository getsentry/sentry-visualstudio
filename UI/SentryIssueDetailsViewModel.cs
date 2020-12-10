using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VSSentry.Shared;
using VSSentry.Shared.CommandParameters;
using VSSentry.Shared.Server;
using VSSentry.Shared.Server.Data;

namespace VSSentry.UI
{
    public class SentryIssueDetailsViewModel : INotifyPropertyChanged
    {
        private bool _initialized;
        private bool _isLoading;
        private bool _isLoadingEvent;
        private bool _isEventListLoading;
        private bool _isError;
        private SentryEvent _sentryEvent;
        private Shared.Server.Data.SentryIssueDetails _sentryIssue;
        private string _errorMessage;
        private SentryEventListItem[] eventList;
        private SentryEventListItem _selectedEvent;

        public Shared.Server.Data.SentryIssueDetails SentryIssue
        {
            get { return _sentryIssue; }
            set
            {
                _sentryIssue = value;
                NotifyPropertyChanged();
            }
        }
        public SentryEvent SentryEvent
        {
            get { return _sentryEvent; }
            set
            {
                _sentryEvent = value;
                NotifyPropertyChanged();
            }
        }
        public bool Initialized
        {
            get { return _initialized; }
            set
            {
                _initialized = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(LoaderVisible));
                NotifyPropertyChanged(nameof(LoadingVisible));
                NotifyPropertyChanged(nameof(ContentVisible));
                NotifyPropertyChanged(nameof(LoaderTooltipVisible));
            }
        }
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(LoadingVisible));
                NotifyPropertyChanged(nameof(ContentVisible));
            }
        }
        public bool IsLoadingEvent
        {
            get { return _isLoadingEvent; }
            set
            {
                _isLoadingEvent = value;
                NotifyPropertyChanged();
            }
        }
        public bool IsEventListLoading
        {
            get { return _isEventListLoading; }
            set
            {
                _isEventListLoading = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(EventListLoadingVisible));
                NotifyPropertyChanged(nameof(EventListVisible));
            }
        }
        public bool IsError
        {
            get { return _isError; }
            set
            {
                _isError = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ContentVisible));
                NotifyPropertyChanged(nameof(ErrorVisible));
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyPropertyChanged();
            }
        }

        public SentryEventListItem SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                NotifyPropertyChanged();
                if (_selectedEvent != null && SentryEvent != null && _selectedEvent.eventID != SentryEvent.eventID)
                {
                    _selectedEvent.OpenIssueCommand?.Execute(null);
                }
            }
        }

        public SentryIssueDetailsViewModel()
        {
            IsLoading = true;
        }
        
        public Visibility LoaderVisible => Initialized ? Visibility.Visible : Visibility.Collapsed;
        public Visibility LoaderTooltipVisible => !Initialized ? Visibility.Visible : Visibility.Collapsed;
        public Visibility LoadingVisible => IsLoading ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ContentVisible => !Initialized || IsLoading || IsError ? Visibility.Collapsed : Visibility.Visible;
        public Visibility ErrorVisible => IsError ? Visibility.Visible : Visibility.Collapsed;
        public Visibility EventListLoadingVisible => IsEventListLoading ? Visibility.Visible : Visibility.Collapsed;
        public Visibility EventListVisible => IsEventListLoading ? Visibility.Collapsed : Visibility.Visible;

        public SentryEventListItem[] EventList
        {
            get { return eventList; }
            set
            {
                eventList = value;
                NotifyPropertyChanged();
            }
        }

        public SentryConnection Connection { get; internal set; }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void LoadData(Shared.Server.Data.SentryIssueDetails sentryIssue, SentryEvent sentryEvent)
        {
            SentryIssue = sentryIssue;
            SentryEvent = sentryEvent;
            IsLoading = false;
        }

        internal void LoadError(string message)
        {
            ErrorMessage = message;
            IsError = true;
            IsLoading = false;
        }

        internal void LoadEventList(SentryEventListItem[] eventList)
        {
            foreach(var evt in eventList)
            {
                evt.OpenIssueCommand = new DelegateCommand((_) => OpenEvent(evt.eventID));
            }
            EventList = eventList;
            SelectedEvent = eventList.FirstOrDefault(x => x.eventID == SentryEvent.eventID);
            IsEventListLoading = false;
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void OpenEvent(string eventId)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            try
            {
                IsLoadingEvent = true;
                SentryEvent = await SentryIssue.Connection.GetIssueEventAsync(SentryIssue, eventId);
                IsLoadingEvent = false;
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                IsLoadingEvent = false;
                IsError = true;
            }
        }

        internal async Task LoadDataAsync(IssueReference arg)
        {
            Initialized = true;
            IsLoading = true;
            try
            {
                var issue = await Connection.GetIssueDetailsAsync(arg.IssueId).ConfigureAwait(false);
                var sentryEvent = await Connection.GetIssueLatestEventAsync(arg.IssueId).ConfigureAwait(false);
                LoadData(issue, sentryEvent);
                IsEventListLoading = true;
                var eventList = await Connection.GetIssueEventsAsync(arg.IssueId);
                LoadEventList(eventList);
            }
            catch(Exception ex)
            {
                LoadError(ex.Message);
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
