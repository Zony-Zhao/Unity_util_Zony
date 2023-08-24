using System.Collections;
using System.Collections.Generic;
using common;
using UnityEngine;
using TapTap.TapAd;

public class AdManager : Singleton<AdManager>, ICustomController
{
    // Start is called before the first frame update
    void Start()
    {
        TapAdSdk.RequestPermissionIfNecessary();
        TapAdConfig config = new TapAdConfig.Builder()
            .MediaId(1003678) // 必选参数。为 TapADN 注册的媒体 ID
            .MediaName("一息") // 必选参数。为 TapADN 注册的媒体名称
            .MediaKey(
                "26nG7Z344Uy6Bw0cTOAoR7FL0VJC4OwxJsg0QT1Et3zQyvGmmUm1EPEY2pHfSagI") // 必选参数。媒体密钥，可以在 TapADN 后台查看（用于传输数据的解密）
            .MediaVersion("1") // 必选参数。默认值 "1"
            .Channel("TapTap") // 必选参数，渠道
            .TapClientId(
                "222837") // 必选参数。TapTap 开发者中心的游戏 Client ID。如果未在 TapTap 开发者中心注册应用，此处填写空字符串。填写正确的 Client ID 有助于提高广告的转化率。 
            .EnableDebugLog(true) // 可选参数，是否打开原生 debug 调试信息输出：true 打开、false 关闭。默认 false 关闭
            .Build();
        TapAdSdk.Init(config, this, () => { Debug.Log("初始化完成"); });
    }


    public bool CanUseLocation
    {
        get => true;
    }

    public TapAdLocation GetTapAdLocation { get; }

    public bool CanUsePhoneState
    {
        get => true;
    }

    public string GetDevImei { get; }

    public bool CanUseWifiState
    {
        get => true;
    }

    public bool CanUseWriteExternal
    {
        get => true;
    }

    public string GetDevOaid { get; }

    public bool Alist
    {
        get => true;
    }

    public bool CanUseAndroidId
    {
        get => true;
    }

    public CustomUser ProvideCustomer()
    {
        var customer = new CustomUser();
        customer.realAge = 18;
        //可以根据SaveLoad信息决定customer是否为新用户等
        return customer;
    }

    TapRewardVideoAd _tapRewardAd = null;

    /// <summary>
    /// 看广告加钱
    /// </summary>
    public void LoadAd_MoneyAdd(int money)
    {
        LoadAd(2,money);
    }
    /// <summary>
    /// 看广告复活
    /// </summary>
    public void LoanAd_Reborn()
    {
        LoadAd(1,0);
    }
    /// <summary>
    /// 加载广告
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    public void LoadAd(int type,int count)
    {
        if (TapAdSdk.IsInited == false)
        {
            Debug.Log("TapAd 需要先初始化!");
            return;
        }

        if (_tapRewardAd != null)
        {
            _tapRewardAd.Dispose();
            _tapRewardAd = null;
        }

        int adId = 1000621;
        // create AdRequest
        string name = "";
        switch (type)
        {
            case 1:
                name = "respawn";
                break;
            case 2:
                name = "coin";
                break;
            case 3:
                name = "buffManaMax";
                break;
            case 4:
                name = "buffManaRegen";
                break;
        }

        var request = new TapAdRequest.Builder()
            .SpaceId(adId)
            .RewardName(name)
            .RewardCount(count)
            .Build();
        _tapRewardAd = new TapRewardVideoAd(request);
// RewardVideoAdLoadListener 为实现了 IRewardVideoAdLoadListener 的类
        _tapRewardAd.SetLoadListener(new RewardVideoAdLoadListener());
        _tapRewardAd.Load();
    }

    public class RewardVideoAdLoadListener : IRewardVideoAdLoadListener
    {
        public void OnError(int code, string message)
        {
            Debug.Log("广告加载出错");
        }

        public void OnRewardVideoAdCached(TapRewardVideoAd ad)
        {
            Debug.Log("广告加载缓存");
        }

        public void OnRewardVideoAdLoad(TapRewardVideoAd ad)
        {
            Debug.Log("广告加载完成");
            AdManager.instance.Play();
        }
    }

    public void Play()
    {
        if (TapAdSdk.IsInited == false)
        {
            Debug.Log("TapAd 需要先初始化!");
            return;
        }

        if (_tapRewardAd != null)
        {
            // RewardVideoInteractionListener 为实现了 IRewardVideoInteractionListener 的类
            _tapRewardAd.SetInteractionListener(new RewardVideoInteractionListener());
            _tapRewardAd.Show();
        }
        else
        {
            Debug.LogErrorFormat($"[Unity::AD] 未加载好视频,无法播放!");
        }
    }

// 激励视频广告播放回调接口说明
    public class RewardVideoInteractionListener : IRewardVideoInteractionListener
    {
        public void OnAdShow(TapRewardVideoAd ad)
        {
            Debug.Log("广告显示");
        }

        public void OnAdClose(TapRewardVideoAd ad)
        {
            Debug.Log("广告关闭");
        }

        public void OnVideoComplete(TapRewardVideoAd ad)
        {
            Debug.Log("视频完成");
        }

        public void OnVideoError(TapRewardVideoAd ad)
        {
            Debug.Log("视频出错");
        }
        /// <summary>
        /// 广告看完回调
        /// </summary>
        /// <param name="ad"></param>
        /// <param name="rewardVerify"></param>
        /// <param name="rewardAmount"></param>
        /// <param name="rewardName"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public void OnRewardVerify(TapRewardVideoAd ad, bool rewardVerify, int rewardAmount, string rewardName,
            int code, string msg)
        {
            Debug.Log($"reward verify:{rewardVerify},{rewardName}");
            if (rewardVerify)
            {
                switch (rewardName)
                {
                    case "respawn":
                        CharacterController.instance.Respawn();　//复活玩家
                        //关闭死亡界面
                        if (LevelManager.instance!=null)
                        {
                            LevelManager.Instance.AdRebornVerify();
                        }
                        break;
                    case "coin":
                        if (StartManager.instance != null)
                        {
                            StartManager.instance.AddCoins(rewardAmount);
                        }
                        break;
                    case "buffManaMax":
                        CharacterController.instance.manaMax += rewardAmount;
                        break;
                    case "buffManaRegen":
                        CharacterController.instance.manaRegen += rewardAmount;
                        break;
                }
            }

            Debug.Log("奖励验证完成");
        }

        public void OnSkippedVideo(TapRewardVideoAd ad)
        {
            Debug.Log("跳过视频");
        }
    }
}