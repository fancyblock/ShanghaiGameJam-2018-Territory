using UnityEngine;


[AddComponentMenu("Tween/Tween Material")]
public class TweenMaterial : UITweener
{
    private string m_propertyName;
    private float m_srcValue;
    private float m_destValue;

    protected Material mMat;

    public Material cachedMaterial { get { if (mMat == null) mMat = gameObject.GetComponent<SpriteRenderer>().material; return mMat; } }

    /// <summary>
    /// set the value 
    /// </summary>
    public float value
    {
        get
        {
            return cachedMaterial.GetFloat(m_propertyName);
        }
        set
        {
            cachedMaterial.SetFloat(m_propertyName, value);
        }
    }


    /// <summary>
    /// update 
    /// </summary>
    /// <param name="factor"></param>
    /// <param name="isFinished"></param>
    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = m_srcValue * (1f - factor) + m_destValue * factor;
    }


    /// <summary>
    /// begin the tween (cubic bezier curve)
    /// </summary>
    /// <param name="go"></param>
    /// <param name="duration"></param>
    /// <param name="targetPoint"></param>
    /// <returns></returns>
    static public TweenMaterial Begin(GameObject go, float duration, string propertyName, float destValue)
    {
        TweenMaterial comp = UITweener.Begin<TweenMaterial>(go, duration);

        comp.m_propertyName = propertyName;
        comp.m_srcValue = comp.value;
        comp.m_destValue = destValue;

        if (duration <= 0f)
        {
            comp.Sample(1f, true);
            comp.enabled = false;
        }

        return comp;
    }
}
