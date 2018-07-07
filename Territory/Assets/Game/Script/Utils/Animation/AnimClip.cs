using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "GameJam/Create AnimClip")]
public class AnimClip : ScriptableObject
{
    public string m_clipName;
    public float m_interval;

    public List<Sprite> m_frames;
}
