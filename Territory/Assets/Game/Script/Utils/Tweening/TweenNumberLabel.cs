//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tween the object's alpha.
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween NumberLabel")]
public class TweenNumberLabel : UITweener
{
	public int from = 0;
    public int to = 1;
    public string formater = "{0:D}";
    public string str_format = "";

	Text mLabel;
    int mNumber = 1;

	/// <summary>
	/// Current alpha.
	/// </summary>

	public int number
	{
		get
		{
            return mNumber;
		}
		set
		{
            mNumber = value;
            if (mLabel != null)
            {
                mLabel.text = string.Format(formater, mNumber.ToString(str_format));
            }
		}
	}

	/// <summary>
	/// Find all needed components.
	/// </summary>

	void Awake ()
	{
        mLabel = GetComponentInChildren<Text>();
        if (mLabel != null)
        {
            string str = mLabel.text;
            int num;
            if (int.TryParse(str.Replace(",", ""), out num))
            {
                from = num;
            }
            else
            {
                from = 0;
            }
            mNumber = from;
        }
	}

	/// <summary>
	/// Interpolate and update the alpha.
	/// </summary>

    override protected void OnUpdate(float factor, bool isFinished)
    {
        number = Mathf.RoundToInt(Mathf.Lerp(from, to, factor));
    }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

    static public TweenNumberLabel Begin(GameObject go, float duration, int number)
	{
        TweenNumberLabel comp = UITweener.Begin<TweenNumberLabel>(go, duration);
		comp.from = comp.number;
        comp.to = number;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}
