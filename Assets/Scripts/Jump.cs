using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public Animator jumpAnim;

    public void JumpAnim()
    {
    
        jumpAnim.SetTrigger("jump");

    }

    public void LandAnim()
    {
    
        jumpAnim.SetTrigger("land");

    }
}
