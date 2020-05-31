using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Models
{
    class Field : INotifyPropertyChanged
    {
        private int _column;
        private int _row;
        private int _square;
        private int _index;
        private bool _writable;
        public int Column { get => _column; set { _column = value; NotifyPropertyChanged(); } }
        public int Row { get => _row; set { _row = value; NotifyPropertyChanged(); } }
        public int Square { get => _square; set { _square = value; NotifyPropertyChanged(); } }
        public int Index { get => _index; set { _index = value; NotifyPropertyChanged(); } }
        public bool Writable { get => _writable; set { _writable = value; NotifyPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
