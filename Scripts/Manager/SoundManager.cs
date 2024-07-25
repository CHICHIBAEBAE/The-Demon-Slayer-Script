using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class SoundManager : Singleton<SoundManager>
{
    [Header("BGM")]
    public AudioClip[] bgmClips;                                    // ����� ����� ����
    private float bgmVolume;                                         // ����� ũ��
    private AudioSource bgmAudioSource;                             // ������� ����� ����� �ҽ�

    [Header("SFX")]
    private float sfxVolume;                                         // ȿ���� ũ��
    public float soundEffectPitchVariance;                          // Pitch, �ϳ��� Ŭ���� �پ��� �Ҹ��� ��������

    [Header("Volume")]
    public AudioMixerGroup bgmmixerGroup;
    public AudioMixer audioMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    public GameObject SoundPanel;
    public ObjectPool objectPool;

    public GameObject SoundSetting;

    protected override void Awake()
    {
        base.Awake();
        Init();
        objectPool = GetComponent<ObjectPool>();

        bgmSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    private void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    private void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }
    public void ClolseSettingBtn()
    {
        SoundSetting.SetActive(false);
    }
    void Init()
    {
        // ����� �÷��̾� �ʱ� ����
        GameObject bgmPlayer = new GameObject("BGM Player");
        bgmPlayer.transform.SetParent(this.transform);
        bgmAudioSource = bgmPlayer.AddComponent<AudioSource>();
        bgmAudioSource.outputAudioMixerGroup = bgmmixerGroup;
        bgmAudioSource.playOnAwake = true;
        bgmAudioSource.loop = true;
        bgmAudioSource.clip = bgmClips[0];
        bgmVolume = bgmSlider.value *1.5f;
        bgmAudioSource.volume = bgmVolume;


        sfxVolume = sfxSlider.value * 0.3f;

        bgmAudioSource.Play();
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmAudioSource.isPlaying) bgmAudioSource.Stop();

        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        GameObject obj = objectPool.SpawnFromPool("SoundSource");
        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, sfxVolume, soundEffectPitchVariance);
    }


}
