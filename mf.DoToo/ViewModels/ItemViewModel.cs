using mf.DoToo.Models;
using mf.DoToo.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace mf.DoToo.ViewModels
{
    /// <summary>
    /// Questo viewModel rappresenta l'elemento todoItem che può essere utilizzato
    /// per creare un nuovo elemento o per modificare uno esistente.
    /// </summary>
    public class ItemViewModel:BaseViewModel
    {
        private TodoItemRepository repository;
        public TodoItem Item { get; set; }
        public ItemViewModel(TodoItemRepository repository)
        {
            this.repository = repository;
            Item = new TodoItem()
            {
                Due = DateTime.Now.AddDays(1)
            };
        }


        public ICommand Save => new Command(async () =>
        {
            await repository.AddOrUpdate(Item);
            await Navigation.PopAsync();
        });
        
        
    }
}
