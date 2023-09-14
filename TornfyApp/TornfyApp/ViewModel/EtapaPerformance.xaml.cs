using System;
using System.Collections.Generic;
using System.Linq;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class EtapaPerformance : ContentPage
    {

        public Jogador info_jogador;
        public Stats performance;
        public List<Stats> conquistas;

        public int id_jogador;
        public int id_master;
        public int ano;
        public int id_categoria;
        private tb_stats ParentTabbedPage { get; }

        public EtapaPerformance(int id_jogador, int id_master, int id_categoria, tb_stats parentTabbedPage)
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
                var mp = "";
                if (int.Parse(item.melhor_posicao) < 11) { mp = "33-64"; }
                if (int.Parse(item.melhor_posicao) > 10 && int.Parse(item.melhor_posicao) <= 50) { mp = "17-32"; }
                if (int.Parse(item.melhor_posicao) > 50 && int.Parse(item.melhor_posicao) <= 100) { mp = "9-16"; }
                if (int.Parse(item.melhor_posicao) > 100 && int.Parse(item.melhor_posicao) <= 175) { mp = "5-8"; }
                if (int.Parse(item.melhor_posicao) > 175 && int.Parse(item.melhor_posicao) <= 250) { mp = "3-4"; }
                if (int.Parse(item.melhor_posicao) == 350) { mp = "2"; }
                if (int.Parse(item.melhor_posicao) == 500) { mp = "1"; }

                item.qtd_cat_string = "Qtd/Cat: " + item.total_etapa + " / " + item.total_cat;
                item.melhor_posicao_string = "Melhor Posição: " + mp;
                item.sets_string = "Set's: " + item.SomaSetGanhos + "/" + item.sets_etapa;
                item.games_string = "Games: " + item.SomaGamesGanhos + "/" + item.games_etapa;
                item.vitder_string = "Vit/Der: " + item.vit_etapa + "/" + item.der_etapa;
                item.performance_string = "Performance: " + item.performance.ToString() + "%";
            }

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

