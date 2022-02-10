using UnityEngine;

[System.Serializable]

public class SaveBlock
{
    public int x;
    public int y;
    public int z;
    public string blocktype;

    public SaveBlock(int x1, int y1, int z1, string name)
    {
        this.x = x1;
        this.y = y1;
        this.z = z1;
        this.blocktype = name;
    }

    public static SaveBlock CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<SaveBlock>(jsonString);
    }
}
