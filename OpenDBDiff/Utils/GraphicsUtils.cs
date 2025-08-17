using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace OpenDBDiff.Utils;

internal static class GraphicsUtils
{
    private static readonly Lazy<Bitmap> bitmap = new(() => new Bitmap(1, 1));
    private static readonly Lazy<Graphics> graphics = new(() => Graphics.FromImage(bitmap.Value));

    private static readonly Lazy<string> preferredFont = new(() =>
    {
        var fontNames = new string[] { "Consolas", "Courier New", "Courier" };
        var fontsCollection = new InstalledFontCollection();
        var installedNames = fontsCollection.Families.Select(ff => ff.Name);
        return fontNames.FirstOrDefault(fn => installedNames.Contains(fn)) ?? "Courier";
    });

    public static SizeF GetTextDimensions(string text, Font font) => GetTextDimensions(text, font, StringFormat.GenericTypographic);

    public static SizeF GetTextDimensions(string text, Font font, StringFormat stringFormat) => graphics.Value.MeasureString(text, font, int.MaxValue, stringFormat);

    public static SizeF GetTextDimensions(string text, string fontName, float fontSize) => GetTextDimensions(text, fontName, fontSize, StringFormat.GenericTypographic);

    public static SizeF GetTextDimensions(string text, string fontName, float fontSize, StringFormat stringFormat)
    {
        var font = new Font(fontName, fontSize);
        return graphics.Value.MeasureString(text, font, int.MaxValue, stringFormat);
    }

    public static string PreferredFont() => preferredFont.Value;
}
