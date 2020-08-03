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
        private int _square;
        private bool _writable;

        public int Value
        {
            get => _value;
            set
            {
                if ((value >= 0) && (value <= 9))
                {
                    _value = value;
                }
                else
                {
                    throw new System.ArgumentException("Číslo má špatné rozmezí");
                }
                NotifyPropertyChanged(); 
            }
        }
        public int Square
        {
            get => _square;
            set
            {
                if ((value >= 0) && (value <= 8))
                {
                    _square = value;
                }
                else
                {
                    throw new System.ArgumentException("Číslo má špatné rozmezí");
                }
                NotifyPropertyChanged();
            }
        }
        public bool Writable { get => _writable; set { _writable = value; NotifyPropertyChanged(); } }

        

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
