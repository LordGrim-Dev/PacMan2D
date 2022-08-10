
using UnityEngine;

namespace PacMan.Utility
{
    
    public static class Vibration
    {

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass mUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject mCurrentActivity = mUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject mVibrator = mCurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
        public static AndroidJavaClass mUnityPlayer;
        public static AndroidJavaObject mCurrentActivity;
        public static AndroidJavaObject mVibrator;
#endif

        
        public static void Vibrate()
        {
            if (isAndroid())
                mVibrator.Call("vibrate");
            else
                Handheld.Vibrate();
        }

        
        public static void Vibrate(long milliseconds)
        {
            if (isAndroid())
                mVibrator.Call("vibrate", milliseconds);
            else
                Handheld.Vibrate();
        }

        
        public static void Vibrate(long[] pattern, int repeat)
        {
            if (isAndroid())
                mVibrator.Call("vibrate", pattern, repeat);
            else
                Handheld.Vibrate();
        }

        
        public static bool HasVibrator()
        {
            return isAndroid();
        }

        
        public static void Cancel()
        {
            if (isAndroid())
                mVibrator.Call("cancel");
        }

        
        private static bool isAndroid()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
	        return true;
#else
            return false;
#endif
        }
        
    }
}