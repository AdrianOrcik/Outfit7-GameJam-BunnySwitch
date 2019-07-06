using UnityEngine;

public class MainBehaviour : MonoBehaviour
{
    //assigned manually, only one instance per entire game
    public GameManager GameManager => MainModel.GameManager;
    public ResourceManager ResourceManager => MainModel.ResourceManager;
    
    //instance per game
    public InputManager InputManager => GetAssignedClass<InputManager>();

    //instance per scene
    public EventManager EventManager => GetAssignedClass<EventManager>();
    public CameraManager CameraManager => GetAssignedClass<CameraManager>();
    public LayerManager LayerManager => GetAssignedClass<LayerManager>();
    public ScreenManager ScreenManager => GetAssignedClass<ScreenManager>();

    void Awake()
    {
        AssignClass(this);
    }

    /// <summary>
    /// Method assigned class into List of references in MainModel for accessing
    /// </summary>
    private T GetAssignedClass<T>() where T : MainBehaviour
    {
        return MainModel.GetAssignedClass<T>();
    }

    /// <summary>
    ///  Method Assigned class manualy for static reference 
    /// </summary>
    public void AssignClass(MainBehaviour common)
    {
        MainModel.CommonBehaviours.Add(common);
    }

    private void OnDestroy()
    {
        MainModel.CommonBehaviours.Remove(this);
    }
}