using System;
using System.Collections.Generic;
using Acr.UserDialogs.Extended;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class Chekings : ContentPage
    {

        public List<Inscricao> Lista_Chegings;
        public int id_jogador;
        public int id_master;

        public Chekings(int id_jogador, int id_master)
        {
            InitializeComponent();

            this.id_jogador = id_jogador;
            this.id_master = id_master;

            lista_chekings.RefreshCommand = new Command(() =>
            {
                CarregarChegings();
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => lista_chekings.IsRefreshing = false);
            });

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarChegings();
        }

        public async void CarregarChegings()
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Lista");
            List<Inscricao> Lista_Chegings = await API_Service.ObterChegings(id_jogador, id_master);

            lista_chekings.ItemsSource = Lista_Chegings;
            UserDialogs.Instance.HideLoading();
        }

        private void lista_chekings_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private async void lista_chekings_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Inscricao cheking = ((ListView)sender).SelectedItem as Inscricao;
            if (cheking == null)
            {
                return;
            }

            await Navigation.PushAsync(new ChekingQr(cheking.existe, cheking.qr_cheking, cheking.id, id_jogador));

        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {


            await Navigation.PushAsync(new ChekingAdm());


        }
    }
}

