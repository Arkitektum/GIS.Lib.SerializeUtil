using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Arkitektum.GIS.Lib.SerializeUtil
{
    public static class SerializeUtil
    {
        /// <summary>
        /// Serialize an object to a string 
        /// </summary>
        /// <param name="inputObject">The object to serialize</param>
        /// <param name="ns">Optional namespaces to use when serializing</param>
        /// <returns></returns>
        public static string SerializeToString(object inputObject, XmlSerializerNamespaces ns = null)
        {
            XmlSerializer serializer = new XmlSerializer(inputObject.GetType());

            StringWriterWithEncoding stringWriter = new StringWriterWithEncoding(Encoding.UTF8);

            if (ns != null) 
                serializer.Serialize(stringWriter, inputObject, ns);
            else
                serializer.Serialize(stringWriter, inputObject, GetDefaultMetadataNamespaces());

            return stringWriter.ToString();
        }
        
        /// <summary>
        /// Serialize an object to a XML file.
        /// </summary>
        /// <param name="inputObject">the object to serialize</param>
        /// <param name="filename">name of the file (without extension, '.xml' is automatically appended)</param>
        /// <param name="path">the path (remember to escape directory separators e.g. mydir\\other\\xml. When path is null a directory named 'xml' is created in the current dir.</param>
        /// <param name="ns">namespaces to use when serializing to get prefixes on elements from different schema</param>
        public static void SerializeToFile(object inputObject, string filename, string path = null, XmlSerializerNamespaces ns = null)
        {
            string serializedContent = SerializeToString(inputObject, ns);
            
            if (path == null)
            {
                if (!Directory.Exists("xml"))
                    Directory.CreateDirectory("xml");

                path = "xml\\";
            }

            var fullPath = path + filename + ".xml";
            using (StreamWriter outfile = new StreamWriter(fullPath, false, Encoding.UTF8))
            {
                outfile.Write(serializedContent);
                Trace.WriteLine("Serialized object to file: " + fullPath);
            }
        }
        
        public static T DeserializeFromString<T>(string objectData)
        {
            return (T) DeserializeFromString(objectData, typeof(T));
        }

        public static object DeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result = null;

            TextReader reader = null;
            try
            {
                reader = new StringReader(objectData);

                result = serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return result;
        }

        public static XmlSerializerNamespaces GetDefaultMetadataNamespaces()
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            ns.Add("gmd", "http://www.isotc211.org/2005/gmd");
            ns.Add("gco", "http://www.isotc211.org/2005/gco");
            ns.Add("gts", "http://www.isotc211.org/2005/gts");
            ns.Add("srv", "http://www.isotc211.org/2005/srv");
            ns.Add("gml", "http://www.opengis.net/gml/3.2");
            return ns;
        }

    }
}
