using System.IO;
using UnityEngine;

public static class UnityPath
{
    public enum AssetPath
    {
        Data = 1,
        StreamingAsset,
        Persistent
    }

    /// <summary>
    /// Determine wherever path is file or directory
    /// </summary>
    /// <param name="path">path</param>
    /// <returns>
    /// -1: exception
    /// 0: file
    /// 1: directory
    /// </returns>
    public static int FileOrDirectory(string path)
    {
        try
        {
            FileAttributes attrs = File.GetAttributes(path);
            if (attrs.HasFlag(FileAttributes.Directory))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        catch (System.Exception e)
        {
            Debugger.ErrorLog(e.ToString());
            return -1;
        }
    }
    public static void CreateFileAnywhere(string path)
    {
        if (string.IsNullOrEmpty(GetExtension(path)))
        {
            // path is directory
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }catch(System.Exception e)
            {
                Debugger.ErrorLog(e.ToString());
            }
        }
        else
        {
            try
            {
                // path include file
                string directory = Path.GetDirectoryName(path);
                Debugger.Log(directory);
                if(!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                using (File.Create(path)) { }

            } catch(System.Exception e)
            {
                Debugger.ErrorLog(e.ToString());
            }
        }
    }
    public static bool Exist(string path)
    {
        if (string.IsNullOrEmpty(GetExtension(path)))
        {
            // path is directory
            try
            {
                return Directory.Exists(path);
            }
            catch (System.Exception e)
            {
                Debugger.ErrorLog(e.ToString());
                return false;
            }
        }
        else
        {
            try
            {
                // path include file
                string directory = Path.GetDirectoryName(path);
                return File.Exists(path);

            }
            catch (System.Exception e)
            {
                Debugger.ErrorLog(e.ToString());
                return false;
            }
        }
    }

    public static string GetDirectory(string path)
    {
        return Path.GetDirectoryName(path);
    }
    public static string GetExtension(string path)
    {
        try
        {
            return Path.GetExtension(path);
        }
        catch (System.Exception e)
        {
            Debugger.ErrorLog(e.ToString());
            return "";
        }
    }
    public static string GetPath(AssetPath type)
    {
        switch (type)
        {
            case AssetPath.Data:
                return Application.dataPath;
            case AssetPath.StreamingAsset:
                return Application.streamingAssetsPath;
            case AssetPath.Persistent:
                return Application.persistentDataPath;
            default:
                return "";
        }
    }
    public static string Combinate(string filePath, AssetPath location)
    {
        return GetPath(location) + @"/" + filePath;
    }
}
