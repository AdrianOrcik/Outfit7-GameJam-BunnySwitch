
using UnityEngine;

public class MainBehaviour : MonoBehaviour
{
    //assigned manually, only one instance per entire game
    public GameManager GameManager => MainModel.GameManager;


    //instance per scene
    public EventManager EventManager => GetAssignedClass<EventManager>();
    public CameraBehaviour CameraManager => GetAssignedClass<CameraBehaviour>();


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