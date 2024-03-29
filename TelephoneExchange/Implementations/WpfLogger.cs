﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TelephoneExchange.Interfaces;

namespace TelephoneExchange.Implementations
{
    public class WpfLogger : INotifyPropertyChanged, ILogger
    {
        private StringBuilder _textHolder = new StringBuilder();
        private object _syncObject = new object(); 

        public event PropertyChangedEventHandler PropertyChanged;

        private string _logMessage;
        public string LogMessage
        {
            get => _logMessage;
            set
            {
                _logMessage = value;
                NotifyPropertyChanged();
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Log(string message)
        {
            try
            {
                lock (_syncObject)
                {
                    _textHolder.Append(message);
                    _textHolder.AppendLine();
                    LogMessage = _textHolder.ToString();
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }
    }
}
