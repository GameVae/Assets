using System;

[Serializable]
public class FileLink
{
    public enum FileType
    {
        Database,
        Xml,
    }

    public string Persistent;
    public string Local;
    public FileType Type;
}