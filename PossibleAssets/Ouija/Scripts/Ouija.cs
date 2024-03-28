using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ouija : MonoBehaviour
{
    bool moving;
    int initialPosition;
    float initialVolume;
    public Transform marker;
    AudioSource audioSource;
    List<Transform> lettersPositions = new List<Transform>();
    char[] msgCharacters;
    List<char> abecedary = new List<char> //This are the characters the script use to match the positions in the ouija.
    {                                     // The order should be same in the hierarchy  
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
        'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R',
        'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        '1', '2', '3','4','5','6','7','8','9','0',
        '+','-','@','*'//YES, NO, Goodbye, Return to center
    };

    [Range(0, 0.1f)]
    public float markerSpeed;
    public float transitionTime;
    public string test;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialVolume = audioSource.volume;
        if(marker == null)
            marker = GameObject.Find("Planchette").transform;
        foreach (Transform item in GetComponentsInChildren<Transform>())
        {
            lettersPositions.Add(item);
        }
        lettersPositions.RemoveAt(0);
        if(test != "")
        {
            CallSpell(test);
        }
    }

    public IEnumerator MoveToPosition(string message)
    {
        msgCharacters = message.ToCharArray();
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < msgCharacters.Length; i++)
        {
            for (int j = 0; j < abecedary.Count; j++)
            {
                if (msgCharacters[i] == abecedary[j])
                {
                    float pause = 0;
                    if (msgCharacters[i] =='*')
                        pause = transitionTime * 0.5f;
                  
                    StartMoving(j);
                    audioSource.Play();
                    audioSource.volume = initialVolume;
                    yield return new WaitForSeconds(transitionTime - pause);
                }
            }
        }
        moving = false;
    }

    public void CallSpell(string message) // CALL THIS METHOD TO USE
    {
        if(!moving)
        StartCoroutine("MoveToPosition", message.ToUpper());
    }

    public void StartMoving(int moveTo)
    {
        initialPosition = moveTo;
        moving = true;
    }

    public void MoveMarker()
    {
        marker.position = Vector3.Lerp(marker.position, lettersPositions[initialPosition].position, markerSpeed);
        marker.rotation = Quaternion.Lerp(marker.rotation, lettersPositions[initialPosition].rotation, markerSpeed);
        audioSource.volume = Mathf.Lerp(audioSource.volume, 0, markerSpeed);
    }

    private void Update()
    {
        if (moving)
            MoveMarker();
    }
}
