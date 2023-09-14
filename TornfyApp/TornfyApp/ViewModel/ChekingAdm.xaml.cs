using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Acr.UserDialogs.Extended;
using TornfyApp.API;
using TornfyApp.Model;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace TornfyApp.ViewModel
{
    public partial class ChekingAdm : ContentPage
    {

        public string token_habilitacao = "";
        static string tkfixact = "a2829b0a-8426-4ebc-b2dd-500f2be75bd1";
        public int id_inscricao;
        public bool dupla;
        public bool status_pgo;
        public bool status_presenca;
        public bool status_kit;

        public ChekingAdm()
        {
            InitializeComponent();
            CarregarSecureStorage();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public async void BuscarQRCode_Cliente(string token, string token_evento, string token_criacao)
        {

            int retorno_habilitacao = await BuscarQRCode_Adm(token_habilitacao);

            if (retorno_habilitacao == 200)
            {
                frame_habilitado.IsVisible = false;

                UserDialogs.Instance.ShowLoading(title: "Buscando Informações");
                Token_Etapa retorno = await API_Service.ObterBuscarQRCode_Cliente(token, token_evento, token_criacao, token_habilitacao);
                UserDialogs.Instance.HideLoading();

                //300 não pago
                //200 sucesso
                //400 token não criado
                //500 erro
                //600 não autorizado
                //701 qr não existe
                //700 qr vencido

                //E7305E vermelho

                if (retorno != null)
                {
                    this.id_inscricao = retorno.id_inscricao;
                    this.dupla = retorno.dupla;
                    txt_titulo_positivo.Text = retorno.nome_etapa;
                    txt_mensagem_positivo.Text = "ID:" + retorno.id_inscricao + " - " + retorno.nome_jogador;
                    btn_presença.BackgroundColor = Color.FromHex("0f9e3e");
                    btn_statusPgo.BackgroundColor = Color.FromHex("0f9e3e");
                    btn_kitBrinde.BackgroundColor = Color.FromHex("0f9e3e");
                    btn_presença.Text = "Presença Confirmada";
                    btn_kitBrinde.Text = "Kit/Brinde Entregue";
                    btn_statusPgo.Text = "Pagamento Realizado";

                    this.status_pgo = retorno.status_pagamento;
                    this.status_presenca = retorno.qr_cheking_bool;
                    this.status_kit = retorno.kit_brinde;

                    //caso não presente
                    if (!retorno.qr_cheking_bool)
                    {
                        btn_presença.Text = "Presença não confirmada";
                        btn_presença.BackgroundColor = Color.FromHex("E7305E");
                    }

                    //brinde não entregue
                    if (!retorno.kit_brinde)
                    {
                        btn_kitBrinde.Text = "Kit/Brinde Pendente";
                        btn_kitBrinde.BackgroundColor = Color.FromHex("E7305E");
                    }


                    switch (retorno.status)
                    {
                        case 200:
                            resposta_positiva.IsVisible = true;
                            img_info.Source = retorno.path_clube;
                            break;
                        case 300:
                            resposta_positiva.IsVisible = true;
                            btn_statusPgo.BackgroundColor = Color.FromHex("E7305E");
                            btn_statusPgo.Text = "Pagamento Pendente";
                            img_info.Source = retorno.path_clube;
                            break;
                        case 400:
                            resposta_positiva.IsVisible = false;
                            resposta_negativa.IsVisible = true;
                            txt_titulo.Text = "QR Code Encerrado";
                            txt_mensagem.Text = "QR code foi baixado, não criado ou inacessível no momento!";
                            break;
                        case 500:
                            resposta_positiva.IsVisible = false;
                            resposta_negativa.IsVisible = true;
                            txt_titulo.Text = "Falha de Servidor";
                            txt_mensagem.Text = "O serviço do servidor encontra-se fora do ar!";
                            break;
                        case 600:
                            resposta_positiva.IsVisible = false;
                            resposta_negativa.IsVisible = true;
                            txt_titulo.Text = "Não Autorizado";
                            txt_mensagem.Text = "Este dispositivo não encontra-se autorizado no momento!";
                            frame_qr.IsVisible = false;
                            btn_pdf.IsVisible = false;
                            break;
                        case 700:
                            resposta_positiva.IsVisible = false;
                            resposta_negativa.IsVisible = true;
                            txt_titulo.Text = "QR code Vencido";
                            txt_mensagem.Text = "Sua habilitação para leittura de Qr Code está vencida. Solicite novo escaneamento ao administrador!";
                            frame_qr.IsVisible = false;
                            btn_pdf.IsVisible = false;
                            break;
                        case 701:
                            resposta_positiva.IsVisible = false;
                            resposta_negativa.IsVisible = true;
                            txt_titulo.Text = "QR code Inexistente";
                            txt_mensagem.Text = "Não há no momento uma habilitação presente pelo organizador. Solicite novo escaneamento ao administrador!";
                            frame_qr.IsVisible = false;
                            btn_pdf.IsVisible = false;
                            break;
                        default:
                            break;
                    }


                }
                else
                {
                    Remover_Habilitacao();
                }
            }






        }

        public async void CarregarSecureStorage()
        {
            frame_qr.IsVisible = false;
            btn_pdf.IsVisible = false;
            frame_habilitado.IsVisible = false;
            resposta_negativa.IsVisible = false;
            resposta_positiva.IsVisible = false;
            ToolbarItems.Remove(btn_escanear);

            string token_habilitacao = await SecureStorage.GetAsync("token_habilitacao");
            _ = BuscarQRCode_Adm(token_habilitacao);

        }

        private async void btn_pdf_Clicked(object sender, EventArgs e)
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();

                    if (status != PermissionStatus.Granted)
                    {
                        return;
                    }
                }

                var scanPage = new ZXingScannerPage();
                // Navigate to our scanner page
                await Navigation.PushAsync(scanPage);

                scanPage.OnScanResult += (result) =>
                {
                    // Stop scanning
                    scanPage.IsScanning = false;

                    // Pop the page and show the result
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var url = result.Text;

                        var queryParameters = HttpUtility.ParseQueryString(new Uri(url).Query);

                        //Habilitar_AcessoQr?token={token_etapa}?tkfixact={tkfixact}

                        // Obtém o valor do parâmetro 'token'
                        string token = queryParameters["token"];

                        //token de segurança de produção pela plataforma
                        string tkfixact_url = queryParameters["tkfixact"];

                        if (tkfixact == tkfixact_url)
                        {
                            //grava no celular o token de habilitação
                            await SecureStorage.SetAsync("token_habilitacao", token);

                            _ = BuscarQRCode_Adm(token);

                        }
                        else
                        {
                            // Lide com quaisquer exceções que possam ocorrer durante o processo.
                            await DisplayAlert("QR Inválido", "Falha na conecção com administrador", "Fechar");
                        }       

                        await Navigation.PopAsync();
                        //await DisplayAlert("QR Inválido", "Solciite acesso adm para escaneamento", "Entendi");
                    });
                };



            }
            catch (Exception ex)
            {
                // Lide com quaisquer exceções que possam ocorrer durante o processo.
                await DisplayAlert("QR Inválido", "Falha na leitura do escaneamento", "Fechar");
            }
        }

        public async Task<int> BuscarQRCode_Adm(string token_habilitacao)
        {
            frame_habilitado.IsVisible = false;

            UserDialogs.Instance.ShowLoading(title: "Buscando Habilitações");
            Token_Etapa retorno = await API_Service.ObterBuscarQRCode_HabilitacaoAdm(token_habilitacao);
            UserDialogs.Instance.HideLoading();

            //400 token não encontrato
            //500 falha de servidor
            //200 sucesso

            //status_token_habilitacao = status_token_habilitacao,
            //tempo_habilitacao = tempo_habilitacao,
            //data_habilitacao = data_habilitacao,
            //existe_qr = existe_qr,

            switch (retorno.status)
            {
                case 200:
                    //true = não vencido
                    if (retorno.status_token_habilitacao)
                    {
                        //tempo de habilitação maior que zero/ainda tem tempo para escaneamento
                        if (retorno.tempo_habilitacao > 0)
                        {
                            this.token_habilitacao = token_habilitacao;
                            await SecureStorage.SetAsync("token_habilitacao", token_habilitacao);

                            frame_qr.IsVisible = false;
                            btn_pdf.IsVisible = false;

                            //determina o tempo de vida do token no dispositivo
                            await SecureStorage.SetAsync("tempo_habilitacao", retorno.tempo_habilitacao.ToString());
                            ToolbarItems.Remove(btn_escanear);
                            ToolbarItems.Add(btn_escanear);
                            frame_habilitado.IsVisible = true;

                            lbl_habilitado.Text = string.Format("Este dispositivo está habilitado a " +
                                "fazer escaneamento para o evento: {0}, até a data: {1}",
                                retorno.nome_etapa, retorno.data_habilitacao);
                        }
                        else
                        {
                            Remover_Habilitacao();
                        }
                    }
                    break;
                case 400:
                    Remover_Habilitacao();
                    break;
                case 500:
                    Remover_Habilitacao();
                    break;
                case 700:
                    lbl_solicitacao.Text = "O seu dispositivo perdeu sua habilitação por vencimento de data de habilitação de dispositivo";
                    Remover_Habilitacao();
                    break;
                case 702:
                    lbl_solicitacao.Text = "O seu dispositivo de escaneamento está desabilitado pelo administrador no momento.";
                    Remover_Habilitacao();
                    break;
                default:
                    Remover_Habilitacao();
                    break;
            }

            return retorno.status;
        }

        public void Remover_Habilitacao()
        {
            frame_qr.IsVisible = true;
            btn_pdf.IsVisible = true;
            frame_habilitado.IsVisible = false;
            resposta_negativa.IsVisible = false;
            resposta_positiva.IsVisible = false;
            ToolbarItems.Remove(btn_escanear);
            SecureStorage.Remove("token_etapa");
            SecureStorage.Remove("token_habilitacao");
        }

        private async void btn_escanear_Clicked(object sender, EventArgs e)
        {


            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();

                    if (status != PermissionStatus.Granted)
                    {
                        return;
                    }
                }

                var scanPage = new ZXingScannerPage();
                // Navigate to our scanner page
                await Navigation.PushAsync(scanPage);

                scanPage.OnScanResult += (result) =>
                {
                    // Stop scanning
                    scanPage.IsScanning = false;

                    // Pop the page and show the result
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var url = result.Text;

                        var queryParameters = HttpUtility.ParseQueryString(new Uri(url).Query);

                        // Obtém o valor do parâmetro 'token'
                        string token = queryParameters["token"];

                        // Obtém o valor do parâmetro 'token_criacao'
                        string token_criacao = queryParameters["token_criacao"];

                        // Obtém o valor do parâmetro 'token_criacao'
                        string token_evento = queryParameters["token_evento"];

                        BuscarQRCode_Cliente(token, token_evento, token_criacao);

                        await Navigation.PopAsync();



                        //await DisplayAlert("QR Inválido", "Solciite acesso adm para escaneamento", "Entendi");
                    });
                };


                //https://localhost:44325/BaixarQr?token=96a5586a-832f-4b01-9856-96738e0947d2&token_evento=1e8601ad-c27a-4093-8de7-2faba0569df3&token_criacao=19cf1fb5-e8d6-4cbb-a460-fb3befd3eada




            }
            catch (Exception ex)
            {
                // Lide com quaisquer exceções que possam ocorrer durante o processo.
                await DisplayAlert("QR Inválido", "Falha na leitura do escaneamento", "Fechar");
            }
        }

        private async void btn_close_Clicked(object sender, EventArgs e)
        {
            //fechar navegação
            await Navigation.PopAsync();
        }


        private async void btn_statusPgo_Clicked(object sender, EventArgs e)
        {
            bool resposta = await DisplayAlert("Confirmação", "Deseja alterar status de pagamento?", "Sim", "Não");
            string mensagem = "";
            if (resposta)
            {
                if (status_pgo) status_pgo = false; else status_pgo = true;
                int tipo = 1; //status pagamento
                int retorno = await API_Service.MudarStatusInscricao(id_inscricao, status_pgo, tipo, dupla);
                if (retorno == 200)
                {
                    if (status_pgo)
                    {
                        mensagem = "Ação de baixa de pagamento realizada!";
                        btn_statusPgo.BackgroundColor = Color.FromHex("0f9e3e");
                        btn_statusPgo.Text = "Pagamento Realizado";
                    }
                    else
                    {
                        mensagem = "Baixa de pagamento falhou!";
                        btn_statusPgo.BackgroundColor = Color.FromHex("E7305E");
                        btn_statusPgo.Text = "Pagamento Pendente";
                    }
                }


            }
            else { mensagem = "Ação de baixa de pagamento cancelada!"; }

            var option = new SnackBarOptions
            {
                MessageOptions = new MessageOptions
                {
                    Foreground = Color.White,
                    Message = mensagem,
                    Padding = 10
                },
                BackgroundColor = Color.FromHex("#1f2225"),
                Duration = TimeSpan.FromSeconds(2),
            };

            await this.DisplaySnackBarAsync(option);
        }

        private async void btn_presença_Clicked(object sender, EventArgs e)
        {
            bool resposta = await DisplayAlert("Confirmação", "Deseja alterar status de presença?", "Sim", "Não");

            string mensagem = "";
            if (resposta)
            {
                if (status_presenca) status_presenca = false; else status_presenca = true;
                int tipo = 2; //presença
                int retorno = await API_Service.MudarStatusInscricao(id_inscricao, status_presenca, tipo, dupla);
                if (retorno == 200)
                {
                    if (status_presenca)
                    {
                        mensagem = "Ação de baixa de presença realizada!";
                        btn_presença.BackgroundColor = Color.FromHex("0f9e3e");
                        btn_presença.Text = "Presença Confirmada";
                    }
                    else
                    {
                        mensagem = "Baixa de presença falhou!";
                        btn_presença.Text = "Presença não confirmada";
                        btn_presença.BackgroundColor = Color.FromHex("E7305E");
                    }
                }

            }
            else { mensagem = "Ação de baixa de presença cancelada!"; }

            var option = new SnackBarOptions
            {
                MessageOptions = new MessageOptions
                {
                    Foreground = Color.White,
                    Message = mensagem,
                    Padding = 10
                },
                BackgroundColor = Color.FromHex("#1f2225"),
                Duration = TimeSpan.FromSeconds(2),
            };

            await this.DisplaySnackBarAsync(option);

        }

        private async void btn_kitBrinde_Clicked(object sender, EventArgs e)
        {
            bool resposta = await DisplayAlert("Confirmação", "Deseja alterar status de Kit/Brinde?", "Sim", "Não");

            string mensagem = "";
            if (resposta)
            {
                if (status_kit) status_kit = false; else status_kit = true;
                int tipo = 3; //Kit/Brinde
                int retorno = await API_Service.MudarStatusInscricao(id_inscricao, status_kit, tipo, dupla);
                if (retorno == 200)
                {
                    if (status_kit)
                    {
                        mensagem = "Ação de baixa de Kit/Brinde realizada!";
                        btn_kitBrinde.BackgroundColor = Color.FromHex("0f9e3e");
                        btn_kitBrinde.Text = "Kit/Brinde Entregue";
                    }
                    else
                    {
                        mensagem = "Baixa de Kit/Brinde falhou!";
                        btn_kitBrinde.Text = "Kit/Brinde Pendente";
                        btn_kitBrinde.BackgroundColor = Color.FromHex("E7305E");
                    }
                }

            }
            else { mensagem = "Ação de baixa de Kit/Brinde cancelada!"; }


            var option = new SnackBarOptions
            {
                MessageOptions = new MessageOptions
                {
                    Foreground = Color.White,
                    Message = mensagem,
                    Padding = 10
                },
                BackgroundColor = Color.FromHex("#1f2225"),
                Duration = TimeSpan.FromSeconds(2),
            };

            await this.DisplaySnackBarAsync(option);
        }

        private async void btn_BaixaQr_Clicked(object sender, EventArgs e)
        {
            bool resposta = await DisplayAlert("Confirmação", "Deseja baixar QRCode de presença? Essa ação irá destruir o qr code de presença, porém não deletará quaiquer outro elemnto dentro da incrição!", "Sim", "Não");

            string mensagem;
            if (resposta) { mensagem = "Ação de baixa de QRCode realizada!"; } else { mensagem = "Ação de baixa de QRCode de presença cancelada!"; }

            int retorno = await API_Service.BaixaQr(id_inscricao, dupla);

            if (retorno == 200)
            {
                resposta_positiva.IsVisible = false;
                resposta_negativa.IsVisible = true;
                txt_titulo.Text = "QR Code finalizado";
                txt_mensagem.Text = "QR code foi baixado com sucesso, deste ponto em diante o acesso do QR code para esta inscrição não é mais acessível!";
                frame_qr.IsVisible = false;
                btn_pdf.IsVisible = false;
            }
            else
            {
                mensagem = "Falha na ação de baixa de QRCode de presença!";
            }

            var option = new SnackBarOptions
            {
                MessageOptions = new MessageOptions
                {
                    Foreground = Color.White,
                    Message = mensagem,
                    Padding = 10
                },
                BackgroundColor = Color.FromHex("#1f2225"),
                Duration = TimeSpan.FromSeconds(2),
            };

            await this.DisplaySnackBarAsync(option);
        }

        private void btn_limpar_Clicked(object sender, EventArgs e)
        {
            SecureStorage.Remove("token_etapa");
            SecureStorage.Remove("token_habilitacao");
        }
    }
}

