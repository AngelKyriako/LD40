﻿using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Display : MonoBehaviour
{
    [SerializeField]
    protected CanvasGroup _canvasGroup;
    
    public virtual void Initialize() {
        if (_canvasGroup == null) {
            _canvasGroup = GetComponentInChildren<CanvasGroup>();
        }

        Game.Instance.Logger.Assert(
            _canvasGroup != null, "Display::" + name,
            "_canvasGroup not found, make sure its attached on gameObject or a child " +
            "or drag and drop it to game object."
        );

        Close();
    }

    public bool IsOpen { get { return _canvasGroup.interactable; } }


    public virtual bool Toggle() {
        if (!IsOpen) {
            Open();
        } else {
            Close();
        }

        return IsOpen;
    }

    public virtual void Open()
	{
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1;
    }

	public virtual void Close()
	{
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0;
    }

#if UNITY_EDITOR
	protected virtual void OnDrawGizmos()
	{

	}
#endif
}

namespace New.UTILITY
{
#if UNITY_EDITOR
	[CustomEditor(typeof(Display))]
	[CanEditMultipleObjects]
	public class DisplayEditor : Editor
	{
		private void OnEnable()
		{

		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

#pragma warning disable 0219
			Display sDisplay = target as Display;
#pragma warning restore 0219
		}
	}
#endif
}