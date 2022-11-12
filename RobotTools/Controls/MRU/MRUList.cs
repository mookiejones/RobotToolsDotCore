using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RobotTools.Controls.MRU
{
    public class MRUList
    {
        #region constructor
        public MRUList()
        {
            Entries = new List<MRUEntry>();
        }

        public MRUList(MRUList copySource)
        {
            if (copySource == null) return;

            Entries = new List<MRUEntry>(copySource.Entries);
        }
        #endregion constructor

        internal enum Spot
        {
            First = 0,
            Last = 1
        }

        #region properties
        [XmlArray(ElementName = "Entries", Namespace = "MRUList")]
        [XmlArrayItem(ElementName = "Entry", Namespace = "MRUList")]
        public List<MRUEntry> Entries { get; set; }
        #endregion properties

        #region AddRemoveEntries
        internal bool AddEntry(MRUEntry emp,
                               MRUList.Spot addInSpot = MRUList.Spot.Last)
        {
            if (emp == null)
                return false;

            if (Entries == null)
                Entries = new List<MRUEntry>();

            switch (addInSpot)
            {
                case Spot.First:
                    Entries.Insert(0, new MRUEntry(emp));
                    return true;

                case Spot.Last:
                    Entries.Add(new MRUEntry(emp));
                    return true;

                default:
                    throw new NotImplementedException(addInSpot.ToString());
            }
        }

        internal void RemoveEntry(Spot addInSpot)
        {
            if (Entries == null) return;

            if (Entries.Count == 0) return;

            switch (addInSpot)
            {
                case Spot.First:
                    Entries.RemoveAt(0);
                    break;
                case Spot.Last:
                    Entries.RemoveAt(Entries.Count - 1);
                    break;
                default:
                    break;
            }
        }
        #endregion AddRemoveEntries

        #region AddRemovePinnedEntries
        internal void AddPinedEntry(MRUEntry emp)
        {
            if (emp == null)
                return;

            if (Entries == null)
                Entries = new List<MRUEntry>();

            Entries.Add(new MRUEntry(emp));
        }
        #endregion AddRemovePinnedEntries

        internal void RemoveMruPath(string p)
        {
            if (Entries != null && p != null)
                Entries.RemoveAll(item => p == item.PathFileName);
        }
    }
}
