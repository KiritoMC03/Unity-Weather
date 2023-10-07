#if UNI_TASK
using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WeatherSDK.Core;

namespace WeatherSDK.Tests.Runtime
{
    public class WeatherProviderTests : MonoBehaviour
    {
        #region Constructors

        [Test]
        public void Create()
        {
            var service = TestService.Default();
            IWeatherProvider provider = default;
            try 
            {
                provider = new WeatherProvider(service);
            }
            catch (Exception)
            {
                // ignored
            }
            
            Assert.IsNotNull(provider);
        }

        [Test]
        public void CreateWithSomeServices()
        {
            var services = new List<IWeatherService>();
            for (var i = 0; i < 5; i++)
                services.Add(TestService.Default());
            IWeatherProvider provider = default;
            try 
            {
                provider = new WeatherProvider(services);
            }
            catch (Exception)
            {
                // ignored
            }
            
            Assert.IsNotNull(provider);
        }

        #endregion
        
        #region GetWeather

        [UnityTest]
        public IEnumerator GetWeatherAt_0_0() => UniTask.ToCoroutine(
            async () =>
            {
                var service = TestService.Default();
                var provider = new WeatherProvider(service);
                var weather = await provider.GetWeather(0f, 0f);
                Assert.IsTrue(
                    weather.infoList.Count == 1 &&
                    weather.infoList[0].IsDataAccepted &&
                    Mathf.Approximately(weather.infoList[0].temperature, service.temperature));
            });

        [UnityTest]
        public IEnumerator GetWeatherAtCurrent() => UniTask.ToCoroutine(
            async () =>
            {
                var service = TestService.Default();
                var provider = new WeatherProvider(service);
                var weather = await provider.GetWeather();

                #if UNITY_EDITOR
                Assert.IsTrue(weather.infoList.Count == 0); // Editor doesn't support Input.location =(
                #endif
            });

        [UnityTest]
        public IEnumerator CannotGetWeatherAt_0_0EmptyService() => UniTask.ToCoroutine(
            async () =>
            {
                var service = TestService.WithEmptyResult();
                var provider = new WeatherProvider(service);
                var weather = await provider.GetWeather(0f, 0f, timeout: 1f);
                Assert.IsTrue(weather.infoList.Count == 0);
            });

        #endregion

        #region AddService

        [Test]
        public void AddService()
        {
            var service1 = TestService.Default();
            var service2 = TestService.Default();
            var provider = new WeatherProvider(service1);
            var result = provider.AddService(service2);
            Assert.AreEqual(result.state, AddServiceResultState.Success);
        }

        [Test]
        public void AddServiceIsNull()
        {
            var service1 = TestService.Default();
            TestService service2 = null;
            var provider = new WeatherProvider(service1);
            var result = provider.AddService(service2);
            Assert.IsTrue(
                result.state is AddServiceResultState.Failed &&
                result.failReason is AddServiceFailReason.ServiceIsNull);
        }

        [Test]
        public void AddServiceIsAlreadyAdded()
        {
            var service1 = TestService.Default();
            var provider = new WeatherProvider(service1);
            var result = provider.AddService(service1);
            Assert.IsTrue(
                result.state is AddServiceResultState.Failed &&
                result.failReason is AddServiceFailReason.CantAddToContainer);
        }

        #endregion
    }
}
#endif