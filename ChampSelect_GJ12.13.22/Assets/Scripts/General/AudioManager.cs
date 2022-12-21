using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public enum MusicType {Peaceful,Battle};

public class AudioManager : MonoBehaviour {
    [SerializeField] AudioClip[] allSounds;
    //[SerializeField] AudioClip peacefulIntro;
    //[SerializeField] AudioClip peacefulLoop;
    //[SerializeField] AudioClip combatIntro;
    //[SerializeField] AudioClip combatLoop;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    //public AudioSource secondary1;
    //public AudioSource secondary2;
    
    [SerializeField] ProgressManager pm;

    public static AudioManager instance;
    //public MusicType currentMusic = MusicType.Peaceful;

    int musicIndex = 0;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    //public async void TransitionMusic(MusicType music) {
    //    if (music == currentMusic) return;
    //    musicIndex++;
    //    int tempIndex = musicIndex;
    //    //ResetAmbients();
    //    currentMusic = music;
    //    LeanTween.value(gameObject, .8f, 0f, .2f).setOnUpdate((float val) => {
    //        musicSource.volume = val;
    //    });
    //    await Task.Delay(205);
    //    switch (music) {
    //        case MusicType.Peaceful:
    //            PlayPeacefulMusic(tempIndex);
    //            break;
    //        case MusicType.Battle:
    //            PlayBattleMusic(tempIndex);
    //            break;
    //    }

    //    //LeanTween.value(gameObject, .0f, .15f, .4f).setOnUpdate((float val) => {
    //    //    musicSource.volume = val;
    //    //});
    //}


    //private async void PlayDialogue() {
    //    musicSource.clip = dialogueThemeIntro;
    //    musicSource.Play();
    //    musicSource.volume = .2f;
    //    await Task.Delay(15484);
    //    if (pm.leftCutscene == true) return;
    //    musicSource.clip = dialogueThemeBase;
    //    musicSource.Play();
    //    secondary1.clip = dialogueThemeLayer;
    //    secondary1.Play();
    //    secondary1.volume = .2f;
    //}

    //public async void PlayScribble() {
    //    secondary2.clip = scribble;
    //    secondary2.time = Random.Range(0f, scribble.length);
    //    secondary2.Play();
    //    secondary2.volume = 1f;
    //    await Task.Delay(1500);
    //    secondary2.Stop();
    //}

    //private async void PlayBattleMusic(int index) {
    //    await Task.Delay(200);
    //    musicSource.clip = combatIntro;
    //    musicSource.Play();
    //    musicSource.volume = .8f;
    //    await Task.Delay(59110);
    //    if (currentMusic != MusicType.Battle || !this.enabled || index != musicIndex) return;
    //    musicSource.clip = combatLoop;
    //    musicSource.Play();
    //}

    //private async void PlayPeacefulMusic(int index) {
    //    await Task.Delay(200);
    //    musicSource.clip = peacefulIntro;
    //    musicSource.Play();
    //    musicSource.volume = .8f;
    //    await Task.Delay(59077);
    //    if (currentMusic != MusicType.Peaceful || !this.enabled || index != musicIndex) return;
    //    musicSource.clip = peacefulLoop;
    //    musicSource.Play();
    //}

    //public void PlayForestAmbiance() {
    //    secondary1.clip = forestAmbiance;
    //    secondary1.Play();
    //    secondary1.volume = 1f;
    //}

    //public void PlayPenguinAmbiance() {
    //    if(secondary2.clip == penguinAmbiance) return;
    //    secondary2.clip = penguinAmbiance;
    //    secondary2.Play();
    //    secondary2.volume = 1f;
    //}

    IEnumerator PlaySFX(AudioClip clip) {
        //musicSource.volume = .4f;
        sfxSource.PlayOneShot(clip);
        yield return new WaitWhile(() => sfxSource.isPlaying);
        //musicSource.volume = 1f;
    }

    public void PlaySound(String input) {
        string output = input;
        AudioClip s = null;
        if (!string.IsNullOrEmpty(output)) {
            s = Array.Find(allSounds, s => s.name == output);
            if (s == null) {
                Debug.LogWarning($"Sound: {output} not found");
                return;
            }
        }
        StartCoroutine(PlaySFX(s));
    }

    public void PlaySound(AudioClip a) {
        StartCoroutine(PlaySFX(a));
    }

    //public void ResetAmbients() {
        //secondary1.clip = null;
        //secondary2.clip = null;
    //}

    // WW - scripts for playing Player/Enemy engine sound
    public void PlaySecondarySound(AudioSource source)
    {
        source.Play(0);
    }

    public void StopSecondarySound(AudioSource source)
    {
        source.Stop();
    }

   
        

    //public void PlaySound(SoundType soundType) {
    //    switch (soundType) {
    //        case SoundType.MeleeHit:
    //            StartCoroutine(PlaySFX(meleeHitSounds[Random.Range(0, meleeHitSounds.Length)]));
    //            break;
    //        case SoundType.RangedHit:
    //            StartCoroutine(PlaySFX(rangedHitSounds[Random.Range(0, rangedHitSounds.Length)]));
    //            break;
    //        case SoundType.Music:
    //            musicSource.clip = music[Random.Range(0, music.Length)];
    //            musicSource.loop = true;
    //            musicSource.Play();
    //            break;
    //        case SoundType.Miss:
    //            StartCoroutine(PlaySFX(miss));
    //            break;
    //    }
    //}

    private void OnDestroy() {
        musicSource.Stop();
        StopAllCoroutines();
    }
}
