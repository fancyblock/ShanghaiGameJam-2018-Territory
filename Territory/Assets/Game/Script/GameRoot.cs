using strange.extensions.context.impl;
using UnityEngine;


public class GameRoot : ContextView
{
    static protected GameRoot m_instance;

    static public GameRoot Instance
    {
        get { return m_instance; }
    }


    void Awake()
    {
        m_instance = this;

        // 限帧30
        Application.targetFrameRate = 30;

        context = new GameContext(this);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        FrameSignal fs = (context as GameContext).injectionBinder.GetInstance<FrameSignal>();
        fs.Dispatch();
    }
}
