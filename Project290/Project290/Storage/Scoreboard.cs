//////__     __               _                 _     _               _   
//////\ \   / /              | |               | |   | |             | |  
////// \ \_/ /__  _   _   ___| |__   ___  _   _| | __| |  _ __   ___ | |_ 
//////  \   / _ \| | | | / __| '_ \ / _ \| | | | |/ _` | | '_ \ / _ \| __|
//////   | | (_) | |_| | \__ \ | | | (_) | |_| | | (_| | | | | | (_) | |_ 
//////   |_|\___/ \__,_| |___/_| |_|\___/ \__,_|_|\__,_| |_| |_|\___/ \__|
//////      _                              _   _     _     _ 
//////     | |                            | | | |   (_)   | |
//////  ___| |__   __ _ _ __   __ _  ___  | |_| |__  _ ___| |
////// / __| '_ \ / _` | '_ \ / _` |/ _ \ | __| '_ \| / __| |
//////| (__| | | | (_| | | | | (_| |  __/ | |_| | | | \__ \_|
////// \___|_| |_|\__,_|_| |_|\__, |\___|  \__|_| |_|_|___(_)
//////                         __/ |                         
//////                        |___/                          

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project290.Storage
{
    /// <summary>
    /// This is used to convert the byte array represntation of the score data into a human
    /// readable format. This is also used for sorting the scoreboard columns as desired.
    /// </summary>
    public class Scoreboard : List<PlayerScore>
    {
        private int[] lastColumnOrderSorted;
        private static int[] sortingColumnOrder;
        private static bool[] sortingOrders;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scoreboard"/> class.
        /// </summary>
        public Scoreboard()
            : base()
        {
            this.lastColumnOrderSorted = new int[] { };
        }

        /// <summary>
        /// Sorts the specified ordered columns to sort.
        /// </summary>
        /// <param name="orderedColumnsToSort">The ordered columns to sort.</param>
        /// <param name="sortOrders">The sort orders. This should be the same length as
        /// the orderedColumnsToSort array, and true at an index means the scores will
        /// be sorted from high to low.</param>
        /// <param name="overrideSkipping">if set to <c>true</c> [override skipping].</param>
        public void Sort(int[] orderedColumnsToSort, bool[] sortOrders, bool overrideSkipping)
        {
            // Failsafe
            if (this.Count == 0 || orderedColumnsToSort.Length == 0 || orderedColumnsToSort.Length > sortOrders.Length)
            {
                return;
            }

            // The set could be large, so avoid sorting at all costs!
            if (orderedColumnsToSort.Length == this.lastColumnOrderSorted.Length && !overrideSkipping)
            {
                bool shouldReturn = true;
                for (int i = 0; i < orderedColumnsToSort.Length; i++)
                {
                    // Failsafe
                    if (orderedColumnsToSort[i] < 0 || orderedColumnsToSort[i] >= this[0].Scores.Length)
                    {
                        return;
                    }

                    if (orderedColumnsToSort[i] != this.lastColumnOrderSorted[i])
                    {
                        shouldReturn = false;
                    }
                }

                if (shouldReturn)
                {
                    return;
                }
            }

            sortingColumnOrder = orderedColumnsToSort;
            sortingOrders = sortOrders;
            this.Sort(Comparer);

            this.lastColumnOrderSorted = orderedColumnsToSort;
        }

        /// <summary>
        /// Comparers the specified first to the specified second.
        /// </summary>
        /// <param name="first">The first player score object.</param>
        /// <param name="second">The second player score object.</param>
        /// <returns>
        /// A negative number if the first should be indexed after the
        /// second when sorted, a positive number otherwise.
        /// </returns>
        private static int Comparer(PlayerScore first, PlayerScore second)
        {
            for (int i = 0; i < sortingColumnOrder.Length; i++)
            {
                int comparison = first.Scores[sortingColumnOrder[i]].CompareTo(second.Scores[sortingColumnOrder[i]]) * (sortingOrders[i] ? -1 : 1);
                if (comparison != 0)
                {
                    return comparison;
                }
            }

            return first.Gamertag.CompareTo(second.Gamertag);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(int columnIndex)
        {
            if (this.Count == 0)
            {
                return "Scoreboard is empty.";
            }

            StringBuilder toReturn = new StringBuilder();
            for (int i = 0; i < this.Count; i++)
            {
                toReturn.Append(this[i].ToString(columnIndex));
                if (i != this.Count - 1)
                {
                    toReturn.AppendLine();
                }
            }

            return toReturn.ToString();
        }
    }
}
