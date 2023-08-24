using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using common;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TapTap.AntiAddiction;
using TapTap.AntiAddiction.Model;
using UnityEngine;
using TapTap.Bootstrap; // 命名空间
using TapTap.Common;
using TapTap.Login;
using TMPro;
using UnityEngine.Serialization;

namespace animals.scripts.contentDB
{
    public class TapLoginManager : Singleton<TapLoginManager>
    {
        [FormerlySerializedAs("text")] public TextMeshProUGUI loginStatusTxt;
        public TextMeshProUGUI antiAddcitionTxt;
        public GameObject modal;
        public TextMeshProUGUI panelTxt;
        private void Awake()
        {
            loginStatusTxt.text = $"无状态";

    
            // 获取登录状态

            AntiAddictionConfig config = new AntiAddictionConfig()
            {
                gameId = "l4s8d3zsjfpbcdhsqa",      // TapTap 开发者中心对应 Client ID
                showSwitchAccount = true,      // 是否显示切换账号按钮
            };         

            Action<int, string> callback = (code, errorMsg) => {
                // code == 500;   // 登录成功
                // code == 1000;  // 用户登出
                // code == 1001;  // 切换账号
                // code == 1030;  // 用户当前无法进行游戏
                // code == 1050;  // 时长限制
                // code == 9002;  // 实名过程中点击了关闭实名窗
                if (code != 500)
                {
                    if(modal!=null) modal.SetActive(true);
                    string txt="登录失败";
                    switch (code)
                    {
                        case 1000:
                            txt = "用户登出";
                            break;
                        case 1001:
                            txt = "切换账号";
                            AntiAddictionUIKit.Exit();

                            logout();
                            break;
                        case 1030:
                            txt = "用户当前无法进行游戏";
                            break;
                        case 1050:
                            txt = "时长限制";
                            break;
                        case 9002:
                            txt = "实名过程中点击了关闭实名窗口";
                            break;

                    }

                    panelTxt.text = txt;
                    antiAddcitionTxt.text = "防沉迷开启失败";

                }
                else
                {
                    antiAddcitionTxt.text = "防沉迷已开启";
                    if(modal!=null) modal.SetActive(false);
                    // CheckLogin();

                }
                UnityEngine.Debug.LogFormat($"code: {code} error Message: {errorMsg}");
            };

            TapLogin.Init( "l4s8d3zsjfpbcdhsqa", true, true);
            AntiAddictionUIKit.Init(config, callback);
            var tapConfig = new TapConfig.Builder()
                .ClientID("l4s8d3zsjfpbcdhsqa")  // 必须，开发者中心对应 Client ID
                .ClientToken("l4s8d3zsjfpbcdhsqa")  // 必须，开发者中心对应 Client Token
                .ServerURL("https://l4s8d3zs.cloud.tds1.tapapis.cn")  // 必须，开发者中心 > 你的游戏 > 游戏服务 > 基本信息 > 域名配置 > API
                .RegionType(RegionType.CN)  // 非必须，CN 表示中国大陆，IO 表示其他国家或地区
                .TapDBConfig(true, "gameChannel", Application.version, true)  // TapDB 会根据 TapConfig 的配置进行自动初始化
                .ConfigBuilder();

            TapBootstrap.Init(tapConfig);
                            // AntiAddictionUIKit.Startup(SystemInfo.deviceUniqueIdentifier, true);

            string userIdentifier = "玩家的唯一标识";
             CheckLogin();
// 如果是 PC 平台还需要额外设置一下 gameId
            // TapTap.AntiAddiction.TapTapAntiAddictionManager.AntiAddictionConfig.gameId = "your_client_id";
        }

        public async void AntiAddictionStartUp()
        {
            AntiAddictionUIKit.Startup(SystemInfo.deviceUniqueIdentifier, true);
            Dictionary<string, object> properties = new Dictionary<string, object>();

            properties.Add("time",        DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());

            properties.Add("user_id",     user_id);

            TapDB.trackEvent("#anti_addiction",properties);

        }
        [Button]
        public async void CheckLogin()
        {
            try 
            {
                // var accesstoken = await TapLogin.GetAccessToken();
                // Debug.Log("已登录");
                int timeout = 2000;
                var profileTask =  TapLogin.FetchProfile();
                if (await Task.WhenAny(profileTask, Task.Delay(timeout)) == profileTask)
                {
                    var profile = await profileTask;

                    Debug.Log($"TapTap 登录成功 profile: {profile.ToJson()}");
                    LoginSuccess(profile.openid);

                } else { 
                    Debug.Log("当前未登录");
                    loginStatusTxt.text = $"TapTap未登录";

                    // 开始登录
                    await Login();
                }
            
                // 直接进入游戏
            } 
            catch (Exception e)
            {
                Debug.Log("当前未登录");
                loginStatusTxt.text = $"未登录";

                // 开始登录
                await Login();
            }
        }

