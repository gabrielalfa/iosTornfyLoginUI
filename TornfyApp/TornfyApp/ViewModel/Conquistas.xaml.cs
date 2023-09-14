using System;
using System.Collections.Generic;
using System.Linq;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class Conquistas : ContentPage
    {

        public Jogador info_jogador;
        public Stats performance;
        public List<Stats> conquistas;

        public int id_jogador;
        public int id_master;
        public int ano;
        public int id_categoria;
        private tb_stats ParentTabbedPage { get; }

        public Conquistas(int id_jogador, int id_master, int id_categoria, tb_stats parentTabbedPage)
        {
            InitializeComponent();

            this.id_jogador = id_jogador;
            this.id_master = id_master;
            this.id_categoria = id_categoria;
            this.ParentTabbedPage = parentTabbedPage;
            this.ano = parentTabbedPage.ano_geral;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //CarregarDadosJogador(id_jogador);
            CarregarPerformance(ParentTabbedPage.id_jogador_geral, id_master);

            ranking_elo.Text = "Rating Elo: " + ParentTabbedPage.elo_geral;
            txt_nome.Text = ParentTabbedPage.Nome_Jogador_geral;
            image_jogador.Source = ParentTabbedPage.img_jogador_geral;

        }

        public async void CarregarPerformance(int id_jogador, int id_master)
        {
            id_categoria = ParentTabbedPage.id_categoria_geral;

            conquistas = await API_Service.ObterConquistas(id_jogador, id_categoria, ParentTabbedPage.ano_geral, id_master);

            foreach (var item in conquistas)
            {
                switch (int.Parse(item.melhor_posicao))
                {
                    case int n when (n > 175 && n <= 250):
                        item.posicao = "Quarta-Final";
                        item.trofeu = "trofeu_bronze";
                        item.status_visivel = true;
                        break;
                    case 350:
                        item.posicao = "Vice-Campeão";
                        item.trofeu = "trofeu_prata";
                        item.status_visivel = true;
                        break;
                    case 500:
                        item.posicao = "Campeão";
                        item.trofeu = "trofeu_ouro";
                        item.status_visivel = true;
                        break;
                    default:
                        item.status_visivel = false;
                        break;
                }
            }


            conquistas = conquistas.Where(item => item.status_visivel).ToList();

            lista_categorias.ItemsSource = conquistas.ToList();

        }

       
        private void lista_categorias_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void lista_categorias_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}

