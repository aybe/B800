B800
====

Renders B800 text to a BGRA buffer.

**Features**

- support of arbitrary fonts
- support of arbitrary column width
- character expansion style like in VGA text mode

**Showcase**

![](https://raw.githubusercontent.com/aybe/B800/master/sample.png)

**Example**

Reading a text file and producing a bitmap out of it (WPF):

```
string path = @"Samples\SAMPLE1.TXT";

byte[] b800Data = File.ReadAllBytes(path);
bool expand = true;
int columns = 80;
Font font = new Cp437Font16();

Tuple<int, int, int[]> render = B800Renderer.Render(font, b800Data, columns, expand);
int pixelWidth = render.Item1;
int pixelHeight = render.Item2;
int[] pixels = render.Item3;
BitmapSource source =
    BitmapSource.Create(pixelWidth, pixelHeight, 96, 96, PixelFormats.Bgra32, null, pixels, pixelWidth*4);
```
**TODO**

- implement the blink flag to produce an animation when content is animated

**Credits**

[Joseph Gil for the fonts.](http://ftp.sunet.se/pub/simtelnet/msdos/screen/fntcol16.zip)

[Malvineous for the B800 text page.](http://www.shikadi.net/moddingwiki/B800_Text)

And me for the code :D
