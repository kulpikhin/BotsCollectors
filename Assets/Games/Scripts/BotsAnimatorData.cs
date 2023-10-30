using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class BotsAnimatorData
{
    static public class Params
    {
        static public int IsMove = Animator.StringToHash(nameof(IsMove));
        static public int IsMining = Animator.StringToHash(nameof(IsMining));
    }
}
