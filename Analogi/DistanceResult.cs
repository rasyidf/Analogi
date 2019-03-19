using System;

namespace Analogi
{
    internal class DistanceResult
    {
        public DistanceResult(string selectedAlgorithm, ScriptItem script, ScriptItem item)
        {
            this.Algorithm = selectedAlgorithm;
            this.Source = script;
            this.Clone = item;
            Calculate();
        }

        private void Calculate()
        {
            switch (Algorithm.ToLower())
            {
                case "cosine":
                    var c = new rasyidf.Analogi.Cosine();
                    Distance = c.Distance(Source.ReadAll(), Clone.ReadAll());
                  
                    break;
                case "levenshtein": 
                    var lv = new rasyidf.Analogi.Levenshtein(); 
                    Distance = lv.Distance(Source.ReadAll(), Clone.ReadAll());
                    break;
                case "jaccard":
                    var s = new rasyidf.Analogi.Jaccard();
                    Distance = s.Distance(Source.ReadAll(), Clone.ReadAll());
                    break;
                case "damerau":
                    var dm = new rasyidf.Analogi.Damerau();
                    Distance = dm.Distance(Source.ReadAll(), Clone.ReadAll());
                    break;
                case "jaro winkler":
                    var jw = new rasyidf.Analogi.JaroWinkler();
                    Distance = jw.Distance(Source.ReadAll(), Clone.ReadAll());
                    break;
                case "lcs":
                    var lc = new rasyidf.Analogi.LongestCommonSubsequence();
                    Distance = lc.Distance(Source.ReadAll(), Clone.ReadAll());
                    break;


                default:
                    break;
            }
        }

        public string Algorithm { get; set; }
        public ScriptItem Source { get; set; }
        public ScriptItem Clone { get; set; }
        public double Similarity { get => 1 - Distance; }
        public double Distance { get; set; }
    }
}