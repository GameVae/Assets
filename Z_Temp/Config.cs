using System;
using System.Collections;
using Generic.Contants;
using Generic.Singleton;
using UnityEngine;
using UnityGameTask;

public class Config : MonoSingle<Config>, IGameTask
{
    private const string localPathKey = "ConfigPath";
    private FileConfig fileConfig;
    public XmlEncoder XmlEncoder;
    public string ConfigPath;
    public CopyAssetsAndroid CopyAssets;

    FileConfig FileConfig
    {
        get
        {
            return fileConfig ?? (fileConfig = LoadConfigFile());
        }
    }

    public bool IsDone
    {
        get;
        private set;
    }
    public float Progress
    {
        get;
        private set;
    }

    private FileConfig LoadConfigFile()
    {
        string configFilePath = PlayerPrefs.GetString(localPathKey);
        return XmlEncoder.Deserialize<FileConfig>(configFilePath);
    }

    public IEnumerator Action()
    {
        IsDone = false;
        Progress = 0.0f;

        string configFilePath = PlayerPrefs.GetString(localPathKey);
        if (string.IsNullOrEmpty(configFilePath))
        {

            string streamingPath = UnityPath.Combinate(Constants.ConfigFilePath, UnityPath.AssetPath.StreamingAsset);
            string localConfigPath =
                UnityPath.GetDirectory(Constants.ConfigFilePath) + @"\" +
                Guid.NewGuid().ToString() +
                UnityPath.GetExtension(Constants.ConfigFilePath);

            yield return CopyAssets.Copy
                (streamingPath, UnityPath.Combinate(localConfigPath, UnityPath.AssetPath.Persistent));
            PlayerPrefs.SetString(localPathKey, localConfigPath);
        }
        IsDone = true;
        Progress = 1.0f;
    }
}
