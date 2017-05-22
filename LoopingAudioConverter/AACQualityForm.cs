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
    public partial class AACQualityForm : Form {
        public string EncodingParameters {
            get {
                if (radTVBR.Checked) {
                    return "--tvbr " + ddlTVBRSetting.SelectedItem;
                } else if (radCVBR.Checked) {
                    return "--cvbr " + (int)numCVBRBitrate.Value;
                } else if (radABR.Checked) {
                    return "--abr " + (int)numABRBitrate.Value;
                } else if (radCBR.Checked) {
                    return "--cbr " + (int)numCBRBitrate.Value;
                } else if (radCustom.Checked) {
                    return txtParameters.Text;
                } else {
                    return "";
                }
            }
        }

        public AACQualityForm(string encodingParameters = null) {
            InitializeComponent();

            ddlTVBRSetting.SelectedItem = "91";

            if (encodingParameters != null) {
                var vbr = Regex.Match(encodingParameters, "^-V ?([0-9]*)$");
                var cbr = Regex.Match(encodingParameters, "^--cbr -b ?([0-9]*)$");
                if (vbr.Success) {
                    radTVBR.Checked = true;
                    ddlTVBRSetting.SelectedIndex = int.Parse(vbr.Groups[1].Value);
                } else if (cbr.Success) {
                    radCBR.Checked = true;
                    numCBRBitrate.Value = int.Parse(cbr.Groups[1].Value);
                } else {
                    radCustom.Checked = true;
                    txtParameters.Text = encodingParameters;
                }
            }
        }
    }
}
