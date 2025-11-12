using NUlid;
using System.Text.RegularExpressions;

namespace Aviscom.DTOs.Usuario
{
    public class ContatoResponse
    {
        public Ulid Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        private string _valor = string.Empty;
        public string Valor { get
            {
                if (string.IsNullOrEmpty(_valor))
                    return _valor;

                if (Tipo.Equals("Celular", StringComparison.OrdinalIgnoreCase) && _valor.Length == 11)
                {
                    return Regex.Replace(_valor, @"(\d{2})(\d{5})(\d{4})", "($1) $2-$3");
                }

                if (Tipo.Equals("Telefone", StringComparison.OrdinalIgnoreCase) && _valor.Length == 10)
                {
                    return Regex.Replace(_valor, @"(\d{2})(\d{4})(\d{4})", "($1) $2-$3");
                }

                return _valor;
            } 
            set
            {  
                _valor = value; 
            }
        } 

        public Ulid? FkPessoaFisicaId { get; set; }
        public Ulid? FkPessoaJuridicaId { get; set; }
    }
}
