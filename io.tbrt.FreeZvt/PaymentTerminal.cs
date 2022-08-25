using System.IO.Ports;
using Portalum.Zvt;

namespace IO.TBRT.FreeZVT;

public class PaymentTerminal
{
    private readonly DataTypes.Options _options;
    private ZvtClientConfig _zvtClientConfig;
    private ZvtClient _zvtClient;
    private ReceiveHandler _zvtReceiveHandler = new();
    public PaymentTerminal(DataTypes.Options options)
    {
        _options = options;
        _zvtClientConfig = new ZvtClientConfig
        {
            Encoding = ZvtEncoding.CodePage437,
            Language = _options.Sprache == DataTypes.SpracheCode.DE ? Language.German : Language.English,
            Password = int.Parse(_options.Passwort)
        };

        IDeviceCommunication deviceCommunication = _options.COM.Equals("LAN")
            ? new TcpNetworkDeviceCommunication(_options.IP, _options.Port)
            : new SerialPortDeviceCommunication(_options.COM, _options.ComSpeed,
                stopBits: _options.ComStop == DataTypes.ComStopType.OneBit ? StopBits.One : StopBits.Two);

        var conn = deviceCommunication.ConnectAsync();
        conn.Wait();
        if (!conn.Result) throw new DataTypes.ConnectionFailureException($"Cannot connect to payment terminal over {_options.COM}");
        
        _zvtClient = new ZvtClient(deviceCommunication, clientConfig: _zvtClientConfig);
    }

    public CommandResponse Registration()
    {
        RegistrationConfig registrationConfig = new()
        {
            ActivateTlvSupport = false,
            AllowAdministrationViaPaymentTerminal = true,
            AllowStartPaymentViaPaymentTerminal = _options.Sperre == DataTypes.SperreType.NichtGesperrt,
            ReceiptPrintoutForAdministrationFunctionsViaPaymentTerminal = true,
            ReceiptPrintoutForPaymentFunctionsViaPaymentTerminal = _options.Kassedruck == DataTypes.DruckType.Terminal,
            SendIntermediateStatusInformation = true,
            ServiceMenuIsDisabledOnTheFunctionKeyOfThePaymentTerminal = false,
            TextAtDisplayInCapitalLettersAtThePaymentTerminal = true
        };
        var task = _zvtClient.RegistrationAsync(registrationConfig);
        task.Wait();

        return task.Result;
    }

    public CommandResponse LogOff()
    {
        var task = _zvtClient.LogOffAsync();
        task.Wait();
        return task.Result;
    }

    public CommandResponse Payment(int amount)
    {
        var str = amount.ToString();
        var len = str.Length;
        var formattedStr = $"{str.Substring(0, len - 2)},{str.Substring(len - 3, 2)}";
        decimal formattedAmount = decimal.Parse(formattedStr);
        var task = _zvtClient.PaymentAsync(formattedAmount);
        task.Wait();
        return task.Result;
    }

    public CommandResponse Reversal(string receiptNo)
    {
        var task = _zvtClient.ReversalAsync(int.Parse(receiptNo));
        task.Wait();
        return task.Result;
    }

    public CommandResponse Refund(int amount)
    {
        var task = _zvtClient.RefundAsync(amount);
        task.Wait();
        return task.Result;
    }

    public CommandResponse EndOfDay()
    {
        var task = _zvtClient.EndOfDayAsync();
        task.Wait();
        return task.Result;
    }

    public CommandResponse SendTurnoverTotals()
    {
        var task = _zvtClient.SendTurnoverTotalsAsync();
        task.Wait();
        return task.Result;
    }

    public CommandResponse RepeatReceipt()
    {
        var task = _zvtClient.RepeatLastReceiptAsync();
        task.Wait();
        return task.Result;
    }

    public CommandResponse Diagnosis()
    {
        var task = _zvtClient.DiagnosisAsync();
        task.Wait();
        return task.Result;
    }

    public CommandResponse Abort()
    {
        var task = _zvtClient.AbortAsync();
        task.Wait();
        return task.Result;
    }

    public CommandResponse SoftwareUpdate()
    {
        var task = _zvtClient.SoftwareUpdateAsync();
        task.Wait();
        return task.Result;
    }

    public void Dispose()
    {
        _zvtClient.Dispose();
    }
}