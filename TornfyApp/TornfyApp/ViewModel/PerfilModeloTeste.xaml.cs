using System;
using System.Collections.Generic;
using System.Net;
using Acr.UserDialogs.Extended;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class PerfilModeloTeste : ContentPage
    {

        public Jogador info_jogador;
        public Stats performance;
        public List<Stats> table_performance_etapas;

        public int id_jogador;
        public int id_master;
        public int ano;
        public int id_categoria;
        private tb_stats ParentTabbedPage { get; }

        public PerfilModeloTeste(int id_jogador, int id_master,
            int id_categoria_geral, int ano_geral,
            tb_stats parentTabbedPage)
        {
            InitializeComponent();

            if (ano_geral == 0)
            {
                ano = DateTime.Now.Year; ano_geral = DateTime.Now.Year;
            }
            else
            {
                ano = ano_geral;
            }

            this.id_jogador = id_jogador;
            this.id_master = id_master;
            this.ano = ano;
            this.id_categoria = id_categoria;   
            this.ParentTabbedPage = parentTabbedPage;



            pc_anos.Text = ano.ToString();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            id_jogador = ParentTabbedPage.id_jogador_geral;

            CarregarDadosJogador(id_jogador);
            CarregarPerformance(id_jogador, id_master);

        }

        public async void CarregarPerformance(int id_jogador, int id_master)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");
            performance = await API_Service.ObterPerformanceNovo(id_jogador, id_master, ano, id_categoria);

            if (performance != null)
            {
                if (id_categoria == 0)
                {
                    ParentTabbedPage.id_categoria_geral = performance.id_categoria;
                    pc_torneio.Text = "Categoria: " + performance.nome_categoria;
                }

                int totalPartidas = performance.SomaVitorias + performance.SomaDerrotas;
                int total_Games = performance.SomaGamesGanhos + performance.SomaGamesPerdidos;
                int total_Sets = performance.SomaSetGanhos + performance.SomaSetPerdidos;

                ranking_elo.Text = "Rating Elo: " + performance.rating_elo;


                try
                {
                    if (performance.SomaGamesGanhos == 0)
                    {
                        game_performance.Text = "0";
                        lbl_somagames_ganhos.Text = string.Format("Performance em Games ({0})", "0%");
                    }
                    else
                    {
                        game_performance.Text = "Games Win/Los: " + performance.SomaGamesGanhos.ToString() + "/" + performance.SomaGamesPerdidos.ToString();
                        int per_games = (performance.SomaGamesGanhos * 100) / total_Games;
                        lbl_somagames_ganhos.Text = string.Format("Performance em Games ({0}%)", per_games);
                    }
                }
                catch (Exception)
                {
                    game_performance.Text = "0";
                    lbl_somagames_ganhos.Text = string.Format("Performance em Games ({0})", "0%");
                }

                try
                {
                    if (performance.SomaSetGanhos == 0)
                    {
                        set_performance.Text = "0";
                        lbl_somagsets_ganhos.Text = string.Format("Performance em Sets ({0})", "0%");
                    }
                    else
                    {
                        set_performance.Text = "Set´s Win/Los: " + performance.SomaSetGanhos.ToString() + "/" + performance.SomaSetPerdidos.ToString();
                        int per_sets = (performance.SomaSetGanhos * 100) / total_Sets;
                        lbl_somagsets_ganhos.Text = string.Format("Performance em Sets ({0}%)", per_sets);
                    }
                }
                catch (Exception)
                {
                    set_performance.Text = "0";
                    lbl_somagsets_ganhos.Text = string.Format("Performance em Sets ({0})", "0%");
                }

                try
                {
                    if (performance.SomaVitorias == 0)
                    {
                        vit_performance.Text = "0";
                        lbl_vitorias.Text = string.Format("Performance em Partidas Vencidas ({0})", "0%");
                    }
                    else
                    {
                        vit_performance.Text = "Vitórias: " + performance.SomaVitorias.ToString();
                        int per_vit = (performance.SomaVitorias * 100) / totalPartidas;
                        lbl_vitorias.Text = string.Format("Performance em Vitórias ({0}%)", per_vit);
                    }
                }
                catch (Exception)
                {
                    vit_performance.Text = "0";
                    lbl_vitorias.Text = string.Format("Performance em Partidas Vencidas ({0})", "0%");
                }

                try
                {
                    if (performance.SomaDerrotas == 0)
                    {
                        der_performance.Text = "0";
                        lbl_derrotas.Text = string.Format("Performance em Partidas Perdidas ({0})", "0%");
                    }
                    else
                    {
                        der_performance.Text = "Derrotas: " + performance.SomaDerrotas.ToString();
                        int per_der = (performance.SomaDerrotas * 100) / totalPartidas;
                        lbl_derrotas.Text = string.Format("Performance em Derrotas ({0}%)", per_der);
                    }
                }
                catch (Exception)
                {
                    der_performance.Text = "0";
                    lbl_derrotas.Text = string.Format("Performance em Partidas Perdidas ({0})", "0%");
                }

                games_win.Text = performance.SomaGamesGanhos.ToString();
                sets_win.Text = performance.SomaSetGanhos.ToString();
                vitorias.Text = performance.SomaVitorias.ToString();
                derrotas.Text = performance.SomaDerrotas.ToString();


            }
            else
            {
                //stk_stats.IsVisible = false;
                //stk_semregistro.IsVisible = true;
                await DisplayAlert("Performance Sem Registros", "Este perfil ainda não apresenta numeros suficientes para mostragem de performance", "Ok");
            }

            UserDialogs.Instance.HideLoading();
        }

        public async void CarregarDadosJogador(int id_jogador)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");

            info_jogador = await API_Service.ObterDadosJogador(id_jogador);

            txt_nome.Text = info_jogador.Nome_Jogador;
            ranking_elo.Text = "Rating Elo: " + info_jogador.rating_elo;
            ParentTabbedPage.elo_geral = info_jogador.rating_elo;
            ParentTabbedPage.Nome_Jogador_geral = info_jogador.Nome_Jogador;

            string url_teste = info_jogador.Path;

            if (!string.IsNullOrEmpty(url_teste))
            {
                HttpWebResponse response = null;
                var request = (HttpWebRequest)WebRequest.Create(url_teste);
                request.Method = "HEAD";
                try { response = (HttpWebResponse)request.GetResponse(); image_jogador.Source = url_teste; ParentTabbedPage.img_jogador_geral = url_teste; }
                catch (WebException ex) { image_jogador.Source = "user_round.png"; ParentTabbedPage.img_jogador_geral = "user_round.png"; }
                finally { if (response != null) { response.Close(); } }
            }
            else { image_jogador.Source = "user_round.png"; ParentTabbedPage.img_jogador_geral = "user_round.png"; }



            UserDialogs.Instance.HideLoading();

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var modal_CategoriaPage = new Modal_Categoria(id_jogador, ano, id_master);

            // Defina um manipulador de eventos para o evento ItemSelecionado
            modal_CategoriaPage.ItemSelecionado += Categoria_ItemSelecionado;

            // Chame a página Rateio_Parceiros
            await Navigation.PushAsync(modal_CategoriaPage);
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            var modal_anosPage = new Modal_Anos();

            // Defina um manipulador de eventos para o evento ItemSelecionado
            modal_anosPage.ItemSelecionado += Ano_ItemSelecionado;

            // Chame a página Rateio_Parceiros
            await Navigation.PushAsync(modal_anosPage);

        }


        private void Ano_ItemSelecionado(object sender, int ano)
        {
            this.ano = ano;

            ParentTabbedPage.ano_geral = ano;
            CarregarPerformance(id_jogador, id_master);

            pc_anos.Text = ano.ToString();
        }

      
        private void Categoria_ItemSelecionado(object sender, CategoriaSelecionada categoriaSelecionada)
        {
            this.id_categoria = categoriaSelecionada.IdCategoria;
            ParentTabbedPage.id_categoria_geral = id_categoria;
            CarregarPerformance(id_jogador, id_master);
            pc_torneio.Text = "Categoria: " + categoriaSelecionada.NomeCategoria;



        }

    }
}

