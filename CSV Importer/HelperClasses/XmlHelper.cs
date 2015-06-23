using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSV_Importer.HelperClasses
{
    public class XMLHelper
    {
        public static T readXml<T>(string fileName)
        {
            T tempList;
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            TextReader textReader = new StreamReader(fileName);
            tempList = (T)deserializer.Deserialize(textReader);
            textReader.Close();

            return tempList;
        }

        public static void writeXml<T>(T tempList, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextWriter textWriter = new StreamWriter(fileName);
            serializer.Serialize(textWriter, tempList);
            textWriter.Close();
        }

    }
}
