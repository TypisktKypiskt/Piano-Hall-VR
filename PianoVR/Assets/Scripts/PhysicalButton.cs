using UnityEngine;
using UnityEngine.Events;

public class PhysicalButton : MonoBehaviour
{
    private enum State
    {
        ON,
        OFF
    }

    [SerializeField]
    private float threshold = 0.25f;

    [Header("Settings")]
    [SerializeField]
    private bool invokeWhileEventsWithPressEvents = true;

    [Header("Events")]
    [SerializeField]
    private UnityEvent onPress;
    [SerializeField]
    private UnityEvent onRelease;
    [SerializeField]
    private UnityEvent whilePressed;


    private Vector3 startPosition;
    private State state;

    private float CurrentOffset => (transform.localPosition - startPosition).magnitude;

    private void Awake()
    {
        state = State.OFF;

        startPosition = transform.localPosition;
    }

    private void FixedUpdate()
    {
        State currentState = GetCurrentState();
        bool stateChange = currentState != state;

        if (currentState == State.ON)
        {
            if (stateChange)
            {
                onPress.Invoke();

                if (invokeWhileEventsWithPressEvents)
                    whilePressed.Invoke();
            }
            else
                whilePressed.Invoke();
        }
        else if (currentState == State.OFF)
        {
            if (stateChange)
            {
                onRelease.Invoke();
            }
        }

        state = currentState;
    }

    private State GetCurrentState()
    {
        if (CurrentOffset >= threshold)
            return State.ON;

        return State.OFF;
    }
}
