using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShowBisaoAPI.Models
{
    public class PerguntaVM
    {
        public int IdPergunta { get; set; }
        public int Rodada { get; set; }
        public string Pergunta { get; set; }
        public string Premio { get; set; }
        public List<RespostaVM> Respostas { get; set; }
    }
}