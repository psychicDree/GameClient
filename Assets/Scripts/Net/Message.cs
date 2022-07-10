using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;

public class Message : MonoBehaviour
{
	private byte[] data = new byte[1024];
	private int startIndex = 0;

	public byte[] Data
	{
		get { return data; }
	}

	public int StartIndex
	{
		get { return startIndex; }
	}

	public int RemainSize
	{
		get { return data.Length - startIndex; }
	}

	public void ReadMessage(int newDataAmount, Action<ActionCode, string> OnProcessDataCallback)
	{
		startIndex += newDataAmount;
		while (true)
		{
			if (startIndex <= 4) return;
			int count = BitConverter.ToInt32(data, 0);
			if (startIndex - 4 >= count)
			{
				ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 4);
				string str = Encoding.UTF8.GetString(data, 8, count - 4);
				OnProcessDataCallback(actionCode, str);
				Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
				startIndex -= count + 4;
			}
			else
				break;
		}
	}

	public static byte[] PackData(RequestCode requestCode,ActionCode actionCode, string data)
	{
		byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
		byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
		byte[] dataBytes = Encoding.UTF8.GetBytes(data);
		int newDataAmount = requestCodeBytes.Length + dataBytes.Length+actionCodeBytes.Length;
		byte[] newDataAmountBytes = BitConverter.GetBytes(newDataAmount);
		byte[] newBytes1 = newDataAmountBytes.Concat(requestCodeBytes).ToArray();
		byte[] newBytes2 = newBytes1.Concat(actionCodeBytes).ToArray();
		return newBytes2.Concat(dataBytes).ToArray();
	}
}