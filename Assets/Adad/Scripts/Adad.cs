using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public enum BannerAnchoreVertical {
    bottom = 0,
    top = 1,
}
public enum BannerAnchoreHorizental {
    left = 0,
    right = 1,
}

public enum AdadVideoType {
    closeable = 0,
    rewarded = 1,
    general = 2
}

public class Adad : MonoBehaviour {

    [Header("Tokens")]
    public string TOKEN;
    public string CLOSABLE_VIDEO_TOKEN;
    public string GENERAL_VIDEO_TOKEN;
    public string REWARD_VIDEO_TOKEN;
    public string[] BANNER_TOKENS;
    public string FULLSCREEN_TOKEN;
    public bool isInitinalize = false;
    [Header("Banner positions")]
    public BannerAnchoreVertical bannerAnchoreVertical;
    public BannerAnchoreHorizental bannerAnchoreHorizental;

    [Header("Refrences")]
    public Dropdown banner_dropdown;

    public Action OnVideoLoaded;
    public Action OnVideoStart;
    public Action OnVideoComplete;
    public Action OnVideoClosed;
    public Action<int, string> OnVideoError;
    public Action<int> OnVideoAction;

    public Action OnBannerLoaded;
    public Action OnBannerShowed;
    public Action OnBannerClosed;
    public Action<int, string> OnBannerError;
    public Action<int> OnBannerAction;

    public static Adad instance {
        get {
            if (minstance == null) {
                minstance = new GameObject("Adad").AddComponent<Adad>();
            }
            return minstance;
        }
    }

    public static Adad minstance;
    private AndroidJavaClass AdadClass = null;
    private AndroidJavaClass VideoClass = null;
    private AndroidJavaClass BannerCalss = null;
    private AndroidJavaClass FullscreenClass = null;
    private AndroidJavaObject activity = null;

    private int bannerIndex;
    private bool adadInitialized = false;

    private void Start() {
        if (banner_dropdown) {
            List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
            for (int i = 0; i < BANNER_TOKENS.Length; i++) {
                options.Add(new Dropdown.OptionData(i.ToString()));
            }
            banner_dropdown.AddOptions(options);
        }
    }

    public void Initinalize() {
        SetTokens(TOKEN, CLOSABLE_VIDEO_TOKEN, GENERAL_VIDEO_TOKEN, REWARD_VIDEO_TOKEN, BANNER_TOKENS, FULLSCREEN_TOKEN);
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AdadClass = new AndroidJavaClass("ir.adad.core.Adad");
        AdadClass.CallStatic("initialize",TOKEN);
        VideoClass = new AndroidJavaClass("ir.adad.video.AdadVideoAd");
        BannerCalss = new AndroidJavaClass("ir.adad.unity.AdadPlugin");
        FullscreenClass = new AndroidJavaClass("ir.adad.fullscreen.AdadFullscreenBannerAd");
        isInitinalize = true;
    }

    public void Initinalize(string TOKEN, string CLOSABLE_VIDEO_TOKEN, string GENERAL_VIDEO_TOKEN, string REWARD_VIDEO_TOKEN, string[] BANNER_TOKENS, string FULLSCREEN_TOKEN) {
        SetTokens(TOKEN, CLOSABLE_VIDEO_TOKEN, GENERAL_VIDEO_TOKEN, REWARD_VIDEO_TOKEN, BANNER_TOKENS, FULLSCREEN_TOKEN);
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AdadClass = new AndroidJavaClass("ir.adad.core.Adad");
        AdadClass.CallStatic("initialize", TOKEN);
        VideoClass = new AndroidJavaClass("ir.adad.video.AdadVideoAd");
        BannerCalss = new AndroidJavaClass("ir.adad.unity.AdadPlugin");
        FullscreenClass = new AndroidJavaClass("ir.adad.fullscreen.AdadFullscreenBannerAd");
        isInitinalize = true;
    }

    public void SetTokens(string TOKEN, string CLOSABLE_VIDEO_TOKEN, string GENERAL_VIDEO_TOKEN, string REWARD_VIDEO_TOKEN, string[] BANNER_TOKENS, string FULLSCREEN_TOKEN) {
        this.TOKEN = TOKEN;
        this.CLOSABLE_VIDEO_TOKEN = CLOSABLE_VIDEO_TOKEN;
        this.GENERAL_VIDEO_TOKEN = GENERAL_VIDEO_TOKEN;
        this.REWARD_VIDEO_TOKEN = REWARD_VIDEO_TOKEN;
        this.BANNER_TOKENS = BANNER_TOKENS;
        this.FULLSCREEN_TOKEN = FULLSCREEN_TOKEN;
    }

