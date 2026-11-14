using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ProjectManager.ViewModels;

public partial class graphViewModel:ViewModelBase
{
    public Action onDatasChanged;

    public List<int> datas
    {
        get;
    

    set=>onDatasChanged?.Invoke();
    }

}