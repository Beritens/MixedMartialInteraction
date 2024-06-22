using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class VoiceAndLoudnessController : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> actions = new Dictionary<string, System.Action>();
    private AudioSource audioSource;

    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;
    private float currentUpdateTime = 0f;
    private float[] clipSampleData;
    
    void Start()
    {
        // Setup the microphone and audio source
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, true, 10, 44100);
        audioSource.loop = true;
        audioSource.mute = true; // Mute or you hear yourself
        while (!(Microphone.GetPosition(null) > 0)) {} // Wait until the microphone starts
        audioSource.Play();
        clipSampleData = new float[sampleDataLength];

        // Setup voice commands
        actions.Add("test", () => TestCommand());
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    void Update()
    {
        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            audioSource.clip.GetData(clipSampleData, Microphone.GetPosition(null) - (sampleDataLength + 1));
            float clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += sample * sample; // sum squared samples
            }
            clipLoudness = Mathf.Sqrt(clipLoudness / sampleDataLength); // RMS of samples

            Debug.Log("Current Audio Loudness: " + clipLoudness);
        }
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        System.Action action;
        if (actions.TryGetValue(speech.text, out action))
        {
            action.Invoke();
        }
    }

    private void TestCommand()
    {
        Debug.Log("TEST command recognized");
        // Additional functionality for the TEST command can go here.
    }

    void OnDestroy()
    {
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
        if (audioSource != null)
        {
            Microphone.End(null);
        }
    }
}
