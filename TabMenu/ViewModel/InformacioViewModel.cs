using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BancDelTemps.Model.Class;

namespace BancDelTemps.ViewModel
{
    class InformacioViewModel
    {

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
        #region RelayCommand
        class RelayCommand : ICommand
        {
            private Action<object> _action;

            public RelayCommand(Action<object> action)
            {
                _action = action;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (parameter != null)
                {
                    _action(parameter);
                }
                else
                {
                    _action("Hello world");
                }
            }

            public event EventHandler CanExecuteChanged;
        }
        #endregion

        public ICommand ButtonCloseApp { get; set; }

        Object obj = new Object();
        List<ObjectInfo> llista = new List<ObjectInfo>();
        public InformacioViewModel()
        {
        }

        public InformacioViewModel(Object ob)
        {
            ButtonCloseApp = new InformacioViewModel.RelayCommand(o => Application.Current.Windows[2].Close());
            obj = ob;
            fillDgv();
        }

        private List<ObjectInfo> _objects;
        public List<ObjectInfo> Objects
        {
            get { return _objects; }
            set { _objects = value; NotifyPropertyChanged(); }
        }

        public void fillDgv()
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (var p in properties)
            {
                if (p.PropertyType.ToString().Contains("ICollection") || p.Name == "password") continue;
                ObjectInfo obi = new ObjectInfo(p.Name, p.GetValue(obj).ToString());
                llista.Add(obi);

            }

            Objects = llista;
        }
    }

    public class ObjectInfo
    {
        public string Atribut { get; set; }
        public string Valor { get; set; }
        
        public ObjectInfo(string atribut, string valor)
        {
            Atribut = atribut;
            Valor = valor;
        }
    }
}
