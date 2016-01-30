//@
// Sound Manager.

// Updated 2015.11.23
// Hill Lu
//-----------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;


public enum  BGMStage
{
    BGM = 0
    //define Background Music stage here
}

public enum SFXType
{
    //define Sound Effect type here
}

public enum DIALOGRole
{
    //define Roles of dialog
}

public enum DIALOG
{
    //define dialogs ID here
}

[System.Serializable]
public class SoundManager : Singleton<SoundManager>
{

    protected SoundManager() { }

    //claims
    public Transform soundManager;
    public Transform sfxManager;

    public AudioSource bgmAS;
    public AudioSource[] dialogAS;

    public AudioClip bgmC;
    public AudioClip[] dialogC;
    //claim AudioClip here

    const int bgmAmount = 1;//setnum
    const int sfxAmount = 0;//setnum
    const int dialogRoleAmount = 0; //setnum

    SoundParam[] bgm = new SoundParam[bgmAmount];   
    SoundParam[][] sfx = new SoundParam[sfxAmount][];

    


    SoundParam currentBGMsp;
    bool startFallingPitch = false;
    bool startRisingPitch = false;
    bool canRemoveNow = true;
    bool soundManagerEffective = true;

    List<GameObject> sfxToBeRemove = new List<GameObject>();

    //A class used for regularizing audios' parameters
    class SoundParam
    {
        public AudioClip clip;
        public int length;
        public float volumn;
        public float pitch;
        public float pitchUpperBound; //used for random pitch
        public bool loop;

        public SoundParam(AudioClip clip, float volumn, float pitch, float pitchUpperBound,bool loop)
        {
            this.clip = clip;
            this.volumn = volumn;
            this.pitch = pitch;
            this.pitchUpperBound = pitchUpperBound;
            this.loop = loop;
        }

        public SoundParam(AudioClip clip, float volumn, float pitch,float pitchUpperBound)
            : this(clip, volumn, pitch, pitchUpperBound,true)
        {        }

        public SoundParam(AudioClip clip, float volumn, float pitch)
            : this(clip,volumn,pitch,pitch)
        {        }

        public SoundParam()
        {        }
    }

    void Start()
    {
		// Debug.Log("Sound Manager start working");
        Initialize();//initialize sound param
        StartCoroutine("CoRoutineRemoveSFX");
        
    }

    void Update()
    {
        if (Input.GetKeyDown("b")) bgmPlay(BGMStage.BGM);
        if (Input.GetKeyDown("s")) bgmStop();
    }

    //Initialize SFX Param (volumn and pitch of each clip)
    void Initialize() {
        //Debug.Log("Initialize Sounds");
        int i,j;

        //bgm initiate
        bgm[(int)BGMStage.BGM] = new SoundParam(bgmC, 1f, 1f);


        //sfx initiate[]
        //for (j = 0; j <= sfxAmount - 1; j++)
        //{
        //    if (j == (int)SFXType.ROBOT_JET)
        //    {
        //        define exceptions of sound effects that has multiple clips.
        //    }
        //    else
        //    {
        //        sfx[j] = new SoundParam[1];
        //    }
        //}
        //sfx initiate[][]

    }

    //general functions
    //bgm
    public void bgmPlay(BGMStage stage)
    {
        Debug.Log("Play BGM");
        StartCoroutine(CoRoutineBgmPlay(bgmAS, stage, 10f, 10f));
        //currentBGMsp = bgm[(int)stage];
    }

    public void bgmStop(float fadeOutSpeed)
    {
        StartCoroutine(MusicFadeOut(bgmAS, fadeOutSpeed, 0.01f, true));
    }

    public void bgmStop()
    {
        bgmStop(10f);
    }

    IEnumerator CoRoutineBgmPlay(AudioSource audioS, BGMStage stage, float fadeOutSpeed, float fadeInSpeed)
    {
        SoundParam sp = new SoundParam();

        StartCoroutine(MusicFadeOut(audioS, fadeOutSpeed, 0.01f, true));
        while (audioS.isPlaying)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }

        sp = bgm[(int)stage];

