Imports System.Dynamic

Namespace Expando.Services

    Public Class WeatherForecastService

        Private Shared ReadOnly Summaries As String() = _("Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching")

        Public Function GetForecastAsync(ByVal startDate As DateTime) As Task(Of WeatherForecast())
            Dim rng = New Random()
            Return Task.FromResult(Enumerable.Range(1, 20).[Select](Function(index) New WeatherForecast With {.[Date] = DateOnly.FromDateTime(startDate.AddDays(index)), .TemperatureC = rng.[Next](-20, 55), .Summary = Summaries(rng.[Next](Summaries.Length))}).ToArray())
        End Function

        Public Async Function GetForecastAsyncExpando(ByVal startDate As DateTime) As Task(Of List(Of ExpandoObject))
            Dim weatherForecasts = Await GetForecastAsync(DateTime.Now)
            Dim dynamicList As List(Of ExpandoObject) = _(_)
            For Each weatherForecast As WeatherForecast In weatherForecasts
                Dim dynamicWeatherForecast As dynamic = New ExpandoObject()
                dynamicWeatherForecast.Id = Guid.NewGuid()
                dynamicWeatherForecast.[Date] = weatherForecast.Date
                dynamicWeatherForecast.TemperatureC = weatherForecast.TemperatureC
                dynamicWeatherForecast.Summary = weatherForecast.Summary
                dynamicList.Add(dynamicWeatherForecast)
            Next

            Return dynamicList
        End Function
    End Class
End Namespace
