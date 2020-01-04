<html>
  <p align="center">
    <img src="https://github.com/Phenek/Global.InputForms/blob/master/Icons/Icon.png" width="250">
    <h1 align="center">Global.InputForms</h1>
    <h4 align="center">Global.InputForms provides a Xamarin.Forms essentials controls for iOS and Android apps.</h1>
  </p>
</html>

## Setup
* Available on NuGet: [Global.InputForms](https://www.nuget.org/packages/Global.InputForms/) [![NuGet](https://img.shields.io/nuget/v/Global.InputForms.svg?label=NuGet)](https://www.nuget.org/packages/Global.InputForms)
* Add nuget package to your Xamarin.Forms .netStandard/PCL project and to your platform-specific projects

|Platform|Version|
| ------------------- | ------------------- |
|Xamarin.iOS|8.0+|
|Xamarin.Android|15+|


## Global.InputForms Initialization
After installing the NuGet package, the following initialization code is required in each application project:

* iOS - AppDelegate.cs file, in the FinishedLaunching method.
```c#
Global.InputForms.iOS.InputForms.Init();
```

 * Android - MainActivity.cs file, in the OnCreate method.
```c#
Global.InputForms.Droid.InputForms.Init(this, bundle);
```

This calls should be made after the `Xamarin.Forms.Forms.Init()` method call. It is recommended to place this calls in the following files for each platform:
Once the NuGet package has been added and the initialization method called inside each application, the InputForms APIs can be used in the common PCL or Shared Project code.

<table>
    <thead>
        <tr>
            <th colspan="2">Controls</th>
        </tr>
    </thead>
  <tbody>
  <tr>
    <th>ButtonContent</th>
    <th>Switch</th>
  </tr>
  <tr>
    <td><img src="https://github.com/Phenek/Global.InputForms/blob/master/Preview/ButtonsPreview.gif" width="200"></td>
    <td><img src="https://github.com/Phenek/Global.InputForms/blob/master/Preview/SwitchPreview.gif" width="200"></td>
  </tr>
  <tr>
    <td><p>Here <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Views/ButtonsPage.xaml">samples</a> and <a href="https://github.com/Phenek/Global.InputForms/wiki/ButtonContent">wiki</a> about ButtonContent</p></td>
    <td><p>Here <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Views/SwitchPage.xaml">samples</a> </p></td>
  </tr>
    </tbody>
</table>

<table>
    <thead>
        <tr>
            <th colspan="4">EntryLayout</th>
        </tr>
    </thead>
    <tbody>
  <tr>
    <th>EntryView</th>
    <th>PickerView</th>
    <th>TimePickerView</th>
    <th>DatePickerView</th>
  </tr>
  <tr>
    <td><img src="https://github.com/Phenek/Global.InputForms/blob/master/Preview/EntryViewPreview.gif" width="200"></td>
    <td><img src="https://github.com/Phenek/Global.InputForms/blob/master/Preview/PickerViewPreview.gif" width="200"></td>
    <td><img src="https://github.com/Phenek/Global.InputForms/blob/master/Preview/TimePickerViewPreview.gif" width="200"></td>
    <td><img src="https://github.com/Phenek/Global.InputForms/blob/master/Preview/DateTimeViewPreview.gif" width="200"></td>
  </tr>
  <tr>
    <td><p>Here <a href="https://github.com/Phenek/Global.InputForms/tree/master/Sample/SampleApp/Views/Entries/EntryViewPage.xaml">samples</a> and <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Styles/EntryStyles.xaml">styles</a></p></td>
    <td><p>Here <a href="https://github.com/Phenek/Global.InputForms/tree/master/Sample/SampleApp/Views/Entries/PickerViewPage.xaml">samples</a> and <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Styles/EntryStyles.xaml">styles</a></p></td>
    <td><p>Here <a href="https://github.com/Phenek/Global.InputForms/tree/master/Sample/SampleApp/Views/Entries/TimePickerViewPage.xaml">samples</a> and <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Styles/EntryStyles.xaml">styles</a></p></td>
    <td><p>Here <a href="https://github.com/Phenek/Global.InputForms/tree/master/Sample/SampleApp/Views/Entries/DatePickerViewPage.xaml">samples</a> and <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Styles/EntryStyles.xaml">styles</a></p></td>
  </tr>
    </tbody>
</table>

<table>
    <thead>
        <tr>
            <th colspan="4">Icheckable: CheckButton & CheckContent</th>
        </tr>
    </thead>
    <tbody>
  <tr>
    <th>CheckTemplate</th>
    <th>CheckGroup</th>
    <th>RadioGroup</th>
    <th>RateGroup</th>
  </tr>
  <tr>
    <td><img src="https://github.com/Phenek/Global.InputForms/blob/master/Preview/CheckPreview.gif" width="200"></td>
    <td><img src="https://github.com/Phenek/Global.InputForms/blob/master/Preview/CheckGroupPreview.gif" width="200"></td>
    <td> <img src="https://github.com/Phenek/Global.InputForms/blob/master/Preview/RadioGroupPreview.gif" width="200"></td>
    <td><img src="https://github.com/Phenek/Global.InputForms/blob/master/Preview/RateGroupPreview.gif" width="200"></td>
  </tr>
  <tr>
    <td><p>Here <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Views/CheckForms.xaml">samples</a> and <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Styles/CheckStyles.xaml">styles</a> about CheckTemplate </p></td>
    <td><p>Here <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Views/Group/CheckGroupPage.xaml">samples</a> and <a href="https://github.com/Phenek/Global.InputForms/wiki/CheckGroup">wiki</a> about CheckGroup</p></td>
    <td><p>Here <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Views/Group/RadioGroupPage.xaml">samples</a> and <a href="https://github.com/Phenek/Global.InputForms/wiki/RadioGroup">wiki</a> about RadioGroup</p></td>
    <td><p>Here <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Views/Group/RateGroupPage.xaml">samples</a> and <a href="https://github.com/Phenek/Global.InputForms/wiki/RateGroup">wiki</a> about RateGroup</p></td>
  </tr>
    </tbody>
</table>

## License
The MIT License (MIT) see [License file](LICENSE)

## Contribution
Feel free to do it for UWP! I think it's not a big deal, but got no time and no need for now :()


<a href="https://www.buymeacoffee.com/phenek" target="_blank"><img src="https://github.com/Phenek/Global.InputForms/blob/master/Icons/BuyMeACoffe.svg" alt="Buy Me A Coffee" style="width: 174 !important;height: auto !important;" ></a>