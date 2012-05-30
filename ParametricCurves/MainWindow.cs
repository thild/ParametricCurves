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
    public MainWindow (): base (Gtk.WindowType.Toplevel)
    {
        Build ();
        canvas.ExposeEvent += OnExposed;
        
        Gdk.Color col = new Gdk.Color ();
        Gdk.Color.Parse ("black", ref col);
        canvas.ModifyBg (StateType.Normal, col);

        canvas.GdkWindow.Move(0,0);
        canvas.SetSizeRequest (400, 200);
        
        canvas.ButtonPressEvent += (o, args) => {
            if (args.Event.Button == 1) {
                if (flagp) {
                    p1 = new Point ((int)args.Event.X, (int)args.Event.Y);
                    flagp = false;
                } else {
                    p2 = new Point ((int)args.Event.X, (int)args.Event.Y);
                    flagp = true;
                }
            }
            if (args.Event.Button == 3) {
                if (flagt) {
                    t1 = new Point ((int)args.Event.X, (int)args.Event.Y);
                    flagt = false;
                } else {
                    t2 = new Point ((int)args.Event.X, (int)args.Event.Y);
                    flagt = true;
                }
            }
            canvas.QueueDraw ();
        };

        var cp1 = new DrawingArea ();
        col = new Gdk.Color ();
        Gdk.Color.Parse ("red", ref col);
        cp1.ModifyBg (StateType.Normal, col);
        cp1.SetSizeRequest (6, 6);
        cp1.AddEvents ((int)EventMask.AllEventsMask);
        fixed3.Add (cp1);
        
        cp1.Show ();

        bool drag = false;
        Point p;

        cp1.MotionNotifyEvent += (o, args) => {
            if (drag) {
                int x, y;
                cp1.GdkWindow.GetPosition(out x, out y);
                cp1.GdkWindow.Move ((int)args.Event.X + x - p.X, (int)args.Event.Y + y - p.Y);
            }
        }; 

        cp1.ButtonPressEvent +=  (o, args) => {
            drag = true;
            p = new Point((int)args.Event.X, (int)args.Event.Y);
        };
        cp1.ButtonReleaseEvent +=  (o, args) => {drag = false;};
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
        g.DrawHermite (gc, p1, p2, t1, t2);
        g.DrawArc (gc, true, p1.X - 3, p1.Y - 3, 6, 6, 0, 360 * 64);
        g.DrawArc (gc, true, p2.X - 3, p2.Y - 3, 6, 6, 0, 360 * 64);
        g.DrawArc (gc, true, t1.X - 3, t1.Y - 3, 6, 6, 0, 360 * 64);
        g.DrawArc (gc, true, t2.X - 3, t2.Y - 3, 6, 6, 0, 360 * 64);

    }

    private Point p1, p2;
    private Point t1, t2;
    private bool flagp = true;
    private bool flagt = true;


}


