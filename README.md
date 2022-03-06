![Leader Analytics](./logo.png)

THIS PROJECT IS IN EARLY DEVELOPMENT STAGES.  DEMO APP AND NUGET PACKAGE DO NOT YET EXIST. SOME LINKS BELOW DO NOT WORK.


# LeaderPivot.XAML.WPF

A pivot table control for WPF.

* Easy to implement and configure
* Drag and drop dimensions across axis
* User configurable measures
* Four color themes provided, customize or create your own.

![Leader Analytics pivot table control](./screencap.png) 

# Getting Started

* [Get the demo application](https://github.com/leaderanalytics/LeaderPivot.XXX)

* [Get the test data application](https://github.com/leaderanalytics/LeaderPivot.TestData)

* [Get the NuGet package](https://www.nuget.org/packages/LeaderAnalytics.LeaderPivot.XXX/)

* Create a data structure to model your denormalized data.  See the [`SalesData`](https://github.com/leaderanalytics/LeaderPivot.TestData/blob/main/LeaderPivot.TestData/SalesData.cs) class for an example.

* Create [Dimensions](https://github.com/leaderanalytics/LeaderPivot/blob/main/LeaderPivot/Dimension.cs) and [Measures](https://github.com/leaderanalytics/LeaderPivot/blob/main/LeaderPivot/Measure.cs).    Dimensions are used to group data.  Measures are used to create the values shown in each cell of the pivot table.  Examples are provided in the [TestData](https://github.com/leaderanalytics/LeaderPivot.TestData/blob/main/LeaderPivot.TestData/SalesData.cs) project.

* Add a [LeaderPivot control](https://github.com/leaderanalytics/LeaderPivot.XXX) to your page.  

