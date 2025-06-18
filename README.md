<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1292610)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
# Editable Blazor Grid with ExpandoObject Collection

The [DevExpress Blazor Grid's](https://docs.devexpress.com/Blazor/403143/components/grid) ability to create, modify, and delete rows extends to dynamic data sources. When bound to a collection of [ExpandoObject](https://learn.microsoft.com/en-us/dotnet/api/system.dynamic.expandoobject) instances, the grid can adapt to user-defined schemas from sources like JSON files or NoSQL databases. It allows you to implement CRUD (Create, Read, Update, Delete) operations when your data structure is not defined at compile time.

This example implements a fully editable `DxGrid` bound to a dynamic `ExpandoObject` list.

![Edit ExpandoObject Data in DxGrid](images/grid-edit-expandoobject.gif)

## Implementation Details

Create a collection of `ExpandoObject` that will hold your dynamic data. Populate it with initial entries.

```cs
private List<ExpandoObject>? forecasts;

protected override async Task OnInitializedAsync() {
    forecasts = await ForecastService.GetForecastAsyncExpando(DateTime.Now);
}
```

Implement a custom edit model in the [CustomizeEditModel](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGrid.CustomizeEditModel) event handler:

```cs
private void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e) {
    if (e.IsNew) {
        dynamic forecast = new ExpandoObject();
        forecast.Id = Guid.NewGuid();
        forecast.Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        forecast.TemperatureC = 0;
        forecast.Summary = "";
        e.EditModel = forecast;
    }
}
```

Implement the [EditModelSaving](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGrid.EditModelSaving) event handler. It will retrieve data from your custom edit model and update the corresponding `ExpandoObject` instance.

```cs
private async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e) {
    if (e.IsNew) {
        forecasts.Add((ExpandoObject)e.EditModel);
    }
    else {
        dynamic editableForecast = (ExpandoObject)e.EditModel;
        dynamic originalForecast = forecasts
            .Cast<dynamic>()
            .First(s => (Guid)s.Id == (Guid)editableForecast.Id);
        originalForecast.Date = editableForecast.Date;
        originalForecast.TemperatureC = editableForecast.TemperatureC;
        originalForecast.Summary = editableForecast.Summary;
    }
}
```

Implement the [DataItemDeleting](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGrid.DataItemDeleting) event handler, which will remove an item from the `ExpandoObject` collection.

```cs
private async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e) {
    forecasts.Remove((ExpandoObject)e.DataItem);
}
```

Add [DxGrid](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGrid) component to the [page](CS/Expando/Components/Pages/Index.razor) and bind it to the previously created `ExpandoObject` list. Attach event handlers to the corresponding grid properties.

Add a custom [CellEditTemplate](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGridCommandColumn.CellEditTemplate) to each column to bind an inline editor to the `ExpandoObject` through `IDictionary<string, object>`.

```razor
<DxGrid Data="@forecasts"
        EditMode="GridEditMode.EditRow"
        CustomizeEditModel="Grid_CustomizeEditModel"
        EditModelSaving="Grid_EditModelSaving"
        DataItemDeleting="Grid_DataItemDeleting">
    <Columns>
        <DxGridCommandColumn />
        <DxGridDataColumn Caption="Date" FieldName="Date">
            <CellEditTemplate>
                @{
                    var editItem = (IDictionary<string, object>)context.EditModel;
                    var date = (DateOnly)editItem["Date"];
                }
                <DxDateEdit Date="@(date)"
                            DateChanged="@((DateOnly newVal) => editItem["Date"] = newVal)"
                            DateExpression="@(() => date)">
                </DxDateEdit>
            </CellEditTemplate>
        </DxGridDataColumn>
        ...
    </Columns>
</DxGrid>
```

## Files to Review

- [Index.razor](CS/Expando/Components/Pages/Index.razor)
- [WeatherForecastService.cs](CS/Expando/Services/WeatherForecastService.cs)
- [Program.cs](CS/Expando/Program.cs)

## Documentation

- [DxGrid Class](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxGrid)
- [Bind Blazor Grid to Data](https://docs.devexpress.com/Blazor/403737/components/grid/bind-to-data)
- [Editing and Validation in Blazor Grid](https://docs.devexpress.com/Blazor/403454/components/grid/editing-and-validation)

<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=blazor-grid-expandoobject&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=blazor-grid-expandoobject&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
