using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utilities : Singleton<Utilities>
{
    public List<string> SortStringList(List<string> stringList)
    {
        return stringList.OrderByDescending(s => GetNumberFromString(s)).ToList();
    }

    public int GetNumberFromString(string str)
    {
        string numberString = new string(str.Where(char.IsDigit).ToArray());

        int number;
        int.TryParse(numberString, out number);

        return number;
    }
}