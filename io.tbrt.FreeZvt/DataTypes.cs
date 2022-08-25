using System.Runtime.Serialization;
using CommandLine;

namespace IO.TBRT.FreeZVT;

public class DataTypes
{
    private DataTypes()
    {
        throw new NotImplementedException();
    }

    [Serializable]
    public class ParsingException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ParsingException()
        {
        }

        public ParsingException(string message) : base(message)
        {
        }

        public ParsingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ParsingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class InvalidDataException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public InvalidDataException()
        {
        }

        public InvalidDataException(string message) : base(message)
        {
        }

        public InvalidDataException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidDataException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class ConnectionFailureException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ConnectionFailureException()
        {
        }

        public ConnectionFailureException(string message) : base(message)
        {
        }

        public ConnectionFailureException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ConnectionFailureException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    public enum ComStopType
    {
        OneBit = 1,
        TwoBit = 2
    }

    public enum DeviceType
    {
        Allgemein = 0,
        //TODO: add device types
    }

    public enum FunctionType
    {
        Zahlung = 0,
        Diagnose = 1,
        Kassenschnitt = 2,
        Storno = 3,
        GutschriftCreditCard = 4,
        Druckwiederholung = 5,
        DruckwiederholungHändler = 51,
        DruckwiederholungKunde = 52,
        DruckwiederholungTagesabschluss = 53,
        TaxFree = 6,
        KontostandabfrageAVSGutscheinkarte = 7,
        Reservierung = 8,
        ReservierungBuchen = 9,
        ReservierungStornieren = 10,
        Trinkgeld = 11,
        Sprachauswahl = 12,
        ReadCardMagnetStreifen = 13
    }

    public enum DruckType
    {
        Terminal = 0,
        Kasse = 1
    }

    public enum TestType
    {
        NoTest = 0,
        TestMode = 1
    }

    public enum PinType
    {
        TerminalSettings = 0,
        Geldkartenzahlung = 1,
        UnterschriftzahlungOnline = 2,
        PinZahlung = 3,
        OfflineLastschriftELV = 4
    }

    public enum DialogType
    {
        Standarddialog = 0,
        NurStatusanzeige = 1,
        OhneLogo = 2,
        OhneDialog = 3
    }

    public enum AbmeldenType
    {
        Standard = 1,
        NoLogout = 0
    }

    public enum DiagType
    {
        Netzwerkdiagnose = 1,
        Erweitert = 2
    }

    public enum SpracheCode
    {
        DE,
        EN,
        FR,
        ES,
        NL,
        IT,
        PL
    }

    public enum WaehrungCode
    {
        EUR,
        CHF,
        PLN
    }

    public enum SperreType
    {
        NichtGesperrt = 0,
        ManuelleEingabeGesperrt = 1
    }

    public enum WarenautomatType
    {
        KeinAutomatAngeschlossen = 0,
        WaitForOutput = 1,
        OutputSuccessful = 2,
        OutputNotSuccessful = 3,
        OutputParciallySuccessful = 4
    }

    public enum VerfahrenType
    {
        StandardKartenzahlung = 0,
        BarzahlungNurStatistik = 1,
        AliPay = 2
    }

    public enum AktivType
    {
        AnwendungLaeuft = 1,
        AnwendungBeendet = 0,
        ZahlungErfolgreichWartenAufWarenausgabe = 2
    }

    public enum ZahlType
    {
        Offline = 40,
        KarteImTerminalGeprueft = 50,
        Online = 60,
        PinZahlung = 70
    }

    public enum AuthentifizierungType
    {
        Keine = 00,
        Unterschrift = 01,
        Online = 02,
        Offline = 03,
        Offline2 = 04,
        Offline3 = 05,
        Offline4 = 06,
        Online2 = 07
    }

    public enum AltersverifikationType
    {
        MindestalterNichtErreicht = 0,
        MindestalterErreicht = 1,
        KarteNichtGeeignet = 2,
        TerminalNichtGeeignet = 3
    }

