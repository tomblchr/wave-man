using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeleteEmptyWav.Cmd
{
    class EmptyWavDetector : IEmptyWavDetector
    {
        string _fileName;
        float _max = 0;
        float _threshold = 0F;
        bool? _isEmpty = null;
        int? _percentEmpty = null;

        public EmptyWavDetector(string fileName, float threshold = 0.0001F)
        {
            _fileName = fileName;
            _threshold = threshold;
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public int PercentEmpty
        {
            get
            {
                if (_percentEmpty.HasValue) return _percentEmpty.Value;

                long over = 0;
                using (WaveFileReader wav = new WaveFileReader(_fileName))
                {
                    for (int i = 0; i < wav.SampleCount; i++)
                    {
                        var frame = wav.ReadNextSampleFrame();

                        var positive = frame.Select(c => Math.Abs(c)).Max();

                        _max = Math.Max(_max, positive);

                        if (positive > _threshold) over++;
                    }

                    _percentEmpty = 100 - Convert.ToInt32(100 * over / wav.SampleCount);
                    _isEmpty = _percentEmpty == 100;
                    return _percentEmpty.Value;
                }
            }
        }

        public float Maximum
        {
            get
            {
                return _max;
            }
        }

        public bool IsEmpty
        {
            get
            {
                if (_isEmpty.HasValue) return _isEmpty.Value;

                using (WaveFileReader wav = new WaveFileReader(_fileName))
                {
                    for (int i = 0; i < wav.SampleCount; i++)
                    {
                        var frame = wav.ReadNextSampleFrame();

                        var positive = frame.Select(c => Math.Abs(c)).Max();

                        if (positive > _threshold)
                        {
                            _isEmpty = false;
                            return false;
                        }
                    }
                }

                _isEmpty = true;

                return _isEmpty.Value;
            }
        }
    }
}
