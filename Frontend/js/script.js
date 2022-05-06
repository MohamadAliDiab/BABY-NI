$(document).ready(async function () {
    let checkedKPI = $('input[name="kpi"]:checked').val();
    let cachedData = [];

    $('input[name="kpi"]').click(() => {
        checkedKPI = $('input[name="kpi"]:checked').val();
        createChart(cachedData, checkedKPI);
    });

    $("#viewButton").click(() => {
        let tableName = $('input[name="agg_type"]:checked').val();
        let dateFrom =  $("#date_from").val();
        let dateTo = $("#date_to").val();
        
        console.log("requesting following info");
        console.log([tableName, dateFrom, dateTo]);

        fetchAggregateData(tableName, dateFrom, dateTo).then((data => {
            console.log(data);

            createGrid(data);
            createChart(data, checkedKPI);

            cachedData = data;
        }))
        
    });
});

function createGrid(data) {
    $("#grid").kendoGrid({
        dataSource: {
            data: data
        },
        height: 550,
        groupable: true,
        sortable: true,
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
        columns: [{
            field: "DateTime_key",
            title: "DateTime_key",
            width: 240
        }, {
            field: "Link",
            title: "Link"
        }, {
            field: "Slot",
            title: "Slot"
        }, {
            field: "NeType",
            title: "NeType"
        }, {
            field: "NeAlias",
            title: "NeAlias"
        }, {
            field: "MAX_RX_LEVEL",
            title: "MAX_RX_LEVEL"
        }, {
            field: "MAX_TX_LEVEL",
            title: "MAX_TX_LEVEL",
        }, {
            field: "RSL_DEVIATION",
            title: "RSL_DEVIATION"
        }]
    });
}
function createChart(data, checkedKPI) {
    let dates = [];
    let values = [];

    console.log(checkedKPI)


    for (var i = 0; i < data.length; i++) {
        dates.push(data[i]["DateTime_key"]);
        values.push(data[i][checkedKPI]);
    }

    console.log(values);

    $("#chart").kendoChart({ 
        seriesDefaults: {
            type: "line",
            style: "smooth"
        },

        categoryAxis: { // x-axis data
            categories: dates,
            majorGridLines: {
                visible: false
            },
            labels: {
                rotation: "auto"
            }
        },

        series: [{ // y-axis data
            data: values
        }]

    });
}


// $(document).bind("kendo:skinChange", createChart);

async function fetchAggregateData(tableName, dateFrom, dateTo){           
    tableName = "aggregate_" + tableName;

    return fetch("https://localhost:44393/api/GetDataByDate/get-data-by-date/"+tableName+"/ "+dateFrom+"/ " + dateTo).then (response => {return response.json()});
 }