using System.Reflection;
using Portalum.Zvt.Models;
using System.Xml.Serialization;
#if RELEASE_WIN
using Microsoft.Win32;
#endif

namespace IO.TBRT.FreeZVT;

public class TransactionStatus
{
    private string _regBasePath;
    private DataTypes.Ausgabe _ausgabe = new();
    private string? _ausgabeDatei;
    private string _ausgabePfad;
    private string _ausgabeDateiPrefix = "ZVT_Ergebnis_";
    private string _receipt = String.Empty;
    public TransactionStatus(string regBasePath, DataTypes.Options options)
    {
        _regBasePath = regBasePath;
        _ausgabePfad = options.Ausgabepfad;
        _ausgabeDatei = string.IsNullOrEmpty(options.Ausgabepfad) ? null : $"{options.Ausgabepfad}\\{_ausgabeDateiPrefix}{options.KasseNr}.xml";
        
        //create receipt folder
        foreach (var receiptType in Enum.GetNames(typeof(ReceiptType)))
        {
            Directory.CreateDirectory($"{options.Ausgabepfad}\\Belege\\{receiptType}");
        }
    }
    
    public void Update()
    {
#if RELEASE_WIN
        //set registry entries
        var properties = _ausgabe.GetType().GetProperties();
        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<XmlIgnoreAttribute>() != null) continue;
            var xmlElement = property.GetCustomAttribute<XmlElementAttribute>();
            var propname = xmlElement != null ? xmlElement.ElementName : property.Name;
            
            Registry.SetValue(_regBasePath, propname, property.GetValue(_ausgabe));
        }
#endif
        if (string.IsNullOrEmpty(_ausgabeDatei)) return;

        var serializer = new XmlSerializer(_ausgabe.GetType());

        FileStream fs = new FileStream(_ausgabeDatei, FileMode.Create, FileAccess.Write);
        serializer.Serialize(fs, _ausgabe);
        fs.Close();
    }

    public void SetActive(bool active)
    {
        _ausgabe.Aktiv = active ? DataTypes.AktivType.AnwendungLaeuft : DataTypes.AktivType.AnwendungBeendet;
    }
    
    public void ReceiptReceived(ReceiptInfo receiptInfo)
    {
        if (receiptInfo is not {CompletelyProcessed: true}) return;

        _receipt = receiptInfo.Content;

        FileStream fs = new FileStream($"{_ausgabePfad}\\{receiptInfo.ReceiptType}\\{DateTime.Now}", FileMode.Create,
            FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(receiptInfo.Content);
        sw.Close();
    }

    public void StatusInformationReceived(StatusInformation statusInformation)
    {
        if (statusInformation == null) return;

        _ausgabe.TID = statusInformation.TerminalIdentifier.ToString();
        _ausgabe.Drucktext = statusInformation.AdditionalText;
        _ausgabe.Drucktext2 = statusInformation.ErrorMessage;
        _ausgabe.Betrag = (int) (statusInformation.Amount * 100);
        _ausgabe.Kartentyp = int.Parse(statusInformation.CardType);
        _ausgabe.KartentypLang = statusInformation.CardName;
        _ausgabe.TID = statusInformation.TraceNumberLongFormat.ToString();
        _ausgabe.Kontonummer = statusInformation.VuNumber;
        _ausgabe.AID = statusInformation.AidAuthorisationAttribute;
        _ausgabe.BelegNr = statusInformation.ReceiptNumber.ToString();
        
        Update();
    }

    public void IntermediateInformationReceived(string message)
    {
        //TODO: implement ui etc
    }
}