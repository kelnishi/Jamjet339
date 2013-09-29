using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioQueue : MonoBehaviour {
	
	[SerializeField]
	private bool _mute = false;
	public bool mute {
		get { return _mute; }
		set { _mute = value; FixPassThrough(); }
	}
	[SerializeField]
	private bool _bypassEffects = false;
	public bool bypassEffects {
		get { return _bypassEffects; }
		set { _bypassEffects = value; FixPassThrough(); }
	}
	
	public bool playOnAwake = false;
	
	[SerializeField]
	private int _priority = 128;
	public int priority {
		get { return _priority; }
		set { _priority = value; FixPassThrough(); }
	}
	[SerializeField]
	private float _volume = 1f;
	public float volume {
		get { return _volume; }
		set { _volume = value; FixPassThrough(); }
	}
	[SerializeField]
	private float _pitch = 1f;
	public float pitch {
		get { return _pitch; }
		set { _pitch = value; FixPassThrough(); }
	}
	
	void FixPassThrough() {
		if (aso[0] != null) {
			aso[0].mute = _mute;
			aso[0].bypassEffects = _bypassEffects;
			aso[0].priority = _priority;
			aso[0].volume = _volume;
			aso[0].pitch = _pitch;
		}
		if (aso[1] != null) {
			aso[1].mute = _mute;
			aso[1].bypassEffects = _bypassEffects;
			aso[1].priority = _priority;
			aso[1].volume = _volume;
			aso[1].pitch = _pitch;
		}
	}
	
	
	public float delay = 0f;
	
	public int queueLength = 0;
	
	[SerializeField]
	public List<AudioClip> sounds = new List<AudioClip>();
	
	[SerializeField]
	public List<int> loops = new List<int>();
	
	int index = -1;
	int clipIndex = 0;
	AudioSource[] aso = new AudioSource[2];
	
	double eventTime;
	int loop = 0;
	
	float fade = 0f;
	
	// Use this for initialization
	void Start () {
		aso[0] = gameObject.AddComponent<AudioSource>();
		aso[1] = gameObject.AddComponent<AudioSource>();
		FixPassThrough();
		
		if (playOnAwake) Play();
	}
	
	void Update() {
		if (fade > 0f) {
			aso[0].volume -= fade * Time.deltaTime;
			aso[1].volume -= fade * Time.deltaTime;
			if (aso[0].volume <= 0f) fade = 0f;
		}
	}
		
	void Play() {
		if (delay > 0f) {
			Invoke("QueueNext", delay);
		} else {
			QueueNext();
		}
	}
	
	public void FadeOut(float over) {
		if (over <= 0f) return;
		fade = 1f/over;	
	}
	
	void QueueNext() {
		if (loop == 0) {
			if (++index >= queueLength) {
				aso[0].clip = aso[1].clip = null;
				clipIndex = 0;
				index = -1;
				return;
			}
			loop = loops[index] - 1;
		} else {
			--loop;
		}
		AudioSource lastSource = aso[clipIndex%2];
		++clipIndex;
		AudioSource currentSource = aso[clipIndex%2];
		currentSource.clip = sounds[index];
		
		if (lastSource.clip == null) {
			eventTime = AudioSettings.dspTime;
			currentSource.Play();
			QueueNext();
		} else {
			eventTime += lastSource.clip.length;
			currentSource.PlayScheduled(eventTime);
			if (loop < 0) {
				currentSource.loop = true;
			} else {
				Invoke("QueueNext", (float)(eventTime - AudioSettings.dspTime) + 0.01f);
			}
		}
	}
}
