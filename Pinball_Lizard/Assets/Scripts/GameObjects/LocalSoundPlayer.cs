using UnityEngine;

public class LocalSoundPlayer : MonoBehaviour
{
    public AudioClip[] Sounds;
    public AudioSource SoundSource;
    public bool PlayOnCreate;

    void Start()
    {
        if(PlayOnCreate == true)
        {
            PlaySound();
        }
    }

    void Update()
    {

    }

    public void PlaySound()
    {
        int soundSelected = Random.Range(0, Sounds.Length - 1);
        SoundSource.PlayOneShot(Sounds[soundSelected]);
    }
    private void OnDestroy()
    {
    }
}
