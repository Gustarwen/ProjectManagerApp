using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ProjectManager.ViewModels;

public partial class graphViewModel:ViewModelBase
{
    public Action onDatasChanged;

    [ObservableProperty] private List<int> _datas = new();

}