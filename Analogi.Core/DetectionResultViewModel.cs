/*
 * MIT
 */

using Analogi.Core.Models;
using System;
using System.ComponentModel;

namespace Analogi.Core;

public class DetectionResultViewModel : INotifyPropertyChanged
{
    #region Events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Events

    #region Fields

    private readonly CodeFile CodeFile;

    #endregion Fields

    #region Constructors

    public DetectionResultViewModel(string path)
    {
        CodeFile = new CodeFile(path);
        Model = new();
        Model.Reasons = [];
        Model.ReasonsList = [];
    }

    public DetectionResultViewModel(CodeFile code)
    {
        CodeFile = code;
        Model = new ();
        Model.Reasons = [];
        Model.ReasonsList = [];
    }

    #endregion Constructors

    #region Properties

    public DetectionResultModel Model { get; set; }

    public DetectionResultModel ToModel()
    {
        return new DetectionResultModel
        {
            Length = CodeFile.Length,
            PlagiarismLevel = (PlagiarismLevel)Convert.ToInt16(Model.IndexPercentage / 10),
            Index = Model.Index,
            IndexColor = Model.IndexColor,
            IndexPercentage = Model.IndexPercentage,
            Name = Model.Name,
            Reason = Model.Reason,
            Reasons = Model.Reasons,
            ReasonsList = Model.ReasonsList
        };
    }

    public void RaisePropertyChanged(string args)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(args));
    }

    #endregion Properties
}


