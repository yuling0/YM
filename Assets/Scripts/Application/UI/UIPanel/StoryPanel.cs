using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryPanel : BasePanel
{
    public Image img_BlackBorder_1;
    public Image img_BlackBorder_2;

    public override void OnInit()
    {
        base.OnInit();
		img_BlackBorder_1 = GetControl<Image>("img_BlackBorder_1");
		img_BlackBorder_2 = GetControl<Image>("img_BlackBorder_2");

    }
}