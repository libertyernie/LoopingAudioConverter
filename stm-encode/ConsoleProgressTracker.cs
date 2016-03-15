using System;
using System.Text;
using System.Windows.Forms;

namespace stm_encode {
    public class ConsoleProgressTracker : IProgressTracker {
        public bool Cancelled { get; set; }

        public float CurrentValue { get; set; }

        public float MaxValue { get; set; }

        public float MinValue { get; set; }

        public void Begin(float min, float max, float current) {
            MinValue = min;
            MaxValue = max;
            Console.Write('\r');
            Console.Write('[');
            for (int i = 0; i < Console.WindowWidth - 3; i++) Console.Write(' ');
            Console.Write(']');
            Update(current);
        }

        public void Cancel() {
            Cancelled = true;
            Console.WriteLine();
        }

        public void Finish() {
            Console.WriteLine();
        }

        public void Update(float value) {
            if (Cancelled) return;
            CurrentValue = value;

            StringBuilder sb = new StringBuilder("\r[");
            Console.Write('\r');
            Console.Write('[');
            float width = (Console.WindowWidth - 3) * Math.Min(CurrentValue / MaxValue, 1);
            for (int i = 0; i < width; i++) sb.Append('#');
            Console.Write(sb);
        }
    }
}