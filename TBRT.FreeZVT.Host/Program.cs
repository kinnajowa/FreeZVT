using Microsoft.Win32;
using Portalum.Zvt;
using Portalum.Zvt.Models;

namespace TBRT.FreeZVT.Host;

public class Program
{
    private const string SoftwareRegKey = "HKEY_CURRENT_USER\\Software\\GUB\\ZVT}";

    public static async Task<int> Main(string[] cmdArgs)
    {
        PaymentMaster.SetActive(true);

        try
        {
            var master = new PaymentMaster();
            await master.Init();

            CommandResponse res = await master.StartAction();

            if (res.State != CommandResponseState.Successful)
                throw new Exception($"Payment unsuccessful, result: {res.State}, {res.ErrorMessage}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            PaymentMaster.SetResult(1);
            PaymentMaster.SetActive(false);
            return 1;
        }
        
        PaymentMaster.SetResult(0);
        PaymentMaster.SetActive(false);
        return 0;
    }

}