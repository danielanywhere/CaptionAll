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
using NAudio.Wave;

namespace NAudio.WaveFormRenderer
{
    class DecibelPeakProvider : IPeakProvider
    {
        private readonly IPeakProvider sourceProvider;
        private readonly double dynamicRange;

        public DecibelPeakProvider(IPeakProvider sourceProvider, double dynamicRange)
        {
            this.sourceProvider = sourceProvider;
            this.dynamicRange = dynamicRange;
        }

        public void Init(ISampleProvider reader, int samplesPerPixel)
        {
            throw new NotImplementedException();
        }

        public PeakInfo GetNextPeak()
        {
            var peak = sourceProvider.GetNextPeak();
            var decibelMax = 20 * Math.Log10(peak.Max);
            if (decibelMax < 0 - dynamicRange) decibelMax = 0 - dynamicRange;
            var linear = (float)((dynamicRange + decibelMax) / dynamicRange);
            return new PeakInfo(0 - linear, linear);
        }
    }
}