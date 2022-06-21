
//Calculates factorial of an integer
public static int Factorial(int n)
{
    if (n <= 1)
        return 1;
    else
        return n * Factorial(n - 1);
}

//Calculates work days between two given dates (excluding weekend).
public static int CalculateWorkDays(DateTime firstDay, DateTime lastDay)
{
    firstDay = firstDay.Date;
    lastDay = lastDay.Date;
    if (firstDay > lastDay)
        throw new ArgumentException("Incorrect last day " + lastDay);

    TimeSpan span = lastDay - firstDay;
    int businessDays = span.Days + 1;
    int fullWeekCount = businessDays / 7;

    if (businessDays > fullWeekCount * 7)
    {
        int firstDayOfWeek = firstDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)firstDay.DayOfWeek;
        int lastDayOfWeek = lastDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)lastDay.DayOfWeek;

        if (lastDayOfWeek < firstDayOfWeek)
            lastDayOfWeek += 7;
        if (firstDayOfWeek <= 6)
        {
            if (lastDayOfWeek >= 7)
                businessDays -= 2;
            else if (lastDayOfWeek >= 6)
                businessDays -= 1;
        }
        else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)
            businessDays -= 1;
    }

    businessDays -= fullWeekCount + fullWeekCount;
    return businessDays;
}

//Converts Celsius to Fahrenheit
public static double CelsiusToFahrenheit(string temperatureCelsius)
{
    return (Double.Parse(temperatureCelsius) * 9 / 5) + 32;
}

//Converts Fahrenheit to Celsius
public static double FahrenheitToCelsius(string temperatureFahrenheit)
{
    return (Double.Parse(temperatureFahrenheit) - 32) * 5 / 9;
}