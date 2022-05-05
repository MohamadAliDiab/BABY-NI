$(document).ready(function () {
    let checkedKPI = $('input[name="kpi"]:checked').val();
    $('input[name="kpi"]').click(() => {
        checkedKPI = $('input[name="kpi"]:checked').val();
        createChart(data, checkedKPI);
    });

    $("#viewButton").click(() => {
        let tableName = $('input[name="agg_type"]:checked').val();
        let dateFrom =  $("#date_from").val();
        let dateTo = $("#date_to").val();
        
        console.log("requesting following info");
        console.log([tableName, dateFrom, dateTo]);

        let data = getDummyData(tableName, dateFrom, dateTo);

        createGrid(data);
        createChart(data, checkedKPI);
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

function getDummyData(tableName, dateFrom, dateTo){           
    var data = [
      {DateTime_key: "2017/6/12 15/00/00", Link: "16/1" , Slot: 20 , NeType: "AMM6PC" , NeAlias: 17, "MAX_RX_LEVEL": 0,"MAX_TX_LEVEL": -5,"RSL_DEVIATION": 20},
      {DateTime_key: "2018/6/12 15/00/00", Link: "16/1" , Slot: 20 , NeType: "AMM6PC" , NeAlias: 17, "MAX_RX_LEVEL": 10,"MAX_TX_LEVEL": -5,"RSL_DEVIATION": 20},
      {DateTime_key: "2010/6/12 15/00/00", Link: "16/1" , Slot: 20 , NeType: "AMM6PC", NeAlias: 17, "MAX_RX_LEVEL": 20,"MAX_TX_LEVEL": -5,"RSL_DEVIATION": 20},
      {DateTime_key: "2019/6/12 15/00/00", Link: "16/1" , Slot: 20 , NeType: "AMM6PC", NeAlias: 17, "MAX_RX_LEVEL": 30,"MAX_TX_LEVEL": -5,"RSL_DEVIATION": 20},
      {DateTime_key: "2016/6/12 15/00/00", Link: "16/1" , Slot: 20 , NeType: "AMM6PC", NeAlias: 17, "MAX_RX_LEVEL": 40,"MAX_TX_LEVEL": -5,"RSL_DEVIATION": 20},
      {DateTime_key: "2017/6/12 15/00/00", Link: "16/1" , Slot: 20 , NeType: "AMM6PC", NeAlias: 17, "MAX_RX_LEVEL": -40,"MAX_TX_LEVEL": -5,"RSL_DEVIATION": 20},
      {DateTime_key: "2017/6/12 15/00/00", Link: "16/1" , Slot: 20 , NeType: "AMM6PC", NeAlias: 17, "MAX_RX_LEVEL": 50,"MAX_TX_LEVEL": -5,"RSL_DEVIATION": 20},
      {DateTime_key: "2017/6/12 15/00/00", Link: "16/1" , Slot: 20 , NeType: "AMM6PC", NeAlias: 17, "MAX_RX_LEVEL": 60,"MAX_TX_LEVEL": -5,"RSL_DEVIATION": 20},
      {DateTime_key: "2017/6/12 15/00/00", Link: "16/1" , Slot: 20 , NeType: "AMM6PC", NeAlias: 17, "MAX_RX_LEVEL": 80,"MAX_TX_LEVEL": -5,"RSL_DEVIATION": 20},
      
    ];
    return data;
 }