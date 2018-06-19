using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ResizeToTexture : MonoBehaviour
{
    Transform myTransform;
    float textureAspectRatio = -1f;

    Vector2 size = Vector2.zero;

    public bool useZAxis;

    void Start()
    {
        myTransform = transform;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Texture texture = meshRenderer.sharedMaterial.mainTexture;

        if (!texture)
        {
            return;
        }

        textureAspectRatio = (float)texture.width / texture.height;

        ScaleWidth();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (!myTransform)
        {
            myTransform = transform;
        }

        if (textureAspectRatio <= 0f)
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            Texture texture = meshRenderer.sharedMaterial.mainTexture;
            textureAspectRatio = (float)texture.width / texture.height;
        }

        float scaleY = useZAxis ? myTransform.localScale.z : myTransform.localScale.y;

        if (scaleY != size.y)
        {
            ScaleWidth();
        }
        else if (myTransform.localScale.x != size.x)
        {
            ScaleHeight();
        }
    }
#endif

    void ScaleWidth()
    {

        Vector3 scale = myTransform.localScale;
        float scaleY = useZAxis ? scale.z : scale.y;

        scale.x = scaleY * textureAspectRatio;

        myTransform.localScale = scale;

        size = new Vector2(myTransform.localScale.x, useZAxis ? myTransform.localScale.z : myTransform.localScale.y);
    }

    void ScaleHeight()
    {
        Vector3 scale = myTransform.localScale;

        if (useZAxis)
        {
            scale.z = scale.x / textureAspectRatio;
        }
        else
        {
            scale.y = scale.x / textureAspectRatio;
        }

        myTransform.localScale = scale;

        size = new Vector3(myTransform.localScale.x, useZAxis ? myTransform.localScale.z : myTransform.localScale.y);
    }
}
