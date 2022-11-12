using System;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using RobotTools.ViewModels;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace RobotTools.Controls.MRU
{
    public class MRUListVM : BaseViewModel
    {
        #region Fields
        private MRUSortMethod _pinEntryAtHeadOfList = MRUSortMethod.PinnedEntriesFirst;

        private ObservableCollection<MRUEntryVM> _listOfMRUEntries;

        private int _maxMruEntryCount;

        private RelayCommand _removeLastEntryCommand;
        private RelayCommand _removeFirstEntryCommand;
        #endregion Fields

        #region Constructor
        public MRUListVM()
        {
            _maxMruEntryCount = 15;
            _pinEntryAtHeadOfList = MRUSortMethod.PinnedEntriesFirst;
        }

        public MRUListVM(MRUSortMethod pinEntryAtHeadOfList = MRUSortMethod.PinnedEntriesFirst)
          : this()
        {
            _pinEntryAtHeadOfList = pinEntryAtHeadOfList;
        }
        #endregion Constructor

        #region Properties
        [XmlAttribute(AttributeName = "MinValidMRUCount")]
        public int MinValidMruEntryCount
        {
            get
            {
                return 5;
            }
        }

        [XmlAttribute(AttributeName = "MaxValidMRUCount")]
        public int MaxValidMruEntryCount
        {
            get
            {
                return 256;
            }
        }

        [XmlAttribute(AttributeName = "MaxMruEntryCount")]
        public int MaxMruEntryCount
        {
            get
            {
                return _maxMruEntryCount;
            }

            set
            {
                if (_maxMruEntryCount != value)
                {
                    if (value < MinValidMruEntryCount || value > MaxValidMruEntryCount)
                        throw new ArgumentOutOfRangeException("MaxMruEntryCount", value, "Valid values are: value >= 5 and value <= 256");

                    _maxMruEntryCount = value;

                    NotifyPropertyChanged(() => MaxMruEntryCount);
                }
            }
        }

        /// <summary>
        /// Get/set property to determine whether a pinned entry is shown
        /// 1> at the beginning of the MRU list
        /// or
        /// 2> remains where it currently is.
        /// </summary>
        [XmlAttribute(AttributeName = "SortMethod")]
        public MRUSortMethod PinSortMode
        {
            get
            {
                return _pinEntryAtHeadOfList;
            }

            set
            {
                if (_pinEntryAtHeadOfList != value)
                {
                    _pinEntryAtHeadOfList = value;
                    NotifyPropertyChanged(() => PinSortMode);
                }
            }
        }

        [XmlArrayItem("MRUList", IsNullable = false)]
        public ObservableCollection<MRUEntryVM> ListOfMRUEntries
        {
            get
            {
                return _listOfMRUEntries;
            }

            set
            {
                if (_listOfMRUEntries != value)
                {
                    _listOfMRUEntries = value;

                    NotifyPropertyChanged(() => ListOfMRUEntries);
                }
            }
        }

        #region RemoveEntryCommands
        public ICommand RemoveFirstEntryCommand
        {
            get
            {
                if (_removeFirstEntryCommand == null)
                    _removeFirstEntryCommand =
                        new RelayCommand(() => OnRemoveMRUEntry(MRUList.Spot.First));

                return _removeFirstEntryCommand;
            }
        }

        public ICommand RemoveLastEntryCommand
        {
            get
            {
                if (_removeLastEntryCommand == null)
                    _removeLastEntryCommand = new RelayCommand(() => OnRemoveMRUEntry(MRUList.Spot.Last));

                return _removeLastEntryCommand;
            }
        }

        #endregion RemoveEntryCommands
        #endregion Properties

        #region Methods
        #region AddRemove Methods
        private void OnRemoveMRUEntry(MRUList.Spot addInSpot = MRUList.Spot.Last)
        {
            if (_listOfMRUEntries == null)
                return;

            if (_listOfMRUEntries.Count == 0)
                return;

            switch (addInSpot)
            {
                case MRUList.Spot.Last:
                    _listOfMRUEntries.RemoveAt(_listOfMRUEntries.Count - 1);
                    break;
                case MRUList.Spot.First:
                    _listOfMRUEntries.RemoveAt(0);
                    break;

                default:
                    break;
            }

            //// this.NotifyPropertyChanged(() => this.ListOfMRUEntries);
        }

        private int CountPinnedEntries()
        {
            if (_listOfMRUEntries != null)
                return _listOfMRUEntries.Count(mru => mru.IsPinned == true);

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bPinOrUnPinMruEntry"></param>
        /// <param name="mruEntry"></param>
        public bool PinUnpinEntry(bool bPinOrUnPinMruEntry, MRUEntryVM mruEntry)
        {
            try
            {
                if (_listOfMRUEntries == null)
                    return false;

                int PinnedMruEntryCount = CountPinnedEntries();

                // pin an MRU entry into the next available pinned mode spot
                if (bPinOrUnPinMruEntry == true)
                {
                    MRUEntryVM e = _listOfMRUEntries.Single(mru => mru.IsPinned == false && mru.PathFileName == mruEntry.PathFileName);

                    if (PinSortMode == MRUSortMethod.PinnedEntriesFirst)
                        _listOfMRUEntries.Remove(e);

                    e.IsPinned = true;

                    if (PinSortMode == MRUSortMethod.PinnedEntriesFirst)
                        _listOfMRUEntries.Insert(PinnedMruEntryCount, e);

                    PinnedMruEntryCount += 1;
                    //// this.NotifyPropertyChanged(() => this.ListOfMRUEntries);

                    return true;
                }
                else
                {
                    // unpin an MRU entry into the next available unpinned spot
                    MRUEntryVM e = _listOfMRUEntries.Single(mru => mru.IsPinned == true && mru.PathFileName == mruEntry.PathFileName);

                    if (PinSortMode == MRUSortMethod.PinnedEntriesFirst)
                        _listOfMRUEntries.Remove(e);

                    e.IsPinned = false;
                    PinnedMruEntryCount -= 1;

                    if (PinSortMode == MRUSortMethod.PinnedEntriesFirst)
                        _listOfMRUEntries.Insert(PinnedMruEntryCount, e);

                    //// this.NotifyPropertyChanged(() => this.ListOfMRUEntries);

                    return true;
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(AppName + " encountered an error when pinning an entry:" + Environment.NewLine
                              + Environment.NewLine
                              + exp.ToString(), "Error when pinning an MRU entry", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false;
        }

        /// <summary>
        /// Standard short-cut method to add a new unpinned entry from a string
        /// </summary>
        /// <param name="newEntry">File name and path file</param>
        public void AddMRUEntry(string newEntry)
        {
            if (newEntry == null || newEntry == string.Empty)
                return;

            AddMRUEntry(new MRUEntryVM() { IsPinned = false, PathFileName = newEntry });
        }

        public void AddMRUEntry(MRUEntryVM newEntry)
        {
            if (newEntry == null) return;

            try
            {
                if (_listOfMRUEntries == null)
                    _listOfMRUEntries = new ObservableCollection<MRUEntryVM>();

                // Remove all entries that point to the path we are about to insert
                MRUEntryVM e = _listOfMRUEntries.SingleOrDefault(item => newEntry.PathFileName == item.PathFileName);

                if (e != null)
                {
                    // Do not change an entry that has already been pinned -> its pinned in place :)
                    if (e.IsPinned == true)
                        return;

                    _listOfMRUEntries.Remove(e);
                }

                // Remove last entry if list has grown too long
                if (MaxMruEntryCount <= _listOfMRUEntries.Count)
                    _listOfMRUEntries.RemoveAt(_listOfMRUEntries.Count - 1);

                // Add model entry in ViewModel collection (First pinned entry or first unpinned entry)
                if (newEntry.IsPinned == true)
                    _listOfMRUEntries.Insert(0, new MRUEntryVM(newEntry));
                else
                {
                    _listOfMRUEntries.Insert(CountPinnedEntries(), new MRUEntryVM(newEntry));
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString(), "An error has occurred", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ////finally
            ////{
            ////   this.NotifyPropertyChanged(() => this.ListOfMRUEntries);
            ////}
        }

        public bool RemoveEntry(string fileName)
        {
            try
            {
                if (_listOfMRUEntries == null)
                    return false;

                MRUEntryVM e = _listOfMRUEntries.Single(mru => mru.PathFileName == fileName);

                _listOfMRUEntries.Remove(e);

                //// this.NotifyPropertyChanged(() => this.ListOfMRUEntries);

                return true;
            }
            catch (Exception exp)
            {
                MessageBox.Show(AppName + " encountered an error when removing an entry:" + Environment.NewLine
                              + Environment.NewLine
                              + exp.ToString(), "Error when pinning an MRU entry", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false;
        }

        public bool RemovePinEntry(MRUEntryVM mruEntry)
        {
            try
            {
                if (_listOfMRUEntries == null)
                    return false;

                MRUEntryVM e = _listOfMRUEntries.Single(mru => mru.PathFileName == mruEntry.PathFileName);

                _listOfMRUEntries.Remove(e);

                //// this.NotifyPropertyChanged(() => this.ListOfMRUEntries);

                return true;
            }
            catch (Exception exp)
            {
                MessageBox.Show(AppName + " encountered an error when removing an entry:" + Environment.NewLine
                              + Environment.NewLine
                              + exp.ToString(), "Error when pinning an MRU entry", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false;
        }
        #endregion AddRemove Methods

        public MRUEntryVM FindMRUEntry(string filePathName)
        {
            try
            {
                if (_listOfMRUEntries == null)
                    return null;

                return _listOfMRUEntries.SingleOrDefault(mru => mru.PathFileName == filePathName);
            }
            catch (Exception exp)
            {
                MessageBox.Show(AppName + " encountered an error when removing an entry:" + Environment.NewLine
                              + Environment.NewLine
                              + exp.ToString(), "Error when pinning an MRU entry", MessageBoxButton.OK, MessageBoxImage.Error);

                return null;
            }
        }

        private string AppName
        {
            get
            {
                return Application.ResourceAssembly.GetName().Name;
            }
        }
        #endregion Methods
    }
}
