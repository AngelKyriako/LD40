﻿using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuDisplay : Display
{
	[SerializeField] private Text _playButtonTextField;

    protected override void Start() {
        base.Start();

        this.Open();
    }

    public void OnPlayClick()
	{
		this._playButtonTextField.text = "Resume";
		this.Close();

		Time.timeScale = 1;

		Cursor.visible = false;

        UserInterfaceController.Instance_._PopUpDisplay.TryWelcomeDisplay();
	}

	public void OnExitClick()
	{
		Application.Quit();
	}

#if UNITY_EDITOR
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
	}
#endif
}

namespace New.UTILITY
{
#if UNITY_EDITOR
	[CustomEditor(typeof(MainMenuDisplay))]
	[CanEditMultipleObjects]
	public class MainMenuDisplayEditor : Editor
	{
		private void OnEnable()
		{

		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

#pragma warning disable 0219
			MainMenuDisplay sMainMenuDisplay = target as MainMenuDisplay;
#pragma warning restore 0219
		}
	}
#endif
}