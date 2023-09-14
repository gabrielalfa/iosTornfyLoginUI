using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using API_Inteleco.Models;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class EscolhaJogadorInscriMultipla : ContentPage
    {
        public event EventHandler<Inscricao> ItemSelecionado;

        public int id_etapa;

        public string lista_idInscricoes;
        public int id_jogador;
        public List<Inscricao> lista_incritos;
        public List<Categoria> Lista_Categorias;

        public EscolhaJogadorInscriMultipla(int id_etapa, string lista_idInscricoes, int id_jogador)
        {
            InitializeComponent();

            this.id_etapa = id_etapa;
            this.lista_idInscricoes = lista_idInscricoes;
            this.id_jogador = id_jogador;


            CarregarCategoriasEtapa(id_etapa);

        }

        public async void CarregarCategoriasEtapa(int id_etapa)
        {
            Lista_Categorias = await API_Service.ObterCategoriaEtapa(id_etapa);
           //Categoria categoria = new Categoria();
           //categoria.Categoria_Nome = "Selecione uma Categoria";
           //categoria.id = 0;
           //Lista_Categorias.Add(categoria);

            pcCategoria.ItemsSource = Lista_Categorias.OrderBy(x => x.Categoria_Nome).ToList();
        }


        private async void Buscar_Incricoes(int id_cat)
        {
            lista_incritos = await API_Service.ObterLista_Preecher_Jogadores_Universal(id_etapa, id_cat, lista_idInscricoes, id_jogador);

            lista_incritos = lista_incritos.Where(item => !item.checado).ToList();

            foreach (var item in lista_incritos)
            {
                string url_teste = item.img_path;

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
                        item.img_path = url_teste;
                    }
                    catch (WebException ex)
                    {
                        item.img_path = "user_round.png";
                    }
                    finally { if (response != null) { response.Close(); } }
                }
                else { item.img_path = "user_round.png"; }
            }

            lista_pagamentos.ItemsSource = lista_incritos;
        }

        private void lista_pagamentos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void lista_pagamentos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedItem = (Inscricao)((ListView)sender).SelectedItem;

            if (selectedItem != null)
            {
                ItemSelecionado?.Invoke(this, selectedItem);
                await Navigation.PopAsync();
            }
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            // Obtém o botão de imagem clicado
            var imageButton = (ImageButton)sender;

            // Obtém o item associado ao botão de imagem
            var selectedItem = (Inscricao)imageButton.BindingContext;

            if (selectedItem != null)
            {
                ItemSelecionado?.Invoke(this, selectedItem);
                await Navigation.PopAsync();
            }
        }

        private void pcCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            Categoria categoria = ((Picker)sender).SelectedItem as Categoria;
            if (categoria == null)
            {
                return;
            }

            Buscar_Incricoes(categoria.id);
        }
    }
}

