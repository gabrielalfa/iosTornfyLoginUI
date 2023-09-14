using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class Modal_Categoria : PopupPage
    {
        List<Inscricao> categorias;

        public int id_jogador;
        public int id_master;
        public int ano;
        public int id_categoria;

        public Modal_Categoria(int id_jogador, int ano, int id_master)
        {
            InitializeComponent();

            this.ano = ano;
            this.id_jogador = id_jogador;
            this.id_master = id_master;
        }

        protected async override void OnAppearing()
        {
            categorias = await API_Service.ObterInscricoes_Temporada(id_jogador, ano, id_master);
            lista_categorias.ItemsSource = categorias;
        }

        private void lista_categorias_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        public event EventHandler<CategoriaSelecionada> ItemSelecionado;

        //private async void lista_categorias_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //	Inscricao cat_sel = ((ListView)sender).SelectedItem as Inscricao;
        //	if (cat_sel != null)
        //	{
        //		ItemSelecionado?.Invoke(this, cat_sel.id);
        //		await Navigation.PopAsync();
        //	}
        //}

        private async void lista_categorias_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Inscricao cat_sel = ((ListView)sender).SelectedItem as Inscricao;
            if (cat_sel != null)
            {
                CategoriaSelecionada categoriaSelecionada = new CategoriaSelecionada
                {
                    IdCategoria = cat_sel.id,
                    NomeCategoria = cat_sel.Categoria // Supondo que o nome da categoria está em uma propriedade chamada "nome"
                };

                ItemSelecionado?.Invoke(this, categoriaSelecionada);
                await Navigation.PopAsync();
            }
        }

        private async void OnCloseButtonClicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }

    public class CategoriaSelecionada
    {
        public int IdCategoria { get; set; }
        public string NomeCategoria { get; set; }
    }
}

