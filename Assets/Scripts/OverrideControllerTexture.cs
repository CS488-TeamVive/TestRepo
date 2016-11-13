using UnityEngine;
using System.Runtime.InteropServices;
using VRTK;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Override the texture of the Vive controllers, with your own texture, after SteamVR has loaded and applied the original texture.
/// </summary>
public class OverrideControllerTexture : SteamVR_RenderModel
{
    #region Instance variables
    public List<Texture2D> skinList;
    enum MenuOption { Default = 0, Mag_Selected = 1, Calendar_Selected = 2, Sun_Selected = 3, Building_Selected = 4};
    int currentMenu = (int)MenuOption.Default;
    #endregion

    void OnEnable()
    {
        EventHandlerLeftController.OnTouchpadPress += TouchPadPress;
    }

    void OnDisable()
    {
        EventHandlerLeftController.OnTouchpadPress -= TouchPadPress;
    }

    protected new void Update()
    {
        base.Update();
        //Subscribe to the event that is called by SteamVR_RenderModel, when the controller mesh + texture, has been loaded completely.
        SteamVR_Utils.Event.Listen("render_model_loaded", OnControllerLoaded);
    }

    /// <summary>
    /// Override the texture of each of the parts, with your texture.
    /// </summary>
    /// <param name="newTexture">Override texture</param>
    /// <param name="modelTransform">Transform of the gameobject, which has the SteamVR_RenderModel component.</param>
    public void UpdateControllerTexture(Texture2D newTexture, Transform modelTransform)
    {
        modelTransform.Find("body").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("button").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("led").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("lgrip").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("rgrip").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("scroll_wheel").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("sys_button").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("trackpad").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("trackpad_scroll_cut").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("trackpad_touch").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        modelTransform.Find("trigger").GetComponent<MeshRenderer>().material.mainTexture = newTexture;
    }

    /// <summary>
    /// Call this method, when the "render_model_loaded".
    /// </summary>
    /// <param name="args">bool success, SteamVR_RenderModel model</param>
    void OnControllerLoaded(params object[] args)
    {
        if (args[0].Equals(this))
        {
            UpdateControllerTexture(skinList.ElementAt(currentMenu), this.gameObject.transform);
        }
    }

    private void TouchPadPress(object sender, ControllerInteractionEventArgs e)
    {
        UpdateMenu(e);
        UpdateControllerTexture(skinList.ElementAt(currentMenu), this.gameObject.transform);
    }

    private void UpdateMenu(ControllerInteractionEventArgs e)
    {
        int menuOptionSelected = (int)GetMenuOptionSelected(e.touchpadAngle);

        if (currentMenu == menuOptionSelected)
        {
            currentMenu = (int)MenuOption.Default;
        }
        else
        {
            currentMenu = menuOptionSelected;
        }    
    }

    private MenuOption GetMenuOptionSelected(float angle)
    {
        if (angle >= 45 && angle < 135)
        {
            return MenuOption.Building_Selected;
        }
        else if (angle >= 135 && angle < 225)
        {
            return MenuOption.Mag_Selected;
        }
        else if (angle >= 225 && angle < 315)
        {
            return MenuOption.Sun_Selected;
        }
        else
        {
            return MenuOption.Calendar_Selected;
        }
    }
}