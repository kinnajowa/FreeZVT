using System;
using System.Collections;
using CommandLine;
using Portalum.Zvt;

namespace IO.TBRT.FreeZVT
{
    class Program
    {
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

        public enum WaerungCode
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
            [Option("Ausgabepfad")]
            public string? Ausgabepfad { get; set; } = null;
            [Option("COM", Required = true)]
            public string COM { get; set; }
            [Option("ComSpeed")]
            public int ComSpeed { get; set; } = 9600;
            [Option("ComStop")]
            public ComStopType ComStop { get; set; } = ComStopType.OneBit;
            [Option("IP")]
            public string? IP { get; set; }
            [Option("Port")]
            public int? Port { get; set; } = 22000;
            [Option("PortKasse")]
            public int? PortKasse { get; set; } = 4102;
            [Option("Type")]
            public DeviceType Typ { get; set; } = DeviceType.Allgemein;
            [Option("Passwort")]
            public int? Passwort { get; set; } = 000000;
            [Option("KasseNr")]
            public int? KasseNr { get; set; }
            [Option("Protokollpfad")]
            public string? Protokollpfad { get; set; }
            [Option("Funktion")]
            public FunctionType Funktion { get; set; } = FunctionType.Zahlung;
            [Option("Betrag")]
            public int Betrag { get; set; }
            [Option("Kassedruck")]
            public DruckType Kassedruck { get; set; } = DruckType.Terminal;
            [Option("Test")]
            public TestType Test { get; set; } = TestType.NoTest;
            [Option("Lizenz")]
            public string? Lizenz { get; set; }
            [Option("Provider")]
            public string? Provider { get; set; }
            [Option("Kartennummer")]
            public string? Kartennummer { get; set; }
            [Option("Kartegueltig")]
            public string? Kartegueltig { get; set; }
            [Option("KarteCVC")]
            public string? KarteCVC { get; set; }
            [Option("PIN")]
            public PinType PIN { get; set; } = PinType.TerminalSettings;
            [Option("Dialog")]
            public DialogType Dialog { get; set; } = DialogType.Standarddialog;
            [Option("StornoBelegNr")]
            public string? StornoBelegNr { get; set; }
            [Option("StornoBetrag")]
            public int? StornoBetrag { get; set; }
            [Option("Abmelden")]
            public AbmeldenType Abmelden { get; set; } = AbmeldenType.Standard;
            [Option("Original")]
            public int Original { get; set; } = 1;
            [Option("Diag")]
            public DiagType Diag { get; set; } = DiagType.Netzwerkdiagnose;
            [Option("Eingabedatei")]
            public string? Eingabedatei { get; set; }
            [Option("ReservierungsBelegNr")]
            public string? ReservierungBelegNr { get; set; }
            [Option("ReservierungrefNr")]
            public string? ReservierungRefNr { get; set; }
            [Option("ReservierungAID")]
            public string? ReservierungAID { get; set; }
            [Option("Sprache")]
            public SpracheCode Sprache { get; set; } = SpracheCode.DE;
            [Option("Waerung")]
            public WaerungCode Waerung { get; set; } = WaerungCode.EUR;
            [Option("TrinkgeldBelegNr")]
            public string? TrinkfgeldBelegNr { get; set; }
            [Option("Alter")]
            public int Alter { get; set; } = 0;
            [Option("Sperre")]
            public SperreType Sperre { get; set; } = SperreType.NichtGesperrt;
            [Option("WWRef")]
            public string? WWRef { get; set; }
            [Option("Warenautomat")]
            public WarenautomatType Warenautomat { get; set; } = WarenautomatType.KeinAutomatAngeschlossen;
            [Option("Breite")]
            public int Breite { get; set; } = 24;
            [Option("Verfahren")]
            public VerfahrenType Verfahren { get; set; } = VerfahrenType.StandardKartenzahlung;
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
            public string KartentypLang { get; set; }
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

        public static void Main(string[] args)
        {
            Options options;
            
            if (args.Any())
            {
                IList<string> formattedArgs = new List<string>();
                for (var i = 0; i < args.Length; i++)
                {
                    var splitted = args[i].Split("=");
                    formattedArgs.Add($"--{splitted[0]}");
                    formattedArgs.Add(splitted[1]);
                }
            
                var parsedInput = Parser.Default.ParseArguments<Options>(formattedArgs.ToArray());
                if (parsedInput.Errors.Any())
                {
                    foreach (var err in parsedInput.Errors)
                    {
                        Console.WriteLine(err.ToString());
                    }
                }

                options = parsedInput.Value;
            }
            else
            {
                //TODO: implement regestry param parsing
            }
            
            
        }
    }
}

