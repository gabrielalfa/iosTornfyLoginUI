using Acr.UserDialogs.Extended;
using API_Inteleco.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Lista_JogadoresTorneio : ContentPage
    {
        public List<Jogador> Lista_jogadores_inscritos;
        public List<Categoria> Lista_Categorias;
        //public int id_etapa = 0;
        Etapa etp = new Etapa();
        public int id_etapa;
        public bool tipo_simples;
        public int id_jogador;
        public int id_master;

        public Lista_JogadoresTorneio(int id_etapa, int id_jogador, int id_master)
        {
            InitializeComponent();
            this.id_etapa = id_etapa;
            this.id_jogador = id_jogador;
            this.id_master = id_master;

        }

        protected override void OnAppearing()
        {
            CarregarCategoriasEtapa(id_etapa);
        }



        public async void CarregarJogadoresEtapa(int id_cat, int id_etapa)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");

            Lista_jogadores_inscritos = await API_Service.ObterJogadoresEtapa(id_etapa, id_cat);

            if (tipo_simples)
            {
                lista_jogadores.IsVisible = true;
                lista_jogadores_duplas.IsVisible = false;

                if (Lista_jogadores_inscritos.Count == 0)
                {
                    lista_jogadores.ItemsSource = Lista_jogadores_inscritos.ToList();
                    txt_subtitle.Text = "Categoria ainda não tem inscritos";
                    await DisplayAlert("Aviso", "Categoria ainda não tem inscritos", "OK");
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    txt_subtitle.Text = "Lista de Jogadores por categoria";
                    lista_jogadores.ItemsSource = Lista_jogadores_inscritos.ToList();
                    UserDialogs.Instance.HideLoading();
                }

            }
            else
            {
                lista_jogadores.IsVisible = false;
                lista_jogadores_duplas.IsVisible = true;


                foreach (var item in Lista_jogadores_inscritos)
                {
                    string[] clube_string = item.nome_clubes_dupla.Split('-');

                    item.nome_j1 = item.nome_j1 + " - " + clube_string[0];
                    item.nome_j2 = item.nome_j2 + " - " + clube_string[1];
                }


                if (Lista_jogadores_inscritos.Count == 0)
                {
                    lista_jogadores_duplas.ItemsSource = Lista_jogadores_inscritos.ToList();
                    txt_subtitle.Text = "Categoria ainda não tem inscritos";
                    await DisplayAlert("Aviso", "Categoria ainda não tem inscritos", "OK");
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    txt_subtitle.Text = "Lista de Jogadores por categoria";
                    lista_jogadores_duplas.ItemsSource = Lista_jogadores_inscritos.ToList();
                    UserDialogs.Instance.HideLoading();
                }
            }



        }

        public async void CarregarCategoriasEtapa(int id_etapa)
        {
            Lista_Categorias = await API_Service.ObterCategoriaEtapa(id_etapa);
            pcCategoria.ItemsSource = Lista_Categorias.OrderBy(x => x.Categoria_Nome).ToList();
        }



        private void pcCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {

            Categoria categoria = ((Picker)sender).SelectedItem as Categoria;
            if (categoria == null)
            {
                return;
            }

            tipo_simples = categoria.tipo_simples;

            CarregarJogadoresEtapa(categoria.id, categoria.id_etapa);
        }

        private void lista_jogadores_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void lista_jogadores_duplas_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Jogador jogador = ((ListView)sender).SelectedItem as Jogador;
            if (jogador == null)
            {
                return;
            }

            await PopupNavigation.Instance.PushAsync(new Modal_Dupla(jogador));

        }

        private async void lista_jogadores_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Jogador jogador = ((ListView)sender).SelectedItem as Jogador;
            if (jogador == null)
            {
                return;
            }

            //await Navigation.PushAsync(new Estatisticas(jogador.id_Jogador, 1));
            //await Navigation.PushAsync(new PerfilModeloTeste());

            await Navigation.PushAsync(new tb_stats(jogador.id_Jogador, id_master));


        }
    }
}