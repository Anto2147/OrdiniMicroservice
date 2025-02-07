using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordini.Repository.Model;

    public class Cliente
    {
        public int Id { get; set; } // Chiave primaria
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public ICollection<Ordine> Ordini { get; set; } = new List<Ordine>(); //un cliente puo fare piu ordini

}

