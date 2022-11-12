using System.Xml;
using System.Xml.Serialization;

using RobotTools.ViewModels;

namespace RobotTools.Controls.MRU
{
    public class MRUEntryVM : BaseViewModel
    {
        #region fields
        private readonly MRUEntry _mruEntry;
        #endregion fields

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MRUEntryVM()
        {
            _mruEntry = new MRUEntry();
            IsPinned = false;
        }

        /// <summary>
        /// Constructor from model
        /// </summary>
        /// <param name="model"></param>
        public MRUEntryVM(MRUEntry model) : this()
        {
            _mruEntry = new MRUEntry(model);
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="copySource"></param>
        public MRUEntryVM(MRUEntryVM copySource)
          : this()
        {
            _mruEntry = new MRUEntry(copySource._mruEntry);
            IsPinned = copySource.IsPinned;
        }
        #endregion Constructor

        #region Properties
        [XmlAttribute(AttributeName = "PathFileName")]
        public string PathFileName
        {
            get
            {
                return _mruEntry.PathFileName;
            }

            set
            {
                if (_mruEntry.PathFileName != value)
                {
                    _mruEntry.PathFileName = value;
                    NotifyPropertyChanged(() => PathFileName);
                    NotifyPropertyChanged(() => DisplayPathFileName);
                }
            }
        }

        [XmlIgnore]
        public string DisplayPathFileName
        {
            get
            {
                if (_mruEntry == null)
                    return string.Empty;

                if (_mruEntry.PathFileName == null)
                    return string.Empty;

                int n = 32;
                return (_mruEntry.PathFileName.Length > n ? _mruEntry.PathFileName.Substring(0, 3) +
                                                        "... " + _mruEntry.PathFileName.Substring(_mruEntry.PathFileName.Length - n)
                                                      : _mruEntry.PathFileName);
            }
        }

        [XmlAttribute(AttributeName = "IsPinned")]
        public bool IsPinned
        {
            get
            {
                return _mruEntry.IsPinned;
            }

            set
            {
                if (_mruEntry.IsPinned != value)
                {
                    _mruEntry.IsPinned = value;
                    NotifyPropertyChanged(() => IsPinned);
                }
            }
        }
        #endregion Properties
    }
}
