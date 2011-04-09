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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Threading;
using Project290.Screens;
using Project290.GameElements;

namespace Project290.Storage
{
    /// <summary>
    /// Used for saving ordered or unordered game data for up to 256 classes of game saves for any
    /// number of users, uniquely identified by a Gamertag.
    /// </summary>
    public class GameSaver
    {
        public byte[] buffer { get; private set; }
        public int Columns { get; private set; }

        private string filename;
        private string backupFilename;
        private string gameTitle;
        private Object stateobj;
        private StorageDevice device;
        private bool successfulWrite;
        private bool hasBufferChanged;
        private bool updateScoreboard;
        private Dictionary<string, int> gamertagLookup;
        private bool[] columnSortOrder;
        private Scoreboard scoreBoardCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSaver"/> class.
        /// </summary>
        /// <param name="gameTitle">The game title. This must be unique for the game,
        /// as if not, two scoreboards saved on the same console may merge together when
        /// they should not.</param>
        /// <param name="columnSortOrder">The sort orders. This should be the same length as
        /// the orderedColumnsToSort array, and true at an index means the scores will
        /// be sorted from high to low.</param>
        public GameSaver(string gameTitle, bool[] columnSortOrder)
        {        
            this.gameTitle = gameTitle;
            this.filename = this.gameTitle.ToLower().Replace(" ", "") + ".sav";
            this.backupFilename = this.gameTitle.ToLower().Replace(" ", "") + "_.sav";
            this.hasBufferChanged = true;
            this.updateScoreboard = true;
            this.columnSortOrder = columnSortOrder;
            this.Columns = columnSortOrder.Length;
        }

        /// <summary>
        /// Resets this instance. This must be called before saving can be done, but
        /// unless the storage device is removed, this need only be called once.
        /// </summary>
        public void Reset()
        {
            try
            {
                if (!Guide.IsVisible)
                {
                    if (GameWorld.controller == null || GameWorld.controller.gamer == null || GameWorld.controller.gamer.IsDisposed)
                    {
                        return;
                    }

                    // Reset the device
                    this.device = null;
                    this.stateobj = new Object();
                    StorageDevice.BeginShowSelector(GameWorld.controller.playerIndex, this.GetDevice, stateobj);
                }
            }
            catch { }
        }

        /// <summary>
        /// Sets the game data for the specified gamertag with the specified
        /// score (value) at the specified score column (index). No change will be
        /// made if the ordering constraint for that column index is not satisfied.
        /// Note that the changes made by calling this method will not be made until
        /// calling the Write() method.
        /// </summary>
        /// <param name="gamertag">The gamertag.</param>
        /// <param name="scoreToSet">The score to write.</param>
        /// <param name="columnIndex">Index of the column.</param>
        public void SetScore(string gamertag, int scoreToSet, int columnIndex)
        {
            this.SetScore(gamertag, scoreToSet, columnIndex, false);
        }

        /// <summary>
        /// Sets the game data for the specified gamertag with the specified
        /// score (value) at the specified score column (index). If overwriteOrdering
        /// is true, then the value of that player for this index will be overwritten,
        /// and if this value is set to false, then no change will be made if the
        /// ordering constraint for that column index is not satisfied. Note that the
        /// changes made by calling this method will not be made until calling the
        /// Write() method.
        /// </summary>
        /// <param name="gamertag">The gamertag.</param>
        /// <param name="scoreToSet">The score to write.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <param name="overwriteOrdering">If overwriteOrdering
        /// is true, then the value of that player for this index will be overwritten,
        /// and if this value is set to false, then no change will be made if the
        /// ordering constraint for that column index is not satisfied.</param>
        public void SetScore(string gamertag, int scoreToSet, int columnIndex, bool overwriteOrdering)
        {
            // Prevent buffer overflow.
            if (gamertag.Length > 20)
            {
                return;
            }

            // Check the buffer.
            this.CheckBuffer();

            // Update the gamertag lookup table before.
            this.UpdateGamertagLookup();

            // If score to set is for some reason int.MinValue, cheat.
            if (scoreToSet == int.MinValue)
            {
                scoreToSet = int.MinValue + 1;
            }

            bool columnSortOrder = this.GetColumnSortOrder(columnIndex);
            int currentScore = this.GetScore(gamertag, columnIndex);
            if (currentScore == int.MinValue || overwriteOrdering ||
                (columnSortOrder && currentScore < scoreToSet ||
                !columnSortOrder && currentScore > scoreToSet))
            {
                // Modify the data.
                this.WriteInfoToBuffer(gamertag, scoreToSet, columnIndex);
            }
        }

