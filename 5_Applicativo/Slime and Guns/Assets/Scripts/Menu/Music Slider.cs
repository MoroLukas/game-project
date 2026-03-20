using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;
public class OptionMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer Mixer;
  
    [SerializeField]
    private AudioSource AudioSource;
    private void Start()
    {
        Mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume", 1) * 20));
    }

    public void SetVolume(float volume)
    {
        Mixer.SetFloat("Volume", Mathf.Log10(volume)*20);
        float a = Mathf.Log10(volume) * 20;

        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();

    }

}
