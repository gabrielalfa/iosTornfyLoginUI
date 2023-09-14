using System;
using System.Collections.Generic;
using Acr.UserDialogs.Extended;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class WebViewChaves : ContentPage
    {
        public WebViewChaves(string url, string title, int id_modelo, int id_TipoSorteio, int contagem)
        {
            InitializeComponent();
            webView.Source = url;
            Title = title;

            switch (id_modelo)
            {
                case 1:
                    //modelo eliminatorio
                    if (id_TipoSorteio == 2)
                    {
                        if (contagem > 32) { stack_scroll.WidthRequest = 1080; stack_scroll.HeightRequest = 3200; }
                        if (contagem < 33 && contagem > 16) { stack_scroll.WidthRequest = 1080; stack_scroll.HeightRequest = 1400; }
                        if (contagem < 17 && contagem > 8) { stack_scroll.WidthRequest = 1080; stack_scroll.HeightRequest = 1400; }
                        if (contagem > 4 && contagem < 9) { stack_scroll.WidthRequest = 1080; stack_scroll.HeightRequest = 1000; }
                        if (contagem == 4) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 1000; }
                        if (contagem == 3) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 1000; }
                        if (contagem == 2) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 1000; }
                    }

                    //modelo grupo
                    if (id_TipoSorteio == 1)
                    {
                        if (contagem == 3) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 2200; }
                        if (contagem == 4) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 2200; }
                        if (contagem == 5) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 2200; }
                        if (contagem == 6) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 2200; }
                        if (contagem == 7) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 2200; }
                        if (contagem == 8) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 2200; }
                        if (contagem == 9) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 2200; }
                        if (contagem == 10) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 2200; }
                        if (contagem == 11) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 2200; }
                        if (contagem == 12) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 2200; }
                        if (contagem == 13) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 2200; }
                        if (contagem == 14) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 2200; }
                        if (contagem == 15) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 2200; }
                        if (contagem == 16) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 2200; }
                        if (contagem == 17) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 18) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 19) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 20) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 21) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 22) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 23) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 24) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 25) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 26) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 27) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 28) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 29) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 30) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 31) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 32) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }

                    }
                    break;
                case 2:
                    //modelo eliminatorio
                    if (id_TipoSorteio == 2)
                    {
                        if (contagem > 32) { stack_scroll.WidthRequest = 1080; stack_scroll.HeightRequest = 3200; }
                        if (contagem < 33 && contagem > 16) { stack_scroll.WidthRequest = 1080; stack_scroll.HeightRequest = 1400; }
                        if (contagem < 17 && contagem > 8) { stack_scroll.WidthRequest = 1080; stack_scroll.HeightRequest = 1400; }
                        if (contagem > 4 && contagem < 9) { stack_scroll.WidthRequest = 1080; stack_scroll.HeightRequest = 1000; }
                        if (contagem == 4) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 1000; }
                        if (contagem == 3) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 1000; }
                        if (contagem == 2) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 1000; }
                    }
                    //modelo grupo
                    if (id_TipoSorteio == 1)
                    {
                        if (contagem == 3) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 4) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 5) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 6) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 7) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 8) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 9) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 10) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 11) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 12) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 13) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 14) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 15) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 16) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 17) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 18) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 19) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 20) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 21) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 3200; }
                        if (contagem == 22) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 23) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 24) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 25) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 26) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 27) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 28) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 29) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 30) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 31) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }
                        if (contagem == 32) { stack_scroll.WidthRequest = 800; stack_scroll.HeightRequest = 4200; }

                    }

                    break;
                case 4: 
                    stack_scroll.WidthRequest = 1080;
                    stack_scroll.HeightRequest = 2200;
                    break;
                default:
                    break;
            }

        }

        private void MyWebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.NavigationEvent == WebNavigationEvent.NewPage)
            {
                UserDialogs.Instance.ShowLoading(title: "Carregando...");
            }
        }

        private void MyWebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (e.NavigationEvent == WebNavigationEvent.NewPage || e.NavigationEvent == WebNavigationEvent.Refresh)
            {
                UserDialogs.Instance.HideLoading();
            }
        }

    }
}

