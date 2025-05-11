using UnityEngine;
using UnityEngine.UIElements;

namespace USpring.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class VisualElementProvider : MonoBehaviour
    {
        private UIDocument uiDocument;

        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
        }

        public VisualElement GetVisualElement(string elementName)
        {
            return uiDocument.rootVisualElement.Q<VisualElement>(elementName);
        }
    }
}