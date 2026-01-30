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

using System.Drawing;

namespace NAudio.WaveFormRenderer
{
    public class SoundCloudBlockWaveFormSettings : WaveFormRendererSettings
    {
        private readonly Color topSpacerStartColor;
        private Pen topPen;
        private Pen topSpacerPen;
        private Pen bottomPen;
        private Pen bottomSpacerPen;

        private int lastTopHeight;
        private int lastBottomHeight;

        public SoundCloudBlockWaveFormSettings(Color topPeakColor, Color topSpacerStartColor, Color bottomPeakColor, Color bottomSpacerColor)
        {
            this.topSpacerStartColor = topSpacerStartColor;
            topPen = new Pen(topPeakColor);
            bottomPen = new Pen(bottomPeakColor);
            bottomSpacerPen = new Pen(bottomSpacerColor);
            PixelsPerPeak = 4;
            SpacerPixels = 2;
            BackgroundColor = Color.White;
            TopSpacerGradientStartColor = Color.White;
        }

        public override Pen TopPeakPen
        {
            get { return topPen; }
            set { topPen = value; }
        }

        public Color TopSpacerGradientStartColor { get; set; }

        public override Pen TopSpacerPen
        {
            get
            {
                if (topSpacerPen == null || lastBottomHeight != BottomHeight || lastTopHeight != TopHeight)
                {
                    topSpacerPen = CreateGradientPen(TopHeight, TopSpacerGradientStartColor, topSpacerStartColor);
                    lastBottomHeight = BottomHeight;
                    lastTopHeight = TopHeight;
                }
                return topSpacerPen;
            }
            set { topSpacerPen = value; }
        }


        public override Pen BottomPeakPen
        {
            get { return bottomPen; }
            set { bottomPen = value; }
        }


        public override Pen BottomSpacerPen
        {
            get { return bottomSpacerPen; }
            set { bottomSpacerPen = value; }
        }

    }
}