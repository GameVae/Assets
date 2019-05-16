using System;
using System.Collections;
using System.IO;
using Generic.Contants;
using Generic.Singleton;
using UnityEngine;
using UnityGameTask;

public class Config : MonoSingle<Config>, IGameTask
{
    private const string localPathKey = "ConfigPath";
    private FileConfig fileConfig;

    public XmlEncoder XmlEncoder;
    public CopyAssetsAndroid CopyAssets;

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
    public FileConfig FileConfig
    {
        get
        {
            return fileConfig ?? (fileConfig = LoadConfigFile());
        }
    }

    private FileConfig LoadConfigFile()
    {
        string configFilePath = PlayerPrefs.GetString(localPathKey);
        FileConfig config = XmlEncoder.Deserialize<FileConfig>(configFilePath);

        if(config.FirstSetup)
        {
            SetupConfig(ref config);
        }

        return config;
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

    private void SetupConfig(ref FileConfig config)
    {
        FileLink[] links = config.Links;
        for (int i = 0; i < links.Length; i++)
        {
            links[i].Local = Guid.NewGuid().ToString() + UnityPath.GetExtension(links[i].Streaming);
        }

        config.Links = links;
        config.FirstSetup = false;

        SerializeConfigFile(config);
    }

    private void SerializeConfigFile(FileConfig config)
    {
        string configFilePath = PlayerPrefs.GetString(localPathKey);
        string tempXml = UnityPath.Combinate("temp.xml",UnityPath.AssetPath.Persistent);

        XmlEncoder.Serialize<FileConfig>(tempXml, config);
        XmlEncoder.Encode(tempXml, configFilePath);

        File.Delete(tempXml);
    }
}
