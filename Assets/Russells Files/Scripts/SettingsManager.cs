using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public bool noteVolume;

    public float sfxVolume;
    public int language;

    public Text sfxVolumeText;
    public Text currentLanguage;


    // Start is called before the first frame update
    void Start()
    {
        sfxVolume = PlayerPrefs.GetFloat("NoteVolume");
        language = PlayerPrefs.GetInt("Language");
    }

    private void Update()
    {
      

    }


    public void OpenMenu()
    {

    }

    public void CloseMenu()
    {
        this.gameObject.SetActive(false);
    }


    public void NoteVolumeUp()
    {
        sfxVolume += 0.1f;

        if (sfxVolume > 1)
        {
            sfxVolume = 1;
        }
        Mathf.RoundToInt(sfxVolume);
        PlayerPrefs.SetFloat("NoteVolume", sfxVolume);
        sfxVolumeText.text = "SFX Volume: " + sfxVolume;
    }

    public void NoteVolumeDown()
    {
        sfxVolume -= 0.1f;
        
        if(sfxVolume < 0)
        {
            sfxVolume = 0;
        }
        Mathf.RoundToInt(sfxVolume);
        PlayerPrefs.SetFloat("NoteVolume", sfxVolume);
        sfxVolumeText.text = "SFX Volume: " + sfxVolume;
    }


    public void CalibrationScreen()
    {

    }


    public void LanguageUp()
    {
        
        language += 1;

        if (language > 2)
        {
            language = 2;
        }
        PlayerPrefs.SetFloat("Language", language);

        if (language == 0)
        {
            currentLanguage.text = "Current Language: English";
        }

        if (language == 1)
        {
            currentLanguage.text = "Current Language: Japanese";
        }

        if (language == 2)
        {
            currentLanguage.text = "Current Language: Chinease";
        }
    }

    public void LanguageDown()
    {
        language -= 1;

        if (language < 0)
        {
            language = 0;
        }
        PlayerPrefs.SetFloat("Language", language);

        if (language == 0)
        {
            currentLanguage.text = "Current Language: English";
        }

        if (language == 1)
        {
            currentLanguage.text = "Current Language: Japanese";
        }

        if (language == 2)
        {
            currentLanguage.text = "Current Language: Chinease";
        }
    }

    public void RewardCode()
    {

    }

}
