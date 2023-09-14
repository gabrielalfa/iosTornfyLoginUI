using System;
namespace TornfyApp.Model
{
    public class Regras
    {
        public string id_conta_recebimento { get; set; }
        public int id_conta { get; set; }
        public bool oferece_pgo { get; set; }
        public bool cancela_bool { get; set; }
        public int cancela_tempo { get; set; }

        public int qtd_dia { get; set; }
        public int qtd_semana { get; set; }
        public int id { get; set; }
        public int valor_ext { get; set; }
        public int valor_int { get; set; }
        public int ilumina_ext { get; set; }
        public int ilumina_int { get; set; }
        public int horas_libera { get; set; }
        public int tempo_desmarcacao { get; set; }
        public bool cancela_passado { get; set; }
        public bool email_marcacao { get; set; }
        public bool email_lembrate { get; set; }
        public bool valor_usuario { get; set; }
        public bool email_marcacao_adm { get; set; }
        public bool relatorio_dia { get; set; }
        public bool relatorio_semana { get; set; }
        public bool relatorio_mes { get; set; }
        public int lista_emails { get; set; }
        public int id_master { get; set; }


        public string email_1 { get; set; }
        public string email_2 { get; set; }
        public string email_3 { get; set; }

        public string nome_clube { get; set; }
        public string nome_responsavel { get; set; }
        public int id_quadra { get; set; }

        public bool email_rateio { get; set; }
        public bool integral_rateio { get; set; }
        public bool valores_rateio { get; set; }


    }
}

