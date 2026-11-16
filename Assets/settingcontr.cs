using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingcontr : MonoBehaviour
{
    public Toggle musicToggle;
    public AudioSource musicAudio;
    public Slider musicSlider;
    public static settingcontr Instance;
    public GameObject BackGround;
    private void Awake()
    {
        BackGround.SetActive(false);
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else if (Instance != this)
        {
            print("重新跳转到这个场景的时候会删除多余的单例物体");
            Destroy(gameObject);
        }
    }
    public void MusicSwitch()
    {
        musicAudio.enabled = musicToggle.isOn;
    }
    public void Music_height()
    {
        musicAudio.volume = musicSlider.value;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MusicSwitch();
        Music_height(); 
    }
}
