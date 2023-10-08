# WeatherSDK
This is SDK for weather data loading with support for custom weather services.

[WeatherProvider](Scripts/Runtime/Core/WeatherProvider.cs) class provides main API of this SDK. 
It has GetWeather() and AddService() methods. 
This class require minimum one added instance implementing [IWeatherService](Scripts/Runtime/Core/Interfaces/IWeatherService.cs).

By default SDK provides simple implementing of 
[OpenMeteo](Scripts/Runtime/Common/OpenMeteo/OpenMeteo.cs) 
([site](https://open-meteo.com/)) 
and 
[OpenWeather](Scripts/Runtime/Common/OpenWeather/OpenWeather.cs)
([site](https://openweathermap.org/)) 
API.

## Installation
- You can add this package in Unity Package Manager by GIT URL (recommended) 
```https://github.com/KiritoMC03/Unity-WeatherSDK.git```
- Also you can download _**.unitypackage**_ file from Releases

## Demo
See [WeatherSDKDemo](Demo/Script/WeatherSDKDemo.cs) or:

```csharp
private void Start()
{
    var provider = new WeatherProvider(new ExampleService());
    var addOpenMeteoResult = provider.AddService(new OpenMeteo());
    if (addOpenMeteoResult.state is AddServiceResultState.Failed)
        Debug.LogWarning($"Cannot add {typeof(OpenMeteo)} service. Reason: {addOpenMeteoResult.failReason}");
    GetWeather(provider);
}

private async void GetWeather(IWeatherProvider weatherProvider)
{
    var cts = new CancellationTokenSource();
    // Get weather in London
    var weather = await weatherProvider.GetWeather(latitude: 51.30, longitude: 0.1, cts.Token, timeout: 5f);
    // Weather struct contains list of WeatherInfo from each responding service 
    foreach (var info in weather)
        Debug.Log($"London (51.3, 0.1): {info}");
}
```

## [IWeatherRequestsRunner](Scripts/Runtime/Core/Interfaces/IWeatherRequestsRunner.cs)
This interface is a main module for assembling weather data from a list of services.
Now SDK contains 2 implementation of this interface:
- [NaiveWeatherRequestsRunner](Scripts/Runtime/Core/Requests/RequestsRunners/NaiveWeatherRequestsRunner.cs) -
    Simple WeatherInfo collector. It creates web request for each service and each StartCollecting() call.
- [CollectiveWeatherRequestsRunner](Scripts/Runtime/Core/Requests/RequestsRunners/CollectiveWeatherRequestsRunner.cs) [by default] -
    Optimized to work with frequent weather requests.

You can use any of implementation (custom too) if drop it into [WeatherProvider](Scripts/Runtime/Core/WeatherProvider.cs) constructor. 

## Dependencies
This SDK depends on the [UniTask](https://github.com/Cysharp/UniTask) package.
If your project contains no UniTask, WeatherSDK will try to detect this and prompt you to install this package automatically.

## Notes

To maintain a balance between security and performance, this package tries to adhere to the following rule:
- **Try-catch** is only used in UnityEditor mode or in Development Build only
- - UNITY_EDITOR preprocessing directive
- - DEVELOPMENT_BUILD preprocessing directive
- - For ex, when SDK calls [IWeatherService](Scripts/Runtime/Core/Interfaces/IWeatherService.cs) instance, see [WeatherRequest.Run()](Scripts/Runtime/Core/Requests/WeatherRequest.cs) 

## License
I love OpenSource and hope you find this code useful.

[MIT](LICENSE)