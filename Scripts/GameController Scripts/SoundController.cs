using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASource {

    public AudioSource audioSource;
    public float original_volume;
    public ASource (AudioSource AS, float volume) { audioSource = AS; original_volume = volume; }
}

public class SoundController : MonoBehaviour
{
    private AudioSource[] all_base_audioSources;
    private List<ASource> all_audioSources;

    // Start is called before the first frame update
    void Start()
    {
        // find all the audio sources in the scene
        all_base_audioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        all_audioSources = new List<ASource>();

        // create a ASource for each audio source and store its original volume
        foreach (AudioSource source in all_base_audioSources) {
            ASource AS = new ASource(source, source.volume);
            all_audioSources.Add(AS);
        }

        // turn all the audio sources' volumes to 0 at first
        ToggleAllSounds(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleAllSounds(bool on) {
        foreach (ASource source in all_audioSources) {
            // make sure the audio source still exists
            if (source.audioSource) {
                // if on is false, set the volume to 0
                if (!on) {
                    source.audioSource.volume = 0;
                } else {
                // if on is true, reset the volume to the original volume
                    source.audioSource.volume = source.original_volume;
                }
            } else {
                // remove the destroyed source
                all_audioSources.Remove(source);
            }
        }
    }
}
