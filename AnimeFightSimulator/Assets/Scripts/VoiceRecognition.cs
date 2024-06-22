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
    
    public ParticleSystem beamEffect; // Reference to the beam particle system
    public ChargeBar chargeBar;

    
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
        actions.Add("beam", () => Beam());
        
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

            
            
            if (clipLoudness >= 0.4)
            {
                Debug.Log("Adding Charge");
                chargeBar.addCharge();
            }
            
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

    private void Beam()
    {
        Debug.Log("BEAM ACTIVATED");
        if (beamEffect != null && chargeBar.isCharged())
        {
            beamEffect.Play(); // Play the beam particle system
            chargeBar.deplete();
        }
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
