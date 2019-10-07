using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


public struct vec3
{
    public float x, y, z;
};

public struct quat
{
    public float x, y, z, w;
};

public struct objectInfo
{
    public int id;
    public vec3 pos;
    public quat rot;
};

public class SaveLoadWrapper
{
    [DllImport("SaveLoadObjectsPlugin")]
    private static extern void saveToFile(string filename, int obCount, objectInfo[] obs);

    [DllImport("SaveLoadObjectsPlugin")]
    private static extern objectInfo[] loadFromFile(string filename, ref int obCount);

    public static void saveObjects(string filename, List<objectInfo> obs)
    {
        saveToFile(filename, obs.Count, obs.ToArray());
    }

    public static void loadObjects(string filename, ref List<objectInfo> obs)
    {
        obs.Clear();
        int count=0;
        objectInfo[] infoArr = loadFromFile(filename, ref count);
        for (int i = 0; i < count; ++i)
            obs.Add(infoArr[i]);
    }

    public static List<objectInfo> goToObjectInfo(List<GameObject> gos)
    {
        List<objectInfo> infolist = new List<objectInfo>();
        foreach (GameObject go in gos)
        {
            Placable p;
            if (go.TryGetComponent<Placable>(out p))
            {
                objectInfo info = new objectInfo();
                info.id = p.objectID;
                info.pos.x = go.transform.position.x;
                info.pos.y = go.transform.position.y;
                info.pos.z = go.transform.position.z;
                info.rot.x = go.transform.rotation.x;
                info.rot.y = go.transform.rotation.y;
                info.rot.z = go.transform.rotation.z;
                info.rot.w = go.transform.rotation.w;
                infolist.Add(info);
            }
        }
        return infolist;
    }
}
