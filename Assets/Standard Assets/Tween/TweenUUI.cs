using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// Nikolayku: попытка заюзать iTween с unity UI
/// </summary>

public static class TweenUUI 
{
	/// <summary>
	/// Код логики твина
	/// </summary>
	public class TweenOpacityUUI : Tween.Base 
	{
		protected float start = 0, end = 1;
		protected bool startUsed = false, endUsed = false;

		public TweenOpacityUUI(TweenUUI_Base target, float time, float delay) 
		{
			this.target = target;
			this.ease = Tween.Ease.linear;
			this.time = time;
			this.runningTime = -delay;
		}

		public TweenOpacityUUI From(float v) 
		{
			start = v;
			startUsed = true;
			return this;
		}
		
		public TweenOpacityUUI To(float v) 
		{
			end = v;
			endUsed = true;
			return this;
		}

		protected override void Init() 
		{
			TweenUUI_Base s = (TweenUUI_Base)target;
			if (!startUsed)
				start = s.GetValue();
			if (!endUsed)
				end = s.GetValue();

			Tween.Stop<TweenOpacityUUI>(this, target);
		}

		protected override void Apply() 
		{
			unchecked 
			{
				TweenUUI_Base s = (TweenUUI_Base)target;
				s.SetValue(start + (end - start) * easevalue);
			}
		}
	}

	/// <summary>
	/// Базовый интерфейс для управляемых объектов
	/// </summary>
	public interface TweenUUI_Base
	{
		float GetValue ();
		void SetValue(float newValue);

	}

	// ====== Классы-контролы для распостраненных ui компонентов

	/// <summary>
	/// Unity UI Image
	/// </summary>
	public class TweenUUI_Image: TweenUUI_Base
	{
		Image m_component;

		public TweenUUI_Image(Image img){m_component = img;}

		public float GetValue (){return m_component.color.a;}
		public void SetValue(float newValue)
		{
			Color c = m_component.color;
			c.a = newValue;
			m_component.color = c;
		}
	}

	/// <summary>
	/// Unity Sprite Renderer
	/// </summary>
	public class TweenUUI_SpriteRenderer: TweenUUI_Base
	{
		SpriteRenderer m_component;

		public TweenUUI_SpriteRenderer(SpriteRenderer img){m_component = img;}

		public float GetValue (){return m_component.color.a;}
		public void SetValue(float newValue)
		{
			Color c = m_component.color;
			c.a = newValue;
			m_component.color = c;
		}
	}
		
	/// <summary>
	/// Unity UI Text
	/// </summary>
	public class TweenUUI_Text: TweenUUI_Base
	{
		Text m_component;

		public TweenUUI_Text(Text txt){m_component = txt;}

		public float GetValue (){return m_component.color.a;}
		public void SetValue(float newValue)
		{
			Color c = m_component.color;
			c.a = newValue;
			m_component.color = c;
		}

	}

    /// <summary>
    /// LayoutElement только minHeight
    /// </summary>
    public class TweenUUI_LayoutElement : TweenUUI_Base
    {
        LayoutElement m_value;

        public TweenUUI_LayoutElement(LayoutElement val) { m_value = val; }

        public float GetValue() { return m_value.minHeight; }
        public void SetValue(float newValue)
        {
            m_value.minHeight = newValue;
        }

    }

    // ====== Статические методы для быстрого создания

    public static TweenOpacityUUI ImageOpacity(this Image widget, float time = 1f, float delay = 0f) 
	{
		return new TweenOpacityUUI(new TweenUUI_Image(widget), time, delay);
	}

	public static TweenOpacityUUI SpriteRendererOpacity(this SpriteRenderer widget, float time = 1f, float delay = 0f) 
	{
		return new TweenOpacityUUI(new TweenUUI_SpriteRenderer(widget), time, delay);
	}

	public static TweenOpacityUUI TextOpacity(this Text widget, float time = 1f, float delay = 0f) 
	{
		return new TweenOpacityUUI(new TweenUUI_Text(widget), time, delay);
	}

    public static TweenOpacityUUI LayoutElement(this LayoutElement widget, float time = 1f, float delay = 0f)
    {
        return new TweenOpacityUUI(new TweenUUI_LayoutElement(widget), time, delay);
    }

}
