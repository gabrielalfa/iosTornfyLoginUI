using System;
namespace TornfyApp.Model
{
    public class OponentesModel
    {

        public int id_inscricao_j1 { get; set; }
        public int id_inscricao_j2 { get; set; }

        public string elo_rating { get; set; }
        public string elo_rating_j1 { get; set; }
        public string elo_rating_j2 { get; set; }
        public bool tipo_simples { get; set; }
        public string img_path { get; set; }
        public string nome_jogador { get; set; }
        public string nome_jogador1 { get; set; }
        public string nome_jogador2 { get; set; }
        public string categoria { get; set; }
        public int id_oponente { get; set; }

        public string img_path1 { get; set; }
        public string img_path2 { get; set; }
        public string j1 { get; set; }
        public string j2 { get; set; }
        public string nome_dupla_gb_j1 { get; set; }
        public string nome_dupla_gb_j2 { get; set; }
        public int id1_j1 { get; set; }
        public int id1_j2 { get; set; }
        public int id2_j1 { get; set; }
        public int id2_j2 { get; set; }

    }
}

