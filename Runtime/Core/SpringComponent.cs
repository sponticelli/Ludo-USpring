using UnityEngine;
using System.Collections.Generic;
using Ludo.UnityInject;
using UnityEngine.SceneManagement;

namespace USpring.Core
{
    /// <summary>
    /// An abstract base class for components that manage and update spring behaviors in Unity.
    /// Provides functionality for initializing, updating, and validating springs, as well as handling
    /// custom initial values, targets, and other spring-related properties.
    /// </summary>
    public abstract class SpringComponent : MonoBehaviour
    {
        [Tooltip("When enabled, the Spring Component will initialize automatically on Awake. " +
                 "Otherwise you'll need to call Initialize() manually.")]
        [SerializeField]
        public bool doesAutoInitialize;

        public bool useScaledTime = true;
        public bool alwaysUseAnalyticalSolution = false;

        private List<Spring> springs;

        [Tooltip("When enabled, sets the initial Current Value to a custom predefined value instead of the default")]
        [SerializeField]
        protected bool hasCustomInitialValues;

        [Tooltip("When enabled, uses a custom Target value instead of using the default Target")] [SerializeField]
        protected bool hasCustomTarget;

        [Inject] protected ISpringSettingsProvider SpringSettings;

        private bool isValidSpringComponent;
        private string errorReason = string.Empty;

        [HideInInspector] public bool generalPropertiesUnfolded = true;
        [HideInInspector] public bool initialValuesUnfolded;
        private bool doFixedUpdateRate;
        private float springFixedTimeStep;

        protected bool initialized;
        
        public bool IsInitialized => initialized;

        private void Awake()
        {
            if (doesAutoInitialize)
            {
                Initialize();
            }
        }

        public virtual void OnDestroy()
        {
            UnregisterSprings();
        }

        public virtual void Initialize()
        {
            springs = new List<Spring>();
            doFixedUpdateRate = SpringSettings.DoFixedUpdateRate;
            springFixedTimeStep = SpringSettings.SpringFixedTimeStep;

            isValidSpringComponent = IsValidSpringComponent();
            if (!isValidSpringComponent)
            {
                SpringComponentNotValid();
            }
            else
            {
                RegisterSprings();
                CheckSpringSizes();

                for (int i = 0; i < springs.Count; i++)
                {
                    springs[i].Initialize();
                }

                SetInitialValues();
            }

            initialized = true;
        }

        protected virtual void SetInitialValues()
        {
            if (!hasCustomInitialValues)
            {
                SetCurrentValueByDefault();
            }

            if (!hasCustomTarget)
            {
                SetTargetByDefault();
            }
        }

        private void SpringComponentNotValid()
        {
            string objectTypeName = GetType().ToString();
            string fullErrorMessage = $"{gameObject.name} {objectTypeName} is not valid! Disabling Component";

            if (!string.IsNullOrEmpty(errorReason))
            {
                fullErrorMessage += $"\nReason: {errorReason}";
            }

            Debug.LogError(fullErrorMessage, gameObject);
            enabled = false;
        }

        protected void AddErrorReason(string reason)
        {
            if (!string.IsNullOrEmpty(errorReason))
            {
                errorReason += "\n";
            }

            errorReason += reason;
        }

        protected abstract void SetCurrentValueByDefault();
        protected abstract void SetTargetByDefault();

        protected abstract void RegisterSprings();

        public abstract bool IsValidSpringComponent();

        protected void RegisterSpring(Spring spring)
        {
            springs.Add(spring);
        }

        private void UnregisterSprings()
        {
        }