        private string user_id;
        public void LoginSuccess(string userid)
        {
            user_id = userid;
            loginStatusTxt.text = $"TapTap已登录";
            Dictionary<string, object> properties = new Dictionary<string, object>();
            TapDB.setUser(user_id);
            properties.Add("time",        DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString());
            properties.Add("user_id",     userid);
            TapDB.trackEvent("#custom",properties);
            TapDB.trackEvent("ad_click",properties);
            if(modal!=null) modal.SetActive(false);
            AntiAddictionStartUp();

        }
    [Button]
        public async UniTask Login()
        {
            try
            {
                // 在 iOS、Android 系统下，会唤起 TapTap 客户端或以 WebView 方式进行登录
                // 在 Windows、macOS 系统下显示二维码（默认）和跳转链接（需配置）
                var accessToken = await TapLogin.Login();
                Debug.Log($"TapTap 登录成功 accessToken: {accessToken.ToJson()}");
                // 获取 TapTap Profile  可以获得当前用户的一些基本信息，例如名称、头像。
                var profile = await TapLogin.FetchProfile();
                Debug.Log($"TapTap 登录成功 profile: {profile.ToJson()}");
                LoginSuccess(profile.openid);
            }
            catch (Exception e)
            {
                if (e is TapException tapError)  // using TapTap.Common
                {
                    Debug.Log($"encounter exception:{tapError.code} message:{tapError.message}");
                    if (tapError.code == (int)TapErrorCode.ERROR_CODE_BIND_CANCEL) // 取消登录
                    {
                        Debug.Log("登录取消");
                    }
                }

                AntiAddictionStartUp();
            }


        }

        [Button]
        public async UniTask logout()
        {
           TapLogin.Logout();
            Debug.Log("登录缓存已删除，请退出游戏重进");
            loginStatusTxt.text = $"未登录";

        }
        
        [Button]
        public  void Openpage()
        {
          Application.OpenURL("https://l.tapdb.net/bAmOzaDk?channel=rep-rep_xysunkofqqa");
        }
        public void Update(){
        
        }

        [Button]
        public void Share()
        {
            string timestamp = System.DateTime.Now.ToString("dd-mm-yyyy-HH-mm-ss");
            string fileName = "oneBreathScreenshot" + timestamp + ".png";
            // ScreenCapture.CaptureScreenshot((Application.persistentDataPath+'/' +fileName));
            Texture2D texture2D = new Texture2D(Screen.width , Screen.height, TextureFormat.RGB24, false);
            texture2D.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);
            texture2D.Apply();
            NativeGallery.SaveImageToGallery(texture2D, "oneBreath", fileName);
            Debug.Log($"图片保存{Application.persistentDataPath+'/' +fileName}");
           using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                 using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                 {
                        // int res= currentActivity.Call<int>("Add");
                        // Debug.Log(res);

                     var pluginInstance = new AndroidJavaObject("com.dawn.common.PluginInstance");
                     if (pluginInstance != null)
                     {
                         int res=pluginInstance.Call<int>("Add");
                         Debug.Log(res);
                         pluginInstance.CallStatic("receiveUnityActivity",currentActivity);

                         // pluginInstance.Call("Toast","Hi!");
                 
                         int res2=pluginInstance.Call<int>("Share");
                         Debug.Log(res2);
                         if (res2 == 0)
                         {
                             Debug.Log("分享成功");
                         }else if (res2 == -1)
                         {
                             Debug.Log("未安装TapTap");
                             Openpage();
                         }
                         else
                         {
                             Debug.Log("sdk不支持");

                         }
                     }   
                     // if (MainActivity != null)
                     // {
                     //     int res=MainActivity.Call<int>("Add");
                     //     Debug.Log(res);
                     // }
                     // if (Random.value > .5f)
                     // {
                
                     //
                     // }
                     // else
                     // {
                     //     // pluginInstance.Call("Toast", "Hi!");
                     // }
                     //      AndroidJavaObject main = new AndroidJavaObject("com.dawn.oneBreath.MainActivity");
                     // int res=main.Call<int>("share2Tap");
                     // int res=currentActivity.Call<int>("share2Tap");
                     // int res = currentActivity.Call<int>("onCreate");

                     // string cacheDirectory = javaFile.Call<string>("getCanonicalPath");
                 }
        }
    }
}