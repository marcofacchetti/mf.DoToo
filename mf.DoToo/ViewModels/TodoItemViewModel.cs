using mf.DoToo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace mf.DoToo.ViewModels
{

    /// <summary>
    /// Questa callsse rappresenta  ogni elemento nella lista della Main View.
    /// Non ha una sua view, nonostante potrebbe averla, ma è renderizzata da
    /// un template in una listView. 
    /// </summary>
    public class TodoItemViewModel:BaseViewModel
    {
        public event EventHandler ItemStatusChanged;
        public event EventHandler ItemRemoved;

        public TodoItemViewModel(TodoItem item)
        {
            this.Item = item;
        }

        public TodoItem Item { get; private set; }
        public string StatusText
        {
            get
            {
                return Item.Completed ? "Reactivate" : "Completed";
            }
        }
        


        #region Command
        public ICommand ToggleCompleted => new Command((arg) =>
        {
            Item.Completed = !Item.Completed;
            ItemStatusChanged?.Invoke(this, new EventArgs());
        });

        public ICommand Remove => new Command((arg) =>
        {
            ItemRemoved?.Invoke(this, new EventArgs());
        });
        #endregion


    }
}
