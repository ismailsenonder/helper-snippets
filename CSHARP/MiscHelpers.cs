using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Text;

//Removes all HTML code from a string and returns only the text
public static string RemoveExtraHtmlCode(this string htmlString)
{
    htmlString = Regex.Replace(htmlString, @"(<style.+?</style>)|(<script.+?</script>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    htmlString = Regex.Replace(htmlString, @"(<img.+?>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    htmlString = Regex.Replace(htmlString, @"(<o:.+?</o:.+?>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    htmlString = Regex.Replace(htmlString, @"<!--.+?-->", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    htmlString = Regex.Replace(htmlString, @"class=.+?>", ">", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    htmlString = Regex.Replace(htmlString, @"class=.+?\s", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    return htmlString;
}

//Returns the specified number of characters + ... from a long string.
//Good for summaries that has a "Read More" button.
public static string ShortenToCharacterLimit(this string strInput, int characterLimit)
{
    if (strInput.Length > characterLimit)
    {
        return strInput.Substring(0, characterLimit) + "...";
    }
    else
    {
        return strInput;
    }
}

//Removes characters that can cause SQL injection from a string
public static string RemoveIllegalQueryCharacters(this string value)
{
    return value.Replace("\"", "")
                .Replace("’", "")
                .Replace("'", "")
                .Replace(",", "");

}

//Returns a list of all Cultures defined in the framework
public static List<string> GetCultureCodes()
{
    CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.AllCultures);
    List<string> cultureCodes = new List<string>();
    foreach (CultureInfo cul in cinfo)
    {
        cultureCodes.Add(cul.DisplayName + " : " + cul.Name);
    }
    return cultureCodes;
}

//Returns true if DataTable is populated, false if it's null or empty
public static bool IsPopulated(this DataTable table)
{
    return (table != null && table.Rows.Count > 0 && table.Columns.Count > 0);
}

//Check if DataTable has a column with specified name
public static bool ContainsColumn(this DataTable table, string columnName)
{
    DataColumnCollection columns = table.Columns;
    return columns.Contains(columnName);
}

//Returns the full command text with parameters from an SqlCommand
//Good for debugging where using SQL profiler is not an option.
//You can change the command type for different kind of connection. i.e MySqlCommand
public static string GetQueryFromMySqlCommand(this SqlCommand cmd)
{
    string CommandTxt = cmd.CommandText;
    foreach (SqlParameter parms in cmd.Parameters)
    {
        string val = String.Empty;
        if (parms.DbType.Equals(DbType.String)
            || parms.DbType.Equals(DbType.DateTime))
        {
            val = "'" + Convert.ToString(parms.Value).Replace(@"\", @"\\").Replace("'", @"\'") + "'";
        }

        if (parms.DbType.Equals(DbType.Int16) || parms.DbType.Equals(DbType.Int32)
            || parms.DbType.Equals(DbType.Int64) || parms.DbType.Equals(DbType.Decimal)
            || parms.DbType.Equals(DbType.Double))
        {
            val = Convert.ToString(parms.Value);
        }

        string paramname = parms.ParameterName;
        CommandTxt = CommandTxt.Replace(paramname, val);
    }
    return (CommandTxt);
}

//Creates a random password from given character set
public static string CreatePassword(int length)
{
    const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    StringBuilder res = new StringBuilder();
    Random rnd = new Random();
    while (0 < length--)
    {
        res.Append(valid[rnd.Next(valid.Length)]);
    }
    return res.ToString();
}

//Gives appropriate string value for the timespan between now and given date.
public static string TimeAgo(this DateTime dateInput)
{
    if (dateInput > DateTime.Now)
        return "coming soon...";
    TimeSpan span = DateTime.Now - dateInput;

    if (span.Days > 365)
    {
        int years = (span.Days / 365);
        if (span.Days % 365 != 0)
            years += 1;
        return String.Format("{0} {1} ago", years, years == 1 ? "year" : "years");
    }

    if (span.Days > 30)
    {
        int months = (span.Days / 30);
        if (span.Days % 31 != 0)
            months += 1;
        return String.Format("{0} {1} ago", months, months == 1 ? "month" : "months");
    }

    if (span.Days > 0)
        return String.Format("{0} {1} ago", span.Days, span.Days == 1 ? "day" : "days");

    if (span.Hours > 0)
        return String.Format("{0} {1} ago", span.Hours, span.Hours == 1 ? "hour" : "hours");

    if (span.Minutes > 0)
        return String.Format("{0} {1} ago", span.Minutes, span.Minutes == 1 ? "minute" : "minutes");

    if (span.Seconds > 5)
        return String.Format("{0} seconds ago", span.Seconds);

    if (span.Seconds <= 5)
        return "now";

    return string.Empty;
}

public string GetRandomColorCode()
{
    Random rnd = new Random();
    string hexOutput = String.Format("{0:X}", rnd.Next(0, 0xFFFFFF));
    while (hexOutput.Length < 6)
        hexOutput = "0" + hexOutput;
    return "#" + hexOutput;
}