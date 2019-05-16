using System;

[Serializable]
public class FileLink
{
    public enum FileType
    {
        Database,
        Xml,
    }

    public string Streaming;
    public string Local;
    public FileType Type;
}