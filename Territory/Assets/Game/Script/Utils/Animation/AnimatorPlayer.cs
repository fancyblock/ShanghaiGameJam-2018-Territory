using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(SpriteRenderer))]
public class AnimatorPlayer : MonoBehaviour
{
    public SpriteRenderer m_sprite;
    public AnimSet m_animSet;

    private Dictionary<string, AnimClip> m_clipDic = new Dictionary<string, AnimClip>();

    private bool m_playing = false;
    private AnimClip m_curClip;
    private int m_curIndex;
    private float m_timer;


    void Awake()
    {
        foreach(AnimClip ac in m_animSet.m_clips)
            m_clipDic.Add(ac.m_clipName, ac);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(m_playing)
        {
            m_timer += Time.deltaTime;

            if(m_timer >= m_curClip.m_interval)
            {
                m_curIndex++;
                m_timer -= m_curClip.m_interval;

                if (m_curIndex >= m_curClip.m_frames.Count)
                {
                    m_playing = false;
                    return;
                }

                m_sprite.sprite = m_curClip.m_frames[m_curIndex];
            }
        }
	}

    public void Play(string clipName)
    {
        AnimClip clip = null;

        if(m_clipDic.TryGetValue(clipName, out clip))
        {
            m_curClip = clip;
            m_curIndex = 0;
            m_timer = 0;

            m_sprite.sprite = m_curClip.m_frames[0];

            m_playing = true;
        }
        else
        {
            Debug.Log("Can not found clip. " + clipName);
        }
    }
}
