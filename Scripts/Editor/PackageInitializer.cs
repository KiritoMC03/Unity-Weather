#if UNITY_EDITOR
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Editor
{
    public class PackageInitializer
    {
        private const string MenuItemPath = "Tools/WeatherSDK/Initialize";
        private const string DialogTitle = "Weather SDK Inititalization";
        private const string DialogMessage = "WeatherSDK has a dependency on the UniTask package." +
            "\nThe \"com.cysharp.unitask\" assembly was not detected in the project, " +
            "so you can accept its automatic download via Unity Package Manager." +
            "\n-- You can also use " + MenuItemPath + " to initialize SDK later.";

        private const string Ok = "Download";
        private const string Cancel = "Cancel";

        private const string RepositoryURL = "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask";

        private const string RequestSuccessMessage = "<color=#A7F432>The UniTask package has been successfully installed!</color>";
        private const string RequestFailedMessage = "<color=#EB4C42>A request to add UniTask was failed. You can try to add the UniTask package yourself." +
            "\nYou can find more information at https://github.com/Cysharp/UniTask</color>" +
            "\n-- You can also try to use " + MenuItemPath + " to try again.";

        [InitializeOnLoadMethod]
        [MenuItem(MenuItemPath)]
        private static void Initialize()
        {
            #if !UNI_TASK
            if (EditorUtility.DisplayDialog(DialogTitle, DialogMessage, Ok, Cancel))
            {
                WaitRequest(Client.Add(RepositoryURL));
            }
            #endif
        }

        private static async void WaitRequest(AddRequest request)
        {
            while (request.Status == StatusCode.InProgress)
                await Task.Yield();
            if (request.Status == StatusCode.Failure)
            {
                Debug.LogError(RequestFailedMessage);
            }
            else
            {
                Debug.Log(RequestSuccessMessage);
                AssetDatabase.Refresh();
            }
        }
    }
}
#endif