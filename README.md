# Global.InputForms
Global.InputForms provides a Xamarin.Forms essentials controls for iOS and Android apps.

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

## Icheckable: CheckButton & CheckBox With CheckTemplate
<p>Here <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Views/CheckForms.xaml">samples</a> and <a href="https://github.com/Phenek/Global.InputForms/wiki/CheckBox-&-CheckButton">wiki</a> about checkBox and CheckButton </p>

## CheckGroup:
<p>Here <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Views/CheckGroupPage.xaml">samples</a> and <a href="https://github.com/Phenek/Global.InputForms/wiki/CheckGroup">wiki</a> about CheckGroup</p>

## RadioGroup:
<p>Here <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Views/RadioGroupPage.xaml">samples</a> and <a href="https://github.com/Phenek/Global.InputForms/wiki/CheckGroup">wiki</a> about RadioGroup</p>

## RateGroup:
<p>Here <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Views/RateGroupPage.xaml">samples</a> and <a href="https://github.com/Phenek/Global.InputForms/wiki/CheckGroup">wiki</a> about RateGroup</p>

## ButtonContent:
<p>Here <a href="https://github.com/Phenek/Global.InputForms/blob/master/Sample/SampleApp/Views/ButtonsPage.xaml">samples</a> and <a href="https://github.com/Phenek/Global.InputForms/wiki/ButtonContent">wiki</a> about ButtonContent</p>

## License
The MIT License (MIT) see [License file](LICENSE)

## Contribution
Feel free to do it for UWP! I think it's not a big deal, but got no time and no need for now :()