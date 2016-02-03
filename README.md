# RectanglePacker
A simple rectangle packer for C#. Designed for packing bitmaps into an atlas to be used for OpenGL rendering.

## Example

If we want to pack a bunch of rectangles, we first need a rectangle struct, so let's create that.

```csharp
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
```

Now, let's create a big list of 100 randomly-sized rectangles to pack.

```csharp
var rand = new Random();
var rects = new List<Rect>();

for (int i = 0; i < 100; ++i)
    rects.Add(new Rect(0, 0, rand.Next(2, 10), rand.Next(2, 10)));
```

Before we pack, let's sort them by size (largest to smallest). This improves the output of the packer.

```csharp
rects.Sort((a, b) => b.Area.CompareTo(a.Area));
```

Now, we pack them. This part is what RectanglePacker does.

```csharp
var packer = new RectanglePacker();
int x, y;

for (int i = 0; i < rects.Count; ++i)
{
    var rect = rects[i];

    if (!packer.Pack(rect.W, rect.H, out rect.X, out rect.Y))
        throw new Exception("Uh oh, we couldn't pack the rectangle :(")

    //This rectangle is now packed into position!
    rects[i] = rect;
}
```
