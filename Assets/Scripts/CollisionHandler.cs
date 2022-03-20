using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float sceneChangeDelay = 2f;
    [SerializeField] AudioClip crashAudio;
    [SerializeField] AudioClip successAudio;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        CheckForCheats(); // RespondToDebugKeys
    }

    void CheckForCheats()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCollisions();
        }
    }

    void ToggleCollisions()
    {
        collisionDisabled = !collisionDisabled;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if we haven't already crashed or succeeded
        if (isTransitioning || collisionDisabled)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }
    
    void StartCrashSequence()
    {
        isTransitioning = true;

        PlayAudio(crashAudio);
        crashParticles.Play();

        SceneChange(nameof(ReloadScene)); // use nameof instead of string
    }

    void ReloadScene()
    {
        var activeScene = SceneManager.GetActiveScene();
        // SceneManager.LoadScene(activeScene.name);
        SceneManager.LoadScene(activeScene.buildIndex);
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;

        PlayAudio(successAudio);
        successParticles.Play();

        SceneChange(nameof(LoadNextScene));
    }

    void LoadNextScene()
    {
        var activeScene = SceneManager.GetActiveScene();
        var sceneCount = SceneManager.sceneCountInBuildSettings;
        var nextSceneIndex = activeScene.buildIndex + 1;

        // This reads better to me than the example given but appreciate it looks like reusing code.
        // explicitly indicates whether or not we're loading the next level or the level at index 1.
        if (nextSceneIndex < sceneCount)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    void SceneChange(string methodName)
    {
        var movement = gameObject.GetComponent<Movement>();
        movement.StopAllParticles();
        movement.enabled = false;

        Invoke(methodName, sceneChangeDelay);
    }

    void PlayAudio(AudioClip audioClip)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.PlayOneShot(audioClip);
    }
}
