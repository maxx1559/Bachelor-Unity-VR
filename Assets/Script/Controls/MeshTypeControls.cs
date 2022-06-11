using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MeshTypeControls : MonoBehaviour
{
    DataBase db = DataBase.getInstance();

    ToggleGroup toggleGroup;
    public GameObject scale;

    private void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        
    }
    void Update()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();

        var gameObject = GameObject.Find("MeshType");

        if (!db.getTriangulationEnabled())
            gameObject.SetActive(false);

        if(Equals(toggle.name, "OceanFloorToggle"))
        {
            db.setShowHeightMap(false);
            db.setShowMesh(true);

            db.setUpdateHeightMap(true);
            db.setUpdateOceanFloor(true);

        } else if(Equals(toggle.name, "HeightmapToggle"))
        {
            db.setShowHeightMap(true);
            db.setShowMesh(false);

            db.setUpdateHeightMap(true);
            db.setUpdateOceanFloor(true);


        } else
        {
            db.setShowHeightMap(false);
            db.setShowMesh(false);

            db.setUpdateHeightMap(true);
            db.setUpdateOceanFloor(true);
        }

    }

}
