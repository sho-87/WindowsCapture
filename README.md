# Windows Capture

This repo is a fork of [Lvhang.WindowsCapture](https://github.com/wanglvhang/OnnxYoloDemo/tree/main/Lvhang.WindowsCapture), updated to work with the new Windows App SDK / WinUI3 namespaces.

This package is for Windows only, so please make sure your project's TFM is later than `net6.0-windows10.0.19041.0`:

`<TargetFramework>net6.0-windows10.0.19041.0</TargetFramework>`

## Original work

Repo: https://github.com/wanglvhang/OnnxYoloDemo/tree/main/Lvhang.WindowsCapture

Nuget package : https://www.nuget.org/packages/Lvhang.WindowsCapture/

**1.1.0 (2021/10/30) release note:**

* fix a bug which the ToBitmap extension method not work on some hardware.
* add IsManual options, by default IsManual is true and you need to call nextFrame to get next frame.
* add nextFrame action in OnFrameArrived to control if send next frame.
* when you call PickAndCapture, you can pass an action that will run after user choose a windows/desktop and before receive first frame.

Sample code:

```C#
_captureSession = new WindowsCaptureSession(this, new WindowsCaptureSessionOptions()
{
    //set minial ms between frames
    MinFrameInterval = 50,
    IsManual = true,
});

_captureSession.OnFrameArrived += CaptureSession_OnFrameArrived;

_captureSession.PickAndCapture();

//CaptureSession_OnFrameArrived method
private async void CaptureSession_OnFrameArrived(Windows.Graphics.Capture.Direct3D11CaptureFrame frame, Action nextFrame)
{
    var bitmap = obj.ToBitmap();

    //do something

    nextFrame(); //enable to get next frame, if IsManual is set to false, you don't have to call this method to get next frame.
}
```

This project is based on the below projects:

https://github.com/robmikh/

https://github.com/Microsoft/Windows.UI.Composition-Win32-Samples/tree/master/dotnet/WPF/ScreenCapture
