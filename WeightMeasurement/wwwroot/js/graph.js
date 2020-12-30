$(function () {

    am4core.useTheme(am4themes_animated);

    var chart = am4core.create("chartdiv", am4charts.XYChart);
    chart.data = data;

    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "date";
    categoryAxis.title.text = "Dates";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.minGridDistance = 50;

    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    //valueAxis.logarithmic = true;
    valueAxis.renderer.minGridDistance = 20;
    valueAxis.dataFields.value = "weight";
    valueAxis.title.text = "Weights";

    var series = chart.series.push(new am4charts.LineSeries());
    series.dataFields.valueY = "weight";
    series.dataFields.categoryX = "date";
    series.tensionX = 0.8;
    series.strokeWidth = 3;
    series.tooltip.label.interactionsEnabled = true;
    series.tooltip.pointerOrientation = "vertical";
    var bullet = series.bullets.push(new am4charts.CircleBullet());
    bullet.circle.fill = am4core.color("#fff");
    bullet.circle.strokeWidth = 3; 

    let range = valueAxis.axisRanges.create();
    range.value = 90.4;
    range.grid.stroke = am4core.color("#396478");
    range.grid.strokeWidth = 1;
    range.grid.strokeOpacity = 1;
    range.grid.strokeDasharray = "3,3";
    range.label.inside = true;
    range.label.text = "Average";
    range.label.fill = range.grid.stroke;
    range.label.verticalCenter = "bottom";

    chart.cursor = new am4charts.XYCursor();
    chart.cursor.fullWidthLineX = true;
    chart.cursor.xAxis = dateAxis;
    chart.cursor.lineX.strokeWidth = 0;
    chart.cursor.lineX.fill = am4core.color("#000");
    chart.cursor.lineX.fillOpacity = 0.1;
    chart.scrollbarX = new am4core.Scrollbar();
    chart.exporting.menu = new am4core.ExportMenu();

});