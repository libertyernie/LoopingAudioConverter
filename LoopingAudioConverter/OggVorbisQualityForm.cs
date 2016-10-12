using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoopingAudioConverter {
    public partial class OggVorbisQualityForm : Form {
        public string EncodingParameters {
            get {
                return radQuality.Checked ? ("-C " + numQuality.Value)
                    : radCustom.Checked ? txtParameters.Text
                    : "";
            }
        }

        public OggVorbisQualityForm() {
            InitializeComponent();
        }
    }
}