        bgmAS.clip = sp.clip;
        bgmAS.pitch = sp.pitch;
        bgmAS.loop = sp.loop;
        StartCoroutine(MusicFadeIn(audioS, fadeInSpeed, sp.volumn, true));
    }


    //sfx
    public void sfxPlay(SFXType type)
    {
        //Debug.Log("Play SFX");
        SoundParam sp = new SoundParam();
        int randomSP;
        float randomPitch;

        randomSP = Random.Range(0, sfx[(int)type].Length);
        sp = sfx[(int)type][randomSP];

        randomPitch = Random.Range(sp.pitch, sp.pitchUpperBound);
        AudioPlay(sp.clip, sfxManager, sp.volumn, randomPitch);
    }



    //dialog

    public void DialogPlay(DIALOGRole role,DIALOG dialogID, float volumn)
    {
        Debug.Log("Dialog Play");
        StartCoroutine(CoRoutineDialogPlay((int)role, (int)dialogID, volumn));
    }

    IEnumerator CoRoutineDialogPlay(int RoleID, int dialogID, float volume)
    {
        while (dialogAS[RoleID].isPlaying)
        {
            //Debug.Log("Sound Manager Warning: Previous dialog doesn't finish. COULD CAUSE SOME SOUNDS ERROR.");
            yield return new WaitForSeconds(Time.deltaTime);
        }
        dialogAS[RoleID].clip = dialogC[dialogID];
        dialogAS[RoleID].volume = volume;
        dialogAS[RoleID].Play();
        //amsDialogSpeaking.TransitionTo(.01f);
        yield return new WaitForSeconds(dialogAS[RoleID].clip.length);
        //amsNormal.TransitionTo(.01f);
        Debug.Log("Dialog Over");
        // Debug.Log (dialogAS[RoleID].clip);
    }

    //special functions

    //private functions
    IEnumerator MusicFadeIn(AudioSource audios, float fadeInSpeed, float maxVolumn, bool restart)
    {
        //Debug.Log("Start fade in.");
        if (restart)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            audios.Play();
        }
        while (audios.volume < maxVolumn)
        {
            audios.volume += fadeInSpeed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        //		Debug.Log("Complete fade in.");
    }

    void MusicFadeInReplay(AudioSource audios, float fadeInSpeed, float maxVolumn)
    {
        audios.Stop();
        audios.volume = 0;
        audios.pitch = 1f;;
        StartCoroutine(MusicFadeIn(audios, fadeInSpeed, maxVolumn, true));
    }

    IEnumerator MusicFadeOut(AudioSource audios, float fadeOutSpeed, float minVolumn,bool stop)
    {
        //		Debug.Log("Start fade out.");
        while (audios.volume >= minVolumn)
        {
            audios.volume -= fadeOutSpeed * Time.deltaTime;
            if (audios.isPlaying)
            {
                //Debug.Log("audios.isPlaying = " + audios.isPlaying);
                yield return new WaitForSeconds(Time.deltaTime);
            }
                
        }
        if (stop)
        {
            audios.Stop();
        }
    }

    IEnumerator PitchChange(AudioSource audios, float changeSpeed, float toPitch)
    {
        if (!audios.isPlaying)
        {
            yield break;
        }
        if (changeSpeed > 0)
        {
            startRisingPitch = true;
            while (audios.pitch < toPitch)
            {
                audios.pitch += changeSpeed * Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            startRisingPitch = false;
        }
        else
        {
            startFallingPitch = true;
            while (audios.pitch >= toPitch)
            {
                audios.pitch += changeSpeed * Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
                if (startRisingPitch)
                {
                    yield break;
                }
            }
            startFallingPitch = false;
        }
    }


    IEnumerator AudioDestory(GameObject go,float t)
    {
        //Debug.Log("Ready to add to list");
        yield return new WaitForSeconds(t);
        sfxToBeRemove.Add(go);
        //Debug.Log("adkd to list");
    }

    void RemoveSFX()
    {
        Debug.Log("Destroy");
        foreach (GameObject go in sfxToBeRemove)
        {
            Destroy(go);
        }
        sfxToBeRemove.Clear();
    }

    IEnumerator CoRoutineRemoveSFX()
    {
        Debug.Log("Start coroutine removesfx");
        while (canRemoveNow)
        {
            yield return new WaitForSeconds(2f);
            if (canRemoveNow)
            {
                RemoveSFX();
            }
        }
        Debug.Log("Exit coroutine removesfx");
    }


    /// Plays a sound by creating an empty game object with an AudioSource
    /// and attaching it to the given transform (so it moves with the transform). Destroys it after it finished playing.
    void AudioPlay(AudioClip clip, Transform emitter, float volume, float pitch)
    {
        //Create an empty game object
        GameObject go = new GameObject("Audio: " + clip.name);
        go.transform.position = emitter.position;
        go.transform.parent = emitter;

        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = false;
        source.Play();

        StartCoroutine(AudioDestory(go,source.clip.length));
        //Destroy(go, clip.length+5f);
    }

    void AudioPlay(AudioClip clip)
    {
        AudioPlay(clip, soundManager, 1f, 1f);
    }

    void AudioPlay(AudioClip clip, Transform emitter)
    {
        AudioPlay(clip, emitter, 1f, 1f);
    }

    void AudioPlay(AudioClip clip, Transform emitter, float volume)
    {
        AudioPlay(clip, emitter, volume, 1f);
    }

    // Plays a sound at the given point in space by creating an empty game object with an AudioSource
    // in that place and destroys it after it finished playing.
    void AudioPlay(AudioClip clip, Vector3 point, float volume, float pitch)
    {
        //Create an empty game object
        GameObject go = new GameObject("Audio: " + clip.name);
        go.transform.position = point;

        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        StartCoroutine(AudioDestory(go, source.clip.length));
    }

    void AudioPlay(AudioClip clip, Vector3 point)
    {
        AudioPlay(clip, point, 1f, 1f);
    }

    void AudioPlay(AudioClip clip, Vector3 point, float volume)
    {
        AudioPlay(clip, point, volume, 1f);
    }

}
