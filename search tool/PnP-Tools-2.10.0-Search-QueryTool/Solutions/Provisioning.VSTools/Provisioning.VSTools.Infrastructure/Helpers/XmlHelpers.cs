﻿using Provisioning.VSTools.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Provisioning.VSTools.Helpers
{
    public static class XmlHelpers
    {

        /// <summary>
        /// Deserializes a file to the specified type
        /// </summary>
        public static T DeserializeObject<T>(string filename)
        {
            T result;

            using (StreamReader sr = new StreamReader(filename))
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                result = (T)ser.Deserialize(sr);
            }

            return result;
        }

        /// <summary>
        /// Serializes an item to the specied file
        /// </summary>
        public static void SerializeObject(object source, string filename)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            XmlSerializer serializer = new XmlSerializer(source.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                XmlWriter writer = XmlWriter.Create(ms, settings);
                serializer.Serialize(writer, source);

                System.IO.File.WriteAllBytes(filename, ms.ToArray());
            }
        }

        /// <summary>
        /// Writes an xml string to a file
        /// </summary>
        public static void WriteXmlStringToFile(string inputXml, string filePath)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(inputXml);
            doc.Save(filePath);
        }
    }
}