    public void PrepareVideo(AdadVideoType type, Action onLoad, Action<int, string> onError = null) {

        OnVideoLoaded = onLoad;

        if (onError != null)
            OnVideoError = onError;

        switch (type) {
            case AdadVideoType.closeable:
                if (IsClosableVideoAdReady()) {
                    OnVideoLoaded();
                    OnVideoLoaded = null;
                    return;
                }
                PrepareClosableVideoAd();
                break;
            case AdadVideoType.rewarded:
                if (IsRewardVideoAdReady()) {
                    OnVideoLoaded();
                    OnVideoLoaded = null;
                    return;
                }
                PrepareRewardVideoAd();
                break;
            case AdadVideoType.general:
                if (IsGeneralVideoAdReady()) {
                    OnVideoLoaded();
                    OnVideoLoaded = null;
                    return;
                }
                PrepareGeneralVideoAd();
                break;
            default:
                break;
        }
    }

    public void ShowVideo(AdadVideoType type, Action onComplete, Action<int, string> onError = null) {

        OnVideoComplete = onComplete;

        if (onError != null)
            OnVideoError = onError;

        switch (type) {
            case AdadVideoType.closeable:
                ShowClosableVideoAd();
                break;
            case AdadVideoType.rewarded:
                ShowRewardVideoAd();
                break;
            case AdadVideoType.general:
                ShowGeneralVideoAd();
                break;
            default:
                break;
        }
    }

    public void PrepareClosableVideoAd() {
        VideoClass.CallStatic("prepareClosableVideoAd", activity, CLOSABLE_VIDEO_TOKEN, new VideoAdListener());
    }

    public void ShowClosableVideoAd() {
        VideoClass.CallStatic("showClosableVideoAd", activity);
    }

    public void PrepareRewardVideoAd() {
        VideoClass.CallStatic("prepareRewardVideoAd", activity, REWARD_VIDEO_TOKEN, new VideoAdListener());
    }

    public void ShowRewardVideoAd() {
        VideoClass.CallStatic("showRewardVideoAd", activity);
    }

    public void PrepareGeneralVideoAd() {
        VideoClass.CallStatic("prepareGeneralVideoAd", activity, GENERAL_VIDEO_TOKEN, new VideoAdListener());
    }

    public void ShowGeneralVideoAd() {
        VideoClass.CallStatic("showGeneralVideoAd", activity);
    }

    public void PrepareFullScreenBannerAd() {
        FullscreenClass.CallStatic("prepare", activity, FULLSCREEN_TOKEN, new AdListener());
    }

    public void PrepareFullScreenBannerAd(Action onLoad) {
    	OnBannerLoaded = onLoad;
        FullscreenClass.CallStatic("prepare", activity, FULLSCREEN_TOKEN, new AdListener());
    }

    public void ShowFullScreenBannerAd() {
        FullscreenClass.CallStatic("show", activity);
    }

    public void ShowFullScreenBannerAd(Action onShowed) {
    	OnBannerShowed = onShowed;
        FullscreenClass.CallStatic("show", activity);
    }

    public void SetBannerAnchoreVertical(Dropdown dropdown) {
        bannerAnchoreVertical = (BannerAnchoreVertical)dropdown.value;
    }

    public void SetBannerAnchoreHorizental(Dropdown dropdown) {
        bannerAnchoreHorizental = (BannerAnchoreHorizental)dropdown.value;
    }

    public void SetBannerToken(Dropdown dropdown) {
        bannerIndex = dropdown.value;
    }

    public void ShowBannerAd() {
        BannerCalss.CallStatic("createBannerAds", activity, BANNER_TOKENS[bannerIndex], bannerAnchoreHorizental.ToString(), bannerAnchoreVertical.ToString(), new AdListener());
    }

    public void ShowBannerAd(string token, BannerAnchoreHorizental hPos, BannerAnchoreVertical vPos, Action onLoad, Action onCloes, Action onShowed) {
    	OnBannerLoaded = onLoad;
    	OnBannerClosed = onCloes;
    	OnBannerShowed = onShowed;
        BannerCalss.CallStatic("createBannerAds", activity, token, hPos.ToString(), vPos.ToString(), new AdListener());
    }

