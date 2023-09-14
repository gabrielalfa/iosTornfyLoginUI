using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Rg.Plugins.Popup.Extensions;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class DetalheRateio : ContentPage
    {
        public string gd;
        public int id_jogador;
        public string data;
        public List<Quadra> Lista_parceiros;

        public int id_locacao;
        public int id_quadra;
        public int id_master;
        public bool bool_liberar;
        public string image_jogador;
        public string nome_jogador;

        public DetalheRateio(string gd, int id_jogador, string string_quadra, string data, string horario,
            int id_locacao, int id_quadra, int id_master, bool bool_liberar)
        {
            InitializeComponent();

            this.gd = gd;
            this.id_jogador = id_jogador;
            this.data = data;

            this.id_locacao = id_locacao;
            this.id_quadra = id_quadra;
            this.id_master = id_master;
            this.bool_liberar = bool_liberar;

            txt_nome_quadra.Text = string_quadra.ToString();
            txt_detalhes.Text = horario + " - " + data;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            btn_pgoPix.IsVisible = false;
            btn_pgoEmail.IsVisible = false;
            CarregarSecureStorage();
            Buscar_Rateio();


        }

        private async void Buscar_Rateio()
        {
            Lista_parceiros = await API_Service.ObterParceiros_gd(gd, id_master);

            lista_pagamentos.ItemsSource = Lista_parceiros.ToList();

            if (bool_liberar)
            {
                btn_pgoPix.IsVisible = true;
                btn_pgoEmail.IsVisible = true;
            }

        }

        public async void CarregarSecureStorage()
        {
            string url_teste = "https://tornfy.com/" + await Xamarin.Essentials.SecureStorage.GetAsync("path");
            nome_jogador = await Xamarin.Essentials.SecureStorage.GetAsync("nome_jogador");

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
                    image_jogador = url_teste;
                }
                catch (WebException ex)
                {
                    image_jogador = "user_round.png";
                }
                finally
                {
                    if (response != null) { response.Close(); }
                }
            }
            else { image_jogador = "user_round.png"; }
        }

        private void lista_pagamentos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void lista_pagamentos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void btn_pgoPix_Clicked(object sender, EventArgs e)
        {
            bool result = await API_Service.Liberar_Quadra(id_locacao, id_quadra, id_master, data);

            if (result)
            {
                btn_pgoPix.IsVisible = false;
                btn_pgoEmail.IsVisible = false;

                var pop = new Mensagem_Simples("Liberação da Quadra realizada com sucesso!", "Cobranças relativas a essa locação podem ou não ser feitas pela secretaria do seu clube. Dúvidas pergunte ao clube responsável.", "#E10555", "Fechar Mensagem");
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            }
            else
            {
                var pop = new Mensagem_Simples("Liberação da Quadra não realizada!", "A desmarcação desta quadra conflita com as regras de marcação e desmarcação deste local.", "#E10555", "Fechar Mensagem");
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            }
        }

        private async void btn_pgoEmail_Clicked(object sender, EventArgs e)
        {
            int result = await API_Service.ReenviarEmailCobranca(gd);
            if (result == 200)
            {


                var pop = new MessageBox("Email enviado com sucesso!", "Email de cobrança enviado com sucesso aos jogadores do Rateio.");
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            }
            else
            {
                var pop = new Mensagem_Simples("Ralha no envio do Email!", "Falha do envio do email aos jogadores do Rateio.", "#E10555", "Fechar Mensagem");
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            }

        }
    }
}

