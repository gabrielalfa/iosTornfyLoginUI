using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Acr.UserDialogs.Extended;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class Oponentes : ContentPage
    {
        public List<OponentesModel> Lista_jogadores;

        public int id_jogador;
        public int ano;
        public Jogador info_jogador;

        private tb_stats ParentTabbedPage { get; }

        public Oponentes(int id_jogador, tb_stats parentTabbedPage)
        {
            InitializeComponent();

            if (parentTabbedPage != null)
            {
                this.ParentTabbedPage = parentTabbedPage;
                this.ano = parentTabbedPage.ano_geral;
                this.id_jogador = id_jogador;
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarOponentes();

            ranking_elo.Text = "Rating Elo: " + ParentTabbedPage.elo_geral;
            txt_nome.Text = ParentTabbedPage.Nome_Jogador_geral;
            image_jogador.Source = ParentTabbedPage.img_jogador_geral;
        }

        

        private void lista_categorias_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void lista_categorias_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            OponentesModel oponente = ((ListView)sender).SelectedItem as OponentesModel;
            if (oponente == null)
            {
                return;
            }

            if (oponente.id_oponente == 1)
            {
                oponente.id_oponente = oponente.id2_j1;
            }


            id_jogador = oponente.id_oponente;
            ParentTabbedPage.id_jogador_geral = oponente.id_oponente;

            CarregarDadosJogador(oponente.id_oponente);
            CarregarOponentes();

        }

        public async void CarregarDadosJogador(int id_jogador)
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");

            info_jogador = await API_Service.ObterDadosJogador(id_jogador);

            txt_nome.Text = info_jogador.Nome_Jogador;
            ranking_elo.Text = "Rating Elo: " + info_jogador.rating_elo;
            ParentTabbedPage.elo_geral = info_jogador.rating_elo;
            ParentTabbedPage.Nome_Jogador_geral = info_jogador.Nome_Jogador;
            //ParentTabbedPage.id_categoria_geral = id_jogador;

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

        public async void CarregarOponentes()
        {
            Lista_jogadores = await API_Service.ObterOponentes(id_jogador, ParentTabbedPage.ano_geral);
            lista_categorias.ItemsSource = Lista_jogadores.ToList();
        }

    }
}

