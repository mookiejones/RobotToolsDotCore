using System.Collections.Generic;

namespace RobotTools.UI.DirectorySearcher;

public interface IFilesDropped
{
    void OnFilesDropped(IEnumerable<string> files);
}
