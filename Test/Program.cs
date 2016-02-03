using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public struct Rect
    {
        public int X;
        public int Y;
        public int W;
        public int H;

        public Rect(int x, int y, int w, int h)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
        }

        public int Area
        {
            get { return W * H; }
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            //Create a list of random rectangles, sorted by size
            var rand = new Random();
            var rects = new List<Rect>();
            for (int i = 0; i < 100; ++i)
                rects.Add(new Rect(0, 0, rand.Next(2, 10), rand.Next(2, 10)));
            for (int i = 0; i < 100; ++i)
                rects.Add(new Rect(0, 0, rand.Next(7, 10), rand.Next(1, 4)));
            for (int i = 0; i < 100; ++i)
                rects.Add(new Rect(0, 0, rand.Next(1, 4), rand.Next(7, 10)));
            for (int i = 0; i < 100; ++i)
                rects.Add(new Rect(0, 0, rand.Next(1, 4), rand.Next(1, 4)));

            rects.Sort((a, b) => b.Area.CompareTo(a.Area));

            var watch = Stopwatch.StartNew();

            //Pack the rectangles
            var packer = new RectanglePacker();
            for (int i = 0; i < rects.Count; ++i)
            {
                var rect = rects[i];

                //Pad the rectangles by 1px so there is a gap between them
                if (!packer.Pack(rect.W + 1, rect.H + 1, out rect.X, out rect.Y))
                {
                    Console.WriteLine("Packing failed");
                    return;
                }

                rects[i] = rect;
            }

            int packTime = (int)watch.ElapsedMilliseconds;
            int totalArea = packer.Width * packer.Height;
            int usedArea = 0;

            //Create a 2D grid of "pixels" and fill them where rectangles are
            var grid = new bool[packer.Width, packer.Height];
            foreach (var rect in rects)
            {
                usedArea += (rect.W + 1) * (rect.H + 1);

                for (int y = 0; y < rect.H; ++y)
                    for (int x = 0; x < rect.W; ++x)
                        grid[rect.X + x, rect.Y + y] = true;
            }

            //Print out the results to a text file
            using (var writer = new StreamWriter("results.txt"))
            {
                int percent = (int)(((float)usedArea / totalArea) * 100f);
                writer.WriteLine("Size: " + packer.Width + " x " + packer.Height);
                writer.WriteLine("Usage: " + usedArea + " / " + totalArea + " (" + percent + "%)");
                writer.WriteLine("Pack Time: " + packTime + " ms");
                writer.WriteLine();

                for (int y = 0; y < packer.Height; ++y)
                {
                    for (int x = 0; x < packer.Width; ++x)
                        writer.Write(grid[x, y] ? '#' : ' ');
                    writer.WriteLine();
                }
            }

            Console.WriteLine("Finished successfully. See results.txt");
        }
    }
}
