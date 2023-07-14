using UnityEngine;

public class KrishnaFluteSongsManager : MonoBehaviour
{
    [SerializeField] private Player player;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] songs;
    public float volume = .5f;

    private bool shouldPlay = false;

    [SerializeField] private float _maxDistance = 60f;
    [SerializeField] private float _minVolume = 0.1f;
    [SerializeField] private float _maxVolume = 1f;

    public static bool PlayerInTemple = false;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Stop();

        Player.PlayerInArea += IsTempleArea;
    }

    private void IsTempleArea(string area)
    {
        if (area == "KrishnaTemple")
        {
            PlayerInTemple = shouldPlay = true;
            GetNextSong();
        } 
        else if (area == "OutsideKrishnaTemple")
        {
            PlayerInTemple = shouldPlay = false;
            _audioSource.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetNextSong();
        if (player != null)
        {
            // Calculate the distance between the audio source and the target
            float distance = Helper.GetDistanceBetweenTwoObjects(
                transform.position, 
                player.transform.position
            );

            // Calculate the volume based on the distance
            float volume = Mathf.Lerp(_maxVolume, _minVolume, distance / _maxDistance);

            // Set the volume of the audio source
            _audioSource.volume = volume;
            //Debug.Log("_audioSource.volume => " + _audioSource.volume);
        }
    }

    private void GetNextSong()
    {
        if (!_audioSource.isPlaying && shouldPlay)
        {
            _audioSource.volume = volume;
            ChangeSong(Random.Range(0, songs.Length));
        }
    }

    private void ChangeSong(int indexToPlay)
    {
        _audioSource.clip = songs[indexToPlay];
        _audioSource.Play();
    }

}
