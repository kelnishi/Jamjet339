using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(AudioQueue))]
public class AudioQueueEditor : Editor {
	
	public override void OnInspectorGUI() {
		EditorGUIUtility.LookLikeControls(90f);
		
		AudioQueue aq = (AudioQueue)target;
				
		bool mute = EditorGUILayout.Toggle("Mute:", aq.mute);
		bool bypass = EditorGUILayout.Toggle("Bypass Effects:", aq.bypassEffects);
		
		bool poa = EditorGUILayout.Toggle("Play on Awake:", aq.playOnAwake);
		float delay = EditorGUILayout.FloatField("Delay:", aq.delay);
		int priority = EditorGUILayout.IntSlider("Priority:", aq.priority, 0, 256);
		float volume = EditorGUILayout.Slider("Volume:", aq.volume, 0f, 1f);
		float pitch = EditorGUILayout.Slider("Pitch:", aq.pitch, -3f, 3f);
		
		int count = aq.queueLength;
		EditorGUILayout.BeginHorizontal();
		count = Mathf.Max(0,EditorGUILayout.IntField("Count:",count));
		if (GUILayout.Button("-")) count = Mathf.Max(0, count - 1);
		if (GUILayout.Button("+")) ++count;
		EditorGUILayout.EndHorizontal();
		
		if (count != aq.queueLength) {
			GUI.changed = true;	
		}
		
		List<AudioClip> effects = new List<AudioClip>(count);
		List<int> loops = new List<int>(count);
		for (int i = 0; i < count; ++i ) {
			effects.Add(null);
			loops.Add(1);
			EditorGUILayout.BeginHorizontal();
			if (aq.sounds.Count > i) {
				effects[i] = (AudioClip)EditorGUILayout.ObjectField("Clip "+i+":",aq.sounds[i],typeof(AudioClip),false);
			} else {
				effects[i] = (AudioClip)EditorGUILayout.ObjectField("Clip "+i+":",null,typeof(AudioClip),false);
			}
			
			loops[i] = EditorGUILayout.IntField(aq.loops.Count > i?aq.loops[i]:1, GUILayout.Width(30));
			EditorGUILayout.EndHorizontal();
		}
		
		if (GUI.changed) {
			Undo.RegisterUndo(aq, "AudioQueue Changed");
			aq.playOnAwake = poa;
			
			aq.mute = mute;
			aq.bypassEffects = bypass;
			aq.priority = priority;
			aq.volume = volume;
			aq.pitch = pitch;
			aq.delay = delay;
			aq.sounds = effects;
			aq.loops = loops;
			aq.queueLength = count;
		}
	}
}
