    using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Rg.Plugins.Popup.Extensions;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;


namespace TornfyApp.ViewModel
{
    public partial class Rateio : ContentPage
    {


        public Config_Home config;
        public int id_quadra;
        public int id_jogador;
        public int id_locacao;
        public int id_master;
        public string _lista_pagamentos;
        List<Quadra> pagamentos;
        public List<Quadra> Lista_pagamentos;

        public Quadra info_locacao;
        public decimal valor_rateio;
        public decimal valor_original;

        public string inicio = "";
        public string fim = "";

        private List<Quadra> Quadras { get; set; }

        public Regras regras;


        public Rateio(Config_Home Config, int id_jogador, int id_master, decimal valor_cobranca,
             string string_quadra, string data, string horario, int id_locacao, int id_quadra)
        {
            InitializeComponent();

            config = Config;
            this.id_locacao = id_locacao;
            this.id_jogador = id_jogador;
            this.id_master = id_master;
            this.id_quadra = id_quadra;



            txt_nome_quadra.Text = string_quadra.ToString();
            txt_detalhes.Text = horario + " - " + data;

            DisplayAlert("Locação da Quadra foi realizada!", "Este complexo de quadra exige o rateio de locação, informe com quem você irá jogar", "informar");

            LoadData();
            Buscar_Valor();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Buscar_Regras();
        }

        public async void Buscar_Regras()
        {
            regras = await API_Service.ObterRegras(id_master);
        }

        private async void Buscar_Valor()
        {
            info_locacao = await API_Service.ObterInfoLocacao(id_locacao, id_master);
            txt_valor_rateio.Text = info_locacao.valor_cobranca.ToString();

            valor_original = decimal.Parse(info_locacao.valor_cobranca.ToString());
            valor_rateio = decimal.Parse(info_locacao.valor_cobranca.ToString());
        }


        private async void LoadData()
        {

            string nome_jogador = await Xamarin.Essentials.SecureStorage.GetAsync("nome_jogador");
            string path = await Xamarin.Essentials.SecureStorage.GetAsync("nome_jogador");
            string url_teste = "https://tornfy.com/" + await Xamarin.Essentials.SecureStorage.GetAsync("path");

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
                    path = url_teste;
                }
                catch (WebException ex)
                {
                    path = "user_round.png";
                }
                finally { if (response != null) { response.Close(); } }
            }
            else { path = "user_round.png"; }


            //adiciona o proprio jogador logado
            Quadras = new List<Quadra>
            {
                new Quadra { nome_jogador = nome_jogador, path = path, id = id_jogador },
            };

            lista_pagamentos.ItemsSource = Quadras;
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
            var lista_rateio = string.Empty;
            foreach (var item in Quadras)
            {
                lista_rateio += "#" + item.id;
            }

