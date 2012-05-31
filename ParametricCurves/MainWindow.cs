// 
// MainWindow.cs
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
using Gtk;
using Gdk;
using ParametricCurves;

public partial class MainWindow: Gtk.Window
{    

    class HermitePoint
    {
        public Gtk.DrawingArea Area { get; set; }

        public Point Position { 
            get {
                var size = Area.GetSize ();
                var point = Area.GetPosition();
                point.X = point.X + size.Width / 2 ;
                point.Y = point.Y + size.Width / 2 ;
                return point;
            }
        }
    }

    HermitePoint[] hp = new HermitePoint[4];

    public MainWindow (): base (Gtk.WindowType.Toplevel)
    {
        Build ();
        canvas.ExposeEvent += OnExposed;
        canvas.Move (0, 0);

        fixed3.SizeAllocated += (o, args) => {
            canvas.SetSizeRequest (fixed3.GetWidth (), fixed3.GetHeight ());
        }  ;
        
        Gdk.Color col = new Gdk.Color ();
        Gdk.Color.Parse ("black", ref col);
        canvas.ModifyBg (StateType.Normal, col);

        

        hp [0] = CreateHermitePoint (new Color (255, 0, 0));
        hp [1] = CreateHermitePoint (new Color (255, 0, 0));
        hp [2] = CreateHermitePoint (new Color (0, 255, 0));
        hp [3] = CreateHermitePoint (new Color (0, 255, 0));

        fixed3.Put(hp [0].Area, 200, 100);
        fixed3.Put(hp [1].Area, 100, 100);
        fixed3.Put(hp [2].Area, 250, 200);
        fixed3.Put(hp [3].Area, 100, 200);

        hp [0].Area.ExposeEvent += (o, args) => {
            hp [0].Area.DrawText (0, 0, "P1");
        };

        hp [1].Area.ExposeEvent += (o, args) => {
            hp [1].Area.DrawText (0, 0, "P2");
        };

        hp [2].Area.ExposeEvent += (o, args) => {
            hp [2].Area.DrawText (0, 0, "T1");
        };

        hp [3].Area.ExposeEvent += (o, args) => {
            hp [3].Area.DrawText (0, 0, "T2");
        };

    }
    
    protected void OnDeleteEvent (object sender, DeleteEventArgs a)
    {
        Application.Quit ();
        a.RetVal = true;
    }

    void OnExposed (object o, ExposeEventArgs args)
    {
        var g = canvas.GdkWindow;
        var gc = canvas.Style.BaseGC (StateType.Normal);
//        g.DrawLine(gc, 0, 0, 400, 300);
//        g.DrawPoint(gc, 5, 10);
        g.DrawHermite (
            gc,
            hp [0].Position,
            hp [1].Position,
            hp [2].Position,
            hp [3].Position
        );
//        g.DrawArc (gc, true, hp[0].Point.X - 3, hp[0].Point.Y - 3, 6, 6, 0, 360 * 64);
//        g.DrawArc (gc, true, hp[1].Point.X - 3, hp[1].Point.Y - 3, 6, 6, 0, 360 * 64);
//        g.DrawArc (gc, true, hp[2].Point.X - 3, hp[2].Point.Y - 3, 6, 6, 0, 360 * 64);
//        g.DrawArc (gc, true, hp[3].Point.X - 3, hp[3].Point.Y - 3, 6, 6, 0, 360 * 64);

    }

    HermitePoint CreateHermitePoint (Color col)
    {
        var cp1 = new DrawingArea ();
        cp1.ModifyBg (StateType.Normal, col);
        cp1.SetSizeRequest (20, 20);
        cp1.AddEvents ((int)EventMask.AllEventsMask);
        fixed3.Add (cp1);
        cp1.Show ();
        bool drag = false;
        Point p;
        cp1.MotionNotifyEvent += (o, args) => {
            if (drag) {
                int x, y;
                cp1.GdkWindow.GetPosition (out x, out y);
                cp1.GdkWindow.Move (
                    (int)args.Event.X + x - p.X,
                    (int)args.Event.Y + y - p.Y
                );
            }
            canvas.QueueDraw ();
        };
        cp1.ButtonPressEvent += (o, args) => {
            drag = true;
            p = new Point ((int)args.Event.X, (int)args.Event.Y);
        };
        cp1.ButtonReleaseEvent += (o, args) => {
            drag = false;
        };
        return new HermitePoint (){Area = cp1 };
    }

}


