using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationThrust = 1000f;
    [SerializeField] AudioClip thrustAudio;

    [SerializeField] ParticleSystem mainThrustParticles;
    [SerializeField] ParticleSystem rightThrustParticles;
    [SerializeField] ParticleSystem leftThrustParticles;

    Rigidbody rb;
    AudioSource audioSource;

    public void StopAllParticles()
    {
        mainThrustParticles.Stop();
        rightThrustParticles.Stop();
        leftThrustParticles.Stop();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        // GetKey -> hold down, GetKeyDown -> one frame
        if (Input.GetKey(KeyCode.Space))
        {
            Thrust();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainThrustParticles.Stop();
    }

    private void Thrust()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustAudio);
        }

        if (!mainThrustParticles.isPlaying)
        {
            mainThrustParticles.Play();
        }
    }

    void ProcessRotation()
    {
        // Default left
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            ApplyRotation(Vector3.forward, rightThrustParticles); // use opposite particles to rotation direction
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            ApplyRotation(Vector3.back, leftThrustParticles); // use opposite particles to rotation direction
        }
        else
        {
            StopRotating();
        }
    }

    private void StopRotating()
    {
        rightThrustParticles.Stop();
        leftThrustParticles.Stop();
    }

    void ApplyRotation(Vector3 vector3, ParticleSystem particleSystem)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        if (!particleSystem.isPlaying)
        {
            particleSystem.Play();
        }
        transform.Rotate(vector3 * rotationThrust * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing rotation so physics system can take over
    }
}
