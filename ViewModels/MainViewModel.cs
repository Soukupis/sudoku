using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private List<List<Field>> _fields = new List<List<Field>>();

        public MainViewModel()
        {
            Fields = new List<List<Field>>();

        }

        public List<List<Field>> Fields { get => _fields; set { _fields = value; NotifyPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
