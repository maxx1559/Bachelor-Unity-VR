using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BoatPathRenderer : MonoBehaviour
{
    DataBase db = DataBase.getInstance();

    Texture2D texColor;
    Texture2D texPosScale;
    VisualEffect vfx;
    uint resolution = 100;
    public Gradient gradient;

    float particleSize = 0.1f;
    bool toUpdate = false;
    uint particleCount = 0;

    List<Vector3> positions;
    List<Vector3> renderPoints;
    Color[] colors;

    private void Start()
    {
        this.gameObject.GetComponent<Renderer>().enabled = false;

        vfx = GetComponent<VisualEffect>();

        positions = new List<Vector3>(db.getBoatPathPoints().ToArray());
        int N = positions.Count;
        renderPoints = new List<Vector3>();

        for (int i = 0; i < N - 1; i++)
        {
            // Calculating boat path, translate points left and right to create a path inbetween them
            Vector3 pCurr = positions[i];
            Vector3 pNext = positions[i + 1];

            if (i == N - 2)
            {
                pCurr = positions[1];
                pNext = positions[0];
            }

            Vector3 vecPoint = pCurr - pNext;
            float vecPointLength = Vector3.Magnitude(vecPoint);

            Vector3 vecHor = pCurr - new Vector3(pCurr[0], pCurr[1], pCurr[2] + 1.0f);

            float theta = Vector3.Angle(vecPoint, vecHor) * Mathf.PI / 180.0f;

            float vecDesiredLength = Mathf.Sqrt(Mathf.Pow(vecPointLength, 2) + 1);

            float beta = Mathf.Acos(vecPointLength / vecDesiredLength);

            float alpha = theta - beta;
            float alpha2 = theta + beta;

            float rightDesiredZComp = pCurr[2] + Mathf.Cos(alpha) * vecDesiredLength;
            float rightDesiredXComp = pCurr[0] + Mathf.Sin(alpha) * vecDesiredLength;

            float leftDesiredZComp = pCurr[2] + Mathf.Cos(alpha2) * vecDesiredLength;
            float leftDesiredXComp = pCurr[0] + Mathf.Sin(alpha2) * vecDesiredLength;

            Vector3 boatPointRight = new Vector3(rightDesiredXComp, 0, rightDesiredZComp);
            Vector3 boatPointLeft = new Vector3(leftDesiredXComp, 0, leftDesiredZComp);

            // Adding the calculated points to the boat path list
            renderPoints.Add(boatPointLeft);
            renderPoints.Add(boatPointRight);

        }

        colors = new Color[2 * N];

        for (int x = 0; x < 2 * N; x++)
        {
            colors[x] = new Color(0.8f, 0.8f, 0.8f);
        }
        
        SetParticles(renderPoints.ToArray(), colors);
    }

    private void Update()
    {
        if (toUpdate)
        {
            toUpdate = false;

            vfx.Reinit();
            vfx.SetUInt(Shader.PropertyToID("ParticleCount"), particleCount);
            vfx.SetTexture(Shader.PropertyToID("TexColor"), texColor);
            vfx.SetTexture(Shader.PropertyToID("TexPosScale"), texPosScale);
            vfx.SetUInt(Shader.PropertyToID("Resolution"), resolution);
        }
        if (db.getUpdateBoatPath() && db.getShowBoatPathPoints())
        {
            this.gameObject.GetComponent<Renderer>().enabled = true;
            db.setUpdateBoatPath(false);
        }
        else if (db.getUpdateBoatPath() && !db.getShowBoatPathPoints())
        {
            this.gameObject.GetComponent<Renderer>().enabled = false;
            db.setUpdateBoatPath(false);
        }

    }

    public void SetParticles(Vector3[] positions, Color[] colors)
    {
        texColor = new Texture2D(positions.Length > (int)resolution ? (int)resolution : positions.Length,
                       Mathf.Clamp(positions.Length / (int)resolution,
                       1,
                       (int)resolution),
                       TextureFormat.RGBAFloat, false);

        texPosScale = new Texture2D(positions.Length > (int)resolution ? (int)resolution : positions.Length,
                          Mathf.Clamp(positions.Length / (int)resolution,
                          1,
                          (int)resolution),
                          TextureFormat.RGBAFloat, false);

        int texWidth = texColor.width;
        int texHeight = texColor.height;

        for (int y = 0; y < texHeight; y++)
        {
            for (int x = 0; x < texWidth; x++)
            {
                int index = x + y * texWidth;
                texColor.SetPixel(x, y, colors[index]);
                var data = new Color(positions[index].x, positions[index].y, positions[index].z, particleSize);
                texPosScale.SetPixel(x, y, data);
            }

        }

        texColor.Apply();
        texPosScale.Apply();
        particleCount = (uint)positions.Length;
        toUpdate = true;
    }

}
