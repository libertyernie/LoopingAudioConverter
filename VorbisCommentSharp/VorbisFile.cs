using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace VorbisCommentSharp {
    [StructLayout(LayoutKind.Explicit, Size = 27)]
    public struct OggPageHeader {
        [FieldOffset(0)]
        public uint CapturePattern;
        [FieldOffset(4)]
        public byte Version;
        [FieldOffset(5)]
        public byte HeaderType;
        [FieldOffset(6)]
        public long GranulePosition;
        [FieldOffset(14)]
        public int BitstreamSerialNumber;
        [FieldOffset(18)]
        public int PageSequenceNumber;
        [FieldOffset(22)]
        public uint Checksum;
        [FieldOffset(26)]
        public byte PageSegments;
    }

    public unsafe class OggPage {
        public VorbisFile Parent { get; private set; }
        internal OggPageHeader* Header { get; private set; }

        internal OggPage(VorbisFile parent, OggPageHeader* header) {
            this.Parent = parent;
            this.Header = header;
        }

        public unsafe VorbisHeader GetCommentHeader() {
            byte* table = (byte*)Header + 27;
            byte* header = table + Header->PageSegments;
            for (int i=0; i<Header->PageSegments; i++) {
                // Don't check a segment if its length is greater than 254 bytes (data is probably split across segments)
                if (table[i] < 255) {
                    VorbisHeader test = new VorbisHeader(this, &table[i], header);
                    if (test.PacketType == 3 && test.VorbisTag == "vorbis") {
                        return test;
                    }
                }
                header += table[i];
            }
            return null;
        }

        public void RecalculateCrc32() {
            Header->Checksum = 0;

            byte* table = (byte*)Header + 27;
            int bodyLength = 0;
            for (int i=0; i<Header->PageSegments; i++) {
                bodyLength += table[i];
            }
            uint x = UnsafeOggCRC.GetChecksum((byte*)Header, sizeof(OggPageHeader) + Header->PageSegments + bodyLength);
            Header->Checksum = x;
        }
    }

    public unsafe class VorbisFile : IDisposable {
        internal IntPtr Data { get; private set; }
        internal int Length { get; private set; }

        public VorbisFile(byte[] data) {
            this.Data = Marshal.AllocHGlobal(data.Length);
            this.Length = data.Length;
            Marshal.Copy(data, 0, this.Data, this.Length);
        }

        public VorbisFile(VorbisFile original, VorbisComments replacement) {
            // Extract comments from original file
            VorbisHeader commentHeader = original.GetPageHeaders().Select(p => p.GetCommentHeader()).Single(h => h != null);
            VorbisCommentsFromFile originalComments = commentHeader.ExtractComments();

            // Get new comments header into byte array
            byte[] replacementData;
            using (var ms = new MemoryStream()) {
                replacement.WriteTo(ms);
                replacementData = ms.ToArray();
            }
            if (replacementData.Length + 7 >= 255) {
                throw new Exception("Comment header is too long for this program to handle (must be under 248 bytes)");
            }

            // Calculate new filesize and allocate memory
            this.Length = original.Length
                - (int)(originalComments.OrigEnd - originalComments.OrigStart)
                + replacementData.Length;
            this.Data = Marshal.AllocHGlobal(this.Length);

            // Copy section before comment header
            byte* sourcePtr = (byte*)original.Data;
            byte* endPtr = sourcePtr + original.Length;
            byte* destinationPtr = (byte*)this.Data;
            while (sourcePtr < originalComments.OrigStart) {
                *destinationPtr++ = *sourcePtr++;
            }

            // Copy new comment header
            Marshal.Copy(replacementData, 0, (IntPtr)destinationPtr, replacementData.Length);
            destinationPtr += replacementData.Length;

            // Copy section after comment header
            sourcePtr = originalComments.OrigEnd;
            while (sourcePtr < endPtr) {
                *destinationPtr++ = *sourcePtr++;
            }

            if (destinationPtr - this.Length != (byte*)this.Data) throw new Exception("not enough data written");

            // Update length of segment containing comments
            byte* comment_header_segment_table_entry = (byte*)this.Data + (commentHeader.segment_table_entry - (byte*)original.Data);
            *comment_header_segment_table_entry = (byte)(replacementData.Length + 7);
            
            // Recalculate all checksums
            foreach (OggPage page in GetPageHeaders()) {
                page.RecalculateCrc32();
            }
        }

        public unsafe List<OggPage> GetPageHeaders() {
            List<OggPage> list = new List<OggPage>();
            byte* ptr = (byte*)Data;
            byte* end = ptr + Length;
            while (ptr < end) {
                string capturePattern = new string((sbyte*)ptr, 0, 4);
                if (capturePattern != "OggS") throw new Exception("OggS expected, but not found");

                OggPageHeader* pageHeader = (OggPageHeader*)ptr;
                list.Add(new OggPage(this, pageHeader));

                byte* segmentTable = (byte*)pageHeader + 27;
                ptr = segmentTable + pageHeader->PageSegments;
                for (int i = 0; i < pageHeader->PageSegments; i++) {
                    ptr += segmentTable[i];
                }
            }
            if (ptr > end) throw new Exception("Unexpected end of file");
            return list;
        }

        public byte[] ToByteArray() {
            byte[] arr = new byte[this.Length];
            Marshal.Copy(this.Data, arr, 0, this.Length);
            return arr;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) { }
                Marshal.FreeHGlobal(Data);
                disposedValue = true;
            }
        }
        
        ~VorbisFile() {
            Dispose(false);
        }
        
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
