using mf.DoToo.Models;
using mf.DoToo.Repositories;
using mf.DoToo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace mf.DoToo.ViewModels
{
    public class MainViewModel:BaseViewModel
    {
        private readonly TodoItemRepository repository;
        public bool ShowAll { get; set; }
        public string FilterText => ShowAll ? "All" : "Active";
        public ObservableCollection<TodoItemViewModel> Items { get; set; }

        public MainViewModel(TodoItemRepository repository)
        {
            // mi aggancio all'evento onItemAdded e quando lo ricevo aggiungo un elemento nella lista
            repository.OnItemAdded += (sender, item) => Items.Add(CreateTodoItemViewModel(item));
            // mi aggancio all'evento onItemUpdate e in questo caso, quando un elemento è aggiornato
            // procedo a ricaricare la lista.
            repository.OnItemUpdate+=(sender, item)=> Task.Run(async()=> await LoadData());
            // mi aggancio all'evento remove del repository , quando scatta l'evento procedo ad eseguire loaddata
            repository.OnItemRemove += (sender, item) => Task.Run(async () => await LoadData());


            this.repository = repository;
            Task.Run(async () => await LoadData());
        }

        private async Task LoadData()
        {
            // estraggo dal repository la lista di tipo TodoItem
            List<TodoItem> items = await repository.GetItems();
            if (!ShowAll)
            {
                items=items.Where(x=>x.Completed==false).ToList();
            }
            // per ogni entita della lista converto l'oggetto in una lista di tipoo TodoItemViewModel
            List< TodoItemViewModel> itemsViewModels = items.Select(i => CreateTodoItemViewModel(i)).ToList();
            // assegno la lista ViewMododelItem all'oggeto ObservableCollection.
            Items = new ObservableCollection<TodoItemViewModel>(itemsViewModels );
        }

        /// <summary>
        /// Questa procedura converte un oggetto di dominio TodoItem in TodoItemViewModel
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private TodoItemViewModel CreateTodoItemViewModel(TodoItem item)
        {
            var itemViewModel = new TodoItemViewModel(item);     
            // l'oggetto TodoItemViewModel si aggancia a due eventi , cambio di stato e elemento rimosso
            itemViewModel.ItemStatusChanged += ItemStatusChanged;            
            itemViewModel.ItemRemoved += ItemRemoved;
            return itemViewModel;
        }

        private void ItemStatusChanged(object sender, EventArgs e)
        {
            if (sender is TodoItemViewModel item)
            {
                if (!ShowAll && item.Item.Completed)
                {
                    Items.Remove(item);
                }
                Task.Run(async () => await repository.UpdateItem(item.Item));
            }
        }

        private void ItemRemoved(object sender, EventArgs e)
        {
            if (sender is TodoItemViewModel item)
            {
                Task.Run(async () => await repository.RemoveItem(item.Item));
            }
        }

       

        public TodoItemViewModel SelectedItem
        {
            get
            {
                return null;
            }
            set
            {
                Device.BeginInvokeOnMainThread(async () => await NavigateToItem(value));
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        private async Task NavigateToItem(TodoItemViewModel item)
        {
            if (item == null)
                return;

            ItemView itemView = Resolver.Resolve<ItemView>();
            ItemViewModel vm = (ItemViewModel)itemView.BindingContext;
            vm.Item = item.Item;
            await Navigation.PushAsync(itemView);

        }


        #region "Command"
        /// <summary>
        /// Comando di aggiunta nuovo elemento
        /// </summary>
        public ICommand AddItem => new Command(async () =>
        {
            // tramite DI recupero istanza della pagina di tipo ItemView
            var itemView = Resolver.Resolve<ItemView>();
            // navigo ....
            await Navigation.PushAsync(itemView);
        });

        public ICommand ToggleFilter => new Command(async () =>
        {
            ShowAll = !ShowAll;
            await LoadData();
        });

      
      
        #endregion
    }

}
