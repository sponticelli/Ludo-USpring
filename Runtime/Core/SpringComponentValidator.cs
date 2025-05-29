using UnityEngine;
using System.Collections.Generic;

namespace USpring.Core
{
    /// <summary>
    /// Utility class for validating spring components and providing centralized validation logic.
    /// Helps reduce code duplication and provides consistent validation behavior across components.
    /// </summary>
    public static class SpringComponentValidator
    {
        /// <summary>
        /// Validates a spring component and collects any error messages.
        /// </summary>
        /// <param name="component">The spring component to validate.</param>
        /// <param name="errorMessages">List to collect error messages.</param>
        /// <returns>True if the component is valid, false otherwise.</returns>
        public static bool ValidateComponent(SpringComponent component, List<string> errorMessages)
        {
            if (component == null)
            {
                errorMessages.Add("SpringComponent is null");
                return false;
            }

            bool isValid = true;

            // Validate springs
            if (!ValidateSprings(component, errorMessages))
            {
                isValid = false;
            }

            // Validate settings provider
            if (!ValidateSettingsProvider(component, errorMessages))
            {
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Validates that the component has properly configured springs.
        /// </summary>
        /// <param name="component">The spring component to validate.</param>
        /// <param name="errorMessages">List to collect error messages.</param>
        /// <returns>True if springs are valid, false otherwise.</returns>
        private static bool ValidateSprings(SpringComponent component, List<string> errorMessages)
        {
            bool isValid = true;

#if UNITY_EDITOR
            var springs = component.GetSpringsArray();
            if (springs == null || springs.Length == 0)
            {
                errorMessages.Add($"No springs found in {component.GetType().Name}");
                isValid = false;
            }
            else
            {
                for (int i = 0; i < springs.Length; i++)
                {
                    if (springs[i] == null)
                    {
                        errorMessages.Add($"Spring at index {i} is null in {component.GetType().Name}");
                        isValid = false;
                    }
                    else if (!springs[i].HasValidSize())
                    {
                        errorMessages.Add($"Spring at index {i} has invalid size in {component.GetType().Name}");
                        isValid = false;
                    }
                }
            }
#endif

            return isValid;
        }

        /// <summary>
        /// Validates that the component has access to spring settings.
        /// </summary>
        /// <param name="component">The spring component to validate.</param>
        /// <param name="errorMessages">List to collect error messages.</param>
        /// <returns>True if settings provider is valid, false otherwise.</returns>
        private static bool ValidateSettingsProvider(SpringComponent component, List<string> errorMessages)
        {
            // Note: SpringSettings is injected via dependency injection
            // We can't easily validate it here without reflection
            // This method is a placeholder for future validation logic
            return true;
        }

        /// <summary>
        /// Validates a Unity component reference.
        /// </summary>
        /// <typeparam name="T">The type of component to validate.</typeparam>
        /// <param name="component">The component to validate.</param>
        /// <param name="componentName">The name of the component for error messages.</param>
        /// <param name="errorMessages">List to collect error messages.</param>
        /// <returns>True if the component is valid, false otherwise.</returns>
        public static bool ValidateUnityComponent<T>(T component, string componentName, List<string> errorMessages) 
            where T : class
        {
            if (component == null)
            {
                errorMessages.Add($"{componentName} is null");
                return false;
            }

            // Additional validation for Unity components
            if (component is Component unityComponent)
            {
                if (unityComponent.gameObject == null)
                {
                    errorMessages.Add($"{componentName} has null GameObject");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates a Transform reference.
        /// </summary>
        /// <param name="transform">The transform to validate.</param>
        /// <param name="transformName">The name of the transform for error messages.</param>
        /// <param name="errorMessages">List to collect error messages.</param>
        /// <returns>True if the transform is valid, false otherwise.</returns>
        public static bool ValidateTransform(Transform transform, string transformName, List<string> errorMessages)
        {
            return ValidateUnityComponent(transform, transformName, errorMessages);
        }

        /// <summary>
        /// Validates a Camera reference.
        /// </summary>
        /// <param name="camera">The camera to validate.</param>
        /// <param name="cameraName">The name of the camera for error messages.</param>
        /// <param name="errorMessages">List to collect error messages.</param>
        /// <returns>True if the camera is valid, false otherwise.</returns>
        public static bool ValidateCamera(Camera camera, string cameraName, List<string> errorMessages)
        {
            if (!ValidateUnityComponent(camera, cameraName, errorMessages))
            {
                return false;
            }

            // Additional camera-specific validation could go here
            return true;
        }

        /// <summary>
        /// Validates a Renderer reference.
        /// </summary>
        /// <param name="renderer">The renderer to validate.</param>
        /// <param name="rendererName">The name of the renderer for error messages.</param>
        /// <param name="errorMessages">List to collect error messages.</param>
        /// <returns>True if the renderer is valid, false otherwise.</returns>
        public static bool ValidateRenderer(Renderer renderer, string rendererName, List<string> errorMessages)
        {
            if (!ValidateUnityComponent(renderer, rendererName, errorMessages))
            {
                return false;
            }

            // Additional renderer-specific validation
            if (renderer.material == null)
            {
                errorMessages.Add($"{rendererName} has null material");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates an AudioSource reference.
        /// </summary>
        /// <param name="audioSource">The audio source to validate.</param>
        /// <param name="audioSourceName">The name of the audio source for error messages.</param>
        /// <param name="errorMessages">List to collect error messages.</param>
        /// <returns>True if the audio source is valid, false otherwise.</returns>
        public static bool ValidateAudioSource(AudioSource audioSource, string audioSourceName, List<string> errorMessages)
        {
            return ValidateUnityComponent(audioSource, audioSourceName, errorMessages);
        }

        /// <summary>
        /// Validates a Light reference.
        /// </summary>
        /// <param name="light">The light to validate.</param>
        /// <param name="lightName">The name of the light for error messages.</param>
        /// <param name="errorMessages">List to collect error messages.</param>
        /// <returns>True if the light is valid, false otherwise.</returns>
        public static bool ValidateLight(Light light, string lightName, List<string> errorMessages)
        {
            return ValidateUnityComponent(light, lightName, errorMessages);
        }

        /// <summary>
        /// Validates a RectTransform reference.
        /// </summary>
        /// <param name="rectTransform">The rect transform to validate.</param>
        /// <param name="rectTransformName">The name of the rect transform for error messages.</param>
        /// <param name="errorMessages">List to collect error messages.</param>
        /// <returns>True if the rect transform is valid, false otherwise.</returns>
        public static bool ValidateRectTransform(RectTransform rectTransform, string rectTransformName, List<string> errorMessages)
        {
            return ValidateUnityComponent(rectTransform, rectTransformName, errorMessages);
        }

        /// <summary>
        /// Creates a formatted error message from a list of error messages.
        /// </summary>
        /// <param name="errorMessages">The list of error messages.</param>
        /// <param name="componentName">The name of the component for the error message.</param>
        /// <returns>A formatted error message string.</returns>
        public static string FormatErrorMessage(List<string> errorMessages, string componentName)
        {
            if (errorMessages == null || errorMessages.Count == 0)
            {
                return string.Empty;
            }

            return $"{componentName} validation failed:\n" + string.Join("\n", errorMessages);
        }
    }
}
