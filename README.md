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

### Demo
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

## Dependencies
This SDK depends on the [UniTask](https://github.com/Cysharp/UniTask) package.
If your project contains no UniTask, WeatherSDK will try to detect this and prompt you to install this package automatically.

## License
We love OpenSource and hope you find this code useful.

[MIT](LICENSE)