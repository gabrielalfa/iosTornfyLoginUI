using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TornfyApp.ViewModel
{
    public partial class tb_stats : TabbedPage
    {
        public int ano_geral = DateTime.Now.Year;
        public int id_categoria_geral;
        public string elo_geral;
        public string img_jogador_geral;
        public string Nome_Jogador_geral;

        public int id_jogador_geral;

        public tb_stats(int id_jogador, int id_master)
        {
            InitializeComponent();

            id_jogador_geral = id_jogador;
                        
            this.Children.Add(new PerfilModeloTeste(id_jogador_geral, id_master, id_categoria_geral, ano_geral, this) { Title = "Stats", IconImageSource = "stats_menu32.png" });
            this.Children.Add(new Conquistas(id_jogador_geral, id_master, id_categoria_geral, this) { Title = "Conquistas", IconImageSource = "menu_ranking32.png" });
            this.Children.Add(new Oponentes(id_jogador_geral, this) { Title = "Oponentes", IconImageSource = "game32.png" });
            this.Children.Add(new EtapaPerformance(id_jogador_geral, id_master, id_categoria_geral, this) { Title = "Etapas", IconImageSource = "menu_regulamento.png" });

        }
    }
}

