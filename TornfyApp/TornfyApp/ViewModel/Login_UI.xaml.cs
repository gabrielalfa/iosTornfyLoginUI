	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Acr.UserDialogs.Extended;
using Rg.Plugins.Popup.Extensions;
using TornfyApp.API;
using TornfyApp.Model;
using TornfyApp.ViewModelAdm;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login_UI : ContentPage
    {

        public List<Temporada> lista_circuitos;
        public string color = "#3897f0";
        public string path = "";
        public string circuito = "";
        public int master = 1;
        public string obs = "";
        public string info = "";
        public static Jogador info_jogador;
        public static string email;
        public static string senha;
        public Login info_login;
        public int id_jogador;
        public Task<int> id_jogador_login;
        public List<Login> circ_favs;
        public Config_Home config = new Config_Home();
        public int acao = 1; //1 login; 2 validar CPF 
        private static readonly Random getrandom = new Random();
        public int ID_Jogador_pos;

        public string CPF;
        public string Email;
        public string Primeiro_nome;
        public string Segundo_Nome;

        public Usuario usuario;

        public Login_UI(bool delete, int id_jogador_pos, string email_pos, string pass_pos)
        {
            //logout
            if (delete)
            {
                SecureStorage.Remove("id_jogador");
                SecureStorage.Remove("password");
                SecureStorage.Remove("email");
                SecureStorage.Remove("path");
            }

            InitializeComponent();

            config.master = 1;
            CarregarSecureStorage();
            if (id_jogador_pos != 0) { CarregarDadosJogadorAsync(id_jogador_pos, email_pos, pass_pos); }

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            Login_click_tab();

            if (id_jogador != 0)
            {
                config.master = 1;
                Navigation.PushAsync(new MasterPage(config, id_jogador, 1), true);
            }
        }

        protected void Login_click_tab()
        {
            segmentHelper(selectSegmentLabel: loginLabel, selectSegmentBox: loginBox, unSelectSegmentLabel: signupLabel, unSelectSegmentBox: signUpBox);

            //limpar campos
            lbl_cpf.Text = string.Empty;
            lbl_Telefone.Text = string.Empty;
            lbl_Email.Text = string.Empty;
            lbl_Pass.Text = string.Empty;
            lbl_Nome_jogador.Text = string.Empty;
            lbl_datanasc.Text = string.Empty;

            linha_div.IsVisible = false;
            voltar_login.IsVisible = false;
            txt_DataNasc.IsVisible = false;
            txt_CPF.IsVisible = false;
            txt_Telefone.IsVisible = false;
            txt_Email.IsVisible = true;
            txt_Pass.IsVisible = true;
            stk_controlls.IsVisible = true;
            txt_Nome_jogador.IsVisible = false;

            btn_global.Text = "LOGIN";
            lbl_title.Text = "Acesso";
            subtitulo.Text = "Acesse seus dados, sua performance, inscrições, pagamentos, locações e muito mais.";
            btn_gradiente.BackgroundGradientEndColor = Color.FromHex("#ffd900");
            btn_gradiente.BackgroundGradientStartColor = Color.FromHex("#F3A866");

            acao = 1;
        }

        protected void Habilitar_Lembrar_senha(object sender, EventArgs e)
        {
            segmentHelper(selectSegmentLabel: loginLabel, selectSegmentBox: loginBox, unSelectSegmentLabel: signupLabel, unSelectSegmentBox: signUpBox);

            //limpar campos
            lbl_cpf.Text = string.Empty;
            lbl_Telefone.Text = string.Empty;
            lbl_Email.Text = string.Empty;
            lbl_Pass.Text = string.Empty;
            lbl_Nome_jogador.Text = string.Empty;
            lbl_datanasc.Text = string.Empty;

            linha_div.IsVisible = false;
            voltar_login.IsVisible = false;
            txt_DataNasc.IsVisible = false;
            txt_CPF.IsVisible = true;
            txt_Telefone.IsVisible = false;
            txt_Email.IsVisible = false;
            txt_Pass.IsVisible = false;
            txt_Nome_jogador.IsVisible = false;

            btn_global.Text = "Enviar Lembrete";
            lbl_title.Text = "Lembrar Senha";
            subtitulo.Text = "Informe seu CPF e enviaremos para o email de cadastro sua senha para acesso!";
            btn_gradiente.BackgroundGradientEndColor = Color.FromHex("#00E589");
            btn_gradiente.BackgroundGradientStartColor = Color.FromHex("#00BF9E");

            acao = 9;
        }

        private async void Lembrar_Senha()
        {

            if (string.IsNullOrEmpty(lbl_cpf.Text))
            {
                var pop = new MessageBox("Informe os Dados!", "Informe o cpf solicitados no formulario.");
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            }
            else
            {
                if (TestarInternet())
                {
                    Formulario_Inscricao dados = new Formulario_Inscricao
                    {
                        cpf = lbl_cpf.Text
                    };

                    RetornoEnvio resultado = await API_Service.Lembrar_Senha_CPF(dados);

                    // 200 = cpf (encontrado) / email (encontratdo e viculado ao cpf) / email enviado 
                    // 500 = cpf inválido
                    // 400 = cpf inesistente
                    // 600 = email inválido
                    // 300 = email inisistente

                    int retorno = resultado.codigo_retorno;

                    switch (retorno)
                    {
                        case 200:
                            var viewPage = new Login_UI(false, 0, "", "");
                            var navigation = Application.Current.MainPage as NavigationPage;
                            var pop5 = new MessageBox("Lembrete de senha enviado!", string.Format("Um lembrete de senha foi enviado para o email: '{0}'", resultado.email_retorno));
                            _ = App.Current.MainPage.Navigation.PushPopupAsync(pop5, true);
                            Login_click_tab();
                            break;
                        case 500:
                            var pop1 = new MessageBox("CPF inválido!", "Informe um cpf verdadeiro!");
                            _ = App.Current.MainPage.Navigation.PushPopupAsync(pop1, true);
                            break;
                        case 400:
                            var pop2 = new MessageBox("CPF inesistente!", "Informe um CPF cadastrado em nossa base ou cadastre-se!");
                            _ = App.Current.MainPage.Navigation.PushPopupAsync(pop2, true);
                            break;
                        case 600:
                            var pop3 = new MessageBox("Email inválido!", "Informe um email verdadeiro!");
                            _ = App.Current.MainPage.Navigation.PushPopupAsync(pop3, true);
                            break;
                        case 300:
                            var pop4 = new MessageBox("Email inválido!", "Informe um email cadastrado em nossa base ou cadastre-se!");
                            _ = App.Current.MainPage.Navigation.PushPopupAsync(pop4, true);
                            break;
                    }



                }
            }


        }

        void LoginClick(System.Object sender, System.EventArgs e)
        {
            Login_click_tab();
        }

        void ValidarCPF(System.Object sender, System.EventArgs e)
        {
            segmentHelper(selectSegmentLabel: signupLabel, selectSegmentBox: signUpBox, unSelectSegmentLabel: loginLabel, unSelectSegmentBox: loginBox);
            txt_Nome_jogador.IsVisible = false;
            txt_Email.IsVisible = false;
            txt_Pass.IsVisible = false;
            txt_CPF.IsVisible = true;
            stk_controlls.IsVisible = false;
            linha_div.IsVisible = true;
            voltar_login.IsVisible = true;
            btn_global.Text = "Validar CPF";
            lbl_title.Text = "Validação CPF";
            subtitulo.Text = "Para registar é necessário validar seu CPF, para termos certeza que não há duplicidade no sistema.";

            btn_gradiente.BackgroundGradientEndColor = Color.FromHex("#D72367");
            btn_gradiente.BackgroundGradientStartColor = Color.FromHex("#F3A866");


            acao = 2;


        }

        public async void step_2()
        {
            //validar CPF
            bool valido = false;

            UserDialogs.Instance.ShowLoading(title: "Validando CPF");

            if (!string.IsNullOrEmpty(lbl_cpf.Text))
            {
                if (IsCpf(lbl_cpf.Text))
                {
                    if (lbl_cpf.Text != "000.000.000-00")
                    {
                        //verificar no banco de dados a existencia do CPF
                        bool cpf_exist = await API_Service.BuscaCPF_BD(lbl_cpf.Text, 1);
                        if (!cpf_exist)
                        {
                            acao = 3; //nome email
                            valido = true;
                            //var viewPage = new Registro(lbl_cpf.Text);
                            //var navigation = Application.Current.MainPage as NavigationPage;
                            //await navigation.PushAsync(viewPage, true);
                        }
                        else
                        {
                            //caso CPF já exista na base de dados
                            var pop = new MessageBox("CPF Já Existente!", "Não é permitida da duplicidade de CPF na Base de dados!");
                            await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                        }
                    }
                    else
                    {
                        //caso CPF não seja validado pelo método IsCpf
                        var pop = new MessageBox("CPF Inválido!", "Para registra é necessário informar CPF válido!");
                        await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                    }

                }
                else
                {
                    //caso CPF não seja validado pelo método IsCpf
                    var pop = new MessageBox("CPF Inválido!", "Para registrar é necessário informar CPF válido!");
                    await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                }
            }
            else
            {
                //caso CPF não seja validado pelo método IsCpf
                var pop = new MessageBox("CPF Inválido!", "Para registrar é necessário informar CPF válido!");
                await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            }



            UserDialogs.Instance.HideLoading();

            if (valido)
            {
                acao = 3; //nome email
                txt_CPF.IsVisible = false;
                txt_Nome_jogador.IsVisible = true;
                txt_Email.IsVisible = true;
                this.CPF = lbl_cpf.Text.Trim();

                btn_global.Text = "Validar seu Email";
                lbl_title.Text = "Nome & Email";
                subtitulo.Text = "Informe seu nome com apenas 1(um) sobrenome e também um email que será usado para acesso posterior.";

            }



        }



        public bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        public async void step_3()
        {
            txt_CPF.IsVisible = false;
            txt_Nome_jogador.IsVisible = false;
            txt_Email.IsVisible = false;

            bool valido = false;





            //validar campos
            bool validacao = true;
            if (string.IsNullOrEmpty(lbl_Nome_jogador.Text)) validacao = false;
            if (string.IsNullOrEmpty(lbl_Email.Text)) validacao = false;
            if (string.IsNullOrEmpty(lbl_cpf.Text)) validacao = false;


            //campos em branco
            if (!validacao)
            {
                var pop = new MessageBox("Informe os Dados!", "Informe todos os dados solicitados!");
                await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            }

            //campos ok
            if (validacao)
            {
                //valida caracteres de email
                Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

                //valida email
                if (!rg.IsMatch(lbl_Email.Text.Trim()))
                {
                    var pop = new MessageBox("Email Inválido!", "Para registrar é necessário informar email válido!");
                    await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                }

                //verifica se for email valido
                if (rg.IsMatch(lbl_Email.Text.Trim()))
                {
                    //verificar no banco de dados a existencia do email
                    bool email_exist = await API_Service.BuscaEmail_BD(lbl_Email.Text.Trim(), 1);
                    if (email_exist)
                    {
                        var pop = new MessageBox("Email Existente!", "Este email já existe na base de dados. A duplicidade não é permitida!");
                        await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                    }
                    else
                    {
                        string nome = lbl_Nome_jogador.Text.Trim();
                        string[] partesDoNome = nome.Split(' ');

                        try
                        {
                            if (partesDoNome.Length >= 2)
                            {
                                // Verifica se o último elemento é uma preposição (pode ser personalizado)
                                string[] preposicoes = { "de", "da", "do", "dos", "das" };
                                string ultimoNome = partesDoNome[partesDoNome.Length - 1].ToLower();

                                if (preposicoes.Contains(ultimoNome))
                                {
                                    // Se for uma preposição, exclua o último elemento
                                    partesDoNome = partesDoNome.Take(partesDoNome.Length - 1).ToArray();
                                }

                                // Formata o nome e o sobrenome
                                this.Primeiro_nome = partesDoNome[0];
                                this.Segundo_Nome = partesDoNome.Length > 1 ? partesDoNome[partesDoNome.Length - 1] : "";

                                this.Email = lbl_Email.Text.Trim();

                                //critérios validos
                                valido = true;

                            }
                            else
                            {
                                var pop = new MessageBox("Nome Inválido!", "Informe um nome e um sobrenome!");
                                await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                            }

                        }
                        catch (Exception ex)
                        {
                            var pop = new MessageBox("Nome Inválido!", "Informe um nome e um sobrenome!");
                            await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                        }





                        acao = 4; //telefone senha
                    }
                }



            }

            if (valido)
            {
                txt_Telefone.IsVisible = true;
                txt_Pass.IsVisible = true;
                txt_DataNasc.IsVisible = true;

                btn_global.Text = "Registrar";
                lbl_title.Text = "Telefone & Senha";
                subtitulo.Text = "Para finalizar seu registro informe um telefone com DDD e uma senha de acesso para login posterior.";
            }




        }

        public async void step_4()
        {
            //validar campos
            bool validacao = true;
            if (string.IsNullOrEmpty(lbl_Telefone.Text)) validacao = false;
            if (string.IsNullOrEmpty(lbl_Pass.Text)) validacao = false;
            if (string.IsNullOrEmpty(lbl_datanasc.Text)) validacao = false;

            //campos em branco
            if (!validacao)
            {
                var pop = new MessageBox("Informe os Dados!", "Informe todos os dados solicitados!");
                await App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            }


            int id_master = 1;
            int gd = getrandom.Next(111111, 999999);

            Usuario usuario = new Usuario
            {
                Telefone = lbl_Telefone.Text.Trim(),
                Password = lbl_Pass.Text.Trim(),
                Nascimento = lbl_datanasc.Text.Trim(),
                CPF = this.CPF,
                Email = this.Email,
                Primeiro_nome = this.Primeiro_nome,
                Segundo_Nome = this.Segundo_Nome,
                clube_beach = 10,
                id_Clube = 10,
                gd = gd.ToString(),
                img_path = "/Content/images/Jogadores/user_icon_blue.png",
                id_master = id_master,
            };

            this.usuario = usuario;

            ID_Jogador_pos = await API_Service.RegistrarJogador(usuario);

            if (ID_Jogador_pos == 0)
            {
                await DisplayAlert("Adição não realizada", "Falha ao salvar jogador!!!", "Fechar");
            }


            //var viewPage = new LoginPage_oficial(true, ID_Jogador_pos, txtEmail.Text.Trim(), txtSenha.Text.Trim());
            //var navigation = Application.Current.MainPage as NavigationPage;
            //await navigation.PushAsync(viewPage, true);

            //Validacao_email(txtEmail.Text.Trim(), gd.ToString(), txtSenha.Text.Trim(), ID_Jogador_pos);

            config.master = 1;
            CarregarSecureStorage();
            if (ID_Jogador_pos != 0) { CarregarDadosJogadorAsync(ID_Jogador_pos, lbl_Email.Text.Trim(), lbl_Pass.Text.Trim()); }


        }

        private void segmentHelper(Label selectSegmentLabel, BoxView selectSegmentBox, Label unSelectSegmentLabel, BoxView unSelectSegmentBox)
        {
            selectSegmentLabel.FontAttributes = FontAttributes.Bold;
            selectSegmentBox.BackgroundColor = Color.FromHex("#1F54C3");
            unSelectSegmentLabel.FontAttributes = FontAttributes.None;
            unSelectSegmentBox.BackgroundColor = Color.White;
        }

        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {

            var view = (View)sender;

            // Crie a animação de clique
            await view.ScaleTo(0.95, 50, Easing.Linear);
            await Task.Delay(90);
            await view.ScaleTo(1, 50, Easing.Linear);

            switch (acao)
            {
                case 1:
                    //logar
                    Logar();
                    break;
                case 2:
                    //validar CPF
                    step_2();
                    break;
                case 3:
                    //nome email
                    //validar email
                    step_3();
                    break;
                case 4:
                    //registrar
                    //validar telefone senha data nasc
                    step_4();
                    break;
                case 9:
                    //lembrar senha
                    Lembrar_Senha();
                    break;
                default:
                    break;
            }
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            //Voltar login Label
            Login_click_tab();
        }

        private void TapGestureRecognizer_LembrarSenha(object sender, EventArgs e)
        {
            //Voltar login Label
            Login_click_tab();
        }

        private async void Logar()
        {
            if (string.IsNullOrEmpty(lbl_Email.Text) || string.IsNullOrEmpty(lbl_Pass.Text))
            {
                var pop = new MessageBox("Informe os Dados!", "Informe todos os dados solicitados no formulario.");
                _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
            }
            else
            {
                email = lbl_Email.Text;
                senha = lbl_Pass.Text;
                bool login_adm = false;
                UserDialogs.Instance.ShowLoading(title: "Buscando Dados");
                if (!login_adm)
                {
                    Jogador info_jogador = await API_Service.ObterDadosLogin_teste(lbl_Email.Text, lbl_Pass.Text, 1);
                    if (info_jogador != null)
                    {
                        if (info_jogador.Email.ToLower().Trim() == lbl_Email.Text.ToLower().Trim() && info_jogador.password.ToLower().Trim() == lbl_Pass.Text.ToLower().Trim())
                        {
                            id_jogador = info_jogador.id_jogador;
                            config.nome_jogador = info_jogador.Nome_Jogador;
                            config.email_jogador = info_jogador.Email;

                            add_login(info_jogador.id_jogador.ToString(), lbl_Email.Text.Trim(), lbl_Pass.Text.Trim(),
                                info_jogador.Nome_Jogador, info_jogador.Path, info_jogador.telefone);

                            await Navigation.PushAsync(new MasterPage(config, id_jogador, 1), true);
                            Navigation.RemovePage(this);
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            var pop = new MessageBox("Dados Incorretos!", "Informe novamente seus dados ou confira se seu cadastro é valido!");
                            _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        var pop = new MessageBox("Dados Incorretos!", "Informe novamente seus dados ou confira se seu cadastro é valido!");
                        _ = App.Current.MainPage.Navigation.PushPopupAsync(pop, true);
                    }
                }
                else
                {
                    //acessar adm
                    //await Navigation.PushAsync(new tb_Adm());

                    var viewPage = new tb_Adm();
                    var navigation = Application.Current.MainPage as NavigationPage;
                    await navigation.PushAsync(viewPage, true);
                }

                UserDialogs.Instance.HideLoading();


            }

        }

        public async void CarregarDadosJogadorAsync(int id_jogador, string email_pos, string pass_pos)
        {

            if (TestarInternet())
            {
                Jogador info_jogador = await API_Service.ObterDadosLogin_teste(email_pos, pass_pos, 1);

                id_jogador = info_jogador.id_jogador;
                config.nome_jogador = info_jogador.Nome_Jogador;
                config.email_jogador = info_jogador.Email;

                add_login(id_jogador.ToString(), email_pos, pass_pos, info_jogador.Nome_Jogador, info_jogador.Path, info_jogador.telefone);

                await Navigation.PushAsync(new MasterPage(config, id_jogador, 1), true);
                Navigation.RemovePage(this);

            }
            else
            {
                _ = Navigation.PushAsync(new NoInternet());
            }

        }

        public static bool TestarInternet()
        {
            bool coneccao = true;
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                coneccao = false;
            }
            return coneccao;
        }
    
        public async void CarregarSecureStorage()
        {
            string id_jogador_str = await SecureStorage.GetAsync("id_jogador");
            if (!string.IsNullOrEmpty(id_jogador_str)) { id_jogador = int.Parse(id_jogador_str); } else { id_jogador = 0; }
            string pass = await SecureStorage.GetAsync("password");
            string _email = await SecureStorage.GetAsync("email");

            string nome_jogador = await SecureStorage.GetAsync("nome_jogador");
            string path = await SecureStorage.GetAsync("path");

            if (pass != null) { lbl_Pass.Text = pass.ToString(); }
            if (_email != null) { lbl_Email.Text = _email.ToString(); }

        }

        private async void add_login(string id_jogador, string email, string pass, string nome_jogador, string path, string telefone)
        {
            await SecureStorage.SetAsync("id_jogador", id_jogador);
            await SecureStorage.SetAsync("password", pass);
            await SecureStorage.SetAsync("email", email);
            await SecureStorage.SetAsync("nome_jogador", nome_jogador);
            await SecureStorage.SetAsync("path", path);
            await SecureStorage.SetAsync("telefone", telefone);

        }
    }
}

