#if RELEASE_WIN
using Microsoft.Win32;
#endif

namespace IO.TBRT.FreeZVT;

public static class Parser
{
    private static string _regDefaultKey = string.Empty;
    
    public static DataTypes.Options ParseCommandLineArgs(string[] args)
    {
        IList<string> formattedArgs = new List<string>();
        foreach (var arg in args)
        {
            var splitted = arg.Split("=");
            
            if (splitted.Length != 2) continue;
            
            formattedArgs.Add($"--{splitted[0]}");
            formattedArgs.Add(splitted[1]);
        }
            
        var parsedInput = CommandLine.Parser.Default.ParseArguments<DataTypes.Options>(formattedArgs.ToArray());
        if (!parsedInput.Errors.Any()) return parsedInput.Value;
        
        foreach (var err in parsedInput.Errors)
        {
            Console.WriteLine(err.ToString());
        }

        throw new DataTypes.ParsingException();
    }

    public static DataTypes.Options ParseRegistryArgs(string? regDefaultKey)
    {
        if (string.IsNullOrEmpty(regDefaultKey))
            throw new DataTypes.ParsingException("Default regestry path cannot be empty.");
        
        _regDefaultKey = regDefaultKey;
        
#if RELEASE_WIN
#pragma warning disable CA1416
        // ReSharper disable once UseObjectOrCollectionInitializer
        var options = new DataTypes.Options();
        
        options.Ausgabepfad = (string) GetRegEntry("Ausgabepfad", string.Empty);
        options.COM = (string) GetRegEntry("COM", string.Empty);
        options.ComSpeed = (int) GetRegEntry("ComSpeed", options.ComSpeed);
        options.ComStop = (DataTypes.ComStopType) GetRegEntry("ComStop", options.ComStop);
        options.IP = (string) GetRegEntry("IP", string.Empty);
        options.Port = (int) GetRegEntry("Port", options.Port);
        options.PortKasse = (int) GetRegEntry("PortKasse", options.PortKasse);
        options.Typ = (DataTypes.DeviceType) GetRegEntry("Typ", options.Typ);
        options.Passwort = (string) GetRegEntry("Passwort", options.Passwort);
        options.KasseNr = (int) GetRegEntry("KasseNr", options.KasseNr);
        options.Protokollpfad = (string) GetRegEntry("Protokollpfad", options.Protokollpfad);
        options.Funktion = (DataTypes.FunctionType) GetRegEntry("Funktion", options.Funktion);
        options.Betrag = (int) GetRegEntry("Betrag", options.Betrag);
        options.Kassedruck = (DataTypes.DruckType) GetRegEntry("Kassedruck", options.Kassedruck);
        options.Test = (DataTypes.TestType) GetRegEntry("Test", options.Test);
        options.Lizenz = (string) GetRegEntry("Lizenz", options.Lizenz);
        options.Provider = (string) GetRegEntry("Provider", options.Provider);
        options.Kartennummer = (string) GetRegEntry("Kartennummer", options.Kartennummer);
        options.Kartegueltig = (string) GetRegEntry("Kartegueltig", options.Kartegueltig);
        options.KarteCVC = (string) GetRegEntry("KarteCVC", options.KarteCVC);
        options.PIN = (DataTypes.PinType) GetRegEntry("PIN", options.PIN);
        options.Dialog = (DataTypes.DialogType) GetRegEntry("Dialog", options.Dialog);
        options.StornoBelegNr = (string) GetRegEntry("StornoBelegNr", options.StornoBelegNr);
        options.StornoBetrag = (int) GetRegEntry("StornoBetrag", options.StornoBetrag);
        options.Abmelden = (DataTypes.AbmeldenType) GetRegEntry("Abmelden", options.Abmelden);
        options.Original = (int) GetRegEntry("Original", options.Original);
        options.Diag = (DataTypes.DiagType) GetRegEntry("Diag", options.Diag);
        options.Eingabedatei = (string) GetRegEntry("Eingabedatei", options.Eingabedatei);
        options.ReservierungBelegNr = (string) GetRegEntry("ReservierungBelegNr", options.ReservierungBelegNr);
        options.ReservierungRefNr = (string) GetRegEntry("ReservierungRefNr", options.ReservierungRefNr);
        options.ReservierungAID = (string) GetRegEntry("ReservierungAID", options.ReservierungAID);
        options.Sprache = (DataTypes.SpracheCode) GetRegEntry("Sprache", options.Sprache);
        options.Waehrung = (DataTypes.WaehrungCode) GetRegEntry("Waehrung", options.Waehrung);
        options.TrinkfgeldBelegNr = (string) GetRegEntry("TrinkfgeldBelegNr", options.TrinkfgeldBelegNr);
        options.Alter = (int) GetRegEntry("Alter", options.Alter);
        options.Sperre = (DataTypes.SperreType) GetRegEntry("Sperre", options.Alter);
        options.WWRef = (string) GetRegEntry("WWRef", options.WWRef);
        options.Warenautomat = (DataTypes.WarenautomatType) GetRegEntry("Warenautomat", options.Warenautomat);
        options.Breite = (int) GetRegEntry("Breite", options.Breite);
        options.Verfahren = (DataTypes.VerfahrenType) GetRegEntry("Verfahren", options.Verfahren);

        return options;
#pragma warning restore CA1416
#elif RELEASE_UNIX
        throw new DataTypes.ParsingException("Registry is only available on a Windows platform.");
#endif
    }

#if RELEASE_WIN
    private static object GetRegEntry(string name, object? defaultVal)
    {
#pragma warning disable CA1416
        return Registry.GetValue(_regDefaultKey, name, defaultVal)!;
#pragma warning restore CA1416
    }
#endif
}