    public bool FullscreenIsVisible() {
        return FullscreenClass.CallStatic<bool>("isVisible");
    }

    public bool FullscreenIsReady() {
        return FullscreenClass.CallStatic<bool>("isReady");
    }

    public void FullscreenDestroy() {
        FullscreenClass.CallStatic("destroy");
    }

    public bool IsClosableVideoAdReady() {
        return VideoClass.CallStatic<bool>("isClosableVideoAdReady");
    }

    public bool IsClosableVideoAdVisible() {
        return VideoClass.CallStatic<bool>("isClosableVideoAdVisible");
    }

    public bool IsRewardVideoAdReady() {
        return VideoClass.CallStatic<bool>("isRewardVideoAdReady");
    }

    public bool IsRewardVideoAdVisible() {
        return VideoClass.CallStatic<bool>("isRewardVideoAdVisible");
    }

    public bool IsGeneralVideoAdReady() {
        return VideoClass.CallStatic<bool>("isGeneralVideoAdReady");
    }

    public bool IsGeneralVideoAdVisible() {
        return VideoClass.CallStatic<bool>("isGeneralVideoAdVisible");
    }

    public void VideoAdDestroy() {
        VideoClass.CallStatic("destroy");
    }

    #region Listeners

    public class AdListener : AndroidJavaProxy {
        public AdListener() : base("ir.adad.ad.AdadAdListener") { }

        ///callback methods will not run on main thread!
        ///

        void onLoaded() {

            Debug.Log("adad banner : loaded");

            if (instance.OnBannerLoaded != null)
                instance.OnBannerLoaded();
            instance.OnBannerLoaded = null;



        }

        void onError(int code, string message) {

            Debug.Log("adad banner : onError : " + code + " Message: " + message);

            if (instance.OnBannerError != null)
                instance.OnBannerError(code, message);
            instance.OnBannerError = null;
        }

        void onShowed() {

            Debug.Log("adad banner : Showed");

            if (instance.OnBannerShowed != null)
                instance.OnBannerShowed();
            instance.OnBannerShowed = null;
        }

        void onClosed() {

            Debug.Log("adad banner : Closed");

            if (instance.OnBannerClosed != null)
                instance.OnBannerClosed();
            instance.OnBannerClosed = null;
        }

        void onActionOccurred(int code) {

            Debug.Log("adad banner : ActionOccurred : " + code);

            if (instance.OnBannerAction != null)
                instance.OnBannerAction(code);
            instance.OnBannerAction = null;
        }
    }

    public class VideoAdListener : AndroidJavaProxy {

        ///callback methods will not run on main thread!
        ///

        public VideoAdListener() : base("ir.adad.video.AdadVideoAdListener") { }

        void onLoaded() {

            Debug.Log("adad video : loaded");

            if (instance.OnVideoLoaded != null)
                instance.OnVideoLoaded();
            instance.OnVideoLoaded = null;

        }

        void onError(int code, string message) {

            Debug.Log("adad video : onError : " + code + " Message: " + message);

            if (instance.OnVideoError != null)
                instance.OnVideoError(code, message);
            instance.OnVideoError = null;
        }

        void onShowed() {

            Debug.Log("adad video : Showed");

            if (instance.OnVideoComplete != null)
                instance.OnVideoComplete();
            instance.OnVideoComplete = null;

        }

        void onClosed() {

            Debug.Log("adad video : Closed");

            if (instance.OnVideoClosed != null)
                instance.OnVideoClosed();
            instance.OnVideoClosed = null;
        }

        void onActionOccurred(int code) {

            Debug.Log("adad video : ActionOccurred : " + code);

            if (instance.OnVideoAction != null)
                instance.OnVideoAction(code);
            instance.OnVideoAction = null;
        }

        public void onVideoAdStart() {

            Debug.Log("adad video : Start");

            if (instance.OnVideoStart != null)
                instance.OnVideoStart();
            instance.OnVideoStart = null;
        }

        public void onVideoAdComplete() {

            Debug.Log("adad video : Complete");

            if (instance.OnVideoComplete != null)
                instance.OnVideoComplete();
            instance.OnVideoComplete = null;

        }

        public void onVideoAdDestroy() {
            Debug.Log("adad video : Destroy");
        }
    }
    #endregion
}


