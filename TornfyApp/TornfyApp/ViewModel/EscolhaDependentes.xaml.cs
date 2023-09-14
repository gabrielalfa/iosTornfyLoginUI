using System;
using System.Collections.Generic;
using System.Net;
using API_Inteleco.Models;
using TornfyApp.Model;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class EscolhaDependentes : ContentPage
    {

        public List<Categoria> Lista_Categorias;
        int id_jogador = 0;

        int id_etapa = 0;
        string nome_etapa = "";
        public Config_Home config;
        public string nome_jogador;
        public int id_tipo;
        public int id_master;


        public EscolhaDependentes(int id_master, int Id_etapa, int id_jogador_inscricao, string Nome_etapa, string _nome_jogador, int _id_tipo, List<Jogador> lista_dependentes)
        {
            InitializeComponent();


            this.id_jogador = id_jogador_inscricao;
            this.id_etapa = Id_etapa;
            this.nome_etapa = Nome_etapa;
            this.nome_jogador = _nome_jogador.ToString();
            this.id_tipo = _id_tipo;
            this.id_master = id_master;
            txt_nome_etapa.Text = nome_etapa.ToString();
            txt_nome_jogador.Text = _nome_jogador.ToString();

            CarregarSecureStorage(lista_dependentes);

        }

        public async void CarregarSecureStorage(List<Jogador> lista_dependentes)
        {
            string url_teste = "https://tornfy.com/" + await Xamarin.Essentials.SecureStorage.GetAsync("path");
            string path_jogador = "";

            string toCheck = "https://tornfy.com/https://tornfy.com/";
            bool contains = url_teste.Contains(toCheck);
            if (contains)
            {
                string toRemove = "https://tornfy.com/https://tornfy.com/";
                url_teste = url_teste.Replace(toRemove, "");
                url_teste = "https://tornfy.com/" + url_teste;
            }

            if (!string.IsNullOrEmpty(url_teste))
            {
                HttpWebResponse response = null;
                var request = (HttpWebRequest)WebRequest.Create(url_teste);
                request.Method = "HEAD";
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    path_jogador = url_teste;
                }
                catch (WebException ex)
                {
                    path_jogador = "user_round.png";
                }
                finally
                {
                    if (response != null) { response.Close(); }
                }
            }
            else { path_jogador = "user_round.png"; }


            Jogador jogador = new Jogador
            {
                id = id_jogador,
                Nome_Jogador = nome_jogador,
                Path = path_jogador,
            };

            lista_dependentes.Add(jogador);

            lista_pagamentos.ItemsSource = lista_dependentes;

        }

        private void lista_pagamentos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void lista_pagamentos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Jogador jogador = ((ListView)sender).SelectedItem as Jogador;
            if (jogador == null) { return; }


            await Navigation.PushAsync(new Categorias_Inscricao(id_master, id_etapa, jogador.id, this.nome_etapa, jogador.Nome_Jogador, this.id_tipo));

        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            // Obtém o botão de imagem clicado
            var imageButton = (ImageButton)sender;

            // Obtém o item associado ao botão de imagem
            var selectedItem = (Jogador)imageButton.BindingContext;

            if (selectedItem != null)
            {
                await Navigation.PushAsync(new Categorias_Inscricao(id_master, id_etapa, selectedItem.id, this.nome_etapa, selectedItem.Nome_Jogador, this.id_tipo));
            }
        }

        private void btn_pgoPix_Clicked(object sender, EventArgs e)
        {

        }
    }
}

