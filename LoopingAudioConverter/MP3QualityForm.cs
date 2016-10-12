using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LoopingAudioConverter {
    public partial class MP3QualityForm : Form {
        public string EncodingParameters {
            get {
                return radVBR.Checked ? ("-V " + ((KeyValuePair<int, string>)ddlVBRSetting.SelectedItem).Key)
                    : radCBR.Checked ? ("--cbr -b " + numBitrate.Value)
                    : radCustom.Checked ? txtParameters.Text
                    : "";
            }
        }

        public MP3QualityForm() {
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
                if (p.Key == 2) ddlVBRSetting.SelectedItem = p;
            }
        }
    }
}
