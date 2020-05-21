using Hellmade.Sound;
using UnityEngine;

public static class PlayerPrefsManager
{
    public static void LoadPrefs()
    {
        EazySoundManager.GlobalMusicVolume = PlayerPrefs.GetFloat("GlobalMusicVolume", 1f);
        EazySoundManager.GlobalSoundsVolume = PlayerPrefs.GetFloat("GlobalSoundsVolume", 1f);
        EazySoundManager.GlobalUISoundsVolume = PlayerPrefs.GetFloat("GlobalUISoundsVolume", 1f);   
    }

    public static void SavePrefs()
    {
        PlayerPrefs.SetFloat("GlobalMusicVolume", EazySoundManager.GlobalMusicVolume);
        PlayerPrefs.SetFloat("GlobalSoundsVolume", EazySoundManager.GlobalSoundsVolume);
        PlayerPrefs.SetFloat("GlobalUISoundsVolume", EazySoundManager.GlobalUISoundsVolume);
    }
}