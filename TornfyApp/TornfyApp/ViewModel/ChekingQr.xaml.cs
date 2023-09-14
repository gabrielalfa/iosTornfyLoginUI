using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Acr.UserDialogs.Extended;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class ChekingQr : ContentPage
    {
        public bool existe;
        public string qr_cheking;
        public int id_inscricao;
        public int id_jogador;

        public ChekingQr(bool existe, string qr_cheking, int id_inscricao, int id_jogador)
        {
            InitializeComponent();

            this.existe = existe;
            this.qr_cheking = qr_cheking;
            this.id_inscricao = id_inscricao;
            this.id_jogador = id_jogador;

            frame_qr.IsVisible = false;
            frame_no.IsVisible = false;
            btn_pdf.IsVisible = false;

            if (existe)
            {
                frame_qr.IsVisible = true;
                btn_pdf.IsVisible = true;
                img_qrcode.Source = qr_cheking;
            }
            else { frame_no.IsVisible = true; }

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {

            var filePath = await DownloadPdfFileAsyncProgramacao();

            if (filePath != null)
            {
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });
            }


        }

        private async Task<string> DownloadPdfFileAsyncProgramacao()
        {
            UserDialogs.Instance.ShowLoading(title: "Buscando Jogos");
            DateTime data = DateTime.Now;
            Guid guid = Guid.NewGuid();
            string url = string.Format("https://tornfy.com/Inscricao/Imprimir_Ingressso?id={1}&id_jogador={0}", id_jogador, id_inscricao);
            //var filePath = Path.Combine(FileSystem.AppDataDirectory, "Ingresso:" + guid.ToString() + "token:"+ data + ".pdf");

            var filePath = Path.Combine(FileSystem.AppDataDirectory, "Ingresso.pdf");


            UserDialogs.Instance.HideLoading();
            var httpClient = new HttpClient();
            var pdfBytes = await httpClient.GetByteArrayAsync(url);
            File.WriteAllBytes(filePath, pdfBytes);

            return filePath;


        }
    }
}

