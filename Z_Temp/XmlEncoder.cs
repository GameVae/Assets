using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class XmlEncoder : MonoBehaviour
{
    public FileConfig links;
    public FileConfig deserLinks;

    public void Start()
    {
        string path = Application.persistentDataPath + @"/xmlFile.xml";
        string binaryPath = Application.persistentDataPath + @"/biFile.xml";

        //Serialize<Config>(path, links);
        //Encode(path, binaryPath);
        //deserLinks = Deserialize<Config>(binaryPath) as Config;
        CreateConfigFile(path, binaryPath);
    }

    public void Encode(string xmlPath, string location)
    {
        byte[] bytes = File.ReadAllBytes(xmlPath);
        string base64Str = Convert.ToBase64String(bytes);

        //Debug.Log(base64Str);
        //Debug.Log(Guid.NewGuid().ToString());
        using (FileStream file = File.Open(location, FileMode.Create))
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(file))
            {
                binaryWriter.Write(base64Str.ToCharArray());
            }
        }
    }
    public void Serialize<T>(string xmlPath, object target)
    {
        try
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (FileStream file = File.Open(xmlPath, FileMode.Create))
            {
                xmlSerializer.Serialize(file, target);
            }
        }
        catch(Exception e)
        {
            Debugger.Log(e.ToString());
        }
    }
    public T Deserialize<T>(string binaryPath)
    {
        try
        {
            string encodedString = File.ReadAllText(binaryPath);
            byte[] data = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(data);

            using (TextReader textReader = new StringReader(decodedString))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(textReader);
            }
        }
        catch (Exception e)
        {
            Debugger.Log(e.ToString());
            return default(T);
        }
    }

    public void CreateConfigFile(string path,string binaryPath)
    {
        FileConfig config = Deserialize<FileConfig>(binaryPath) as FileConfig;
        for (int i = 0; i < config.Links.Length; i++)
        {
            config.Links[i].Local = Guid.NewGuid().ToString() 
                + UnityPath.GetExtension(config.Links[i].Persistent);
        }

        Serialize<FileConfig>(path, config);
        deserLinks = Deserialize<FileConfig>(binaryPath) as FileConfig;
    }
       
}
