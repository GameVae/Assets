

using static WindowManager;

public interface IWindow
{
    void Load(params object[] input);
    void Open();
    void Close();
}

public interface IWindowGroup
{
    WindowGroup Group { get; }
    WindowGroupType GroupType { get; }
}
