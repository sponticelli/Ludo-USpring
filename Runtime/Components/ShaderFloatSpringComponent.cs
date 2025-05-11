using UnityEngine;
using UnityEngine.UI;
using USpring.Core;

namespace USpring.Components
{
	[AddComponentMenu("Ludo/USpring/Components/Shader Float Spring")]
	public partial class ShaderFloatSpringComponent : SpringComponent
	{
		[SerializeField] private SpringFloat shaderValueSpring = new SpringFloat();

		[SerializeField] private string shaderPropertyName;
		
		[SerializeField] private bool targetIsRenderer = true;
		[SerializeField] private Renderer targetRenderer;
		[SerializeField] private Graphic targetGraphic;

		[SerializeField, Tooltip("Get auto updated material from target renderer or graphic")] 
		private bool getAutoUpdatedMaterialFromTarget;
		[SerializeField] private Material autoUpdatedMaterial;        

        private int shaderPropertyID;
        private float initialShaderValue;
        
        public SpringEvents Events => shaderValueSpring.springEvents;
        public float GetTarget() => shaderValueSpring.GetTarget();
        public void SetTarget(float target) => shaderValueSpring.SetTarget(target);
        public float GetCurrentValue() => shaderValueSpring.GetCurrentValue();
        public void SetCurrentValue(float currentValues) => shaderValueSpring.SetCurrentValue(currentValues);
        public float GetVelocity() => shaderValueSpring.GetVelocity();
        public void SetVelocity(float velocity) => shaderValueSpring.SetVelocity(velocity);
        public void AddVelocity(float velocityToAdd) =>	shaderValueSpring.AddVelocity(velocityToAdd);
        public float GetForce() => shaderValueSpring.GetForce();
        public void SetForce(float force) => shaderValueSpring.SetForce(force);
        public float GetDrag() => shaderValueSpring.GetDrag();
        public void SetDrag(float drag) => shaderValueSpring.SetDrag(drag);
        public void SetMinValues(float minValue) => shaderValueSpring.SetMinValue(minValue);
        public void SetMaxValues(float maxValue) => shaderValueSpring.SetMaxValue(maxValue);
        public void SetClampCurrentValues(bool clamp) => shaderValueSpring.SetClampCurrentValue(clamp);
        public void SetClampTarget(bool clamp) => shaderValueSpring.SetClampTarget(clamp);
        public void StopSpringOnClamp(bool stop) => shaderValueSpring.SetStopOnClamp(stop);
        public float GetCommonForce() => shaderValueSpring.GetCommonForce();
        public float GetCommonDrag() => shaderValueSpring.GetCommonDrag();
        public void SetCommonForce(float force)
        {
	        shaderValueSpring.SetCommonForceAndDrag(true);
	        shaderValueSpring.SetCommonForce(force);
        }
        public void SetCommonDrag(float drag)
        {
	        shaderValueSpring.SetCommonForceAndDrag(true);
	        shaderValueSpring.SetCommonDrag(drag);
        }
        public void SetCommonForceAndDrag(float force, float drag)
        {
	        SetCommonForce(force);
	        SetCommonDrag(drag);
        }

        public override void Initialize()
        {
	        shaderPropertyID = Shader.PropertyToID(shaderPropertyName);
	        if(getAutoUpdatedMaterialFromTarget)
	        {
		        if(targetIsRenderer)
		        {
			        autoUpdatedMaterial = targetRenderer.material;
		        }
		        else
		        {
			        autoUpdatedMaterial = new Material(targetGraphic.material);
			        targetGraphic.material = autoUpdatedMaterial;
		        }
	        }
	        
			base.Initialize();
        }

		private float GetDefaultShaderValue()
		{
			float res = autoUpdatedMaterial.GetFloat(shaderPropertyID);
			return res;
		}

		protected override void SetCurrentValueByDefault()
		{
			float defaultShaderValue = GetDefaultShaderValue();
			shaderValueSpring.SetCurrentValue(defaultShaderValue);
		}

		protected override void SetTargetByDefault()
		{
			float defaultShaderValue = GetDefaultShaderValue();
			shaderValueSpring.SetTarget(defaultShaderValue);
		}


		public void Update()
		{
			if (!initialized) { return; }

			autoUpdatedMaterial.SetFloat(shaderPropertyID, shaderValueSpring.GetCurrentValue());
		}
        
        public void ChangeTargetProperty(string newPropertyName)
        {
            shaderPropertyName = newPropertyName;
            shaderPropertyID = Shader.PropertyToID(shaderPropertyName);
        }
        
		protected override void RegisterSprings()
		{
			RegisterSpring(shaderValueSpring);
		}

		public override bool IsValidSpringComponent()
		{
			if(targetIsRenderer && targetRenderer == null)
			{
				AddErrorReason($"{gameObject.name} ShaderFloatSpringComponent targetRenderer is null.");
				return false;
			}
			
			if(!targetIsRenderer && targetGraphic == null)
			{
				AddErrorReason($"{gameObject.name} ShaderFloatSpringComponent targetGraphic is null.");
				return false;
			}
			
			if(getAutoUpdatedMaterialFromTarget)
			{
				if(targetIsRenderer)
				{
					autoUpdatedMaterial = targetRenderer.material;
				}
				else
				{
					autoUpdatedMaterial = new Material(targetGraphic.material);
					targetGraphic.material = autoUpdatedMaterial;
				}
			}

			if(autoUpdatedMaterial == null)
			{
				AddErrorReason($"{gameObject.name} ShaderFloatSpringComponent autoUpdatedMaterial is null.");
				return false;
			}

			return true;
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			if(!getAutoUpdatedMaterialFromTarget)
			{
				autoUpdatedMaterial.SetFloat(shaderPropertyID, initialShaderValue);	
			}
		}

#if UNITY_EDITOR
		protected override void Reset()
		{
			base.Reset();

			if(targetRenderer == null) targetRenderer = GetComponent<Renderer>();
			if(targetGraphic == null) targetGraphic = GetComponent<Graphic>();
			if(targetRenderer == null && targetGraphic != null) targetIsRenderer = false;
			if(targetRenderer != null && targetGraphic == null) targetIsRenderer = true;
			if(targetIsRenderer) targetGraphic = null;
			else targetRenderer = null;   
		}

		internal override Spring[] GetSpringsArray()
		{
			Spring[] res = new Spring[]
			{
				shaderValueSpring
			};

			return res;
		}
#endif
	}
}