using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// A class that holds the <see cref="AudioMixerSnapshot"/> that'll be used when the player is facing an audioSource's <see cref="Transform"/>.
/// </summary>
[System.Serializable]
class AudioSnapShotData
{
    [SerializeField]
    [Tooltip("Not used.\nJust to make Unity look nice and readable. :)")]
    private string name;

    [SerializeField]
    [Tooltip("The direction that this snapshot represents.\nDon't forget to set this")]
    private Direction direction;

    //The snapshot to transition to when we want to hear this audioSource
    [Tooltip("Drag your audio snapshot for this audiosource's playback here")]
    [SerializeField]
    private AudioMixerSnapshot snapShot;

    //The audiosource's transform (So we know where it is)
    [Tooltip("Drag your audiosource's gameobject/transform in here.")]
    [SerializeField]
    private Transform audioSourceTransform;

    //Used just so we can debug when my bad code fails to work! :)
    [SerializeField]
    private float debugMagnitude;

    //Public "Getters"
    public Direction Direction { get => direction; }
    public AudioMixerSnapshot SnapShot { get => snapShot; }
    public Transform AudioSourceTransform { get => audioSourceTransform; }
    public float DebugMagnitude { get => debugMagnitude; set => debugMagnitude = value; }

    //Constructor
    public AudioSnapShotData(Direction directionToSetup)
    {
        name = directionToSetup.ToString();
        direction = directionToSetup;
    }
}

enum Direction
{
    North = 0,
    South = 1,
    East = 2,
    West = 3
}

/// <summary>
/// A component that changes audio snapshots based on the transform that the player is "nearest" to in terms of rotation.
/// </summary>
/// <remarks>
/// Would probably work with more snapshots and sources... I might look into this... sounds fun!
/// </remarks>
public class DirectionalAudioSnapshotSwitcher : MonoBehaviour
{
    [Tooltip("The fade time.\nYou could move this into AudioSnapShotData if you wanted a unique time for each snapshot")]
    [Range(0, 10)]
    [SerializeField]
    float snapshotFadeTime = 1f;

    [SerializeField]
    Direction currentDirection;

    //Transform of the main camera, we can compare rotations etc
    Transform cameraTransform;

    //Storing so that we can check if it's changed.
    AudioSnapShotData currentSnapshotData;

    //We can assign these in inspector. I also added a context menu for ease of setup. :)
    [SerializeField]
    AudioSnapShotData[] snapShotsAndAudioSources;


#if UNITY_EDITOR
    // Made this editor only, as calling this accidentally in a build would nuke your object's stored info.
    [ContextMenu("Set up directions")]
    void Setup()
    {
        snapShotsAndAudioSources = new AudioSnapShotData[]
        {
            new AudioSnapShotData(Direction.North),
            new AudioSnapShotData(Direction.South),
            new AudioSnapShotData(Direction.East),
            new AudioSnapShotData(Direction.West)
        };
    }
#endif

    /// <summary>
    /// Assigns <see cref="cameraTransform"/>, so that we're not calling <see cref="Camera.main"/> and killing performance.
    /// </summary>
    void Start()
    {
        cameraTransform = Camera.main.transform;
        //Just any old default
        currentSnapshotData = snapShotsAndAudioSources[0];
    }

    /// <summary>
    /// Gets the rotation of <see cref="cameraTransform"/>, and if the direction has changed, transitions to the new <see cref="AudioMixerSnapshot"/>.
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        float smallestRotationMagnitude = 360f;

        foreach (AudioSnapShotData snapShot in snapShotsAndAudioSources)
        {
            // Calculate the rotation from the camera to the audio source
            Vector3 directionToAudioSource = snapShot.AudioSourceTransform.position - cameraTransform.position;
            Quaternion rotationToAudioSource = Quaternion.LookRotation(directionToAudioSource);

            // Calculate the angle between the camera's forward vector and the direction to the audio source
            float angle = Quaternion.Angle(cameraTransform.rotation, rotationToAudioSource);

            // Update the debug magnitude for visualization
            snapShot.DebugMagnitude = angle;

            if (angle < smallestRotationMagnitude)
            {
                smallestRotationMagnitude = angle;
                currentSnapshotData = snapShot;
            }
        }

        if (currentDirection != currentSnapshotData.Direction)
        {
            currentSnapshotData.SnapShot.TransitionTo(snapshotFadeTime);
            currentDirection = currentSnapshotData.Direction;
        }
    }
}