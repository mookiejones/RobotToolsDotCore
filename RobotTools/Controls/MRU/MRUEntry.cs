namespace RobotTools.Controls.MRU
{
    public class MRUEntry
    {
        #region constructor
        /// <summary>
        /// Standard Constructor
        /// </summary>
        public MRUEntry()
        {
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public MRUEntry(MRUEntry copyFrom)
        {
            if (copyFrom == null) return;

            PathFileName = copyFrom.PathFileName;
            IsPinned = copyFrom.IsPinned;
        }

        /// <summary>
        /// Convinience constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fullTime"></param>
        public MRUEntry(string name, bool fullTime)
        {
            PathFileName = name;
            IsPinned = fullTime;
        }
        #endregion constructor

        #region properties
        public string PathFileName { get; set; }

        public bool IsPinned { get; set; }
        #endregion properties

        #region methods
        public override string ToString()
        {
            return string.Format("Path {0}, IsPinned:{1}", (PathFileName == null ? "(null)" : PathFileName),
                                                           IsPinned);
        }
        #endregion methods
    }
}
