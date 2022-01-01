using System;

namespace LoopingAudioConverter.Conversion {
    public class ProgressSubset : IProgress<double> {
        private readonly IProgress<double> _parent;
        private readonly double _from, _to;

        public ProgressSubset(IProgress<double> parent, double from, double to) {
            _parent = parent;
            _from = from;
            _to = to;
        }

        public void Report(double value) {
            if (value < 0) value = 0;
            if (value > 1) value = 1;
            _parent?.Report(_from + value * (_to - _from));
        }
    }
}
