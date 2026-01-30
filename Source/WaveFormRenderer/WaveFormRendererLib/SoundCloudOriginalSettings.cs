/*
MIT License

Copyright (c) 2021 NAudio

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NAudio.WaveFormRenderer
{
    public class SoundCloudOriginalSettings : WaveFormRendererSettings
    {
        private int lastTopHeight;
        private int lastBottomHeight;

        public SoundCloudOriginalSettings()
        {
            PixelsPerPeak = 1;
            SpacerPixels = 0;
            BackgroundColor = Color.White;
        }

        public override Pen TopPeakPen
        {
            get
            {
                if (base.TopPeakPen == null || lastTopHeight != TopHeight)
                {
                    base.TopPeakPen = CreateGradientPen(TopHeight, Color.FromArgb(120, 120, 120), Color.FromArgb(50, 50, 50));
                    lastTopHeight = TopHeight;
                }
                return base.TopPeakPen;
            }
            set { base.TopPeakPen = value; }
        }


        public override Pen BottomPeakPen 
        {
            get
            {
                if (base.BottomPeakPen == null || lastBottomHeight != BottomHeight || lastTopHeight != TopHeight)
                {
                    base.BottomPeakPen = CreateSoundcloudBottomPen(TopHeight, BottomHeight);
                    lastBottomHeight = BottomHeight;
                    lastTopHeight = TopHeight;
                }
                return base.BottomPeakPen;
            }
            set { base.BottomPeakPen = value; }
        }


        public override Pen BottomSpacerPen
        {
            get { throw new InvalidOperationException("No spacer pen required"); }
        }

        private Pen CreateSoundcloudBottomPen(int topHeight, int bottomHeight)
        {
            var bottomGradient = new LinearGradientBrush(new Point(0, topHeight), new Point(0, topHeight + bottomHeight), 
                Color.FromArgb(16, 16, 16), Color.FromArgb(150, 150, 150));
            var colorBlend = new ColorBlend(3);
            colorBlend.Colors[0] = Color.FromArgb(16, 16, 16);
            colorBlend.Colors[1] = Color.FromArgb(142, 142, 142);
            colorBlend.Colors[2] = Color.FromArgb(150, 150, 150);
            colorBlend.Positions[0] = 0;
            colorBlend.Positions[1] = 0.1f;
            colorBlend.Positions[2] = 1.0f;
            bottomGradient.InterpolationColors = colorBlend;
            return new Pen(bottomGradient);
        }
    }
}