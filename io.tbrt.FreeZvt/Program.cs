using Microsoft.Extensions.Configuration;
using Portalum.Zvt;

#if RELEASE_WIN
using Microsoft.Win32;
#endif

namespace IO.TBRT.FreeZVT
{
    class Program
    {
        private static DataTypes.Options _options = null!;
        private static PaymentTerminal _terminal = null!;
        public static void Main(string[] args)
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                var regDefaultKey = config["Registry:DefaultKey"];
                _options = args.Any() ? Parser.ParseCommandLineArgs(args) : Parser.ParseRegistryArgs(regDefaultKey);
                
                if (!string.IsNullOrEmpty(_options.Eingabedatei))
                    _options = Parser.ParseCommandLineArgs(File.ReadAllText(_options.Eingabedatei).Split());

                if (_options == null) throw new DataTypes.InvalidDataException("invalid arguments.");

                if (_options.Betrag == 0) throw new DataTypes.InvalidDataException("invalid amount argument.");
                if (_options.KasseNr == 0) throw new DataTypes.InvalidDataException("invalid KasseNr argument.");
                if (string.IsNullOrEmpty(_options.COM)) throw new DataTypes.InvalidDataException("invalid COM argument");
                if (_options.COM.Equals("LAN"))
                {
                    if (string.IsNullOrEmpty(_options.IP) || _options.Port == 0)
                        throw new DataTypes.InvalidDataException("invalid ip address or port,");
                }

                _terminal = new PaymentTerminal(_options);
                _terminal.Registration();

                CommandResponse res;
                switch (_options.Funktion)
                {
                    case (DataTypes.FunctionType.Zahlung):
                        res = _terminal.Payment(_options.Betrag);
                        break;
                    case DataTypes.FunctionType.Diagnose:
                        res = _terminal.Diagnosis();
                        break;
                    case DataTypes.FunctionType.Kassenschnitt:
                        res = _terminal.EndOfDay();
                        break;
                    case DataTypes.FunctionType.Storno:
                        res = _terminal.Reversal(_options.StornoBelegNr);
                        break;
                    case DataTypes.FunctionType.GutschriftCreditCard:
                        throw new NotImplementedException();
                        break;
                    case DataTypes.FunctionType.Druckwiederholung:
                        res = _terminal.RepeatReceipt();
                        break;
                    case DataTypes.FunctionType.DruckwiederholungHändler:
                        res = _terminal.RepeatReceipt();
                        break;
                    case DataTypes.FunctionType.DruckwiederholungKunde:
                        res = _terminal.RepeatReceipt();
                        break;
                    case DataTypes.FunctionType.DruckwiederholungTagesabschluss:
                        res = _terminal.RepeatReceipt();
                        break;
                    case DataTypes.FunctionType.TaxFree:
                        throw new NotImplementedException();
                        break;
                    case DataTypes.FunctionType.KontostandabfrageAVSGutscheinkarte:
                        throw new NotImplementedException();
                        break;
                    case DataTypes.FunctionType.Reservierung:
                        throw new NotImplementedException();
                        break;
                    case DataTypes.FunctionType.ReservierungBuchen:
                        throw new NotImplementedException();
                        break;
                    case DataTypes.FunctionType.ReservierungStornieren:
                        throw new NotImplementedException();
                        break;
                    case DataTypes.FunctionType.Trinkgeld:
                        throw new NotImplementedException();
                        break;
                    case DataTypes.FunctionType.Sprachauswahl:
                        throw new NotImplementedException();
                        break;
                    case DataTypes.FunctionType.ReadCardMagnetStreifen:
                        throw new NotImplementedException();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var ausgabe = new DataTypes.Ausgabe();

                switch (res.State)
                {
                    case CommandResponseState.Unknown:
                        ausgabe.Ergebnis = 1;
                        ausgabe.Aktiv = 0;
                        ausgabe.ErgebnisText = res.ErrorMessage;
                        break;
                    case CommandResponseState.Successful:
                        ausgabe.Ergebnis = 0;
                        ausgabe.Aktiv = 0;
                        ausgabe.Autorisierungsergebnis = res.ErrorMessage;
                        break;
                    case CommandResponseState.Abort:
                        ausgabe.Ergebnis = 1;
                        ausgabe.Aktiv = 0;
                        ausgabe.ErgebnisText = res.ErrorMessage;
                        break;
                    case CommandResponseState.Timeout:
                        ausgabe.Ergebnis = 1;
                        ausgabe.Aktiv = 0;
                        ausgabe.ErgebnisText = res.ErrorMessage;
                        break;
                    case CommandResponseState.NotSupported:
                        ausgabe.Ergebnis = 1;
                        ausgabe.Aktiv = 0;
                        ausgabe.ErgebnisText = res.ErrorMessage;
                        break;
                    case CommandResponseState.Error:
                        ausgabe.Ergebnis = 1;
                        ausgabe.Aktiv = 0;
                        ausgabe.ErgebnisText = res.ErrorMessage;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //Todo: logging

                _terminal.Dispose();
                Environment.Exit(-1);
            }
        }
    }
}

