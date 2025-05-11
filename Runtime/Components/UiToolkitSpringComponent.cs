using UnityEngine;
using UnityEngine.UIElements;
using USpring.UI;

namespace USpring.Components
{
    [AddComponentMenu("Ludo/USpring/Components/UiToolkit Spring")]
    public class UiToolkitSpringComponent : MonoBehaviour
    {
        [SerializeField] private string visualTargetElementName;
        [SerializeField] private VisualElementProvider visualElementProvider;
    
        [HideInInspector] public TransformSpringComponent transformSpringComponent;
        private VisualElement targetElement;
        private Transform childObjectTransform;

        private void Start()
        {
            GameObject childObject = new GameObject($"UiToolkitSpring-{visualTargetElementName}");
            childObjectTransform = childObject.transform;
            childObjectTransform.parent = transform;
        
            targetElement = visualElementProvider.GetVisualElement(visualTargetElementName);
            transformSpringComponent = childObject.AddComponent<TransformSpringComponent>();
        
            //childObjectTransform will be an empty object that we'll use to pass the values to the targetElement
            childObjectTransform.position = targetElement.transform.position;
            childObjectTransform.rotation = targetElement.transform.rotation;
            childObjectTransform.localScale = targetElement.transform.scale;
        
            //We assign the new object, we'll then use this object to pass it's values to the targetElement in the Update method
            transformSpringComponent.followerTransform = childObjectTransform;
            transformSpringComponent.Initialize();
        }

        private void Update()
        {
            targetElement.transform.position = childObjectTransform.position;
            targetElement.transform.rotation = childObjectTransform.rotation;
            targetElement.transform.scale = childObjectTransform.localScale;
        }
    
        //You can access any method from the TransformSpringComponent class though the transformSpringComponent variable
        //You can check TransformSpringComponentAPI.cs to see all the methods available
        /*For example:
      transformSpringComponent.AddVelocityScale(f1 * Vector3.one);
      transformSpringComponent.AddVelocityPosition(f2 * Vector3.right);
      transformSpringComponent.AddVelocityRotation(f3 * Vector3.forward);
    */
    }
}