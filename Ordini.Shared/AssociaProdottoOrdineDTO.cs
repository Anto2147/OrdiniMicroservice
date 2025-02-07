using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordini.Shared;

public class AssociaProdottoOrdineDTO
{
    //public int Id { get; set; }
    public String NomeProdotto { get; set; }
    public int QuantitaOrdinata { get; set; }
    public int IdOrdine { get; set;}
}