        public virtual void LateUpdate()
        {
            if (!initialized)
            {
                return;
            }

            isValidSpringComponent = IsValidSpringComponent();
            if (!isValidSpringComponent)
            {
                SpringComponentNotValid();
                return;
            }

            float deltaTime = useScaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
            if (doFixedUpdateRate)
            {
                int numIterations = GetNumberOfFixedSteps(deltaTime);
                if (numIterations > 1)
                {
                    float remainingDeltaTime = deltaTime;
                    while (remainingDeltaTime > 0f)
                    {
                        float correctedDeltaTime;
                        if (remainingDeltaTime > springFixedTimeStep)
                        {
                            correctedDeltaTime = springFixedTimeStep;
                        }
                        else
                        {
                            correctedDeltaTime = remainingDeltaTime;
                        }

                        UpdateSprings(correctedDeltaTime);
                        remainingDeltaTime -= springFixedTimeStep;
                    }
                }
                else
                {
                    UpdateSprings(deltaTime);
                }
            }
            else
            {
                UpdateSprings(deltaTime);
            }
        }

        private void UpdateSprings(float deltaTime)
        {
            for (int i = 0; i < springs.Count; i++)
            {
                if (!springs[i].springEnabled)
                {
                    continue;
                }

                if (alwaysUseAnalyticalSolution) SpringMath.UpdateSpring(deltaTime, springs[i], -1f);
                else SpringMath.UpdateSpring(deltaTime, springs[i], SpringSettings.MaxForceBeforeAnalyticalIntegration);

                springs[i].CheckEvents();
                springs[i].ProcessCandidateValue();
            }
        }

        public virtual void ReachEquilibrium()
        {
            ValidateSpringsList();

            for (int i = 0; i < springs.Count; i++)
            {
                springs[i].ReachEquilibrium();
            }
        }

        private void ValidateSpringsList()
        {
            if (springs == null)
            {
                if (doesAutoInitialize)
                {
                    Initialize();
                }
                else
                {
                    Debug.LogWarning($"{gameObject.name} is not initialized. " +
                                     $" DoesAutoInitialize is disabled and you forgot to Initialize()", gameObject);
                }
            }
        }

        private void CheckSpringSizes()
        {
            for (int i = 0; i < springs.Count; i++)
            {
                bool hasValidSize = springs[i].CheckCorrectSize();

                if (!hasValidSize)
                {
                    Debug.LogWarning(
                        $"Spring size not valid! Fix in the editor to avoid performance issues: [{gameObject.name}]",
                        gameObject);
                }
            }
        }

        private int GetNumberOfFixedSteps(float deltaTime)
        {
            int res = 1;
            if (deltaTime > springFixedTimeStep)
            {
                res = Mathf.RoundToInt(deltaTime / springFixedTimeStep);
            }

            return res;
        }

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            doesAutoInitialize = true;
        }

        private void OnDrawGizmosSelected()
        {
            DrawGizmosSelected();
        }

        protected virtual void DrawGizmosSelected()
        {
            if (springs == null)
            {
                return;
            }

            for (int i = 0; i < springs.Count; i++)
            {
                springs[i].DrawGizmosSelected(transform.position);
            }
        }


        [ContextMenu("Fix Springs Size")]
        public void FixSpringsSizes()
        {
            GameObject springComponentGO = gameObject;

            bool isPrefab = UnityEditor.PrefabUtility.IsAnyPrefabInstanceRoot(gameObject);
            string prefabPath = string.Empty;
            if (isPrefab)
            {
                prefabPath = UnityEditor.PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
                springComponentGO = UnityEditor.PrefabUtility.LoadPrefabContents(prefabPath);
            }

            SpringComponent _springComponent = springComponentGO.GetComponent<SpringComponent>();
            _FixSpringsSizes(_springComponent);

            if (isPrefab)
            {
                UnityEditor.PrefabUtility.SaveAsPrefabAsset(springComponentGO, prefabPath);
                UnityEditor.PrefabUtility.UnloadPrefabContents(springComponentGO);
            }
            else
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }

        private void _FixSpringsSizes(SpringComponent springComponent)
        {
            Spring[] springs = springComponent.GetSpringsArray();
            for (int i = 0; i < springs.Length; i++)
            {
                springs[i].CheckCorrectSize();
            }
        }

        internal virtual Spring[] GetSpringsArray()
        {
            Spring[] res = new Spring[0];
            return res;
        }
#endif
    }
}