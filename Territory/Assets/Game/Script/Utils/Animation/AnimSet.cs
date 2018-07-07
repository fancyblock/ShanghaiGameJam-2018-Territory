﻿using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "GameJam/Create AnimSet")]
public class AnimSet : ScriptableObject
{
    public List<AnimClip> m_clips;
}
