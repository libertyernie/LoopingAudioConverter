using System;
using System.Collections.Generic;
using System.Linq;

namespace LoopingAudioConverter.Conversion {
    public class ProgressCollection {
        private class ProgressCollectionItem : IProgress<double> {
            private readonly Action _update;

            public double Value { get; private set; }

            public ProgressCollectionItem(Action update) {
                _update = update;
            }

            public void Report(double value) {
                Value = value;
                _update();
            }
        }

        private readonly IProgress<double> _parent;
        private readonly ProgressCollectionItem[] _items;

        public IProgress<double> this[int index] => _items[index];

        public ProgressCollection(IProgress<double> parent, int count) {
            _parent = parent;
            _items = Enumerable.Range(0, count).Select(_ => new ProgressCollectionItem(() => Update())).ToArray();
        }

        private void Update() {
            _parent.Report(_items.Select(x => x.Value).Average());
        }
    }
}
