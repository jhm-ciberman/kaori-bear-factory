using Hellmade.Sound;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsUI : MonoBehaviour
{

   public class SliderDrag : MonoBehaviour,IPointerUpHandler 
   {
        public System.Action onEndSlide = null;
        public void OnPointerUp(PointerEventData eventData)
        {
            this.onEndSlide?.Invoke();
        }
    }

    public WindowUI progressClearedPopupUI = null;

    public Slider soundVolumeSlider = null;

    public Slider musicVolumeSlider = null;

    public AudioClip audioTestClip = null;

    public void Awake()
    {
        this.progressClearedPopupUI.HideNow();

        this.musicVolumeSlider.value = EazySoundManager.GlobalMusicVolume;
        this.soundVolumeSlider.value = EazySoundManager.GlobalSoundsVolume;
        this.soundVolumeSlider.gameObject.AddComponent<SliderDrag>().onEndSlide += this._SoundVolumeEndSlide;
    }

    public void _SoundVolumeEndSlide()
    {
        if (this.audioTestClip)
        {
            EazySoundManager.PlaySound(this.audioTestClip);
        }
    }

    public void OnClearProgressPressed()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        this.progressClearedPopupUI.Show();
    }

    public void OnMusicVolumeChange(float volume)
    {
        EazySoundManager.GlobalMusicVolume = volume;
    }

    public void OnFxVolumeChange(float volume)
    {
        EazySoundManager.GlobalSoundsVolume = volume;
        EazySoundManager.GlobalUISoundsVolume = volume;
    }
}