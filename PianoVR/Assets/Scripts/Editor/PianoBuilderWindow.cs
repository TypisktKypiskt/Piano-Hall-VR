using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PianoBuilderWindow : EditorWindow
    {
        private Transform parent;
        private MIDIPlayer midiPlayer;

        private PianoKeyObjectSettings whiteKeyObjectSettings;
        private PianoKeyObjectSettings blackKeyObjectSettings;

        private int octaves = 2;
        private int skippedOctaves = 2;

        private float distanceBetweenWhiteKeys = 0.1f;

        [MenuItem("Window/Piano Builder")]
        private static void Init()
        {
            EditorWindow window = GetWindow(typeof(PianoBuilderWindow));
            window.Show();
        }

        private void OnGUI()
        {
            parent = (Transform)EditorGUILayout.ObjectField("Parent: ", parent, typeof(Transform), true);

            GUILayout.Space(10);

            // Button: Clear parent
            EditorGUI.BeginDisabledGroup(parent == null);
            if (GUILayout.Button("Clear parent"))
            {
                PianoBuilder.ClearParentOfChildren(parent);
            }
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);

            midiPlayer = (MIDIPlayer)EditorGUILayout.ObjectField("MidiPlayer: ", midiPlayer, typeof(MIDIPlayer), true);
            whiteKeyObjectSettings = (PianoKeyObjectSettings)EditorGUILayout.ObjectField("WhiteKey Object Settings: ", whiteKeyObjectSettings, typeof(PianoKeyObjectSettings), false);
            blackKeyObjectSettings = (PianoKeyObjectSettings)EditorGUILayout.ObjectField("BlackKey Object Settings: ", blackKeyObjectSettings, typeof(PianoKeyObjectSettings), false);

            GUILayout.Space(20);

            if (IsRequiredValuesSet())
            {
                PianoBuilder pianoBuilder = new PianoBuilder(midiPlayer, whiteKeyObjectSettings, blackKeyObjectSettings);

                octaves = EditorGUILayout.IntField("Octaves: ", octaves);
                skippedOctaves = EditorGUILayout.IntField("Skipped Octaves: ", skippedOctaves);

                GUILayout.Space(20);

                distanceBetweenWhiteKeys = EditorGUILayout.FloatField("Distance Between WhiteKeys: ", distanceBetweenWhiteKeys);

                if (GUILayout.Button("Build"))
                {
                    pianoBuilder.BuildPiano(parent, octaves, skippedOctaves, distanceBetweenWhiteKeys);
                }
            }
        }

        private bool IsRequiredValuesSet()
        {
            return parent != null && midiPlayer != null && whiteKeyObjectSettings != null && blackKeyObjectSettings != null;
        }
    }
}