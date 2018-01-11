﻿using System.ComponentModel;

namespace LteDev
{
    public class UriQueryParam : INotifyPropertyChanged
    {
        private int _order = 0;
        private string _key = "";
        private string _value = "";
        private bool _hasValue = false;

        public event PropertyChangedEventHandler PropertyChanged;

        [DataObjectField(true, false, false)]
        public int Order
        {
            get { return _order; }
            set
            {
                if (value == _order)
                    return;
                _order = value;
                RaisePropertyChanged("Order");
            }
        }

        [DataObjectField(false, false, false)]
        public string Key
        {
            get { return _key; }
            private set
            {
                string s = (value == null) ? "" : value;
                if (s == _key)
                    return;
                _key = s;
                RaisePropertyChanged("Key");
            }
        }

        [DataObjectField(false, false, false)]
        public string Value
        {
            get { return _value; }
            private set
            {
                string s = (value == null) ? "" : value;
                if (s == _value)
                    return;
                _value = s;
                RaisePropertyChanged("Value");
            }
        }

        [DataObjectField(false, false, false)]
        public bool HasValue
        {
            get { return _hasValue; }
            set
            {
                if (value == _hasValue)
                    return;
                _hasValue = value;
                RaisePropertyChanged("HasValue");
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args) { }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
            try { OnPropertyChanged(args); }
            finally
            {
                PropertyChangedEventHandler propertyChanged = PropertyChanged;
                if (propertyChanged != null)
                    propertyChanged(this, args);
            }
        }

    }
}