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
    public partial class MP3QualityForm : Form {
        public string EncodingParameters {
            get {
                return radVBR.Checked ? ("-V " + ddlVBRSetting.SelectedIndex)
                    : radCBR.Checked ? ("--cbr -b " + numBitrate.Value)
                    : radCustom.Checked ? txtParameters.Text
                    : "";
            }
        }

        public MP3QualityForm(string encodingParameters = null) {
            InitializeComponent();
            
            ddlVBRSetting.DisplayMember = "Value";
            foreach (var p in new Dictionary<int, string> {
                [0] = "-V 0 (extreme)",
                [1] = "-V 1",
                [2] = "-V 2 (standard)",
                [3] = "-V 3",
                [4] = "-V 4 (medium)",
                [5] = "-V 5",
                [6] = "-V 6",
                [7] = "-V 7",
                [8] = "-V 8",
                [9] = "-V 9",
            }) {
                ddlVBRSetting.Items.Add(p);
            }
            ddlVBRSetting.SelectedIndex = 2;

            if (encodingParameters != null) {
                var vbr = Regex.Match(encodingParameters, "^-V ?([0-9]*)$");
                var cbr = Regex.Match(encodingParameters, "^--cbr -b ?([0-9]*)$");
                if (vbr.Success) {
                    radVBR.Checked = true;
                    ddlVBRSetting.SelectedIndex = int.Parse(vbr.Groups[1].Value);
                } else if (cbr.Success) {
                    radCBR.Checked = true;
                    numBitrate.Value = int.Parse(cbr.Groups[1].Value);
                } else {
                    radCustom.Checked = true;
                    txtParameters.Text = encodingParameters;
                }
            }
        }
    }
}
