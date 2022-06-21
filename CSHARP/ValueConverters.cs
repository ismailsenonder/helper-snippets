using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Xml.Serialization;
using System.Reflection;

//Converts object to decimal
public static decimal? ToDecimal(this object value)
{
    decimal retVal = 0;
    if (decimal.TryParse(value.ToString()
    .Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
    .Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator),
    NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out retVal))
        return retVal;
    else
        return null;
}

//Converts List<T> to DataTable
public static DataTable ToDataTable<T>(this List<T> data)
{
    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
    DataTable table = new DataTable();
    foreach (PropertyDescriptor prop in properties)
        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
    foreach (T item in data)
    {
        DataRow row = table.NewRow();
        foreach (PropertyDescriptor prop in properties)
            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        table.Rows.Add(row);
    }
    return table;
}

//Converts double value to Turkish currency string
public static string ToTurkishLiraString(this double amount)
{
    string strAmount = amount.ToString("F2").Replace('.', ',');
    string lira = strAmount.Substring(0, strAmount.IndexOf(','));
    string kurus = strAmount.Substring(strAmount.IndexOf(',') + 1, 2);
    string retVal = "";

    string[] units = { "", "BİR", "İKİ", "ÜÇ", "DÖRT", "BEŞ", "ALTI", "YEDİ", "SEKİZ", "DOKUZ" };
    string[] tens = { "", "ON", "YİRMİ", "OTUZ", "KIRK", "ELLİ", "ALTMIŞ", "YETMİŞ", "SEKSEN", "DOKSAN" };
    string[] thousands = { "MİLYAR", "MİLYON", "BİN", "" }; //Add more at the beginning of this array if needed.


    //This number indicates the count of every 3 digit groups in the amount
    //We have up to Billions (Milyar in Turkish) in thousands array so it will be; 1,000,000,000.00 = 4
    //if you add more to thousands array, you have to raise this count.
    int threeDigitsGroupCount = 4;

    lira = lira.PadLeft(threeDigitsGroupCount * 3, '0');

    string groupValue;

    for (int i = 0; i < threeDigitsGroupCount * 3; i += 3)
    {
        groupValue = "";

        if (lira.Substring(i, 1) != "0")
            groupValue += units[Convert.ToInt32(lira.Substring(i, 1))] + "YÜZ";

        if (groupValue == "BİRYÜZ")
            groupValue = "YÜZ";

        groupValue += tens[Convert.ToInt32(lira.Substring(i + 1, 1))];

        groupValue += units[Convert.ToInt32(lira.Substring(i + 2, 1))];

        if (groupValue != "")
            groupValue += thousands[i / 3];

        if (groupValue == "BİRBİN")
            groupValue = "BİN";

        retVal += groupValue;
    }

    if (retVal != "")
        retVal += " LİRA ";

    int stringLength = retVal.Length;

    if (kurus.Substring(0, 1) != "0")
        retVal += tens[Convert.ToInt32(kurus.Substring(0, 1))];

    if (kurus.Substring(1, 1) != "0")
        retVal += units[Convert.ToInt32(kurus.Substring(1, 1))];

    if (retVal.Length > stringLength)
        retVal += " Kuruş";
    //else
    //    retVal += "SIFIR Kuruş";

    return retVal;
}

//Converts a generic object T or a List<T> of objects to XML formatted string
public static string ToXML(this object instanceToConvert)
{
    using (var stringwriter = new System.IO.StringWriter())
    {
        var serializer = new XmlSerializer(instanceToConvert.GetType());
        serializer.Serialize(stringwriter, instanceToConvert);
        return stringwriter.ToString();
    }
}

//Converts an XML formatted string to a generic object.
public static T XmlStringToObject<T>(string xmlText)
{
    using (var stringReader = new System.IO.StringReader(xmlText))
    {
        var serializer = new XmlSerializer(typeof(T));
        return (T)serializer.Deserialize(stringReader);
    }
}

//Converts the type of variable
//If the value is null or can't be converted to the specified type, returns default value
public static T ConvertOrReturnDefault<T>(this object value, T DefaultValue)
{
    T returnValue = default(T);
    try
    {
        if (value != null)
        {
            //TimeSpan is not IConvertible, so:
            if (value.GetType() == typeof(TimeSpan) && typeof(T) == typeof(String))
            {
                TimeSpan ts = TimeSpan.Parse(value.ToString());
                DateTime d = new DateTime(ts.Ticks);
                returnValue = (T)Convert.ChangeType(d.ToString("HH:mm:ss"), typeof(T));
            }
            else
            {
                returnValue = (T)value;
            }
        }
        else
        {
            returnValue = DefaultValue;
        }
    }
    catch
    {
        try
        {
            returnValue = (T)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            return returnValue;
        }
    }

    return returnValue;
}

//Converts a DataRow to a generic object T
//Object property names and DataRow column names must be identical.
//HOW TO USE:
//MyObject myobject = new MyObject();
//DataRow r = table.Rows[0];
//IList<PropertyInfo> properties = typeof(MyObject).GetProperties().ToList();
//myobject = ConvertDataRowToObject<MyObject>(r, properties);
public static T ConvertDataRowToObject<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
{
    string p = "";
    T item = new T();
    try
    {
        foreach (var property in properties)
        {
            p = property.Name;
            if (row.Table.Columns.Contains(p))
            {
                Type t = null;
                if (property.PropertyType == typeof(string))
                    t = typeof(string);
                else
                    t = Nullable.GetUnderlyingType(property.PropertyType);
                if (row[p] != DBNull.Value && property.CanWrite)
                    property.SetValue(item, Convert.ChangeType(row[p], t));
            }
        }
        return item;
    }
    catch
    {
        return item;
    }
}