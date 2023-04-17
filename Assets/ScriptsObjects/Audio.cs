
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    public AudioMixer mainMixer;
    public AudioSource soundAudioSource;
    public AudioClip[] sound;
    // Start is called before the first frame update

    //Permettent aux sliders (IHM) de changer les mixers du jeux
    public void SetVolumeMusic(float volume)
    {
        mainMixer.SetFloat("Music", volume);
    }

    public void SetVolumeEffects(float volume)
    {
        mainMixer.SetFloat("Sound", volume);
    }

    //Lancement des differents sons
    public void PlaySoundWin()
    {
        soundAudioSource.PlayOneShot(sound[0]);
    }

    public void PlaySoundLose()
    {
        soundAudioSource.PlayOneShot(sound[1]);
    }

    public void PlaySoundNextPhase()
    {
        soundAudioSource.PlayOneShot(sound[2]);
    }

}
