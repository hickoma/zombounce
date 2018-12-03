using UnityEngine;
using System.Collections;

public class TweenPath : MonoBehaviour {
	public enum Action {
		Activate,
		Wait,
		Move,
		Rotate,
		Scale,
		Run
	}

	[System.Serializable]
	public class Node {
		public Action action;
		public Transform transform = null;
		public bool waitForEnd = true;
		public float delay = 0, time = 1;
		public Vector3 position;
		public bool active;
		public float value;
		public Tween.EaseType easeType = Tween.EaseType.easeInOutQuad;
		public Tween.LoopType loopType;
		public int count; // 0 - repeat forever
		public string runName;
	}

	public string pathName, tweenTag;
	public int playCount = 1;
	public bool startOnAwake = true, snap = false;
	public float wait = 0;
	public Node[] nodes;

	int index = 0, playedCount = 0;
	bool isPlaying = false;
	System.Action onComplete = null;
	Tween.Base tween = null;

	void OnEnable() {
		if (startOnAwake) {
			if (wait == 0)
				Play();
			else {
				tween = Tween.Delay(wait, () => Play());
				tween.Start();
			}
		}
	}

	void OnDisable() {
		Stop();
	}

	public void Play(System.Action onComplete=null) {
		if (isPlaying)
			return;
		this.onComplete = onComplete;
		index = 0;
		playedCount = 0;
		isPlaying = true;
		Next();
	}

	public void Stop() {
		foreach (Node node in nodes)
			if (node.transform != null)
				Tween.Stop(node.transform);
		Tween.Stop(this);
		if (tween != null)
			tween.Stop();
		tween = null;
		isPlaying = false;
	}

	void Next() {
		if (!isPlaying)
			return;
		if (index >= nodes.Length) {
			playedCount++;

			// finished
			index = 0;
			if (playCount > 0 && playCount <= playedCount) {
				isPlaying = false;
				if (onComplete != null)
					onComplete();
				return;
			}
		}

		Node node = nodes[index++];
		Transform t = node.transform == null ? transform : node.transform;
		tween = null;

		switch (node.action) {
		case Action.Activate:
			t.gameObject.SetActive(node.active);
			break;

		case Action.Wait:
			tween = Tween.Delay(node.time + node.delay, null).Target(this);
			break;

		case Action.Run:
			if (t != null) {
				TweenPath[] pathes = t.GetComponents<TweenPath>();
				foreach (TweenPath tp in pathes) {
					if (tp.pathName == node.runName) {
						tp.Play();
						break;
					}
				}
			}
			break;
		
		case Action.Move:
			if (node.time == 0 && node.delay == 0)
				t.localPosition = node.position;
			else {
				tween = Tween.Move(t, node.time, node.delay).To(node.position);
				((Tween.TweenMove)tween).SetSnap(snap);
			}
			break;

		case Action.Rotate:
			if (node.time == 0 && node.delay == 0)
				t.localEulerAngles = node.position;
			else
				tween = Tween.Rotate(t, node.time, node.delay).To(node.position);
			break;
		
		case Action.Scale:
			if (node.time == 0 && node.delay == 0)
				t.localScale = node.position;
			else
				tween = Tween.Scale(t, node.time, node.delay).To(node.position);
			break;
		}

		if (tween != null) {
			tween.Ease(node.easeType);
			if (tweenTag != null)
				tween.Tags(tweenTag);
			if (node.waitForEnd)
				tween.OnComplete(Next);
			tween.Start();
		}
		if (tween == null || !node.waitForEnd)
			Next();
	}
}
