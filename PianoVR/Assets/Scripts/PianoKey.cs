using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKey : MonoBehaviour
{
    [SerializeField]
    private MIDIPlayer midiPlayer;
    [SerializeField]
    private int midiNote;
    [SerializeField]
    private float maxVelocity;

    private float currentVolumeMultiplier = 0f;
    public void Press()
    {
        float velocityMagnitude = GetComponent<Rigidbody>().velocity.magnitude;
        currentVolumeMultiplier = Mathf.Lerp(0.1f, 1f, velocityMagnitude / maxVelocity);
        
        midiPlayer.PlayNote(midiNote, currentVolumeMultiplier);
    }

    public void Release()
    {
        midiPlayer.EndNoteAll(midiNote);
    }

    public void SetMidiPlayer(MIDIPlayer midiPlayer)
    {
        this.midiPlayer = midiPlayer;
    }

    public void SetMidiNote(int midiNote)
    {
        this.midiNote = midiNote;
    }
}
