namespace RobotTools.Controls.MRU
{
    /// <summary>
    /// This enumeration is used to control the behaviour of pinned entries.
    /// </summary>
    public enum MRUSortMethod
    {
        /// <summary>
        /// Pinned entries are sorted and displayed at the beginning of the list or just be bookmarked
        /// and stay wehere they are in the list.
        /// </summary>
        PinnedEntriesFirst = 0,

        /// <summary>
        /// Pinned entries are just be bookmarked and stay wehere they are in the list. This can be useful
        /// for a list of favourites (which stay if pinned) while other unpinned entries are changed as the
        /// user keeps opening new items and thus, changing the MRU list...
        /// </summary>
        UnsortedFavourites = 1
    }
}
