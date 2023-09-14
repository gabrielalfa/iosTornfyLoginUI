using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Acr.UserDialogs.Extended;
using API_Inteleco.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;

namespace TornfyApp.ViewModel   
{
    public partial class Modal : PopupPage
    {

        public Categoria categoria;

        public Modal(Categoria categoria)
        {
            InitializeComponent();

            this.categoria = categoria;

            //teste
            // url = string.Format("https://tornfy.com/{0}", "/Chave_PDFView/ChavePDF_8?id_classe=5&id_tipochave=8&target=_blank&id_etapa=306");
            //await Navigation.PushAsync(new WebViewGlobal(url, "Editar Foto"));
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string controller = "Chave_PDF";

            var filePath = await DownloadPdfFileAsync(categoria.id_cat, categoria.id_etapa, categoria.Categoria_Nome,
                categoria.id_TipoSorteio, int.Parse(categoria.contagem), categoria.nome_etapa, categoria.id_modelo, controller);
            UserDialogs.Instance.HideLoading();

            if (filePath != null)
            {
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });

                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
            }

        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            //forçar modelo 3 para PDF
            if (categoria.id_modelo == 3)
            {
                string controller = "Chave_PDF";

                var filePath = await DownloadPdfFileAsync(categoria.id_cat, categoria.id_etapa, categoria.Categoria_Nome,
                    categoria.id_TipoSorteio, int.Parse(categoria.contagem), categoria.nome_etapa, categoria.id_modelo, controller);
                UserDialogs.Instance.HideLoading();
                if (filePath != null)
                {
                    await Launcher.OpenAsync(new OpenFileRequest
                    {
                        File = new ReadOnlyFile(filePath)
                    });

                    await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
                }
            }
            else
            {

                string controller = "Chave_PDFView";

                var url = RedirectAovidoAsync(categoria.id_cat, categoria.id_etapa, categoria.Categoria_Nome,
                    categoria.id_TipoSorteio, int.Parse(categoria.contagem), categoria.nome_etapa, categoria.id_modelo, controller);
                UserDialogs.Instance.HideLoading();
                if (url != null)
                {
                    await Navigation.PushAsync(new WebViewChaves(url, categoria.Categoria_Nome,
                        categoria.id_modelo, categoria.id_TipoSorteio, int.Parse(categoria.contagem)));

                    await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
                }
            }


        }


        private string RedirectAovidoAsync(int id_cat, int id_etapa, string nome_chave,
           int id_tipoChave, int contagem, string nome_etapa, int id_modelo, string controller)
        {
            string view_path = "";
            int id_tipochave = 0;
            UserDialogs.Instance.ShowLoading(title: "Buscando Jogos");


            //modelos de rei da quadra
            if (id_modelo == 4)
            {
                switch (contagem)
                {
                    case 4:
                        id_tipochave = 4;
                        view_path = "GrupoPDF_ReiQuadra";
                        break;
                    case 8:
                        id_tipochave = 8;
                        view_path = "GrupoPDF_ReiQuadra";
                        break;
                    case 12:
                        id_tipochave = 12;
                        view_path = "GrupoPDF_ReiQuadra";
                        break;
                    case 16:
                        id_tipochave = 16;
                        view_path = "GrupoPDF_ReiQuadra";
                        break;
                    default:
                        id_tipochave = id_tipoChave;
                        view_path = "GrupoPDF_ReiQuadra";
                        break;

                }
            }


            //modelos de tenis e beach (simples e duplas)
            if (id_modelo == 1 || id_modelo == 2)
            {

                if (id_tipoChave == 1)
                {
                    //if (contagem == 3) { id_tipochave = 3; view_path = "GrupoPDF_3"; }
                    //if (contagem == 4) { id_tipochave = 4; view_path = "GrupoPDF_4"; }
                    //if (contagem == 5) { id_tipochave = 5; view_path = "GrupoPDF_5"; }
                    //if (contagem == 6) { id_tipochave = 6; view_path = "GrupoPDF_6"; }
                    //if (contagem == 7) { id_tipochave = 7; view_path = "GrupoPDF_7"; }
                    //if (contagem == 8) { id_tipochave = 8; view_path = "GrupoPDF_8"; }
                    //if (contagem == 9) { id_tipochave = 9; view_path = "GrupoPDF_9"; }
                    //if (contagem == 10) { id_tipochave = 10; view_path = "GrupoPDF_10"; }
                    //if (contagem == 11) { id_tipochave = 11; view_path = "GrupoPDF_11"; }
                    //if (contagem == 12) { id_tipochave = 12; view_path = "GrupoPDF_12"; }
                    //if (contagem == 13) { id_tipochave = 13; view_path = "GrupoPDF_13_plus"; }
                    //if (contagem == 14) { id_tipochave = 14; view_path = "GrupoPDF_14"; }
                    //if (contagem == 15) { id_tipochave = 15; view_path = "GrupoPDF_15_plus"; }
                    //if (contagem == 16) { id_tipochave = 16; view_path = "GrupoPDF_16_plus"; }
                    //if (contagem == 17) { id_tipochave = 17; view_path = "GrupoPDF_17"; }
                    //if (contagem == 18) { id_tipochave = 18; view_path = "GrupoPDF_18_plus"; }
                    //if (contagem == 19) { id_tipochave = 19; view_path = "GrupoPDF_18_plus"; }
                    //if (contagem == 20) { id_tipochave = 20; view_path = "GrupoPDF_18_plus"; }
                    //if (contagem == 21) { id_tipochave = 21; view_path = "GrupoPDF_21_plus"; }
                    //if (contagem == 22) { id_tipochave = 22; view_path = "GrupoPDF_21_plus"; }
                    //if (contagem == 23) { id_tipochave = 23; view_path = "GrupoPDF_23_plus"; }
                    //if (contagem == 24) { id_tipochave = 24; view_path = "GrupoPDF_24_plus"; }
                    //if (contagem == 25) { id_tipochave = 25; view_path = "GrupoPDF_24_plus"; }
                    //if (contagem == 26) { id_tipochave = 26; view_path = "GrupoPDF_24_plus"; }
                    //if (contagem == 27) { id_tipochave = 27; view_path = "GrupoPDF_27_plus"; }
                    //if (contagem == 28) { id_tipochave = 28; view_path = "GrupoPDF_27_plus"; }
                    //if (contagem == 29) { id_tipochave = 29; view_path = "GrupoPDF_27_plus"; }
                    //if (contagem == 30) { id_tipochave = 30; view_path = "GrupoPDF_30_plus"; }
                    //if (contagem == 31) { id_tipochave = 31; view_path = "GrupoPDF_30_plus"; }
                    //if (contagem == 32) { id_tipochave = 32; view_path = "GrupoPDF_30_plus"; }

                }

                //direcionar para tipo de chave de eliminatória
                if (id_tipoChave == 2)
                {
                    if (contagem > 32) { id_tipochave = 64; view_path = "ChavePDF_64"; }
                    if (contagem < 33 && contagem > 16) { id_tipochave = 32; view_path = "ChavePDF_32_teste"; }
                    if (contagem < 17 && contagem > 8) { id_tipochave = 16; view_path = "ChavePDF_16"; }
                    if (contagem > 4 && contagem < 9) { id_tipochave = 8; view_path = "ChavePDF_8"; }
                    if (contagem == 4) { id_tipochave = 4; view_path = "ChavePDF_4"; }
                    if (contagem == 3) { id_tipochave = 3; view_path = "ChavePDF_3"; }
                    if (contagem == 2) { id_tipochave = 2; view_path = "ChavePDF_2"; }
                }

            }


            string url = string.Format("https://tornfy.com/{5}/{3}?id_classe={0}&id_etapa={1}&nome_chave={2}&id_tipochave={4}", id_cat, id_etapa, nome_chave, view_path, id_tipochave, controller);

            //etapa tipo desafios redirecionar para programação
            if (id_modelo == 3)
            {
                url = string.Format("https://tornfy.com/Progrmacao/ProgramacaoPDF_Desafios_Master?id={0}&bool_anexo={1}&id_categoria={2}", id_etapa, false, id_cat);
            }

            if (id_tipoChave == 1)
            {
                ///Chave_PDFView/Grupo?id_classe=51&id_tipochave=8&nome_chave=Dupla%20Masculina%20C&target=_blank&id_etapa=201&id_esquema=7

                url = string.Format("https://tornfy.com/{5}/EditarChave?id={0}&id_etapa={1}&nome_chave={2}", id_cat, id_etapa, nome_chave, view_path, id_tipochave, controller);
            }





            return url;

        }

        private async Task<string> DownloadPdfFileAsync(int id_cat, int id_etapa, string nome_chave,
            int id_tipoChave, int contagem, string nome_etapa, int id_modelo, string controller)
        {
            string view_path = "";
            int id_tipochave = 0;
            UserDialogs.Instance.ShowLoading(title: "Buscando Jogos");


            //modelos de rei da quadra
            if (id_modelo == 4)
            {
                switch (contagem)
                {
                    case 4:
                        id_tipochave = 4;
                        view_path = "GrupoPDF_ReiQuadra";
                        break;
                    case 8:
                        id_tipochave = 8;
                        view_path = "GrupoPDF_ReiQuadra";
                        break;
                    case 12:
                        id_tipochave = 12;
                        view_path = "GrupoPDF_ReiQuadra";
                        break;
                    case 16:
                        id_tipochave = 16;
                        view_path = "GrupoPDF_ReiQuadra";
                        break;
                    default:
                        id_tipochave = id_tipoChave;
                        view_path = "GrupoPDF_ReiQuadra";
                        break;

                }
            }


            //modelos de tenis e beach (simples e duplas)
            if (id_modelo == 1 || id_modelo == 2)
            {

                //if (id_tipoChave == 1)
                //{
                //	if (contagem == 3) { id_tipochave = 3; view_path = "GrupoPDF_3"; }
                //	if (contagem == 4) { id_tipochave = 4; view_path = "GrupoPDF_4"; }
                //	if (contagem == 5) { id_tipochave = 5; view_path = "GrupoPDF_5"; }
                //	if (contagem == 6) { id_tipochave = 6; view_path = "GrupoPDF_6"; }
                //	if (contagem == 7) { id_tipochave = 7; view_path = "GrupoPDF_7"; }
                //	if (contagem == 8) { id_tipochave = 8; view_path = "GrupoPDF_8"; }
                //	if (contagem == 9) { id_tipochave = 9; view_path = "GrupoPDF_9"; }
                //	if (contagem == 10) { id_tipochave = 10; view_path = "GrupoPDF_10"; }
                //	if (contagem == 11) { id_tipochave = 11; view_path = "GrupoPDF_11"; }
                //	if (contagem == 12) { id_tipochave = 12; view_path = "GrupoPDF_12"; }
                //	if (contagem == 13) { id_tipochave = 13; view_path = "GrupoPDF_13_plus"; }
                //	if (contagem == 14) { id_tipochave = 14; view_path = "GrupoPDF_14"; }
                //	if (contagem == 15) { id_tipochave = 15; view_path = "GrupoPDF_15_plus"; }
                //	if (contagem == 16) { id_tipochave = 16; view_path = "GrupoPDF_16_plus"; }
                //	if (contagem == 17) { id_tipochave = 17; view_path = "GrupoPDF_17"; }
                //	if (contagem == 18) { id_tipochave = 18; view_path = "GrupoPDF_18_plus"; }
                //	if (contagem == 19) { id_tipochave = 19; view_path = "GrupoPDF_18_plus"; }
                //	if (contagem == 20) { id_tipochave = 20; view_path = "GrupoPDF_18_plus"; }
                //	if (contagem == 21) { id_tipochave = 21; view_path = "GrupoPDF_21_plus"; }
                //	if (contagem == 22) { id_tipochave = 22; view_path = "GrupoPDF_21_plus"; }
                //	if (contagem == 23) { id_tipochave = 23; view_path = "GrupoPDF_23_plus"; }
                //	if (contagem == 24) { id_tipochave = 24; view_path = "GrupoPDF_24_plus"; }
                //	if (contagem == 25) { id_tipochave = 25; view_path = "GrupoPDF_24_plus"; }
                //	if (contagem == 26) { id_tipochave = 26; view_path = "GrupoPDF_24_plus"; }
                //	if (contagem == 27) { id_tipochave = 27; view_path = "GrupoPDF_27_plus"; }
                //	if (contagem == 28) { id_tipochave = 28; view_path = "GrupoPDF_27_plus"; }
                //	if (contagem == 29) { id_tipochave = 29; view_path = "GrupoPDF_27_plus"; }
                //	if (contagem == 30) { id_tipochave = 30; view_path = "GrupoPDF_30_plus"; }
                //	if (contagem == 31) { id_tipochave = 31; view_path = "GrupoPDF_30_plus"; }
                //	if (contagem == 32) { id_tipochave = 32; view_path = "GrupoPDF_30_plus"; }

                //}

                //direcionar para tipo de chave de eliminatória
                if (id_tipoChave == 2)
                {
                    if (contagem > 32) { id_tipochave = 64; view_path = "ChavePDF_64_teste"; }
                    if (contagem < 33 && contagem > 16) { id_tipochave = 32; view_path = "ChavePDF_32_teste"; }
                    if (contagem < 17 && contagem > 8) { id_tipochave = 16; view_path = "ChavePDF_16"; }
                    if (contagem > 4 && contagem < 9) { id_tipochave = 8; view_path = "ChavePDF_8"; }
                    if (contagem == 4) { id_tipochave = 4; view_path = "ChavePDF_4"; }
                    if (contagem == 3) { id_tipochave = 3; view_path = "ChavePDF_3"; }
                    if (contagem == 2) { id_tipochave = 2; view_path = "ChavePDF_2"; }
                }

            }

            Guid guid = Guid.NewGuid();
            var filePath = Path.Combine(FileSystem.AppDataDirectory, guid.ToString() + "_" + nome_chave + ".pdf");

            //if (File.Exists(filePath))
            //{
            //	File.Delete(filePath);
            //}


            string url = string.Format("https://tornfy.com/Chave_PDF/{3}?id_classe={0}&id_etapa={1}&nome_chave={2}&id_tipochave={4}", id_cat, id_etapa, nome_chave, view_path, id_tipochave, controller);

            //etapa tipo desafios redirecionar para programação
            if (id_modelo == 3)
            {
                url = string.Format("https://tornfy.com/Progrmacao/ProgramacaoPDF_Desafios_Master?id={0}&bool_anexo={1}&id_categoria={2}", id_etapa, false, id_cat);
            }

            if (id_tipoChave == 1)
            {
                ///Chave_PDFView/Grupo?id_classe=51&id_tipochave=8&nome_chave=Dupla%20Masculina%20C&target=_blank&id_etapa=201&id_esquema=7

                url = string.Format("https://tornfy.com/{5}/EditarChave?id={0}&id_etapa={1}&nome_chave={2}", id_cat, id_etapa, nome_chave, view_path, id_tipochave, controller);
            }



            var httpClient = new HttpClient();
            var pdfBytes = await httpClient.GetByteArrayAsync(url);
            File.WriteAllBytes(filePath, pdfBytes);

            return filePath;

        }
    }
}

        