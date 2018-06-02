using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTJ.FrameCapturer;

public class PNGSeqExporter : MonoBehaviour {

    [Header("Info")]
    public AnimationClip currentAnimationClip;

    [Header("Export Directory")]
    public string outputDir = "Capture/animationName";

    [Header("Model Initializes")]
    public Animation model;
    public List<AnimationClip> animationClips = new List<AnimationClip>();

    private bool _isRecording = false;
    private MovieRecorder _movieRecorder;
    private int _currentAnimationClipIndex = -1;

    private const string ERR_MODEL_NOT_FOUND = "Animation model not found!";
    private const string ERR_MOVIE_RECORDER_NOT_FOUND = "Movie Recorder not found in Camera's components!";
    private const string ON_RECORD_COMPLETED = "Record has been finished!";

    private void Awake() {
        _movieRecorder = Camera.main.GetComponent<MovieRecorder>();
    }

    private void Start() {
        if (model != null) {
            InitAnimationClips(model);
        } else {
            LogError(ERR_MODEL_NOT_FOUND);
        }
    }

    private void Update() {
        if (_isRecording) {
            if (!model.isPlaying) {
                RecordNextAnimation();
            }
        }
    }

    public void Action() {
        if (model == null) {
            LogError(ERR_MODEL_NOT_FOUND);
            return;
        }
        if (_movieRecorder == null) {
            LogError(ERR_MOVIE_RECORDER_NOT_FOUND);
            return;
        }

        model.Stop();

        RecordNextAnimation();

        _isRecording = true;
    }

    private void RecordNextAnimation() {
        _movieRecorder.EndRecording();

        AnimationClip nextClip = GetNextClip();
        if (nextClip == null) {
            _isRecording = false;
            Debug.Log(ON_RECORD_COMPLETED);
        } else {
            currentAnimationClip = nextClip; 
            model.clip = nextClip;
            outputDir = "Capture/" + model.clip.name;
            model.Play();

            _movieRecorder.outputDir = new DataPath(DataPath.Root.Current, outputDir);
            _movieRecorder.BeginRecording();

            Debug.Log(model.clip.name + " animation is recording to: " + _movieRecorder.outputDir.GetFullPath());
        }
    }

    private AnimationClip GetNextClip() {
        if (_currentAnimationClipIndex >= animationClips.Count - 1)
            return null;

        return animationClips[++_currentAnimationClipIndex];
    }

    private void InitAnimationClips(Animation anim) {
        foreach (AnimationState state in anim) {
            animationClips.Add(state.clip);
        }
    }

    private void LogError(string message) {
        Debug.LogError("PNGSeqExporter: " + message);
    }

}
