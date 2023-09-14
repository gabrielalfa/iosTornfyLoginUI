using System;
namespace TornfyApp.Model
{
    public class Token_Etapa
    {
        public bool kit_brinde { get; set; }
        public bool qr_cheking_bool { get; set; }
        public bool status_pagamento { get; set; }
        public int status { get; set; }
        public bool dupla { get; set; }

        public string nome_jogador { get; set; }
        public string nome_etapa { get; set; }

        public int id_inscricao { get; set; }

        public string path_clube { get; set; }

        public bool status_token_habilitacao { get; set; }
        public double tempo_habilitacao { get; set; }
        public string data_habilitacao { get; set; }
        public bool existe_qr { get; set; }

    }
}

