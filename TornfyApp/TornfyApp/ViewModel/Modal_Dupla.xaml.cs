using System;
using System.Collections.Generic;
using System.Linq;
using Rg.Plugins.Popup.Pages;
using TornfyApp.Model;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class Modal_Dupla : PopupPage
    {
        Jogador jogador = new Jogador();

        public Modal_Dupla(Jogador jogador)
        {
            InitializeComponent();
            this.jogador = jogador;
            List<Jogador> jogadores = new List<Jogador>();

            Jogador jogador1 = new Jogador
            {
                path_1 = jogador.path_1,
                nome_j1 = jogador.nome_j1,
                id_j1 = jogador.id_Jogador,
            };

            jogadores.Add(jogador1);

            Jogador jogador2 = new Jogador
            {
                path_1 = jogador.path_2,
                nome_j1 = jogador.nome_j2,
                id_j1 = jogador.id_dupla,
            };

            jogadores.Add(jogador2);


            lista_jogadores.ItemsSource = jogadores.ToList();
        }

        private void lista_jogadores_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void lista_jogadores_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Jogador jogador = ((ListView)sender).SelectedItem as Jogador;
            if (jogador == null)
            {
                return;
            }

            await Navigation.PushAsync(new tb_stats(jogador.id_j1, 1));

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}

