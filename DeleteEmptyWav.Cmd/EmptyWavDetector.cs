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

        public EmptyWavDetector(string fileName, float threshold = 0.0003F)
        {
            _fileName = fileName;
            _threshold = threshold;
        }

        public bool IsEmpty
        {
            get
            {
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

                return _max < _threshold;
            }
        }
    }
}
