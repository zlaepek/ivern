using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region VARIABLES
    public AudioSource audioSource;

    public AudioMixer masterMixer;

    public AudioMixerGroup sfxMixer;
    public AudioMixerGroup bgmMixer;

    public static readonly string sfxVolumeParam = "SFXVol";
    public static readonly string bgmVolumeParam = "BGMVol";

    [SerializeField] private AudioBank soundBank;
    [SerializeField] private AudioBank musicBank;

    public static AudioSource camAudio;
    /// Singleton
    public static AudioManager Instance { get; private set; }
    #endregion

    #region LIFE CYCLE
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitBanks();
            audioSource.outputAudioMixerGroup = bgmMixer;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitMixer();
        masterMixer.updateMode = AudioMixerUpdateMode.UnscaledTime;
    }
    #endregion

    #region INITIALIZATION
    private void InitBanks()
    {
        soundBank.Build();
        musicBank.Build();
    }

    private void InitMixer()
    {
        SetMixerFromPref(bgmVolumeParam);
        SetMixerFromPref(sfxVolumeParam);
    }
    #endregion

    #region METHODS
    public static void Play(string clip, AudioMixerGroup mixerTarget, Vector3? position = null)
    {
        if (Instance.soundBank.TryGetAudio(clip, out AudioClip audioClip))
        {
            GameObject clipObj = new GameObject(clip, typeof(AudioDestroyer));
            AudioSource src = clipObj.AddComponent<AudioSource>();
            if (position.HasValue)
            {
                clipObj.transform.position = position.Value;
                src.spatialBlend = 1;
                src.rolloffMode = AudioRolloffMode.Linear;
                src.maxDistance = 50;
                src.dopplerLevel = 0;
            }
            src.clip = audioClip;
            src.outputAudioMixerGroup = mixerTarget;
            src.Play();
        }
        else
        {
            Debug.LogWarning($"AudioClip '{clip}' not present in audio bank");
        }
    }

    public static void Play(string clip, MixerTarget mixerTarget, Vector3? position = null)
    {
        Play(clip, Instance.GetMixerGroup(mixerTarget), position);
    }

    public static void Play(string clip, string mixerTarget, Vector3? position = null)
    {
        Play(clip, Instance.GetMixerGroup(mixerTarget), position);
    }

    public static void Play(string clip, Vector3? position = null)
    {
        Play(clip, MixerTarget.BGM, position);
    }
    #endregion

    #region PLAY & STOP BGM
    public static void PlayMusic(string music)
    {
        if (string.IsNullOrEmpty(music) == false)
        {
            if (camAudio == null)
            {
                camAudio = Camera.main.GetComponentInChildren<AudioSource>();
            }
            if (Instance.musicBank.TryGetAudio(music, out AudioClip audio))
            {
                camAudio.clip = audio;
                camAudio.Play();
            }
            else
            {
                Debug.LogWarning($"AudioClip '{music}' not present in music bank");
            }
        }
    }

    public static void PauseMusic()
    {
        Instance.masterMixer.FindSnapshot("Paused").TransitionTo(0.5f);
        Instance.audioSource.Pause();
    }

    public static void UnpauseMusic()
    {
        Instance.masterMixer.FindSnapshot("Default").TransitionTo(0.5f);
        Instance.audioSource.UnPause();
    }

    public static void StopMusic()
    {
        Instance.audioSource.Stop();
        Instance.audioSource.clip = null;
    }
    #endregion

    #region VOLUME
    public static void SetVolumeSFX(float value)
    {
        Instance.masterMixer.SetFloat(sfxVolumeParam, ToDecibels(value));
        SetPref(sfxVolumeParam, value);
    }

    public static void SetVolumeBGM(float value)
    {
        Instance.masterMixer.SetFloat(bgmVolumeParam, ToDecibels(value));
        SetPref(bgmVolumeParam, value);
    }

    public static float ToDecibels(float value)
    {
        if (value == 0) return -80;
        return Mathf.Log10(value) * 20;
    }

    public static float FromDecibels(float db)
    {
        if (db == -80) return 0;
        return Mathf.Pow(10, db / 20);
    }

    public static float GetFloatNormalized(string param)
    {
        if (Instance.masterMixer.GetFloat(param, out float v)) return FromDecibels(v);
        return -1;
    }
    #endregion

    #region PREFS
    // returns a linear [0-1] volume value
    public static float GetPref(string pref)
    {
        float v = PlayerPrefs.GetFloat(pref, 0.75f);
        return v;
    }

    // sets a linear [0-1] volume value
    private static void SetPref(string pref, float val)
    {
        PlayerPrefs.SetFloat(pref, val);
    }

    private void SetMixerFromPref(string pref)
    {
        masterMixer.SetFloat(pref, ToDecibels(GetPref(pref)));
    }
    #endregion

    #region MIXER
    private AudioMixerGroup GetMixerGroup(MixerTarget target)
    {
        if (target == MixerTarget.None) return null;
        if (target == MixerTarget.BGM) return bgmMixer;
        if (target == MixerTarget.SFX) return sfxMixer;
        throw new System.Exception("Invalid MixerTarget");
    }

    private AudioMixerGroup GetMixerGroup(string target)
    {
        AudioMixerGroup[] foundGroups = masterMixer.FindMatchingGroups(target);
        if (foundGroups.Length > 0) return foundGroups[0];
        throw new System.Exception($"No mixer group by the name {target} could be found");
    }

    public enum MixerTarget { None, BGM, SFX }
    public enum DefaultMixerTarget { None = MixerTarget.None, BGM = MixerTarget.BGM, SFX = MixerTarget.SFX }
    #endregion

    #region AUDIO BANK
    [System.Serializable]
    public class BankKVP
    {
        public string Key;
        public AudioClip Value;
    }

    [System.Serializable]
    public class AudioBank
    {
        [SerializeField] private BankKVP[] kvps;
        private readonly Dictionary<string, AudioClip> dictionary = new Dictionary<string, AudioClip>();

        public bool Validate()
        {
            if (kvps.Length == 0) return false;

            List<string> keys = new List<string>();
            foreach (var kvp in kvps)
            {
                if (keys.Contains(kvp.Key)) return false;
                keys.Add(kvp.Key);
            }
            return true;
        }

        public void Build()
        {
            if (Validate())
            {
                for (int i = 0; i < kvps.Length; i++)
                {
                    dictionary.Add(kvps[i].Key, kvps[i].Value);
                }
            }
        }

        public bool TryGetAudio(string key, out AudioClip audio)
        {
            return dictionary.TryGetValue(key, out audio);
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AudioBank))]
    public class AudioBankDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("kvps"));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property.FindPropertyRelative("kvps"), label, true);
            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(BankKVP))]
    public class BankKVPDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            EditorGUI.BeginProperty(position, label, property);

            Rect rect1 = new Rect(position.x, position.y, position.width / 2 - 4, position.height);
            Rect rect2 = new Rect(position.center.x + 2, position.y, position.width / 2 - 4, position.height);

            EditorGUI.PropertyField(rect1, property.FindPropertyRelative("Key"), GUIContent.none);
            EditorGUI.PropertyField(rect2, property.FindPropertyRelative("Value"), GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
#endif
    #endregion
}