    public class Options
    {
        [Option("Ausgabepfad")] public string Ausgabepfad { get; set; } = string.Empty;
        [Option("COM", Required = true)] public string COM { get; set; } = string.Empty;
        [Option("ComSpeed")] public int ComSpeed { get; set; } = 9600;
        [Option("ComStop")] public ComStopType ComStop { get; set; } = ComStopType.OneBit;
        [Option("IP")] public string IP { get; set; } = string.Empty;
        [Option("Port")] public int Port { get; set; } = 22000;
        [Option("PortKasse")] public int PortKasse { get; set; } = 4102;
        [Option("Type")] public DeviceType Typ { get; set; } = DeviceType.Allgemein;
        [Option("Passwort")] public string Passwort { get; set; } = string.Empty;
        [Option("KasseNr")] public int KasseNr { get; set; } = 0;

        [Option("Protokollpfad")] public string Protokollpfad { get; set; } = Path.Combine(new[] { Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GUB", "ZVTLOG"});

        [Option("Funktion")] public FunctionType Funktion { get; set; } = FunctionType.Zahlung;
        [Option("Betrag")] public int Betrag { get; set; }
        [Option("Kassedruck")] public DruckType Kassedruck { get; set; } = DruckType.Terminal;
        [Option("Test")] public TestType Test { get; set; } = TestType.NoTest;

        [Option("Lizenz")] public string Lizenz { get; set; } = string.Empty;
        [Option("Provider")] public string Provider { get; set; } = string.Empty;
        [Option("Kartennummer")] public string Kartennummer { get; set; } = string.Empty;
        [Option("Kartegueltig")] public string Kartegueltig { get; set; } = string.Empty;
        [Option("KarteCVC")] public string KarteCVC { get; set; } = string.Empty;
        [Option("PIN")] public PinType PIN { get; set; } = PinType.TerminalSettings;
        [Option("Dialog")] public DialogType Dialog { get; set; } = DialogType.Standarddialog;
        [Option("StornoBelegNr")] public string StornoBelegNr { get; set; } = string.Empty;
        [Option("StornoBetrag")] public int StornoBetrag { get; set; }
        [Option("Abmelden")] public AbmeldenType Abmelden { get; set; } = AbmeldenType.Standard;
        [Option("Original")] public int Original { get; set; } = 1;
        [Option("Diag")] public DiagType Diag { get; set; } = DiagType.Netzwerkdiagnose;
        [Option("Eingabedatei")] public string Eingabedatei { get; set; } = string.Empty;
        [Option("ReservierungsBelegNr")] public string ReservierungBelegNr { get; set; } = string.Empty;
        [Option("ReservierungrefNr")] public string ReservierungRefNr { get; set; } = string.Empty;
        [Option("ReservierungAID")] public string ReservierungAID { get; set; } = string.Empty;
        [Option("Sprache")] public SpracheCode Sprache { get; set; } = SpracheCode.DE;
        [Option("Waehrung")] public WaehrungCode Waehrung { get; set; } = WaehrungCode.EUR;
        [Option("TrinkgeldBelegNr")] public string TrinkfgeldBelegNr { get; set; } = string.Empty;
        [Option("Alter")] public int Alter { get; set; }
        [Option("Sperre")] public SperreType Sperre { get; set; } = SperreType.NichtGesperrt;
        [Option("WWRef")] public string WWRef { get; set; } = string.Empty;

        [Option("Warenautomat")]
        public WarenautomatType Warenautomat { get; set; } = WarenautomatType.KeinAutomatAngeschlossen;

        [Option("Breite")] public int Breite { get; set; } = 24;
        [Option("Verfahren")] public VerfahrenType Verfahren { get; set; } = VerfahrenType.StandardKartenzahlung;
    }

    public class Ausgabe
    {
        public string? START { get; set; }
        public AktivType Aktiv { get; set; }
        public int Ergebnis { get; set; }
        public string? ErgebnisText { get; set; }
        public string? ErgebnisLang { get; set; }
        public string? Autorisierungsergebnis { get; set; }
        public string? PAN { get; set; }
        public int Betrag { get; set; }
        public int Kartentyp { get; set; }
        public string? KartentypLang { get; set; }
        public string? BLZ { get; set; }
        public string? Kontonummer { get; set; }
        public string? Drucktext { get; set; }
        public string? Drucktext2 { get; set; }
        public string? Haendlerbeleg { get; set; }
        public string BelegNr { get; set; }
        public ZahlType Zahlart { get; set; }
        public AuthentifizierungType Authentifizierung { get; set; }
        public string? TID { get; set; }
        public string? Geraetetyp { get; set; }
        public string? Softwareversion { get; set; }
        public string RefNr { get; set; }
        public string AID { get; set; }
        public AltersverifikationType Altersverifikation { get; set; }
    }
}