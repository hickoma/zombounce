﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Yandex AppMetrica interface
/// </summary>
public static class Analytics
{
	private static void TagEvent (string categoryName, string subCategoryName, object attributes)
	{
		Dictionary<string, object> analyticData = new Dictionary<string, object> { { subCategoryName, attributes }};

		YandexAppMetricaConfig? conf = AppMetrica.Instance.ActivationConfig;
		if (conf.HasValue) 
		{
			if(!string.IsNullOrEmpty(conf.Value.ApiKey))
				AppMetrica.Instance.ReportEvent (categoryName,  analyticData);
		}
	}

	public static void TagError (string name, string stackTrace)
	{
		YandexAppMetricaConfig? conf = AppMetrica.Instance.ActivationConfig;
		if (conf.HasValue) 
		{
			if(!string.IsNullOrEmpty(conf.Value.ApiKey))
				AppMetrica.Instance.ReportError (name, stackTrace);
		}
	}

	public static void SendEventAnalytic(string eventName, object eventData)
	{
		TagEvent("general_events", eventName, eventData);
	}
}
