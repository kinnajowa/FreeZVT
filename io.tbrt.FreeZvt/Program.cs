using Microsoft.Extensions.Configuration;
using Portalum.Zvt;
using Portalum.Zvt.Models;

namespace IO.TBRT.FreeZVT
{
    class Program
    {
        private static DataTypes.Options _options = null!;
        private static PaymentTerminal _terminal = null!;
        private static TransactionStatus _status = null!;
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

                _status = new TransactionStatus(regDefaultKey, _options);
                _status.SetActive(true);
                _status.Update();

                _terminal = new PaymentTerminal(_options, _status);
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
                
                switch (res.State)
                {
                    case CommandResponseState.Successful:
                        Environment.ExitCode = 0;
                        break;
                    default:
                        Environment.ExitCode = 1;
                        break;
                }
                
                _status.SetActive(false);
                _status.Update();
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //Todo: logging

                if (_terminal != null) _terminal.Dispose();
                _status.SetActive(false);
                _status.Update();
                Environment.Exit(1);
            }
        }
    }
}

