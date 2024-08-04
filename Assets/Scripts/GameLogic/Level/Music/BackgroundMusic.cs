using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic.Level.Music
{
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundMusic : MonoBehaviour
    {
        [Tooltip("Список фоновой музыки")]
        [SerializeField] private List<AudioClip> musicList;

        private AudioSource _audioSource;
        private int _playingClipIndex;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();

            _playingClipIndex = Random.Range(0, musicList.Count);  
            PlayNextClip();
            
            StartCoroutine(WaitForEndOfClip());
        }

        private void PlayNextClip()
        {
            _playingClipIndex = (_playingClipIndex + 1) % musicList.Count;
            var clip = musicList[_playingClipIndex];
            _audioSource.PlayOneShot(clip);

            StartCoroutine(WaitForEndOfClip());
        }

        private IEnumerator WaitForEndOfClip()
        {
            yield return new WaitForSeconds(musicList[_playingClipIndex].length);

            PlayNextClip();
        }
    }
}