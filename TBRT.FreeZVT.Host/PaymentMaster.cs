using Microsoft.Win32;
using Portalum.Zvt;
using Portalum.Zvt.Models;

namespace TBRT.FreeZVT.Host;

public class PaymentMaster
{
    private const string SoftwareRegKey = "HKEY_CURRENT_USER\\Software\\GUB\\ZVT";
    private ZvtClient zvtClient;

    private string? com;
    private string? ip;
    private int? port;
    private int? kasse_nr;
    private string? passwort;
    private int? funktion;
    private int? betrag;


    private bool merchantReceiptReceived;
    private ReceiptInfo merchantReceipt;
    private bool cardholderReceiptReceived;
    private ReceiptInfo carholderReceipt;
    private bool adminReceiptReceived;
    private ReceiptInfo adminReceipt;
    
    
    public async Task Init()
    {
        com = (string?)Registry.GetValue(SoftwareRegKey, "COM", "LAN");
        ip = (string?)Registry.GetValue(SoftwareRegKey, "IP", "127.0.0.1");
        port = (int?)Registry.GetValue(SoftwareRegKey, "Port", 5577);
        kasse_nr = (int?)Registry.GetValue(SoftwareRegKey, "KasseNr", 1);
        passwort = (string?)Registry.GetValue(SoftwareRegKey, "Passwort", "12345");
        funktion = (int?)Registry.GetValue(SoftwareRegKey, "Funktion", 0);
        betrag = (int?)Registry.GetValue(SoftwareRegKey, "Betrag", 0);

        if (com != "LAN" | ip == null) throw new Exception("Please connect to a LAN terminal and provide an ip.");

        var deviceCommunication = new TcpNetworkDeviceCommunication(ip, port: (int)port);
        if (!await deviceCommunication.ConnectAsync())
        {
            throw new Exception("Connection to payment terminal failed");
        }

        zvtClient = new ZvtClient(deviceCommunication);
        zvtClient.StatusInformationReceived += StatusInformationReceived;
        zvtClient.IntermediateStatusInformationReceived += IntermediateStatusInformationReceived;
        zvtClient.ReceiptReceived += ReceiptReceived;
        
        merchantReceiptReceived = false;
        cardholderReceiptReceived = false;
        adminReceiptReceived = false;
    }
    
    public async Task<CommandResponse> StartAction()
    {
        CommandResponse res;
        switch (funktion)
        {
            case 0:
                //Zahlung
                if (betrag == null) throw new Exception("Please enter a payment amount.");
                var b = ConvertEuroCent((int)betrag);
                res = await zvtClient.PaymentAsync(b);
                break;
            case 5:
                //Belegwiederholung
                res = await zvtClient.RepeatLastReceiptAsync();
                break;
            case 2:
                //Kassenschnitt
                res = await zvtClient.EndOfDayAsync();
                break;
            default:
                throw new Exception("Command not supported.");
        }

        var cancel = false; 

        while (!cardholderReceiptReceived && !cancel)
        {
            break; // remove this when handling receipts :)
            Thread.Sleep(1);
        }

        return res;
    }
    
    public static void SetActive(bool active)
    {
        Registry.SetValue(SoftwareRegKey,"Aktiv", active ? 1 : 0);
    }

    public static void SetResult(int result)
    {
        Registry.SetValue(SoftwareRegKey, "Ergebnis", result);
    }

    private void StatusInformationReceived(StatusInformation status)
    {
    }

    private void IntermediateStatusInformationReceived(string intermediateStatus)
    {
        Console.WriteLine(intermediateStatus);
    }

    private void ReceiptReceived(ReceiptInfo receiptInfo)
    {
        if (!receiptInfo.CompletelyProcessed) return;

        switch (receiptInfo.ReceiptType)
        {
            case ReceiptType.Administration:
                adminReceipt = receiptInfo;
                adminReceiptReceived = true;
                break;
            case ReceiptType.Cardholder:
                carholderReceipt = receiptInfo;
                cardholderReceiptReceived = true;
                break;
            case ReceiptType.Merchant:
                merchantReceipt = receiptInfo;
                merchantReceiptReceived = true;
                break;
        }
    }

    private decimal ConvertEuroCent(int eurocent)
    {
        if (eurocent < 0) throw new Exception("payment amount can't be below 0.");
        if (eurocent >= 1000000000) throw new Exception($"payment amount must be less than {10000000}.");
        
        var s = $"{eurocent,8}";
        var spl = s[..6] + "," + s[6..];
        return Convert.ToDecimal(spl);
    }
}