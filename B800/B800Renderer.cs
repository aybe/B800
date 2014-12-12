using System;
using System.Collections.Generic;
using B800.Fonts;

namespace B800
{
    /// <summary>
    ///     Renders B800 content.
    /// </summary>
    public static class B800Renderer
    {
        private static readonly uint[] Colors =
        {
            0xFF000000,
            0xFF0000AA,
            0xFF00AA00,
            0xFF00AAAA,
            0xFFAA0000,
            0xFFAA00AA,
            0xFFAA5500,
            0xFFAAAAAA,
            0xFF555555,
            0xFF5555FF,
            0xFF55FF55,
            0xFF55FFFF,
            0xFFFF5555,
            0xFFFF55FF,
            0xFFFFFF55,
            0xFFFFFFFF
        };

        /// <summary>
        ///     Renders B800 data (see Remarks).
        /// </summary>
        /// <param name="font">Font to use for rendering B800 data.</param>
        /// <param name="b800">An array of bytes containing B800 data.</param>
        /// <param name="columns">Columns desired (row count will be determined from this value).</param>
        /// <param name="widenChar">True to expand char width by 1 like VGA text mode.</param>
        /// <returns>A tuple containing bitmap width, height and pixels.</returns>
        /// <remarks>
        ///     Dimensions of characters in <paramref name="font" /> and <paramref name="widenChar" /> affects the dimensions of
        ///     the output. The order of color components in the returned array is BGRA.
        /// </remarks>
        public static Tuple<int, int, int[]> Render(Font font, byte[] b800, int columns, bool widenChar)
        {
            if (font == null) throw new ArgumentNullException("font");
            if (b800 == null) throw new ArgumentNullException("b800");
            if (columns <= 0) throw new ArgumentOutOfRangeException("columns");
            int chars = b800.Length/2;
            var rows = (int) Math.Ceiling((double) chars/columns);
            int charWidth = widenChar ? font.CharWidth + 1 : font.CharWidth;
            int charHeight = font.CharHeight;
            int screenWidth = charWidth*columns;
            int screenHeight = charHeight*rows;
            var buffer = new int[screenWidth*screenHeight];
            var queue = new Queue<byte>(b800);
            for (int i = 0; i < chars; i++)
            {
                byte ascii = queue.Dequeue();
                byte attributes = queue.Dequeue();
                int column = i%columns;
                int row = i/columns;
                byte[] @char = font.GetCharData(ascii);
                int x = column*charWidth;
                int y = row*charHeight;
                var foreground = (int) Colors[(attributes & 0x0F)];
                var background = (int) Colors[(attributes & 0xF0) >> 4];
                for (int y1 = 0; y1 < font.CharHeight; y1++)
                {
                    byte line = @char[y1];
                    for (int x1 = 0; x1 < font.CharWidth; x1++)
                    {
                        int bit = (line & (1 << (7 - x1))) >> (7 - x1);
                        bool on = bit == 1;
                        int color = on ? foreground : background;
                        int offset = (y + y1)*screenWidth + (x + x1);
                        buffer[offset] = color;
                        if (widenChar && x1 == font.CharWidth - 1)
                        {
                            buffer[offset + 1] = ascii >= 192 && ascii <= 223 && @on ? foreground : background;
                        }
                    }
                }
            }
            return new Tuple<int, int, int[]>(screenWidth, screenHeight, buffer);
        }
    }
}