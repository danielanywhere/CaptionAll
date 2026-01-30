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
using System.Linq;

namespace NAudio.WaveFormRenderer
{
  /// <summary>
  /// Average Peak Provider
  /// </summary>
    public class AveragePeakProvider : PeakProvider
    {
        private readonly float scale;

    /// <summary>
    /// Create an instance of the AveragePeakProvider object.
    /// </summary>
    /// <param name="scale">
    /// The average scale to which the peak will be converted.
    /// </param>
        public AveragePeakProvider(float scale)
        {
            this.scale = scale;
        }

    /// <summary>
    /// Return a reference to the next peak in the chain.
    /// </summary>
    /// <returns>
    /// Reference to information about the next peak.
    /// </returns>
        public override PeakInfo GetNextPeak()
        {
            var samplesRead = Provider.Read(ReadBuffer, 0, ReadBuffer.Length);
            var sum = (samplesRead == 0) ? 0 : ReadBuffer.Take(samplesRead).Select(s => Math.Abs(s)).Sum();
            var average = sum/samplesRead;
            
            return new PeakInfo(average * (0 - scale), average * scale);
        }
    }
}