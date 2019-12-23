using System;
using System.Collections.Generic;
using System.Text;

namespace testFileUpload.Core.Services
{
    public abstract class Importer
    {
    }

    public class NullImporter : Importer
    {
    }

    public class CsvImporter:Importer
    {
        /*
         * StreamReader reader = new StreamReader( stream );
string text = reader.ReadToEnd();
         */
    }

    public class XmlImporter :Importer
    {
        /*
         * using (XmlReader reader = XmlReader.Create(inputStream)
{
    if (CorrectFileFormat(reader))
    {
        DisplayLicenseInfo(reader);
    }
    else
    {
        StatusLabel.Text = "Selected file is not a LicensingDiag XML file";
    }
}
         */
    }
}
