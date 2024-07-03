using Analogi.Core.Interfaces;
using System;
using System.IO;

namespace Analogi.Core.Reasons
{
    public class IdenticalSizeReason : IReason
    {
        #region Properties
        //nilai bias sudah ditentukan yaitu 1
        public double Weight { get; set; } = 1;
        public double Index { get; set; }
        public string ReasonString => $"The File Size is Identical with {TargetFile}";

        public string TargetFile { get; private set; }
        //penentuan nilai treshold
        public double Treshold => 0.5;
        //konversi string to int
        public int IndexPercentage => Convert.ToInt32(Index * 100);
        #endregion Properties

        #region Methods



        public void SetTargetFile(string value)
        {
            TargetFile = value;
        }

        public double Check(string source, string target)
        {
            try //try catch itu bentuk excpetion handling, dimana apabila terjadi error pada salah satu baris code, maka akan langsung lompat ke baris code berikutnya
            {
                //Long adalah tipe data untuk menyatakan bilangan dengan kapasitas 8 bytes dan bisa berisi bilangan negatif.
                long len1 = new FileInfo(source).Length;//source pembanding
                                                        //disini,size dari kedua source code di bandingkan
                long len2 = new FileInfo(target).Length;//source yang dibandingkan
                Index = len1 == len2 ? 1 : 0;
            }
            catch
            {
                Index = 0;
            }
            return Index;
        }

        #endregion Methods
    }
}