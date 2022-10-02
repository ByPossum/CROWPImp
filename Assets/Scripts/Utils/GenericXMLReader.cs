using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public static class GenericXMLReader
{
    public static T ReadXML<T>(string _filePath, string _fileName) where T : new()
    {
        XmlSerializer rawData = new XmlSerializer(typeof(T));
        FileStream fs = OpenFile<T>(FileMode.Open, _filePath, _fileName);
        try
        {
            T newObj = (T)rawData.Deserialize(fs);
            fs.Close();
            return newObj;
        }
        catch (System.InvalidOperationException e)
        {
            fs.Close();
            File.Delete(ValidateFilePath<T>(_filePath, _fileName));
            return ReadXML<T>(_filePath, _fileName);
        }
    }

    public static void WriteToXML<T>(T _objectToWrite, string _filePath, string _fileName) where T : new()
    {
        XmlSerializer rawData = new XmlSerializer(typeof(T));
        string path = ValidateFilePath<T>(_filePath, _fileName);
        File.WriteAllText(path, string.Empty);
        FileStream fs = OpenFile<T>(FileMode.Open, _filePath, _fileName);
        try
        {
            rawData.Serialize(fs, _objectToWrite);
        }
        catch (System.InvalidOperationException e)
        {
            fs.Close();
            File.Delete(path);
            WriteToXML<T>(_objectToWrite, _filePath, _fileName);
        }
        fs.Close();
    }

    private static FileStream OpenFile<T>(FileMode _mode, string _filePath, string _fileName) where T : new()
    {
        return new FileStream(ValidateFilePath<T>(_filePath, _fileName), _mode);
    }

    private static string ValidateFilePath<T>(string _filePath, string _fileName) where T : new()
    {
        if (!Directory.Exists(Application.persistentDataPath + _filePath))
        {
            DirectoryInfo dr = Directory.CreateDirectory(Application.persistentDataPath + _filePath);
        }
        if (!File.Exists(Application.persistentDataPath + _filePath + _fileName))
        {
            FileStream fs = new FileStream(Application.persistentDataPath + _filePath + _fileName, FileMode.Create);
            fs.Close();
            WriteToXML(new T(), _filePath, _fileName);
        }
        return Application.persistentDataPath + _filePath + _fileName;
    }
}
