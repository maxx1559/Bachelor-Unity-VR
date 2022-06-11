using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SetScale : MonoBehaviour
{
    DataBase db = DataBase.getInstance();

    public TMP_Text textShallow;
    public TMP_Text textHalf;
    public TMP_Text textDeep;
    public GameObject scale;
    public Toggle toggleDisplay;
    public Toggle toggleHeightMap;
    public Toggle toggleGradient;

    void Start()
    {
        // Checking if there is 0 or 1 points in point cloud after filtering, else null pointer errors
        if (db.getNewShallowDepth() == int.MinValue + 1 && db.getNewDeepDepth() == int.MaxValue - 1)
        {
            textShallow.text = "Null";
            textDeep.text = "Null";
            textHalf.text = "Null";
        }
        else
        {
            if (db.getNewShallowDepth() == int.MinValue + 1)
            {
                db.setNewShallowDepth(db.getNewDeepDepth());

            }
            else if (db.getNewDeepDepth() == int.MaxValue - 1)
            {
                db.setNewDeepDepth(db.getNewShallowDepth());
            }
            textShallow.text = Math.Abs((int)db.getNewShallowDepth()).ToString();
            textDeep.text = Math.Abs((int)db.getNewDeepDepth()).ToString();
            textHalf.text = (Math.Abs((int)db.getNewShallowDepth() + db.getNewDeepDepth()) / 2).ToString();
        }

    }

}
