//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Inputsystem/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""PlayerMovement"",
            ""id"": ""0be6a357-24dc-4e8b-b382-42e6b81903c0"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""730f18ad-5e04-410c-bbf5-9a43df47a15c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""34ae221a-5e4c-4501-a3bd-a4227a19f033"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""83972b49-47cf-475b-9248-a9304f5836b1"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9227dd87-c9db-42e6-a445-1a603ad8f3e7"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""49216a0e-32f2-4890-b847-a337821a6466"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a76a621c-9d65-4bb5-b554-db687951efd8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""PlayerCamera"",
            ""id"": ""15cde203-989a-4cac-a039-f5b5320c4903"",
            ""actions"": [
                {
                    ""name"": ""SwitchCameraAngle"",
                    ""type"": ""Button"",
                    ""id"": ""7c82e6e3-6bc5-4c8c-a0b1-427412ea4d3a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwitchCamera"",
                    ""type"": ""Button"",
                    ""id"": ""94481cee-fab5-4c01-ad2c-d41940116c85"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f80d290e-d243-4d63-b95f-1f9fabdc8f14"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchCameraAngle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9fa40eee-3efe-4433-a979-c172affa5baf"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerActions"",
            ""id"": ""edb09bca-2792-42a5-8010-3f1dc9339438"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""0df3d8f2-1495-4f5b-8de6-000c9eead346"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Tool1"",
                    ""type"": ""Button"",
                    ""id"": ""4e563221-3587-42e1-aa5b-23bde2338760"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Tool2"",
                    ""type"": ""Button"",
                    ""id"": ""6a4f0287-982b-44b2-af03-ee4d7c2ef312"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Tool3"",
                    ""type"": ""Button"",
                    ""id"": ""c677f466-e421-4e2f-8001-9f55bfc8abc7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Tool4"",
                    ""type"": ""Button"",
                    ""id"": ""281a08b0-bca6-4a72-a97c-2e3f8e9427ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Tool5"",
                    ""type"": ""Button"",
                    ""id"": ""3ded6811-05f7-4d1e-b468-7c9b92cb111b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""988bc3d6-4e79-4bb7-9d27-1e481122567e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""53ae44ab-1acf-40d0-b68b-7c7d639825fb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""Button"",
                    ""id"": ""8d817963-b3bd-4c0f-b226-70def4f69a36"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8dd78d71-4f78-4bdf-b204-681999f1ae4e"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6232c109-3d90-4c23-a7d4-e71fc950164c"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tool1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ad5cfa7-b367-4f5b-895c-d575880680f0"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tool2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1bebb379-5dbc-46fe-a240-bf9d42a72af0"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tool3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb399dfe-0fe5-49d8-a808-fc265e9b3400"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tool4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bc97c62e-09ab-4af3-8b72-0582a8ebb638"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tool5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""10e5fc0f-c402-4863-a6ca-88f1b45400cf"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""70987817-3167-43fe-8d1f-b812aca5031a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1a9e21e5-c659-4932-bc56-16404a3eaf6f"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerMovement
        m_PlayerMovement = asset.FindActionMap("PlayerMovement", throwIfNotFound: true);
        m_PlayerMovement_Movement = m_PlayerMovement.FindAction("Movement", throwIfNotFound: true);
        // PlayerCamera
        m_PlayerCamera = asset.FindActionMap("PlayerCamera", throwIfNotFound: true);
        m_PlayerCamera_SwitchCameraAngle = m_PlayerCamera.FindAction("SwitchCameraAngle", throwIfNotFound: true);
        m_PlayerCamera_SwitchCamera = m_PlayerCamera.FindAction("SwitchCamera", throwIfNotFound: true);
        // PlayerActions
        m_PlayerActions = asset.FindActionMap("PlayerActions", throwIfNotFound: true);
        m_PlayerActions_Jump = m_PlayerActions.FindAction("Jump", throwIfNotFound: true);
        m_PlayerActions_Tool1 = m_PlayerActions.FindAction("Tool1", throwIfNotFound: true);
        m_PlayerActions_Tool2 = m_PlayerActions.FindAction("Tool2", throwIfNotFound: true);
        m_PlayerActions_Tool3 = m_PlayerActions.FindAction("Tool3", throwIfNotFound: true);
        m_PlayerActions_Tool4 = m_PlayerActions.FindAction("Tool4", throwIfNotFound: true);
        m_PlayerActions_Tool5 = m_PlayerActions.FindAction("Tool5", throwIfNotFound: true);
        m_PlayerActions_Escape = m_PlayerActions.FindAction("Escape", throwIfNotFound: true);
        m_PlayerActions_LeftClick = m_PlayerActions.FindAction("LeftClick", throwIfNotFound: true);
        m_PlayerActions_RightClick = m_PlayerActions.FindAction("RightClick", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerMovement
    private readonly InputActionMap m_PlayerMovement;
    private List<IPlayerMovementActions> m_PlayerMovementActionsCallbackInterfaces = new List<IPlayerMovementActions>();
    private readonly InputAction m_PlayerMovement_Movement;
    public struct PlayerMovementActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerMovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerMovement_Movement;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerMovementActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
        }

        private void UnregisterCallbacks(IPlayerMovementActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
        }

        public void RemoveCallbacks(IPlayerMovementActions instance)
        {
            if (m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerMovementActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerMovementActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);

    // PlayerCamera
    private readonly InputActionMap m_PlayerCamera;
    private List<IPlayerCameraActions> m_PlayerCameraActionsCallbackInterfaces = new List<IPlayerCameraActions>();
    private readonly InputAction m_PlayerCamera_SwitchCameraAngle;
    private readonly InputAction m_PlayerCamera_SwitchCamera;
    public struct PlayerCameraActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerCameraActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @SwitchCameraAngle => m_Wrapper.m_PlayerCamera_SwitchCameraAngle;
        public InputAction @SwitchCamera => m_Wrapper.m_PlayerCamera_SwitchCamera;
        public InputActionMap Get() { return m_Wrapper.m_PlayerCamera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerCameraActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerCameraActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerCameraActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerCameraActionsCallbackInterfaces.Add(instance);
            @SwitchCameraAngle.started += instance.OnSwitchCameraAngle;
            @SwitchCameraAngle.performed += instance.OnSwitchCameraAngle;
            @SwitchCameraAngle.canceled += instance.OnSwitchCameraAngle;
            @SwitchCamera.started += instance.OnSwitchCamera;
            @SwitchCamera.performed += instance.OnSwitchCamera;
            @SwitchCamera.canceled += instance.OnSwitchCamera;
        }

        private void UnregisterCallbacks(IPlayerCameraActions instance)
        {
            @SwitchCameraAngle.started -= instance.OnSwitchCameraAngle;
            @SwitchCameraAngle.performed -= instance.OnSwitchCameraAngle;
            @SwitchCameraAngle.canceled -= instance.OnSwitchCameraAngle;
            @SwitchCamera.started -= instance.OnSwitchCamera;
            @SwitchCamera.performed -= instance.OnSwitchCamera;
            @SwitchCamera.canceled -= instance.OnSwitchCamera;
        }

        public void RemoveCallbacks(IPlayerCameraActions instance)
        {
            if (m_Wrapper.m_PlayerCameraActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerCameraActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerCameraActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerCameraActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerCameraActions @PlayerCamera => new PlayerCameraActions(this);

    // PlayerActions
    private readonly InputActionMap m_PlayerActions;
    private List<IPlayerActionsActions> m_PlayerActionsActionsCallbackInterfaces = new List<IPlayerActionsActions>();
    private readonly InputAction m_PlayerActions_Jump;
    private readonly InputAction m_PlayerActions_Tool1;
    private readonly InputAction m_PlayerActions_Tool2;
    private readonly InputAction m_PlayerActions_Tool3;
    private readonly InputAction m_PlayerActions_Tool4;
    private readonly InputAction m_PlayerActions_Tool5;
    private readonly InputAction m_PlayerActions_Escape;
    private readonly InputAction m_PlayerActions_LeftClick;
    private readonly InputAction m_PlayerActions_RightClick;
    public struct PlayerActionsActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActionsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_PlayerActions_Jump;
        public InputAction @Tool1 => m_Wrapper.m_PlayerActions_Tool1;
        public InputAction @Tool2 => m_Wrapper.m_PlayerActions_Tool2;
        public InputAction @Tool3 => m_Wrapper.m_PlayerActions_Tool3;
        public InputAction @Tool4 => m_Wrapper.m_PlayerActions_Tool4;
        public InputAction @Tool5 => m_Wrapper.m_PlayerActions_Tool5;
        public InputAction @Escape => m_Wrapper.m_PlayerActions_Escape;
        public InputAction @LeftClick => m_Wrapper.m_PlayerActions_LeftClick;
        public InputAction @RightClick => m_Wrapper.m_PlayerActions_RightClick;
        public InputActionMap Get() { return m_Wrapper.m_PlayerActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActionsActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActionsActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsActionsCallbackInterfaces.Add(instance);
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Tool1.started += instance.OnTool1;
            @Tool1.performed += instance.OnTool1;
            @Tool1.canceled += instance.OnTool1;
            @Tool2.started += instance.OnTool2;
            @Tool2.performed += instance.OnTool2;
            @Tool2.canceled += instance.OnTool2;
            @Tool3.started += instance.OnTool3;
            @Tool3.performed += instance.OnTool3;
            @Tool3.canceled += instance.OnTool3;
            @Tool4.started += instance.OnTool4;
            @Tool4.performed += instance.OnTool4;
            @Tool4.canceled += instance.OnTool4;
            @Tool5.started += instance.OnTool5;
            @Tool5.performed += instance.OnTool5;
            @Tool5.canceled += instance.OnTool5;
            @Escape.started += instance.OnEscape;
            @Escape.performed += instance.OnEscape;
            @Escape.canceled += instance.OnEscape;
            @LeftClick.started += instance.OnLeftClick;
            @LeftClick.performed += instance.OnLeftClick;
            @LeftClick.canceled += instance.OnLeftClick;
            @RightClick.started += instance.OnRightClick;
            @RightClick.performed += instance.OnRightClick;
            @RightClick.canceled += instance.OnRightClick;
        }

        private void UnregisterCallbacks(IPlayerActionsActions instance)
        {
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Tool1.started -= instance.OnTool1;
            @Tool1.performed -= instance.OnTool1;
            @Tool1.canceled -= instance.OnTool1;
            @Tool2.started -= instance.OnTool2;
            @Tool2.performed -= instance.OnTool2;
            @Tool2.canceled -= instance.OnTool2;
            @Tool3.started -= instance.OnTool3;
            @Tool3.performed -= instance.OnTool3;
            @Tool3.canceled -= instance.OnTool3;
            @Tool4.started -= instance.OnTool4;
            @Tool4.performed -= instance.OnTool4;
            @Tool4.canceled -= instance.OnTool4;
            @Tool5.started -= instance.OnTool5;
            @Tool5.performed -= instance.OnTool5;
            @Tool5.canceled -= instance.OnTool5;
            @Escape.started -= instance.OnEscape;
            @Escape.performed -= instance.OnEscape;
            @Escape.canceled -= instance.OnEscape;
            @LeftClick.started -= instance.OnLeftClick;
            @LeftClick.performed -= instance.OnLeftClick;
            @LeftClick.canceled -= instance.OnLeftClick;
            @RightClick.started -= instance.OnRightClick;
            @RightClick.performed -= instance.OnRightClick;
            @RightClick.canceled -= instance.OnRightClick;
        }

        public void RemoveCallbacks(IPlayerActionsActions instance)
        {
            if (m_Wrapper.m_PlayerActionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActionsActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActionsActions @PlayerActions => new PlayerActionsActions(this);
    public interface IPlayerMovementActions
    {
        void OnMovement(InputAction.CallbackContext context);
    }
    public interface IPlayerCameraActions
    {
        void OnSwitchCameraAngle(InputAction.CallbackContext context);
        void OnSwitchCamera(InputAction.CallbackContext context);
    }
    public interface IPlayerActionsActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnTool1(InputAction.CallbackContext context);
        void OnTool2(InputAction.CallbackContext context);
        void OnTool3(InputAction.CallbackContext context);
        void OnTool4(InputAction.CallbackContext context);
        void OnTool5(InputAction.CallbackContext context);
        void OnEscape(InputAction.CallbackContext context);
        void OnLeftClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
    }
}