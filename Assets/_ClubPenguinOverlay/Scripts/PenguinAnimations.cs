using UnityEngine;

public class PenguinAnimations : MonoBehaviour
{
    private Penguin penguin;
    [SerializeField] private Animator penguinAnimator;
    [SerializeField] private Animator dirAnimator;

    private PenguinState currentState;

    private int idleHash;
    private int walkHash;
    private int sitHash;
    private int waveHash;
    private int throwHash;
    private int danceHash;

    private void OnEnable()
    {
        SetInitialReferences();
        penguin.EventChangeState += OnPenguinStateChange;
        penguin.EventLookAt+= OnPenguinLookAt;
    }
    private void OnDisable()
    {
        penguin.EventChangeState -= OnPenguinStateChange;
        penguin.EventLookAt -= OnPenguinLookAt;
    }

    private void SetInitialReferences()
    {
        penguin = GetComponent<Penguin>();
        OnPenguinLookAt(penguin.HorizontalDir, penguin.VerticalDir);

        idleHash = Animator.StringToHash("Idle");
        walkHash = Animator.StringToHash("Walk");
        sitHash = Animator.StringToHash("Sit");
        waveHash = Animator.StringToHash("Wave");
        throwHash = Animator.StringToHash("Throw");
        danceHash = Animator.StringToHash("Dance");
    }

    private void OnPenguinStateChange(PenguinState newState)
    {
        if (currentState == newState) return;

        if (newState == PenguinState.Idle)
        {
            penguinAnimator.CrossFade(idleHash, 0.01f, 0);
        }
        else if (newState == PenguinState.Walking)
        {
            penguinAnimator.CrossFade(walkHash, 0.01f, 0);
        }
        else if (newState == PenguinState.Sitting)
        {
            penguinAnimator.CrossFade(sitHash, 0.01f, 0);
        }
        else if (newState == PenguinState.Throwing)
        {
            penguinAnimator.CrossFade(throwHash, 0.01f, 0);
        }
        else if(newState == PenguinState.Waving)
        {
            penguinAnimator.CrossFade(waveHash, 0.01f, 0);
        }
        else if (newState == PenguinState.Dancing)
        {
            penguinAnimator.CrossFade(danceHash, 0.01f, 0);
        }

        currentState = newState;
    }

    private void OnPenguinLookAt(int posX, int posY)
    {
        dirAnimator.SetFloat("xDir", posX);
        dirAnimator.SetFloat("yDir", posY);

        penguinAnimator.SetFloat("xDir", posX);
        penguinAnimator.SetFloat("yDir", posY);
    }
}
