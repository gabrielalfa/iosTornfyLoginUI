using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Programacao_Tela : ContentPage
    {

        public List<Programacao> Lista_Jogos;
        public string nome_etapa;
        public string nome_jogador;
        public int id_etapa;

        public Programacao_Tela(int _id_etapa, string _nome_etapa, string _nome_jogador)
        {
            InitializeComponent();

            nome_etapa = _nome_etapa;
            id_etapa = _id_etapa;
            nome_jogador = _nome_jogador;


            txt_busca.Text = _nome_jogador.ToString();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (TestarInternet()) CarregarEtapasPonrtuadas(nome_jogador); else Navigation.PushAsync(new NoInternet());
        }

        public static bool TestarInternet()
        {
            bool coneccao = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                coneccao = false;
            }
            return coneccao;
        }

        public async void CarregarEtapasPonrtuadas(string busca)
        {
            Lista_Jogos = await API_Service.ObterJogosTela(id_etapa, busca, true);
            listagem_torneios.ItemsSource = Lista_Jogos.ToList();

            if (Lista_Jogos.Count == 0)
            {
                no_data.IsVisible = true;
                listagem_torneios.IsVisible = false;
            }
            else
            {
                no_data.IsVisible = false;
                listagem_torneios.IsVisible = true;
            }

        }

        private async void txt_busca_SearchButtonPressed(object sender, EventArgs e)
        {
            Lista_Jogos = await API_Service.ObterJogosTela(id_etapa, txt_busca.Text, true);
            listagem_torneios.ItemsSource = Lista_Jogos.ToList();

            if (Lista_Jogos.Count == 0)
            {
                no_data.IsVisible = true;
                listagem_torneios.IsVisible = false;
            }
            else
            {
                no_data.IsVisible = false;
                listagem_torneios.IsVisible = true;
            }

        }

        private void listagem_torneios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {

        }

        private async void listagem_torneios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Programacao jogador = ((ListView)sender).SelectedItem as Programacao;
            if (jogador == null)
            {
                return;
            }

            await PopupNavigation.Instance.PushAsync(new Modal_Dupla_Programacao(jogador));
        }

        private async void ToolbarItem_Clicked_2(object sender, EventArgs e)
        {
            Lista_Jogos = await API_Service.ObterJogosTela(id_etapa, "", true);
            listagem_torneios.ItemsSource = Lista_Jogos.ToList();

            if (Lista_Jogos.Count == 0)
            {
                no_data.IsVisible = true;
                listagem_torneios.IsVisible = false;
            }
            else
            {
                no_data.IsVisible = false;
                listagem_torneios.IsVisible = true;
            }
        }
    }
}