using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineEffect : MonoBehaviour {
    private Camera mainCamera;
    private Camera secondaryCamera;

    private void Awake() {
        mainCamera = GetComponent<Camera>();
    }

    private void Start() {
        secondaryCamera = transform.Find("Secondary Camera").GetComponent<Camera>();
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        print("Got here");
        secondaryCamera.CopyFrom(mainCamera);
        secondaryCamera.clearFlags = CameraClearFlags.Color;
        secondaryCamera.backgroundColor = Color.black;
        secondaryCamera.cullingMask = 1 << LayerMask.NameToLayer("Interactable");
        
        //make the temporary rendertexture
        RenderTexture TempRT = new RenderTexture(src.width, src.height, 0, RenderTextureFormat.R8);
 
        //put it to video memory
        TempRT.Create();
 
        //set the camera's target texture when rendering
        secondaryCamera.targetTexture = TempRT;
 
        //render all objects this camera can render, but with our custom shader.
        secondaryCamera.Render();
 
        //copy the temporary RT to the final image
        Graphics.Blit(TempRT, dest);
 
        //release the temporary RT
        TempRT.Release();
    }
}
