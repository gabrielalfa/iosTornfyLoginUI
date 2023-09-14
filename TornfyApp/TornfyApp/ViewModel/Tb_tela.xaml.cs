using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class Tb_tela : TabbedPage
    {
        public Tb_tela(int id_etapa, string nome_etapa, string nome_jogador)
        {
            InitializeComponent();

            this.Children.Add(new Programacao_Tela(id_etapa, nome_etapa, nome_jogador) { Title = "Simples", IconImageSource = "user_simples.png" });
            this.Children.Add(new Programacao_Tela_Dupla(id_etapa, nome_etapa, nome_jogador) { Title = "Duplas", IconImageSource = "user_dupla.png" });

        }
    }
}

