using System;
using System.Collections.Generic;
using TornfyApp.Model;
using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class tb_historico : TabbedPage
    {
        public tb_historico(Config_Home config, int id_jogador)
        {
            InitializeComponent();

            this.Children.Add(new Meu_Historico(config, id_jogador) { Title = "Simples", IconImageSource = "user_simples.png" });
            this.Children.Add(new Meu_Historico_Duplas(config, id_jogador) { Title = "Duplas", IconImageSource = "user_dupla.png" });

        }

    }
}

