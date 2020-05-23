using Hellmade.Sound;
using UnityEngine;

class SoundManager : MonoBehaviour
{
    public AudioClip newClient = null;

    public void Start()
    {
        EazySoundManager.PlaySound(this.newClient);
    }
}