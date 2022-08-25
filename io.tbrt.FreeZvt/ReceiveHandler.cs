using Portalum.Zvt;
using Portalum.Zvt.Models;

namespace IO.TBRT.FreeZVT;

public class ReceiveHandler : IReceiveHandler
{
    public ProcessDataState ProcessData(Span<byte> data)
    {
        throw new NotImplementedException();
    }

    public event Action? CompletionReceived;
    public event Action<string>? AbortReceived;
    public event Action<StatusInformation>? StatusInformationReceived;
    public event Action<string>? IntermediateStatusInformationReceived;
    public event Action<PrintLineInfo>? LineReceived;
    public event Action<ReceiptInfo>? ReceiptReceived;
}