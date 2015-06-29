using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
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

         public static T readDataContractXml<T>(string fileName)
        {
            var serializer = new DataContractSerializer(typeof(T));
            T temp;

            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                temp = (T)serializer.ReadObject(fs);
            }
            return temp;
        }

        public static void writeDataContractXml<T>(T tempList, string fileName)
        {
            var serializer = new DataContractSerializer(typeof(T));
            string xmlString;
            using (var sw = new StringWriter())
            {
                using (var writer = new XmlTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented; // indent the Xml so it's human readable
                    serializer.WriteObject(writer, tempList);
                    writer.Flush();
                    xmlString = sw.ToString();
                }
            }
            TextWriter textWriter = new StreamWriter(fileName);
            textWriter.Write(xmlString);
            textWriter.Close();
        }
    }
    }

