using System;

namespace ShowBisaoAPI.Models
{
    public class RespostaVM
    {
        public int IdAlternativa { get; set; }
        public int Rodada { get; set; }
        public string Alternativa { get; set; }
        public Nullable<bool> EhCorreta { get; set; }
        public int IdPergunta { get; set; }
    }
}