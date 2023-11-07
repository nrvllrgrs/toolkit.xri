using UnityEngine;
using UnityEngine.InputSystem;

namespace ToolkitEngine.XR
{
    // Animated hand visuals for a user of a Touch controller.
    public class XRHandAnimatorControl : MonoBehaviour
    {
        public const string ANIM_LAYER_NAME_POINT = "Point Layer";
        public const string ANIM_LAYER_NAME_THUMB = "Thumb Layer";
        public const string ANIM_PARAM_NAME_FLEX = "Flex";
        public const string ANIM_PARAM_NAME_PINCH = "Pinch";

        [SerializeField]
        private Animator m_animator = null;

        [SerializeField]
        private InputActionProperty m_selectValueAction;

        [SerializeField]
        private InputActionProperty m_activateValueAction;

        [SerializeField]
        private InputActionProperty m_thumbValueAction;

        private int m_animLayerIndexThumb = -1;
        private int m_animLayerIndexPoint = -1;
        private int m_animParamIndexFlex = -1;
        private int m_animParamIndexPinch = -1;

        private float m_selectValue;
        private float m_activateValue;
        private float m_thumbValue;

        private void Awake()
        {
            // Get animator layer indices by name, for later use switching between hand visuals
            m_animLayerIndexPoint = m_animator.GetLayerIndex(ANIM_LAYER_NAME_POINT);
            m_animLayerIndexThumb = m_animator.GetLayerIndex(ANIM_LAYER_NAME_THUMB);
            m_animParamIndexFlex = Animator.StringToHash(ANIM_PARAM_NAME_FLEX);
            m_animParamIndexPinch = Animator.StringToHash(ANIM_PARAM_NAME_PINCH);
        }

        private void OnEnable()
        {
            m_selectValueAction.EnableDirectAction();
            m_selectValueAction.action.performed += SelectValue_Performed;
            m_selectValueAction.action.canceled += SelectValue_Canceled;

            m_activateValueAction.EnableDirectAction();
            m_activateValueAction.action.performed += ActivateValue_Performed;
            m_activateValueAction.action.canceled += ActivateValue_Canceled;

            m_thumbValueAction.EnableDirectAction();
            m_thumbValueAction.action.performed += ThumbValue_Performed;
            m_thumbValueAction.action.canceled += ThumbValue_Canceled;
        }

        private void OnDisable()
        {
            m_selectValueAction.DisableDirectAction();
            m_selectValueAction.action.performed -= SelectValue_Performed;
            m_selectValueAction.action.canceled -= SelectValue_Canceled;

            m_activateValueAction.DisableDirectAction();
            m_activateValueAction.action.performed -= ActivateValue_Performed;
            m_activateValueAction.action.canceled -= ActivateValue_Canceled;
        }

        private void SelectValue_Performed(InputAction.CallbackContext obj)
        {
            m_selectValue = obj.ReadValue<float>();
            UpdateAnimStates();
        }

        private void SelectValue_Canceled(InputAction.CallbackContext obj)
        {
            m_selectValue = 0f;
            UpdateAnimStates();
        }

        private void ActivateValue_Performed(InputAction.CallbackContext obj)
        {
            m_activateValue = obj.ReadValue<float>();
            UpdateAnimStates();
        }

        private void ActivateValue_Canceled(InputAction.CallbackContext obj)
        {
            m_activateValue = 0f;
            UpdateAnimStates();
        }

        private void ThumbValue_Performed(InputAction.CallbackContext obj)
        {
            m_thumbValue = obj.ReadValue<float>();
            UpdateAnimStates();
        }

        private void ThumbValue_Canceled(InputAction.CallbackContext obj)
        {
            m_thumbValue = 0f;
            UpdateAnimStates();
        }

        private void UpdateAnimStates()
        {
            m_animator.SetFloat(m_animParamIndexFlex, m_selectValue);
            m_animator.SetFloat(m_animParamIndexPinch, m_activateValue);
            m_animator.SetLayerWeight(m_animLayerIndexPoint, m_selectValue * (1f - m_activateValue));
            m_animator.SetLayerWeight(m_animLayerIndexThumb, m_thumbValue);
        }
    }
}
