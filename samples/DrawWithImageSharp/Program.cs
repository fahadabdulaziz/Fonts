﻿using SixLabors.ImageSharp;
using System.IO;

namespace SixLabors.Fonts.DrawWithImageSharp
{
    using global::DrawWithImageSharp;
    using Shapes;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;
    using SixLabors.ImageSharp.Processing.Drawing;
    using SixLabors.Shapes.Temp;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    public static class Program
    {
        public static void Main(string[] args)
        {
            FontCollection fonts = new FontCollection();
            FontFamily font = fonts.Install(@"..\..\tests\SixLabors.Fonts.Tests\Fonts\SixLaborsSampleAB.ttf");
            FontFamily fontWoff = fonts.Install(@"..\..\tests\SixLabors.Fonts.Tests\Fonts\SixLaborsSampleAB.woff");
            FontFamily font2 = fonts.Install(@"..\..\tests\SixLabors.Fonts.Tests\Fonts\OpenSans-Regular.ttf");
            FontFamily carter = fonts.Install(@"..\..\tests\SixLabors.Fonts.Tests\Fonts\Carter_One\CarterOne.ttf");
            FontFamily Wendy_One = fonts.Install(@"..\..\tests\SixLabors.Fonts.Tests\Fonts\Wendy_One\WendyOne-Regular.ttf");

            RenderText(font, "abc", 72);
            RenderText(font, "ABd", 72);
            RenderText(fontWoff, "abe", 72);
            RenderText(fontWoff, "ABf", 72);
            RenderText(font2, "ov", 72);
            RenderText(font2, "a\ta", 72);
            RenderText(font2, "aa\ta", 72);
            RenderText(font2, "aaa\ta", 72);
            RenderText(font2, "aaaa\ta", 72);
            RenderText(font2, "aaaaa\ta", 72);
            RenderText(font2, "aaaaaa\ta", 72);
            RenderText(font2, "Hello\nWorld", 72);
            RenderText(carter, "Hello\0World", 72);
            RenderText(Wendy_One, "Hello\0World", 72);

            RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 4 }, "\t\tx");
            RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 4 }, "\t\t\tx");
            RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 4 }, "\t\t\t\tx");
            RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 4 }, "\t\t\t\t\tx");

            RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 0 }, "Zero\tTab");

            RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 0 }, "Zero\tTab");
            RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 1 }, "One\tTab");
            RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 6 }, "\tTab Then Words");
            RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 1 }, "Tab Then Words");
            RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 1 }, "Words Then Tab\t");
            RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 1 }, "                 Spaces Then Words");
            RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 1 }, "Words Then Spaces                 ");
            RenderText(new RendererOptions(new Font(font2, 72)) { TabWidth = 1 }, "\naaaabbbbccccddddeeee\n\t\t\t3 tabs\n\t\t\t\t\t5 tabs");

            RenderText(new Font(SystemFonts.Find("Arial"), 20f, FontStyle.Regular), "á é í ó ú ç ã õ", 200, 50);
            RenderText(new Font(SystemFonts.Find("Arial"), 10f, FontStyle.Regular), "PGEP0JK867", 200, 50);

            RenderText(new RendererOptions(SystemFonts.CreateFont("consolas", 72)) { TabWidth = 4 }, "xxxxxxxxxxxxxxxx\n\txxxx\txxxx\n\t\txxxxxxxx\n\t\t\txxxx");

            BoundingBoxes.Generate("a b c y q G H T", SystemFonts.CreateFont("arial", 40f));

            TextAlignment.Generate(SystemFonts.CreateFont("arial", 50f));
            TextAlignmentWrapped.Generate(SystemFonts.CreateFont("arial", 50f));

            StringBuilder sb = new StringBuilder();
            for (char c = 'a'; c <= 'z'; c++)
            {
                sb.Append(c);
            }
            for (char c = 'A'; c <= 'Z'; c++)
            {
                sb.Append(c);
            }
            for (char c = '0'; c <= '9'; c++)
            {
                sb.Append(c);
            }
            string text = sb.ToString();

            foreach (FontFamily f in fonts.Families)
            {
                RenderText(f, text, 72);
            }
        }


        public static void RenderText(Font font, string text, int width, int height)
        {
            string path = System.IO.Path.GetInvalidFileNameChars().Aggregate(text, (x, c) => x.Replace($"{c}", "-"));
            string fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine("Output", System.IO.Path.Combine(path)));

            using (Image<Rgba32> img = new Image<Rgba32>(width, height))
            {
                img.Mutate(x=>x.Fill(Rgba32.White));

                IPathCollection shapes = SixLabors.Shapes.Temp.TextBuilder.GenerateGlyphs(text, new Primitives.PointF(50f, 4f), new RendererOptions(font, 72));
                img.Mutate(x => x.Fill(Rgba32.Black, shapes));

                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullPath));

                using (FileStream fs = File.Create(fullPath + ".png"))
                {
                    img.SaveAsPng(fs);
                }
            }
        }

        public static void RenderText(RendererOptions font, string text)
        {
            GlyphBuilder builder = new GlyphBuilder();
            TextRenderer renderer = new TextRenderer(builder);
            Primitives.SizeF size = TextMeasurer.Measure(text, font);
            renderer.RenderText(text, font);

            builder.Paths
                .SaveImage((int)size.Width + 20, (int)size.Height + 20, font.Font.Name, text + ".png");
        }
        public static void RenderText(FontFamily font, string text, float pointSize = 12)
        {
            RenderText(new RendererOptions(new Font(font, pointSize), 96) { ApplyKerning = true, WrappingWidth = 340 }, text);
        }

        public static void SaveImage(this IEnumerable<IPath> shapes, int width, int height, params string[] path)
        {
            path = path.Select(p => System.IO.Path.GetInvalidFileNameChars().Aggregate(p, (x, c) => x.Replace($"{c}", "-"))).ToArray();
            string fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine("Output", System.IO.Path.Combine(path)));

            using (Image<Rgba32> img = new Image<Rgba32>(width, height))
            {
                img.Mutate(x => x.Fill(Rgba32.DarkBlue));

                foreach (IPath s in shapes)
                {
                    // In ImageSharp.Drawing.Paths there is an extension method that takes in an IShape directly.
                    img.Mutate(x => x.Fill(Rgba32.HotPink, s.Translate(new Vector2(0, 0))));
                }
                // img.Draw(Color.LawnGreen, 1, shape);

                // Ensure directory exists
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullPath));

                using (FileStream fs = File.Create(fullPath))
                {
                    img.SaveAsPng(fs);
                }
            }
        }

        public static void SaveImage(this IEnumerable<IPath> shapes, params string[] path)
        {
            IPath shape = new ComplexPolygon(shapes.ToArray());
            shape = shape.Translate(shape.Bounds.Location * -1) // touch top left
                    .Translate(new Vector2(10)); // move in from top left

            StringBuilder sb = new StringBuilder();
            IEnumerable<ISimplePath> converted = shape.Flatten();
            converted.Aggregate(sb, (s, p) =>
            {
                foreach (Vector2 point in p.Points)
                {
                    sb.Append(point.X);
                    sb.Append('x');
                    sb.Append(point.Y);
                    sb.Append(' ');
                }
                s.Append('\n');
                return s;
            });
            string str = sb.ToString();
            shape = new ComplexPolygon(converted.Select(x => new Polygon(new LinearLineSegment(x.Points))).ToArray());

            path = path.Select(p => System.IO.Path.GetInvalidFileNameChars().Aggregate(p, (x, c) => x.Replace($"{c}", "-"))).ToArray();
            string fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine("Output", System.IO.Path.Combine(path)));
            // pad even amount around shape
            int width = (int)(shape.Bounds.Left + shape.Bounds.Right);
            int height = (int)(shape.Bounds.Top + shape.Bounds.Bottom);
            if (width < 1)
            {
                width = 1;
            }
            if (height < 1)
            {
                height = 1;
            }
            using (Image<Rgba32> img = new Image<Rgba32>(width, height))
            {
                img.Mutate(x => x.Fill(Rgba32.DarkBlue));

                // In ImageSharp.Drawing.Paths there is an extension method that takes in an IShape directly.
                img.Mutate(x => x.Fill(Rgba32.HotPink, shape));
                // img.Draw(Color.LawnGreen, 1, shape);

                // Ensure directory exists
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullPath));

                using (FileStream fs = File.Create(fullPath))
                {
                    img.SaveAsPng(fs);
                }
            }
        }
    }
}
