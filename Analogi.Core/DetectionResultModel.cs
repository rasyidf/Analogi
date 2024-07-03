/*
 * MIT
 */

using Analogi.Core.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Analogi.Core;


public class DetectionResultModel
{
    public long Length { get; set; }
    public PlagiarismLevel PlagiarismLevel { get; set; }
    public double Index { get; set; }
    public SolidColorBrush IndexColor { get; set; }
    public int IndexPercentage { get; set; }
    public string Name { get; set; }
    public string Reason { get; set; }
    public List<IReason> Reasons { get; set; }
    public ObservableCollection<IReason> ReasonsList { get; set; }
}
