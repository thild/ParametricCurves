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

namespace ParametricCurves
{
    public static class DrawUtil
    {

//        x=(2*t*t*t-3*t*t+1)*p1.x+(-2*t*t*t+3*t*t)*p4.x+(t*t*t-2*t*t+t)*r1+(t*t*t-t*t)*r4;
//        y=(2*t*t*t-3*t*t+1)*p1.y+(-2*t*t*t+3*t*t)*p4.y+(t*t*t-2*t*t+1)*r1+(t*t*t-t*t)*r4;
        public static void DrawHermite (this Gdk.Window g, Gdk.GC gc, Point p1, Point p2, Point t1, Point t2)
        {
            for (double t=0.0; t < 1.0; t += 0.001) {
//                double t = (float)s / (float)10;    // scale s to go from 0 to 1
                double h1 = 2 * Math.Pow (t, 3) - 3 * Math.Pow (t, 2) + 1;          // calculate basis function 1
                double h2 = -2 * Math.Pow (t, 3) + 3 * Math.Pow (t, 2);              // calculate basis function 2
                double h3 = (Math.Pow (t, 3)) - 2 * Math.Pow (t, 2) + t;         // calculate basis function 3
                double h4 = Math.Pow (t, 3) - Math.Pow (t, 2);              // calculate basis function 4
                var a = new Point((int)(h1 * p1.X + h2 * p2.X + h3 * t1.X + h4 * t2.X), (int)(h1 * p1.Y + h2 * p2.Y + h3 * t1.Y + h4 * t2.Y));
            
//            Point p = h1 * p1 +                    // multiply and sum all funtions
//                      h2 * p2 +                    // together to build the interpolated
//                      h3 * t1 +                    // point along the curve.
//                      h4 * t2;
                g.DrawPoint(gc, a.X, a.Y);                            // draw to calculated point on the curve
                
            }
//        
        }
    }
}

