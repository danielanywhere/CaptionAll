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
using System.Diagnostics;
using System.Drawing;
using NAudio.Wave;

namespace NAudio.WaveFormRenderer
{
    public class WaveFormRenderer
    {
        public Image Render(WaveStream waveStream, WaveFormRendererSettings settings)
        {
            return Render(waveStream, new MaxPeakProvider(), settings);
        }        

        public Image Render(WaveStream waveStream, IPeakProvider peakProvider, WaveFormRendererSettings settings)
        {
            int bytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8);
            var samples = waveStream.Length / (bytesPerSample);
            var samplesPerPixel = (int)(samples / settings.Width);
            var stepSize = settings.PixelsPerPeak + settings.SpacerPixels;
            peakProvider.Init(waveStream.ToSampleProvider(), samplesPerPixel * stepSize);
            return Render(peakProvider, settings);
        }

        private static Image Render(IPeakProvider peakProvider, WaveFormRendererSettings settings)
        {
            if (settings.DecibelScale)
                peakProvider = new DecibelPeakProvider(peakProvider, 48);

            var b = new Bitmap(settings.Width, settings.TopHeight + settings.BottomHeight);
            if (settings.BackgroundColor == Color.Transparent)
            {
                b.MakeTransparent();
            }
            using (var g = Graphics.FromImage(b))
            {
                g.FillRectangle(settings.BackgroundBrush, 0,0,b.Width,b.Height);
                var midPoint = settings.TopHeight;

                int x = 0;
                var currentPeak = peakProvider.GetNextPeak();
                while (x < settings.Width)
                {

                    var nextPeak = peakProvider.GetNextPeak();
                    
                    for (int n = 0; n < settings.PixelsPerPeak; n++)
                    {
                        var lineHeight = settings.TopHeight * currentPeak.Max;
                        g.DrawLine(settings.TopPeakPen, x, midPoint, x, midPoint - lineHeight);
                        lineHeight = settings.BottomHeight * currentPeak.Min;
                        g.DrawLine(settings.BottomPeakPen, x, midPoint, x, midPoint - lineHeight);
                        x++;
                    }

                    for (int n = 0; n < settings.SpacerPixels; n++)
                    {
                        // spacer bars are always the lower of the 
                        var max = Math.Min(currentPeak.Max, nextPeak.Max);
                        var min = Math.Max(currentPeak.Min, nextPeak.Min);

                        var lineHeight = settings.TopHeight * max;
                        g.DrawLine(settings.TopSpacerPen, x, midPoint, x, midPoint - lineHeight);
                        lineHeight = settings.BottomHeight * min;
                        g.DrawLine(settings.BottomSpacerPen, x, midPoint, x, midPoint - lineHeight); 
                        x++;
                    }
                    currentPeak = nextPeak;
                }
            }
            return b;
        }


    }
}
