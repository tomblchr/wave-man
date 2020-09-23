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

        public EmptyWavDetector(string fileName, float threshold = 0.0003F)
        {
            _fileName = fileName;
            _threshold = threshold;
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

                        var positive = frame.Max();

                        _max = Math.Max(_max, Math.Abs(positive));

                        if (_max > _threshold) over++;
                    }

                    _percentEmpty = 100 - Convert.ToInt32(100 * over / wav.SampleCount);
                    _isEmpty = _percentEmpty == 0;
                    return _percentEmpty.Value;
                }
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

                        var positive = frame.Max();

                        _max = Math.Max(_max, Math.Abs(positive));

                        if (_max > _threshold) return false;
                    }
                }

                _isEmpty = _max < _threshold;

                return _isEmpty.Value;
            }
        }
    }
}
