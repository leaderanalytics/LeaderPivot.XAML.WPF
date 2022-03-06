using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LeaderPivot.XAML.WPF.Host;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
    #if DEBUG
            // For better XAML Hot Reloading of themes\generic.xaml content add it to App.Resources
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri("pack://application:,,,/LeaderAnalytics.LeaderPivot.XAML.WPF;component/Themes/Generic.xaml");
            this.Resources.MergedDictionaries.Add(rd);
    #endif    
    }
}
