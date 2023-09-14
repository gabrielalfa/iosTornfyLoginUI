using System;
using System.Collections.Generic;
using System.Linq;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class Modal_Anos : PopupPage
    {
        public Modal_Anos()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            List<Ano> anosList = new List<Ano>();
            for (int i = 2015; i < DateTime.Today.Year + 1; i++)
            {
                anosList.Add(new Ano { ano_sel = i });
            }

            lista_anos.ItemsSource = anosList.OrderByDescending(x => x.ano_sel);
        }



        private void lista_anos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        public event EventHandler<int> ItemSelecionado;


        private async void lista_anos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Ano ano_sel = ((ListView)sender).SelectedItem as Ano;
            if (ano_sel != null)
            {
                ItemSelecionado?.Invoke(this, ano_sel.ano_sel);
                await Navigation.PopAsync();
            }
        }
    }

    public class Ano
    {
        public int ano_sel { get; set; }
    }
}

