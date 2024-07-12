using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationSelector : MonoBehaviour
{
    
    public enum SelectedAnimation
    {
        HeartAn,
        HeartAn1,
        HeartAn2,
        HeartAn3,
        
        CubeAn,
        CubeAn1,
        CubeAn2
    }

    private Animator _mainAnimator;
    public SelectedAnimation selectedAnimation;

    private static readonly int AnimationSelectorNumber = Animator.StringToHash("AnimationSelectorNumber");

    // Start is called before the first frame update
    void Start()
    {
        //Gets the animator from either itself or its children
        _mainAnimator = GetComponentInChildren<Animator>();
        if (_mainAnimator == null) { _mainAnimator = GetComponent<Animator>(); }
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if animation state needs updating
        if ((int)selectedAnimation == _mainAnimator.GetInteger(AnimationSelectorNumber)) { return; }
        //Sets the animation based on the Enum's index
        _mainAnimator.SetInteger(AnimationSelectorNumber, (int)selectedAnimation);
    }
}
