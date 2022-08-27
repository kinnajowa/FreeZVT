using System.Reflection;
#if RELEASE_WIN
using System.Xml.Serialization;
using Microsoft.Win32;
#endif

namespace IO.TBRT.FreeZVT;

public class TransactionStatus
{
    private string _regBasePath;
    private DataTypes.Ausgabe _ausgabe = new();
    private string? _ausgabePfad;
    private string _ausgabeDateiPrefix = "ZVT_Ergebnis_";
    public TransactionStatus(string regBasePath, DataTypes.Options options)
    {
        _regBasePath = regBasePath;
        _ausgabePfad = string.IsNullOrEmpty(options.Ausgabepfad) ? null : $"{options.Ausgabepfad}\\{_ausgabeDateiPrefix}{options.KasseNr}.xml";
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
        if (string.IsNullOrEmpty(_ausgabePfad)) return;

        var serializer = new System.Xml.Serialization.XmlSerializer(_ausgabe.GetType());

        FileStream fs = new FileStream(_ausgabePfad, FileMode.Create, FileAccess.Write);
        serializer.Serialize(fs, _ausgabe);
        fs.Close();
    }
}