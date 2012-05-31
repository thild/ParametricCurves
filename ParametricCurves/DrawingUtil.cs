// 
// DrawingUtil.cs
//  
// Author:
//       Tony Alexander Hild <tony_hild@yahoo.com>
// 
// Copyright (c) 2012 
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using Gdk;
using Gtk;
using Pango;

namespace ParametricCurves
{
    public static class GtkUtil
    {
        public static Point GetPosition (this Gtk.Widget widget)
        {
            int x = 0, y = 0;
            widget.GdkWindow.GetPosition (out x, out y);
            return new Point (x, y);
        }

        public static Point GetOrigin (this Gtk.Widget widget)
        {
            int x = 0, y = 0;
            widget.GdkWindow.GetOrigin (out x, out y);
            return new Point (x, y);
        }

        public static Point GetRootOrigin (this Gtk.Widget widget)
        {
            int x = 0, y = 0;
            widget.GdkWindow.GetRootOrigin (out x, out y);
            return new Point (x, y);
        }

        public static void DrawText (this Gtk.Widget widget, int x, int y, string text, Gdk.Color? color = null)
        {
            var layout = new Pango.Layout (widget.PangoContext);
            layout.Width = Pango.Units.FromPixels(20);
            layout.Wrap = Pango.WrapMode.Word;
            layout.Alignment = Pango.Alignment.Center;
            layout.FontDescription = Pango.FontDescription.FromString ("Sans Bold 8");
            layout.SetText(text);
            widget.GdkWindow.DrawLayout (
                widget.Style.TextGC (StateType.Normal),
                x, y, layout
            );
        }

        public static void Move (this Gtk.Widget widget, int x, int y)
        {
            widget.GdkWindow.Move (x, y);
        }

        public static Size GetSize (this Gtk.Widget widget)
        {
            int x = 0, y = 0;
            widget.GdkWindow.GetSize (out x, out y);
            return new Size(x, y);
        }

        public static int GetWidth (this Gtk.Widget widget)
        {
            int x = 0, y = 0;
            widget.GdkWindow.GetSize (out x, out y);
            return x;
        }

        public static int GetHeight (this Gtk.Widget widget)
        {
            int x = 0, y = 0;
            widget.GdkWindow.GetSize (out x, out y);
            return y;
        }

    }
    
    public static class DrawUtil
    {

//        x=(2*t*t*t-3*t*t+1)*p1.x+(-2*t*t*t+3*t*t)*p4.x+(t*t*t-2*t*t+t)*r1+(t*t*t-t*t)*r4;
//        y=(2*t*t*t-3*t*t+1)*p1.y+(-2*t*t*t+3*t*t)*p4.y+(t*t*t-2*t*t+1)*r1+(t*t*t-t*t)*r4;
        public static void DrawHermite (this Gdk.Window g, Gdk.GC gc, Point p1, Point p2, Point t1, Point t2)
        {
            g.DrawLine (gc, p1.X, p1.Y, t1.X, t1.Y);                            // draw to calculated point on the curve
            g.DrawLine (gc, p2.X, p2.Y, t2.X, t2.Y);                            // draw to calculated point on the curve
            
            t1.X = t1.X - p1.X;
            t1.Y = t1.Y - p1.Y;
            t2.X = t2.X - p2.X;
            t2.Y = t2.Y - p2.Y;
            Point old = p1;
            for (double t=0.0; t < 1.0; t += 0.01) {
                double h1 = 2 * Math.Pow (t, 3) - 3 * Math.Pow (t, 2) + 1;          // calculate basis function 1
                double h2 = -2 * Math.Pow (t, 3) + 3 * Math.Pow (t, 2);              // calculate basis function 2
                double h3 = (Math.Pow (t, 3)) - 2 * Math.Pow (t, 2) + t;         // calculate basis function 3
                double h4 = Math.Pow (t, 3) - Math.Pow (t, 2);              // calculate basis function 4
                var a = new Point (
                    (int)(h1 * p1.X + h2 * p2.X + h3 * t1.X + h4 * t2.X),
                    (int)(h1 * p1.Y + h2 * p2.Y + h3 * t1.Y + h4 * t2.Y)
                );
                g.DrawLine (gc, old.X, old.Y, a.X, a.Y);                            // draw to calculated point on the curve
                old = a;
            }
//        
        }
    }
}

