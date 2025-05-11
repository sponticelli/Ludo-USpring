using UnityEditor;
using UnityEngine;
using USpring.Drawers;
using static USpring.SpringsEditorUtility;

namespace USpring.Components
{
	[CustomEditor(typeof(AudioSourceSpringComponent))]
	[CanEditMultipleObjects]
	public class AudioSourceSpringComponentEditor : SpringComponentCustomEditor
	{
		private SerializedProperty spAutoUpdatedAudioSource;

		private SpringFloatDrawer volumeSpringDrawer;
		private SpringFloatDrawer pitchSpringDrawer;

		protected override void RefreshSerializedProperties()
		{
			base.RefreshSerializedProperties();

			spAutoUpdatedAudioSource = serializedObject.FindProperty("autoUpdatedAudioSource");
		}

		protected override void CreateDrawers()
		{
			volumeSpringDrawer = new SpringFloatDrawer(serializedObject.FindProperty("volumeSpring"), false, false);
			pitchSpringDrawer = new SpringFloatDrawer(serializedObject.FindProperty("pitchSpring"), false, false);
		}

		protected override void DrawCustomInitialTarget()
		{
			DrawCustomTargetBySpring("Target Volume", LabelWidth, volumeSpringDrawer);
			DrawCustomTargetBySpring("Target Pitch", LabelWidth, pitchSpringDrawer);
		}

		protected override void DrawCustomInitialValuesSection()
		{
			DrawInitialValuesBySpring("Initial Values Volume", LabelWidth, volumeSpringDrawer);
			DrawInitialValuesBySpring("Initial Values Pitch", LabelWidth, pitchSpringDrawer);
		}

		protected override void DrawMainAreaUnfolded()
		{
			DrawSerializedProperty(spAutoUpdatedAudioSource, LabelWidth);
		}

		protected override void DrawSprings()
		{
			// Draw presets dropdown
			USpringPresets.DrawPresetDropdown(preset => {
				serializedObject.Update();

				// Get the force and drag properties from both springs
				SerializedProperty volumeForceProperty = volumeSpringDrawer.SpringEditorObject.SpCommonForce;
				SerializedProperty volumeDragProperty = volumeSpringDrawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty pitchForceProperty = pitchSpringDrawer.SpringEditorObject.SpCommonForce;
				SerializedProperty pitchDragProperty = pitchSpringDrawer.SpringEditorObject.SpCommonDrag;
				SerializedProperty analyticalSolutionProperty = SpAlwaysUseAnalyticalSolution;

				// Apply the preset to both springs
				USpringPresets.ApplyPreset(serializedObject, preset, volumeForceProperty, volumeDragProperty, analyticalSolutionProperty);
				USpringPresets.ApplyPreset(serializedObject, preset, pitchForceProperty, pitchDragProperty, null);

				serializedObject.ApplyModifiedProperties();
			});

			// Draw the springs
			DrawSpring(volumeSpringDrawer);
			DrawSpring(pitchSpringDrawer);
		}

		/// <summary>
		/// Draws a preview of the audio source spring behavior
		/// </summary>
		protected override void DrawSpringPreview()
		{
			// Get the spring parameters
			float volumeForce = volumeSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
			float volumeDrag = volumeSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;
			float pitchForce = pitchSpringDrawer.SpringEditorObject.SpCommonForce.floatValue;
			float pitchDrag = pitchSpringDrawer.SpringEditorObject.SpCommonDrag.floatValue;

			// Draw the preview for both springs
			float[] forces = new float[] { volumeForce, pitchForce };
			float[] drags = new float[] { volumeDrag, pitchDrag };
			Color[] colors = new Color[] {
				USpringEditorStyles.FloatSpringColor, // Volume
				new Color(0.2f, 0.6f, 1f, 1f)  // Pitch (different color for distinction)
			};
			string[] labels = new string[] { "Volume", "Pitch" };

			// Draw the multi-component preview
			USpringPreviewUtility.DrawMultiComponentSpringPreview(forces, drags, colors, labels);
		}

		/// <summary>
		/// Draws component-specific help information
		/// </summary>
		protected override void DrawComponentSpecificHelp()
		{
			USpringEditorStyles.DrawSubHeader("Audio Source Spring Component");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("The Audio Source Spring Component allows you to animate audio properties with spring physics.");
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("It can animate both volume and pitch of an AudioSource component.");

			USpringEditorStyles.EndSection();

			// Draw example usage
			USpringEditorStyles.DrawSubHeader("Example Usage");
			USpringEditorStyles.BeginSection();

			EditorGUILayout.LabelField("1. Attach this component to a GameObject with an AudioSource");
			EditorGUILayout.LabelField("2. Assign the AudioSource to the Auto Updated Audio Source field");
			EditorGUILayout.LabelField("3. Set your desired target volume and pitch");
			EditorGUILayout.LabelField("4. Call SetTarget() from code to animate to new values");

			USpringEditorStyles.EndSection();
		}

		protected override void DrawInfoArea()
		{
			EditorGUILayout.Space(2);

			if (spAutoUpdatedAudioSource.objectReferenceValue == null)
			{
				EditorGUILayout.HelpBox("Auto Updated Audio Source is not assigned!", MessageType.Error);
			}
		}
	}
}