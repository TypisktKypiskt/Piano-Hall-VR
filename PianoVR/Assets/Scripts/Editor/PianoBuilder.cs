using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PianoBuilder
    {
        private enum PianoKeyColor
        {
            WHITE,
            BLACK
        }

        readonly int lowestC = 24;
        readonly int wholeOctaveLength = 12;

        private MIDIPlayer midiPlayer;

        private PianoKeyObjectSettings whiteKeyObjectSettings;
        private PianoKeyObjectSettings blackKeyObjectSettings;

        private Bounds whiteKeyBounds;
        private Vector3 blackKeyOffset;

        private Vector3 nextWhiteKeyPosition;
        private Vector3 parentScale;

        public PianoBuilder(MIDIPlayer midiPlayer, PianoKeyObjectSettings whiteKeyObjectSettings, PianoKeyObjectSettings blackKeyObjectSettings)
        {
            this.midiPlayer = midiPlayer;
            this.whiteKeyObjectSettings = whiteKeyObjectSettings;
            this.blackKeyObjectSettings = blackKeyObjectSettings;

            whiteKeyBounds = GetBounds(whiteKeyObjectSettings.prefab);

            Bounds blackKeyBounds = GetBounds(blackKeyObjectSettings.prefab);
            blackKeyOffset = new Vector3(0, blackKeyBounds.size.y * 0.5f, 0); 
        }

        public void BuildPiano(Transform parent, int octaves, int skippedOctaves, float distanceBetweenWhiteKeys)
        {
            parentScale = parent.localScale;
            parent.localScale = Vector3.one;

            int startNote = lowestC + skippedOctaves * wholeOctaveLength;
            int pianoKeyCount = octaves * wholeOctaveLength;

            for (int i = 0; i < pianoKeyCount; i++)
            {
                int midiNote = startNote + i;

                PianoKeyColor pianoKeyColor = GetPianoKeyColorForNote(midiNote);
                CreatePianoKey(parent, pianoKeyColor, midiNote, distanceBetweenWhiteKeys);
            }

            parent.localScale = parentScale;
        }

        private PianoKeyColor GetPianoKeyColorForNote(int midiNote)
        {
            int offSetFromC = midiNote % wholeOctaveLength;

            switch (offSetFromC)
            {
                case 0: // C
                case 2: // D
                case 4: // E
                case 5: // F
                case 7: // G
                case 9: // A
                case 11: // B
                    return PianoKeyColor.WHITE;
                case 1: // C# or D-flat
                case 3: // D# or E-flat
                case 6: // F# or G-flat
                case 8: // G# or A-flat
                case 10: // A# or B-flat
                    return PianoKeyColor.BLACK;
            }

            return PianoKeyColor.WHITE;
        }

        private void CreatePianoKey(Transform parent, PianoKeyColor pianoKeyColor, int midiNote, float distanceBetweenWhiteKeys)
        {
            PianoKeyObjectSettings pianoKeyObjectSettings = GetPianoKeyObjectSettings(pianoKeyColor);
            Vector3 position = GetPosition(pianoKeyColor, pianoKeyObjectSettings, distanceBetweenWhiteKeys);

            GameObject pianoKeyObject = PrefabUtility.InstantiatePrefab(pianoKeyObjectSettings.prefab) as GameObject;
            pianoKeyObject.transform.parent = parent;
            pianoKeyObject.transform.localPosition = position;
            pianoKeyObject.transform.localRotation = Quaternion.identity;

            //PianoKey pianoKey = pianoKeyObject.GetComponent<PianoKey>();
            PianoKey pianoKey = pianoKeyObject.GetComponentInChildren<PianoKey>();
            pianoKey.SetMidiPlayer(midiPlayer);
            pianoKey.SetMidiNote(midiNote);
        }

        private PianoKeyObjectSettings GetPianoKeyObjectSettings(PianoKeyColor pianoKeyColor)
        {
            if (pianoKeyColor == PianoKeyColor.WHITE)
                return whiteKeyObjectSettings;
            else
                return blackKeyObjectSettings;
        }

        private Bounds GetBounds(GameObject prefab)
        {
            Bounds bounds = new Bounds();

            foreach (Renderer renderer in prefab.GetComponentsInChildren<Renderer>())
            {
                if (renderer)
                {
                    if (bounds.size == Vector3.zero)
                    {
                        bounds = renderer.bounds;
                        continue;
                    }

                    bounds.Encapsulate(bounds);
                }
            }

            return bounds;
        }

        private Vector3 GetPosition(PianoKeyColor pianoKeyColor, PianoKeyObjectSettings pianoKeyObjectSettings, float distanceBetweenWhiteKeys)
        {
            Vector3 position = nextWhiteKeyPosition;

            Vector3 baseOffset = pianoKeyObjectSettings.baseOffset;

            if (pianoKeyColor == PianoKeyColor.WHITE)
            {
                nextWhiteKeyPosition += Vector3.right * (whiteKeyBounds.size.x + distanceBetweenWhiteKeys);
            }
            else if (pianoKeyColor == PianoKeyColor.BLACK)
            {
                position += Vector3.left * (whiteKeyBounds.size.x + distanceBetweenWhiteKeys) * 0.5f + blackKeyOffset;
            }

            return position + baseOffset;
        }

        public static void ClearParentOfChildren(Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0 ; i--)
            {
                GameObject.DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }
    }
}
