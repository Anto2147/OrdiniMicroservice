namespace Microsoft.Extensions.DependencyInjection;


//configurazione Client HTTP
public class OrdiniClientOptions
{

    /// <summary>
    /// Nome sezione nel file di configurazione "appsettings.json".
    /// </summary>
    public const string SectionName = "OrdiniClientHttp";

    /// <summary>
    /// Base URL di destinazione.
    /// </summary>
    public string BaseAddress { get; set; } = "";

}