        /// <summary>
        /// Writes the data in the buffer to the file.
        /// </summary>
        /// <returns>
        /// True if the write was successful and false otherwise.
        /// </returns>
        public bool Write()
        {
            try
            {
                if (device == null || !device.IsConnected)
                {
                    // TODO: Force device selection.
                    return false;
                }

                this.successfulWrite = true;
                device.BeginOpenContainer(this.gameTitle, this.WriteData, this.stateobj);
                return this.successfulWrite;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Reads the file and stores it to the buffer. WARNING:
        /// This process will overwrite the buffer.
        /// </summary>
        /// <returns>True if the read was successful and false otherwise.</returns>
        public bool Read()
        {
            try
            {
                if (this.hasBufferChanged)
                {
                    if (device == null || !device.IsConnected)
                    {
                        // TODO: Force device selection.
                        this.buffer = null;
                        return false;
                    }

                    device.BeginOpenContainer(this.gameTitle, this.ReadInBuffer, stateobj);
                }

                if (this.buffer == null || this.buffer.Length < 33)
                {
                    this.buffer = null;
                    this.hasBufferChanged = true;
                    this.updateScoreboard = true;
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the score value at the specified column index for the
        /// specified gamertag. Returns int.MinValue if there is a problem.
        /// </summary>
        /// <param name="gamertag">The gamertag.</param>
        /// <param name="columnIndex">The zero-based column index.</param>
        /// <returns>
        /// The score value at the specified column index for the
        /// specified gamertag. Returns int.MinValue if there is a problem.
        /// </returns>
        public int GetScore(string gamertag, int columnIndex)
        {
            if (gamertag.Length > 20)
            {
                return int.MinValue;
            }

            if (this.gamertagLookup == null || !this.gamertagLookup.ContainsKey(gamertag))
            {
                return int.MinValue;
            }

            int valueIndex = this.gamertagLookup[gamertag] + 20 + 4 * columnIndex;
            return GetValueFromBuffer(this.buffer, valueIndex);
        }

        /// <summary>
        /// Gets the value from buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="startingIndex">The starting index.</param>
        /// <returns>The integer value from the 4 bytes from the starting index.</returns>
        private static int GetValueFromBuffer(byte[] buffer, int startingIndex)
        {
            if (buffer == null || startingIndex + 3 >= buffer.Length)
            {
                return int.MinValue;
            }

            int toReturn = 0;
            toReturn |= (int)buffer[startingIndex + 3];
            toReturn |= (int)(((int)buffer[startingIndex + 2]) << 8);
            toReturn |= (int)(((int)buffer[startingIndex + 1]) << 16);
            toReturn |= (int)(((int)buffer[startingIndex]) << 24);
            return toReturn;
        }

        /// <summary>
        /// Gets the device.
        /// </summary>
        /// <param name="result">The result.</param>
        private void GetDevice(IAsyncResult result)
        {
            try
            {
                this.device = StorageDevice.EndShowSelector(result);

                this.Read();
                this.CheckBuffer();
                this.GetAllColumnSortOrders();
            }
            catch { }
        }

        /// <summary>
        /// Reads the in buffer.
        /// </summary>
        /// <param name="result">The result.</param>
        private void ReadInBuffer(IAsyncResult result)
        {
            try
            {
                StorageContainer container = device.EndOpenContainer(result);

                // Create a new file if it does not exist.
                if (!container.FileExists(filename))
                {
                    // Check to see whether the save exists.
                    if (container.FileExists(this.backupFilename))
                    {
                        Stream file = container.OpenFile(this.backupFilename, FileMode.Open);
                        Stream copyfile = container.CreateFile(this.filename);

                        for (int i = 0; i < file.Length; i++)
                        {
                            copyfile.WriteByte((byte)file.ReadByte());
                        }

                        copyfile.Close();
                        file.Close();
                    }
                    else
                    {
                        Stream file = container.CreateFile(this.filename);
                        file.Close();
                    }
                }

                this.ReadOverwriteBuffer(container);
                container.Dispose();

                if (this.buffer.Length < 33)
                {
                    this.buffer = null;
                    this.hasBufferChanged = true;
                    this.updateScoreboard = true;
                    return;
                }
            }
            catch { }
        }

        /// <summary>
        /// Reads the buffer, and overwrites the content of it.
        /// </summary>
        /// <param name="container">The container.</param>
        private void ReadOverwriteBuffer(StorageContainer container)
        {
            // Failsafe
            if (container == null)
            {
                return;
            }

            this.CheckBuffer();

            if (container.FileExists(filename))
            {
                Stream fileRead = container.OpenFile(filename, FileMode.Open);
                this.buffer = new byte[fileRead.Length];
                fileRead.Read(this.buffer, 0, buffer.Length);
                fileRead.Close();
            }

            this.hasBufferChanged = false;
            this.updateScoreboard = true;

            this.CheckBuffer();

            this.UpdateGamertagLookup();
        }

        /// <summary>
        /// Checks the buffer to make sure it is not brand new.
        /// If so, then it sets the size of the columns of the buffer.
        /// </summary>
        private void CheckBuffer()
        {
            if (this.buffer == null || this.buffer.Length < 33)
            {
                this.buffer = new byte[33];
                this.buffer[0] = (byte)this.Columns;
                for (int i = 0; i < (byte)this.Columns; i++)
                {
                    this.SetColumnSortOrder(this.columnSortOrder[i], i);
                }

                this.hasBufferChanged = true;
                this.updateScoreboard = true;
            }
        }

        /// <summary>
        /// Sets the column sort order for the corresponding column, depending on the 
        /// specified order (ascending or not).
        /// </summary>
        /// <param name="ascending">if set to <c>true</c> [ascending].</param>
        /// <param name="column">The column index.</param>
        private void SetColumnSortOrder(bool ascending, int column)
        {
            if (this.buffer == null || this.buffer.Length < 33 ||
                column < 0 || column > 255)
            {
                return;
            }

            byte activeByte = 0;
            switch (column % 8)
            {
                case 0:
                    activeByte = 0x01;
                    break;
                case 1:
                    activeByte = 0x02;
                    break;
                case 2:
                    activeByte = 0x04;
                    break;
                case 3:
                    activeByte = 0x08;
                    break;
                case 4:
                    activeByte = 0x10;
                    break;
                case 5:
                    activeByte = 0x20;
                    break;
                case 6:
                    activeByte = 0x40;
                    break;
                case 7:
                    activeByte = 0x80;
                    break;
            }

            if (ascending)
            {
                this.buffer[1 + column / 8] |= activeByte;
            }
            else
            {
                this.buffer[1 + column / 8] &= (byte)~activeByte;
            }

            this.hasBufferChanged = true;
            this.updateScoreboard = true;
        }

        /// <summary>
        /// Gets the column sort order for the specified column index.
        /// </summary>
        /// <param name="column">The column index.</param>
        /// <returns>Whether or not the sort order for the specified column index is ascending.</returns>
        private bool GetColumnSortOrder(int column)
        {
            if (this.buffer == null || this.buffer.Length < 33)
            {
                CheckBuffer();
            }

            byte activeByte = 0;
            switch (column % 8)
            {
                case 0:
                    activeByte = 0x01;
                    break;
                case 1:
                    activeByte = 0x02;
                    break;
                case 2:
                    activeByte = 0x04;
                    break;
                case 3:
                    activeByte = 0x08;
                    break;
                case 4:
                    activeByte = 0x10;
                    break;
                case 5:
                    activeByte = 0x20;
                    break;
                case 6:
                    activeByte = 0x40;
                    break;
                case 7:
                    activeByte = 0x80;
                    break;
            }

            return (this.buffer[1 + column / 8] & activeByte) == activeByte;
        }

        /// <summary>
        /// From the buffer data, this writes the column sort orders to the 
        /// global variable this.columnSortOrder.
        /// </summary>
        private void GetAllColumnSortOrders()
        {
            if (this.columnSortOrder == null || this.columnSortOrder.Length != 256)
            {
                this.columnSortOrder = new bool[256];
            }

            for (int i = 0; i < 256; i++)
            {
                this.columnSortOrder[i] = this.GetColumnSortOrder(i);
            }
        }

        /// <summary>
        /// Writes the data.
        /// </summary>
        /// <param name="result">The result.</param>
        private void WriteData(IAsyncResult result)
        {
            try
            {
                StorageContainer container = device.EndOpenContainer(result);
                this.CheckBuffer();

                // Failsafe.
                if (container == null)
                {
                    return;
                }

                // Create a new file if it does not exist.
                if (!container.FileExists(this.filename))
                {
                    // Check to see whether the save exists.
                    if (container.FileExists(this.backupFilename))
                    {
                        Stream file = container.OpenFile(this.backupFilename, FileMode.Open);
                        Stream copyfile = container.CreateFile(this.filename);

                        for (int i = 0; i < file.Length; i++)
                        {
                            copyfile.WriteByte((byte)file.ReadByte());
                        }

                        copyfile.Close();
                        file.Close();
                    }
                    else
                    {
                        Stream file = container.CreateFile(this.filename);
                        file.Close();
                    }
                }

                // Back up the file in case the machine gets turned off mid-process.
                if (container.FileExists(this.filename))
                {
                    // Check to see whether the save exists.
                    if (container.FileExists(this.backupFilename))
                    {
                        // Delete it so that we can create one fresh.
                        container.DeleteFile(this.backupFilename);
                    }

                    Stream file = container.OpenFile(this.filename, FileMode.Open);
                    Stream copyfile = container.CreateFile(this.backupFilename);

                    for (int i = 0; i < file.Length; i++)
                    {
                        copyfile.WriteByte((byte)file.ReadByte());
                    }

                    copyfile.Close();
                    file.Close();
                }

                // Backup the buffer.
                byte[] backup = new byte[this.buffer.Length];
                this.buffer.CopyTo(backup, 0);

                // Read the data from the file into the main buffer.
                this.ReadOverwriteBuffer(container);

                // Check to see whether the save exists.
                if (container.FileExists(this.filename))
                {
                    // Delete it so that we can create one fresh.
                    container.DeleteFile(this.filename);
                }

                // Check the buffer.
                this.CheckBuffer();

                // And in case whatever is saved in the new buffer is better
                // than what just got read in, merge it back in.
                this.MergeBuffer(backup);

                // Write the data.
                Stream fileWrite = container.CreateFile(this.filename);
                fileWrite.Write(this.buffer, 0, this.buffer.Length);
                fileWrite.Close();

                // Dispose the container, to commit the data.
                container.Dispose();

                // Make sure the column sort orders are up to date.
                this.GetAllColumnSortOrders();
            }
            catch { }
        }

        /// <summary>
        /// Updates the gamertag lookup table based on the buffer.
        /// </summary>
        private void UpdateGamertagLookup()
        {
            if (this.buffer == null || this.buffer.Length < 33)
            {
                this.gamertagLookup = null;
                return;
            }

            int numberOfBufferEntries = this.NumberOfBufferEntries();

            if (this.gamertagLookup == null)
            {
                this.gamertagLookup = new Dictionary<string, int>(2 * numberOfBufferEntries);
            }

            // Make sure the lookup table contains all keys in the buffer.
            for (int i = 0; i < numberOfBufferEntries; i++)
            {
                int gamertagIndex = this.GetGamertagIndex(i);
                string gamertag = this.GetGamertag(gamertagIndex, 20);
                if (this.gamertagLookup.ContainsKey(gamertag))
                {
                    int tempIndex;
                    if (this.gamertagLookup.TryGetValue(gamertag, out tempIndex))
                    {
                        if (tempIndex != gamertagIndex)
                        {
                            this.gamertagLookup[gamertag] = gamertagIndex;
                        }
                    }
                }
                else
                {
                    this.gamertagLookup.Add(gamertag, gamertagIndex);
                }
            }

            // Make sure the lookup table does not contain keys that are not in the buffer.
            if (this.gamertagLookup.Count > numberOfBufferEntries)
            {
                for (int i = 0; i < this.gamertagLookup.Count; i++)
                {
                    KeyValuePair<string, int> temp = this.gamertagLookup.ElementAt<KeyValuePair<string, int>>(i);
                    if (temp.Value >= this.buffer.Length)
                    {
                        this.gamertagLookup.Remove(temp.Key);
                        i--;
                        continue;
                    }
                    if (!this.GetGamertag(temp.Value, 20).Equals(temp.Key))
                    {
                        this.gamertagLookup.Remove(temp.Key);
                        i--;
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the number of bytes needed to store a gamertag and
        /// the data in the associated columns.
        /// </summary>
        /// <returns>The number of bytes needed to store a gamertag and
        /// the data in the associated columns.</returns>
        private int GetBlockSize()
        {
            if (this.buffer == null || this.buffer.Length < 33)
            {
                this.CheckBuffer();
            }

            // 20 bytes per gamertag, 4 bytes per each column, the number
            // of columns is identified by the first byte, which is reserved.
            return 20 + 4 * this.buffer[0];
        }

        /// <summary>
        /// Gets the starting index in the buffer of the nth gamertag.
        /// </summary>
        /// <param name="nthGamertag">The NTH gamertag to get the index of.</param>
        /// <returns>
        /// The starting index in the buffer of the nth gamertag.
        /// </returns>
        private int GetGamertagIndex(int nthGamertag)
        {
            if (this.buffer == null || this.buffer.Length < 33)
            {
                return -1;
            }
            
            // 20 bytes per gamertag, 4 bytes per each column, the number
            // of columns is identified by the first byte, which is reserved.
            // 32 bytes also reserved at the beginning.
            return 33 + (this.GetBlockSize()) * nthGamertag;
        }

        /// <summary>
        /// Gets the number of actual buffer entries.
        /// </summary>
        /// <returns>
        /// The number of gamertag-associated buffer entries.
        /// </returns>
        private int NumberOfBufferEntries()
        {
            if (this.buffer == null || this.buffer.Length < 33)
            {
                this.CheckBuffer();
            }

            // 20 bytes per gamertag, 4 bytes per each column, the number
            // of columns is identified by the first byte, which is reserved.
            // 32 bytes also reserved at the beginning.
            return (this.buffer.Length - 33) / this.GetBlockSize();
        }

        /// <summary>
        /// Gets the gamertag from the buffer between the startIndex
        /// (inclusive) and startIndex + length (exclusive).
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns>
        /// The gamertag from the buffer between the startIndex
        /// (inclusive) and startIndex + length (exclusive).
        /// </returns>
        private string GetGamertag(int startIndex, int length)
        {
            return GetGamertag(this.buffer, startIndex, length);
        }

        /// <summary>
        /// Gets the gamertag from the buffer between the startIndex
        /// (inclusive) and startIndex + length (exclusive).
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns>
        /// The gamertag from the buffer between the startIndex
        /// (inclusive) and startIndex + length (exclusive).
        /// </returns>
        private static string GetGamertag(byte[] buffer, int startIndex, int length)
        {
            StringBuilder toReturn = new StringBuilder();
            
            for (int i = startIndex; i < startIndex + length; i++)
            {
                if (buffer[i] != 0)
                {
                    toReturn.Append((char)buffer[i]);
                }
            }

            return toReturn.ToString();
        }

        /// <summary>
        /// Writes the gamer tag to the buffer at the specified index.
        /// </summary>
        /// <param name="gamertag">The gamertag.</param>
        /// <param name="index">The index.</param>
        private void WriteGamerTag(string gamertag, int index)
        {
            WriteGamerTag(this.buffer, gamertag, index);
        }

        /// <summary>
        /// Writes the gamer tag to the buffer at the specified index.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="gamertag">The gamertag.</param>
        /// <param name="index">The index.</param>
        private static void WriteGamerTag(byte[] buffer, string gamertag, int index)
        {
            if (gamertag.Length > 20)
            {
                return;
            }

            for (int i = 0; i < gamertag.Length && i + index < buffer.Length; i++)
            {
                buffer[index + i] = (byte)gamertag[i];
            }
        }

        /// <summary>
        /// Writes the specified info to the buffer.
        /// </summary>
        /// <param name="gamertag">The gamertag.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>
        /// True if the operation was successful and false otherwise.
        /// </returns>
        private bool WriteInfoToBuffer(string gamertag, int value, int columnIndex)
        {
            // Failsafe.
            if (this.buffer == null || this.buffer.Length < 33 ||
                columnIndex >= this.buffer[0] || gamertag.Length > 20)
            {
                return false;
            }

            this.hasBufferChanged = true;
            this.updateScoreboard = true;

            // If value is int.MinValue, cheat.
            if (value == int.MinValue)
            {
                value = int.MinValue + 1;
            }

            if (this.gamertagLookup.ContainsKey(gamertag))
            {
                int index = this.gamertagLookup[gamertag] + 20 + 4 * columnIndex;
                if (index + 3 >= this.buffer.Length)
                {
                    return false;
                }

                this.buffer[index + 3] = (byte)(value & 0xFF);
                this.buffer[index + 2] = (byte)((value & 0xFF00) >> 8);
                this.buffer[index + 1] = (byte)((value & 0xFF0000) >> 16);
                this.buffer[index] = (byte)((value & 0xFF000000) >> 24);
            }
            else
            {
                // This is the case where the buffer needs to grow.
                byte[] temp = new byte[this.buffer.Length + this.GetBlockSize()];
                for (int i = 0; i < temp.Length; i++)
                {
                    if (i < this.buffer.Length)
                    {
                        temp[i] = this.buffer[i];
                    }
                    else
                    {
                        // Have to special condition to set all new values to int.minvalue
                        if (i > this.buffer.Length + 20 && (i - this.buffer.Length - 20) % 4 == 0)
                        {
                            temp[i] = 128;
                        }
                        else
                        {
                            temp[i] = 0;
                        }
                    }
                }
                
                // Recreate the buffer.
                this.buffer = new byte[temp.Length];
                for (int i = 0; i < this.buffer.Length; i++)
                {
                    this.buffer[i] = temp[i];
                }

                // Write the gamertag and value.
                int index = this.buffer.Length - this.GetBlockSize();
                this.WriteGamerTag(gamertag, index);

                index += 4 * columnIndex + 20;
                if (index + 3 >= this.buffer.Length)
                {
                    return false;
                }

                this.buffer[index + 3] = (byte)(value & 0xFF);
                this.buffer[index + 2] = (byte)((value & 0xFF00) >> 8);
                this.buffer[index + 1] = (byte)((value & 0xFF0000) >> 16);
                this.buffer[index] = (byte)((value & 0xFF000000) >> 24);
            }

            this.UpdateGamertagLookup();

            return true;
        }

        /// <summary>
        /// Merges the specified buffer with this buffer.
        /// </summary>
        /// <param name="bufferToMerge">The buffer to merge.</param>
        public void MergeBuffer(byte[] bufferToMerge)
        {
            // Failsafe. Is the bufferToMerge null or not compatable?
            this.CheckBuffer();
            this.UpdateGamertagLookup();
            if (bufferToMerge == null || bufferToMerge.Length < 33 ||
                (bufferToMerge.Length - 33) % this.GetBlockSize() != 0 ||
                this.buffer == null || this.buffer.Length < 33 ||
                this.buffer[0] != bufferToMerge[0])
            {
                return;
            }

            int blockSize = this.GetBlockSize();

            // First, store the gamertags the must be added.
            List<string> newGamertags = new List<string>();
            List<int[]> newColumns = new List<int[]>();
            int numberOfBufferToMergeEntries = (bufferToMerge.Length - 33) / blockSize;
            for (int i = 0; i < numberOfBufferToMergeEntries; i++)
            {
                int gamertagIndex = this.GetGamertagIndex(i);
                string gamertagAtI = GetGamertag(bufferToMerge, gamertagIndex, 20);
                if (!this.gamertagLookup.ContainsKey(gamertagAtI))
                {
                    newGamertags.Add(gamertagAtI);
                    int[] toAdd = new int[bufferToMerge[0]];
                    for (int j = 0; j < toAdd.Length; j++)
                    {
                        toAdd[j] = GetValueFromBuffer(bufferToMerge, gamertagIndex + 20 + j * 4);
                    }
                    newColumns.Add(toAdd);
                }
            }

            // If there are gamertags to add, the buffer needs expanded.
            if (newGamertags.Count > 0)
            {
                this.hasBufferChanged = true;
                this.updateScoreboard = true;
                int oldBufferLength = this.buffer.Length;
                byte[] temp = new byte[this.buffer.Length + newGamertags.Count * blockSize];
                for (int i = 0; i < temp.Length; i++)
                {
                    if (i < this.buffer.Length)
                    {
                        temp[i] = this.buffer[i];
                    }
                    else
                    {
                        temp[i] = 0;
                    }
                }

                // Recreate the buffer.
                this.buffer = new byte[temp.Length];
                for (int i = 0; i < this.buffer.Length; i++)
                {
                    this.buffer[i] = temp[i];
                }

                // At this point, set the values for all new gamertag scores
                for (int i = 0; i < newGamertags.Count; i++)
                {
                    int currentIndex = oldBufferLength + i * blockSize;

                    // Write the gamertag.
                    WriteGamerTag(this.buffer, newGamertags[i], currentIndex);

                    // Add the gamertag to the lookup table.
                    this.gamertagLookup.Add(newGamertags[i], currentIndex);

                    // Shift for the gamertag length
                    currentIndex += 20;

                    // Write the column data.
                    for (int j = 0; j < newColumns[i].Length; j++)
                    {
                        if (currentIndex + 3 < this.buffer.Length)
                        {
                            this.buffer[currentIndex + 3] = (byte)(newColumns[i][j] & 0xFF);
                            this.buffer[currentIndex + 2] = (byte)((newColumns[i][j] & 0xFF00) >> 8);
                            this.buffer[currentIndex + 1] = (byte)((newColumns[i][j] & 0xFF0000) >> 16);
                            this.buffer[currentIndex] = (byte)((newColumns[i][j] & 0xFF000000) >> 24);
                            currentIndex += 4;
                        }
                    }
                }
            }

            // At this point, all brand new gamertags have been added, so now merge
            // the high scores from the merge buffer with this buffer.
            foreach (KeyValuePair<string, int> kvp in this.gamertagLookup)
            {
                for (int j = 0; j < this.buffer[0]; j++)
                {
                    int index = kvp.Value + 20 + 4 * j;
                    int newScore = GetValueFromBuffer(bufferToMerge, index);
                    
                    if (index + 3 < this.buffer.Length)
                    {
                        int currentScore = this.GetScore(kvp.Key, j);
                        bool columnSortOrder = this.GetColumnSortOrder(j);
                        if (currentScore == int.MinValue ||
                            (columnSortOrder && currentScore < newScore ||
                            !columnSortOrder && currentScore > newScore))
                        {
                            this.buffer[index + 3] = (byte)(newScore & 0xFF);
                            this.buffer[index + 2] = (byte)((newScore & 0xFF00) >> 8);
                            this.buffer[index + 1] = (byte)((newScore & 0xFF0000) >> 16);
                            this.buffer[index] = (byte)((newScore & 0xFF000000) >> 24);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the Scoreboard corresponding to this game save data.
        /// </summary>
        /// <param name="columnIndexToSort">The column index to sort on.</param>
        /// <returns>The Scoreboard corresponding to this game save data.</returns>
        public Scoreboard GetScoreBoard(int columnIndexToSort)
        {
            return GetScoreBoard(new int[] { columnIndexToSort } );
        }

        /// <summary>
        /// Gets the score board corresponding to this game save data.
        /// </summary>
        /// <param name="orderedColumnIndicesToSort">The ordered column indices to sort on. 
        /// The indicides that are specified first in the aray are the first that are sorted
        /// on. If there is a tie, the next indicies in the array are used, and so on.</param>
        /// <returns>The Scoreboard corresponding to this game save data.</returns>
        public Scoreboard GetScoreBoard(int[] orderedColumnIndicesToSort)
        {
            // If the scoreboard is already created, just sort and return.
            if (!this.updateScoreboard && this.scoreBoardCache != null)
            {
                this.scoreBoardCache.Sort(orderedColumnIndicesToSort, this.columnSortOrder, false);
                return this.scoreBoardCache;
            }

            // Failsafe.
            if (this.buffer == null || this.buffer.Length < 33)
            {
                return null;
            }

            this.scoreBoardCache = new Scoreboard();

            int numberOfBufferEntries = this.NumberOfBufferEntries();
            for (int i = 0; i < numberOfBufferEntries; i++)
            {
                int gamertagIndex = this.GetGamertagIndex(i);
                string gamertag = this.GetGamertag(gamertagIndex, 20);
                int[] columns = new int[this.buffer[0]];
                int blockSize = this.GetBlockSize();
                int index = 0;
                for (int j = gamertagIndex + 20; j < gamertagIndex + blockSize && j + 3 < this.buffer.Length; j += 4)
                {
                    columns[index] = 0;
                    columns[index] |= (int)this.buffer[j + 3];
                    columns[index] |= (int)(((int)this.buffer[j + 2]) << 8);
                    columns[index] |= (int)(((int)this.buffer[j + 1]) << 16);
                    columns[index] |= (int)(((int)this.buffer[j]) << 24);
                    index++;
                }

                this.scoreBoardCache.Add(new PlayerScore(gamertag, columns));
            }

            this.updateScoreboard = false;
            this.scoreBoardCache.Sort(orderedColumnIndicesToSort, this.columnSortOrder, true);
            return this.scoreBoardCache;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            // Don't update if at start screen.
            if (GameWorld.screens.Count == 0)
            {
                return;
            }

            foreach (Screen screen in GameWorld.screens)
            {
                if (screen is StartScreen)
                {
                    return;
                }
            }

            if (this.device == null)
            {
                this.Reset();
            }
        }
    }
}
