using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Models
{
    public class Field : INotifyPropertyChanged
    {
        private int _value;
        private bool _writable;
        public int Value { get => _value; set { _value = value; NotifyPropertyChanged(); } }
        public bool Writable { get => _writable; set { _writable = value; NotifyPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
