using System;

namespace B800.Fonts
{
    /// <summary>
    ///     Represents a raster font.
    /// </summary>
    public abstract class Font
    {
        /// <summary>
        ///     Gets the numbers of characters present in this font.
        /// </summary>
        public abstract int CharCount { get; }

        /// <summary>
        ///     Gets the width in pixels of a character.
        /// </summary>
        public abstract int CharWidth { get; }

        /// <summary>
        ///     Gets the height in pixels of a character.
        /// </summary>
        public abstract int CharHeight { get; }

        /// <summary>
        ///     Gets the character data of this font (see Remarks).
        /// </summary>
        /// <remarks>
        ///     The content of this array is a raw dump of a font file, prefer <see cref="GetCharData" /> to get the data of a
        ///     character.
        /// </remarks>
        public abstract byte[] CharData { get; }

        /// <summary>
        ///     Gets the character data of a specified character.
        /// </summary>
        /// <param name="index">Index of the character for which to return data.</param>
        /// <returns>An array containing rows of pixels (as bits) for the specified character.</returns>
        public byte[] GetCharData(int index)
        {
            if (index <= 0 || index >= CharCount)
                throw new ArgumentOutOfRangeException("index");
            int length = CharWidth/8*CharHeight;
            var bytes = new byte[length];
            int sourceIndex = length*index;
            Array.Copy(CharData, sourceIndex, bytes, 0, length);
            return bytes;
        }
    }
}