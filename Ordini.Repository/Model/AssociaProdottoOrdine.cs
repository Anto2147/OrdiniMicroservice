using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordini.Repository.Model;

    public class AssociaProdottoOrdine
    {
    public int Id { get; set; }
    public String NomeProdotto { get; set; }
   
    public int QuantitaOrdinata { get; set; }
    public int IdOrdine { get; set; } //chiave esterna per Ordini
    public Ordine Ordine { get; set; } //un ordine puo avere piu AssocaizioniProddottoOrdine
}
    

