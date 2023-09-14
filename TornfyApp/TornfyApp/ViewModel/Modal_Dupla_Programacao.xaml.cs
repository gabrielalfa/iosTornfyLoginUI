using System;
using System.Collections.Generic;
using System.Linq;
using Rg.Plugins.Popup.Pages;
using TornfyApp.Model;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class Modal_Dupla_Programacao : PopupPage
    {
        public Modal_Dupla_Programacao(Programacao jogador)
        {
            InitializeComponent();

            List<Jogador> jogadores = new List<Jogador>();


            if (jogador.tipo_simples)
            {
                Jogador jogador1 = new Jogador
                {
                    path_1 = jogador.img_path_1,
                    nome_j1 = jogador.Nome_Jogador1,
                    id_j1 = jogador.id_j1,
                };

                jogadores.Add(jogador1);

                Jogador jogador2 = new Jogador
                {
                    path_1 = jogador.img_path_2,
                    nome_j1 = jogador.Nome_Jogador2,
                    id_j1 = jogador.id_j2,
                };
                jogadores.Add(jogador2);

                lista_jogadores.HeightRequest = 180;
            }
            else
            {
                Jogador jogador1 = new Jogador
                {
                    path_1 = jogador.img_path_1,
                    nome_j1 = jogador.Nome_Jogador1,
                    id_j1 = jogador.id_j1,
                };

                jogadores.Add(jogador1);

                Jogador jogador2 = new Jogador
                {
                    path_1 = jogador.img_path_2,
                    nome_j1 = jogador.Nome_Jogador2,
                    id_j1 = jogador.id_j2,
                };
                jogadores.Add(jogador2);


                Jogador jogador3 = new Jogador
                {
                    path_1 = jogador.img_path_3,
                    nome_j1 = jogador.Nome_Jogador3,
                    id_j1 = jogador.id_j3,
                };

                jogadores.Add(jogador3);

                Jogador jogador4 = new Jogador
                {
                    path_1 = jogador.img_path_4,
                    nome_j1 = jogador.Nome_Jogador4,
                    id_j1 = jogador.id_j4,
                };
                jogadores.Add(jogador4);

                lista_jogadores.HeightRequest = 320;
            }

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

