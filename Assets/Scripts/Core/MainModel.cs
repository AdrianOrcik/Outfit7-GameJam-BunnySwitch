using System.Collections.Generic;

public static class MainModel
{
    public static List<MainBehaviour> CommonBehaviours = new List<MainBehaviour>();
    public static GameManager GameManager;

    public static T GetAssignedClass<T>() where T : MainBehaviour
    {
        foreach (object o in CommonBehaviours)
        {
            if (o is T)
            {
                return (T) o;
            }
        }

        return null;
    }
}