            await Navigation.PushAsync(new Fechamento_PagamentoLocacao(
                config, id_jogador, id_master, lista_rateio, true,
                 Math.Round(valor_rateio, 2), Math.Round(valor_original, 2), false, id_locacao, id_quadra));
        }


        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var quadra = button.BindingContext as Quadra;

            if (Quadras.Count != 1)
            {
                if (quadra != null)
                {
                    // Remova o item da lista com base no id_jogador
                    int idJogador = quadra.id;
                    Quadras.Remove(Quadras.FirstOrDefault(q => q.id == idJogador));
                    lista_pagamentos.ItemsSource = null; // Remova o vínculo atual
                    lista_pagamentos.ItemsSource = Quadras; // Associe a nova coleção Quadras


                    valor_rateio = valor_original / Quadras.Count;
                    txt_valor_rateio.Text = valor_rateio.ToString("F2"); // Formata o valor com duas casas decimais
                }
            }
            
        }



        private async void Button_Clicked(object sender, EventArgs e)
        {
            int count = 0;
            try
            {
                count = Quadras.Count;
            }
            catch (Exception)
            {
                count = 0;
            }

            int limite = 4;
            if (count < limite)
            {
                try
                {
                    var rateioParceirosPage = new Rateio_Parceiros(id_jogador, id_master, true, "", Quadras);

                    // Defina um manipulador de eventos para o evento ItemSelecionado
                    rateioParceirosPage.ItemSelecionado += RateioParceiros_ItemSelecionado;

                    // Chame a página Rateio_Parceiros
                    await Navigation.PushAsync(rateioParceirosPage);
                }
                catch (Exception)
                {
                    // Lida com exceções, se necessário
                }
            }
            else
            {
                await DisplayAlert("Limite de Rateio!",
                    string.Format("O Limite de divisão de locação para este complexo é: {0}", limite),
                    "informar");
            }
        }
        

        private async void RateioParceiros_ItemSelecionado(object sender, Quadra itemSelecionado)
        {
            if (itemSelecionado != null)
            {
                //verificar a quantidade de locações do parceito
                int retorno_liberacao = await API_Service.ObterLimite(id_locacao, id_master, itemSelecionado.id);

                if (retorno_liberacao == 200)
                {

                    Quadras.Add(itemSelecionado);
                    lista_pagamentos.ItemsSource = null; // Remova o vínculo atual
                    lista_pagamentos.ItemsSource = Quadras; // Associe a nova coleção Quadras

                    //adicionar regra de valor integral
                    //em caso de apenas informar jogadores
                    //e não dividir valores
                    if (regras.valores_rateio)
                    {
                        //se true, não é para dividir valores
                        var option = new SnackBarOptions
                        {
                            MessageOptions = new MessageOptions
                            {
                                Foreground = Color.White,
                                Message = "Este complexo não divide valores, apenas informa jogadores!",
                                Padding = 10
                            },
                            BackgroundColor = Color.FromHex("#1f2225"),
                            Duration = TimeSpan.FromSeconds(2),
                            // Actions = new[] { actions }


                        };

                        this.DisplaySnackBarAsync(option);
                    }
                    else
                    {
                        valor_rateio = valor_original / Quadras.Count;
                        txt_valor_rateio.Text = valor_rateio.ToString("F2"); // Formata o valor com duas casas decimais
                    }

                }
                else
                {
                    switch (retorno_liberacao)
                    {
                        case 5:
                            //Limite diária excedido
                            var pop = new Mensagem_Simples("Locação da Quadra não foi realizada!", "Quantidade de locações por usuários por dia excede o limite determinado pelo administrador.", "#E10555", "Fechar Mensagem");
                            _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                            break;
                        case 6:
                            //Limite semanal excedido
                            var pop2 = new Mensagem_Simples("Locação da Quadra não foi realizada!", "Quantidade de locações por usuários por semana excede o limite determinado pelo administrador.", "#E10555", "Fechar Mensagem");
                            _ = App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
                            break;
                        default:
                            break;
                    }
                }

            }
        }

        private void RateioParceiros_ItemSelecionado_antigo(object sender, Quadra itemSelecionado)
        {
            if (itemSelecionado != null)
            {
                Quadras.Add(itemSelecionado);
                lista_pagamentos.ItemsSource = null; // Remova o vínculo atual
                lista_pagamentos.ItemsSource = Quadras; // Associe a nova coleção Quadras

                //adicionar regra de valor integral
                //em caso de apenas informar jogadores
                //e não dividir valores
                if (regras.valores_rateio)
                {
                    //se true, não é para dividir valores
                    var option = new SnackBarOptions
                    {
                        MessageOptions = new MessageOptions
                        {
                            Foreground = Color.White,
                            Message = "Este complexo não divide valores, apenas informa jogadores!",
                            Padding = 10
                        },
                        BackgroundColor = Color.FromHex("#1f2225"),
                        Duration = TimeSpan.FromSeconds(2),
                        // Actions = new[] { actions }


                    };

                    this.DisplaySnackBarAsync(option);
                }
                else
                {
                    valor_rateio = valor_original / Quadras.Count;
                    txt_valor_rateio.Text = valor_rateio.ToString("F2"); // Formata o valor com duas casas decimais
                }





            }
        }



    }
}

    