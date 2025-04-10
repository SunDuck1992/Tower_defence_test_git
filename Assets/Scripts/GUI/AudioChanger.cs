using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioChanger : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _pauseButton;

    private void Start()
    {
        if (AudioListener.volume > 0)
        {
            _playButton.gameObject.SetActive(true);
            _pauseButton.gameObject.SetActive(false);
        }
        else
        {
            _playButton.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(true);
        }
    }

    public void ChangeVolume(float targetVolume)
    {
       StartCoroutine(FadeOut(targetVolume));
    }

    private IEnumerator FadeOut(float targetVolume)
    {
        float startVolume = AudioListener.volume;

        for (float i = 0; i < _duration; i += Time.deltaTime)
        {
            AudioListener.volume = Mathf.Lerp(startVolume, targetVolume, i / _duration);
            yield return null;
        }

        AudioListener.volume = targetVolume;
    }
}
