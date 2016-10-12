using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public OggVorbisQualityForm(string encodingParameters = null) {
            InitializeComponent();

            if (encodingParameters != null) {
                var quality = Regex.Match(encodingParameters, "^-C ?(-?[0-9\\.]*)$");
                if (quality.Success) {
                    radQuality.Checked = true;
                    numQuality.Value = decimal.Parse(quality.Groups[1].Value);
                } else {
                    radCustom.Checked = true;
                    txtParameters.Text = encodingParameters;
                }
            }
        }
    }
}
