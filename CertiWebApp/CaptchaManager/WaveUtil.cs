/**********************************
 Wave concatenation class

 CopyRights Ehab Essa 2006
***********************************/
using System;

using System.Text;
using System.IO;
using System.Runtime.InteropServices;
namespace WaveKit
{
  
    public sealed class WaveUtil
    {
        static readonly WaveUtil instance = new WaveUtil();

        static WaveUtil()
        {
        }

        WaveUtil()
        {
        }

        public static WaveUtil Instance
        {
            get
            {
                return instance;
            }
        }

        public Wave getWav(String path)
        {
            Wave wav = new Wave();
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            wav.length = (int)fs.Length - 8;
            fs.Position = 22;
            wav.channels = br.ReadInt16();
            fs.Position = 24;
            wav.samplerate = br.ReadInt32();
            fs.Position = 34;
            wav.BitsPerSample = br.ReadInt16();
            wav.DataLength = (int)fs.Length - 44;

            wav.arrfile = new byte[fs.Length - 44];
            fs.Position = 44;
            fs.Read(wav.arrfile, 0, wav.arrfile.Length);

            br.Close();
            fs.Close();
            return wav;
        }

        public void WaveHeaderOUT(Wave wav, Stream basket)
        {
            BinaryWriter bw = new BinaryWriter(basket);

            bw.Write(new char[4] { 'R', 'I', 'F', 'F' });
            bw.Write(wav.length);
            bw.Write(new char[8] { 'W', 'A', 'V', 'E', 'f', 'm', 't', ' ' });
            bw.Write((int)16);
            bw.Write((short)1);
            bw.Write(wav.channels);
            bw.Write(wav.samplerate);
            bw.Write((int)(wav.samplerate * ((wav.BitsPerSample * wav.channels) / 8)));
            bw.Write((short)((wav.BitsPerSample * wav.channels) / 8));
            bw.Write(wav.BitsPerSample);
            bw.Write(new char[4] { 'd', 'a', 't', 'a' });
            bw.Write(wav.DataLength);
            bw.Write(wav.arrfile);
            bw.Close();
        }

        public Wave Merge(Wave a, Wave b)
        {
            Wave merged = new Wave();

            merged.DataLength = 0;
            merged.length = 0;
            merged.BitsPerSample = a.BitsPerSample;
            merged.channels = a.channels;
            merged.samplerate = a.samplerate;
            merged.DataLength = a.DataLength + b.DataLength;
            merged.length = merged.DataLength + 40;
            merged.arrfile = new byte[a.arrfile.Length + b.arrfile.Length];
            a.arrfile.CopyTo(merged.arrfile, 0);
            b.arrfile.CopyTo(merged.arrfile, a.arrfile.Length);
            return merged;
        }

        public void addNoise(WaveKit.Wave wav)
        {
            Random r = new Random();

            for (int i = 0; i < wav.arrfile.Length; i++)
            {
                int noise = r.Next(35);
                int sign = r.Next(1);
                if (sign == 1 && Convert.ToInt32(wav.arrfile[i]) + noise < 256) wav.arrfile[i] = Convert.ToByte(Convert.ToInt32(wav.arrfile[i]) + noise);
                if (sign == 1 && Convert.ToInt32(wav.arrfile[i]) + noise >= 256) wav.arrfile[i] = Convert.ToByte(255);
                if (sign == 0 && Convert.ToInt32(wav.arrfile[i]) - noise > 0) wav.arrfile[i] = Convert.ToByte(Convert.ToInt32(wav.arrfile[i]) - noise);
                if (sign == 1 && Convert.ToInt32(wav.arrfile[i]) - noise <= 0) wav.arrfile[i] = Convert.ToByte(0);
            }
        }
    }
}
