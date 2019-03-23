using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VorbisCommentSharp {
    public class VorbisComments {
        public string Vendor { get; set; }
        public Dictionary<string, string> Comments { get; private set; }

        public VorbisComments() {
            Comments = new Dictionary<string, string>();
        }

        public void WriteTo(Stream output) {
            byte[] vendor = Encoding.UTF8.GetBytes(this.Vendor);
            output.Write(BitConverter.GetBytes(vendor.Length), 0, 4);
            output.Write(vendor, 0, vendor.Length);
            output.Write(BitConverter.GetBytes(this.Comments.Count), 0, 4);
            foreach (var p in this.Comments) {
                byte[] comment = Encoding.UTF8.GetBytes(p.Key + "=" + p.Value);
                output.Write(BitConverter.GetBytes(comment.Length), 0, 4);
                output.Write(comment, 0, comment.Length);
            }
            output.Write(new byte[] { 1 }, 0, 1);
        }
    }

    public unsafe class VorbisCommentsFromFile : VorbisComments {
        internal byte* OrigStart { get; private set; } // inclusive
        internal byte* OrigEnd { get; private set; } // exclusive

        public unsafe VorbisCommentsFromFile(byte* data) {
            byte* ptr = this.OrigStart = data;

            uint length = *(uint*)ptr;
            ptr += 4;

            Vendor = new string((sbyte*)ptr, 0, (int)length);
            ptr += length;

            uint user_comment_list_length = *(uint*)ptr;
            ptr += 4;

            for (int i = 0; i < user_comment_list_length; i++) {
                uint vector_length = *(uint*)ptr;
                ptr += 4;

                string combined = new string((sbyte*)ptr, 0, (int)vector_length);
                ptr += vector_length;

                int eq_index = combined.IndexOf('=');
                Comments.Add(combined.Substring(0, eq_index), combined.Substring(eq_index + 1));
            }

            if (*ptr == 0) throw new Exception("Unexpected end of comment header");

            this.OrigEnd = ptr + 1;
        }
    }

    public unsafe class VorbisHeader {
        public OggPage Parent { get; private set; }
        
        internal byte* segment_table_entry { get; private set; }
        private byte* header;

        public byte PacketType {
            get {
                return *header;
            }
        }

        public string VorbisTag {
            get {
                return new string((sbyte*)header + 1, 0, 6);
            }
        }

        public VorbisHeader(OggPage parent, byte* segment_table_entry, byte* header) {
            this.segment_table_entry = segment_table_entry;
            this.header = header;
        }

        public VorbisCommentsFromFile ExtractComments() {
            if (PacketType != 3) throw new Exception("This is not a comment header");
            if (VorbisTag != "vorbis") throw new Exception("This is not a Vorbis header");
            return new VorbisCommentsFromFile(header + 7);
        }
    }
}
