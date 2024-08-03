using System;

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