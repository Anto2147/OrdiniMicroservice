using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace Ordini.Repository.Model;

//un Cliente puo effettuare piu ordine (IdCliente puo ripetersi mentre IdOrdine è unico)
public class Ordine
{
    public int Id { get; set; } // Chiave primaria
    public bool StatoPagamento { get; set; }
    public int IdCliente { get; set; } //chiave esterna per Cliente
    public decimal TotaleDaPagare { get; set; }
    public Cliente Cliente { get; set; } //un cliente puo essere associato a piu ordini
    public ICollection<AssociaProdottoOrdine> AssociaProdottiOrdine { get; set; } = new List<AssociaProdottoOrdine>(); //Un ordine puo avere lista di prodotti
    
}

