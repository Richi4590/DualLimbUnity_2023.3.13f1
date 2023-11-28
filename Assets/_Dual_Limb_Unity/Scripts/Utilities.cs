using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Utilities : MonoBehaviour
{

    public static async System.Threading.Tasks.Task WaitForSecondsAsync(float seconds)
    {
        // Task.Delay is a non-blocking delay that does not freeze the main thread
        await System.Threading.Tasks.Task.Delay(Mathf.CeilToInt(seconds * 1000)); // Convert seconds to milliseconds
    }

    public static List<GameObject> getListOfGameObjects(Transform root)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        getAllChildren(root.transform, ref gameObjects);

        return gameObjects;
    }

    public static List<GameObject> getListOfPlainGameObjects(Transform root)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        getAllPlainChildren(root.transform, ref gameObjects);

        return gameObjects;
    }

    public static List<GameObject> getListOfGameObjectsWithTag(Transform root, string tag)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        getAllChildrenWithTag(root.transform, ref gameObjects, tag);

        return gameObjects;
    }

    public static GameObject getGameObjectWithName(Transform root, string name)
    {
        GameObject obj = null;

        getChildWithName(root.transform, ref obj, name, false);

        return obj;
    }

    public static T getComponentOfGameObjectWithName<T>(Transform root, string name)
    {
        GameObject obj = null;
        T component = default(T);

        getChildWithName(root.transform, ref obj, name, false);

        if (obj != null)
            component = obj.GetComponent<T>();

        return component;
    }

    public static List<GameObject> getListOfGameObjectsWithLayerName(Transform root, string layer)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        getAllChildrenWithLayerName(root.transform, ref gameObjects, layer);

        return gameObjects;
    }

    public static List<T> getListOfComponentsRecursiveGeneric<T>(Transform root)
    {
        List<T> result = new List<T>();

        getAllChildrenGeneric(root.transform, ref result);

        return result;
    }

    public static GameObject getFirstGameObjectWithComponent<R>(Transform root)
    {
        List<GameObject> result = new List<GameObject>();

        getAllChildrenGenericWithComponentType<R>(root.transform, ref result);

        return result[0];
    }

    public static List<GameObject> getListOfGameObjectsFirstLayer(Transform root)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        getAllChildrenFirstLayer(root.transform, ref gameObjects);

        return gameObjects;
    }

    public static List<GameObject> getListOfGameObjectWithComponentRecursiveGeneric<R>(Transform root)
    {
        List<GameObject> result = new List<GameObject>();

        getAllChildrenGenericWithComponentType<R>(root.transform, ref result);

        return result;
    }

    // returns a List of Lists of Gameobjects
    public static List<GameObject> traverseCurrentGameObjectHierarchy(Transform root)
    {
        List<GameObject> result = new List<GameObject>();
        getAllChildren(root.transform, ref result);
        return result;
    }

    public static void getAllPlainChildren(Transform parent, ref List<GameObject> ARVRGameObjects)
    {
        foreach (Transform t in parent)
        {
            if (t.GetComponent<Renderer>() != null)
            {
                if (!t.gameObject.CompareTag("TextAR"))
                    if (t.GetComponent<Light>() == null)
                        ARVRGameObjects.Add(t.gameObject);
            }

            getAllPlainChildren(t, ref ARVRGameObjects);
        }
    }

    public static void getAllChildren(Transform parent, ref List<GameObject> list)
    {
        foreach (Transform t in parent)
        {
            list.Add(t.gameObject);
            getAllChildren(t, ref list);
        }
    }

    public static void getAllChildrenFirstLayer(Transform parent, ref List<GameObject> list)
    {
        foreach (Transform t in parent)
            list.Add(t.gameObject);

    }

    public static void getAllChildrenWithTag(Transform parent, ref List<GameObject> list, string tag)
    {
        foreach (Transform t in parent)
        {
            if (t.gameObject.tag == tag)
                list.Add(t.gameObject);

            getAllChildrenWithTag(t, ref list, tag);
        }
    }

    public static void getAllChildrenWithLayerName(Transform parent, ref List<GameObject> list, string layer)
    {
        foreach (Transform t in parent)
        {
            if (t.gameObject.layer == LayerMask.NameToLayer(layer))
                list.Add(t.gameObject);

            getAllChildrenWithLayerName(t, ref list, layer);
        }
    }

    public static void getChildWithName(Transform parent, ref GameObject g, string name, bool found)
    {
        foreach (Transform t in parent)
        {

            if (found)
                break;

            if (t.gameObject.name == name)
            {
                g = t.gameObject;
                found = true;
            }

            getChildWithName(t, ref g, name, found);
        }
    }


    public static void getAllChildrenGeneric<T>(Transform parent, ref List<T> list)
    {
        foreach (Transform t in parent)
        {
            foreach (Component c in t.GetComponents(typeof(T)))
                list.Add(t.gameObject.ConvertTo<T>());

            getAllChildrenGeneric(t, ref list);
        }
    }

    public static void getAllChildrenGenericWithComponentType<R>(Transform parent, ref List<GameObject> list)
    {
        foreach (Transform t in parent)
        {
            foreach (Component c in t.GetComponents(typeof(R)))
                list.Add(t.gameObject);



            getAllChildrenGenericWithComponentType<R>(t, ref list);
        }
    }

}
