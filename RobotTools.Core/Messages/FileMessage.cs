namespace RobotTools.Core.Messages;

public sealed class FileMessage
{

    public FileMessage()
    {

    }

    public FileMessage(string name)
    {
        Name = name;
    }
    public string Name { get; set; }
    public string Path { get; set; }


}
