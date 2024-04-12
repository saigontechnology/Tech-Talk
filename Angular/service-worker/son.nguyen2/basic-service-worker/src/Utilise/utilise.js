import _ from "lodash";
import moment from "moment";

export const checkLogin = () => {
  return window.localStorage.getItem("user");
};

// bind color follow to number of cases
export const checkColor = (number) => {
  if (number < 0) return;
  if (number >= 1e7) return "red";
  if (number >= 1e6 && number < 1e7) return "green";
  if (number >= 0 && number < 1e6) return "blue";
};

export const transformToMapData = (data) => {
  const newData = [];
  for (let i = 0; i < data.length; i++) {
    const object = {};
    object.code3 = data[i].countryInfo.iso3;
    object.z = data[i].cases;
    object.textCases = new Intl.NumberFormat().format(data[i].cases);
    object.color = checkColor(data[i].cases);
    newData.push(object);
  }
  return newData;
};

// Create option file for map
export const createOptionForMap = (
  // Highcharts = null,
  map,
  mapData,
  title,
  subTile,
  ...rest
) => {
  return {
    chart: {
      borderWidth: 0,
      map: map,
      styleMode: true,
    },

    title: {
      text: title,
    },

    subtitle: {
      text: subTile,
    },

    colorAxis: {
      dataClasses: [
        {
          color: "blue",
          from: 0,
          name: "<1M",
          to: 1e6 - 1,
        },
        {
          color: "green",
          from: 1e6,
          name: "<10M",
          to: 1e7 - 1,
        },
        {
          color: "red",
          from: 1e7,
          name: ">10M",
        },
      ],
    },

    legend: {
      enabled: rest?.length > 0 ? false : true,
      layout: "horizontal",
      align: "center",
      verticalAlign: "bottom",
    },

    mapNavigation: {
      enabled: true,
      buttonOptions: {
        verticalAlign: "bottom",
      },
    },
    loading: {
      showDuration: 100,
    },
    tooltip: {
      pointFormat: "{point.properties.name}: {point.textCases}",
    },

    series: [
      {
        data: mapData,
        mapData: map,
        joinBy: ["iso-a3", "code3"],
        name: "Total cases",
        states: {
          hover: {
            color: "pink",
          },
        },
      },

      {
        type: "mapline",
        nullColor: "gray",
        showInLegend: false,
        enableMouseTracking: true,
      },
    ],
  };
};

// transform value map to chart
export const transformDataMapToChart = (data, gap = 1) => {
  if (!data) return;

  const { cases, deaths, recovered } = data;
  const dataCases = _.values(cases);
  const dataDeaths = _.values(deaths);
  const dataRecovered = _.values(recovered);
  const timeline = _.map(_.keys(cases), (x) =>
    moment(x, "MM/DD/YY").format("DD/MM/YYYY")
  );
  const timeStampList = _.map(timeline, (x) => moment.utc(x, "DD/MM/YYYY").format("x"));

  if (gap > 1) {
    const dataLength = dataCases.length;
    const z = (dataLength - 1) % gap;
    const dataCasesSortedByGap = [];
    const dataDeathsSortedByGap = [];
    const dataRecoveredSortedByGap = [];
    const timelineSortedByGap = [];

    for (let i = 0; i < dataLength - 1; i += gap) {
      dataCasesSortedByGap.push(dataCases[i]);
      dataDeathsSortedByGap.push(dataDeaths[i]);
      dataRecoveredSortedByGap.push(dataRecovered[i]);
      timelineSortedByGap.push(timeline[i]);
    }
    if (z) {
      dataCasesSortedByGap.push(dataCases[dataLength - 1]);
      dataDeathsSortedByGap.push(dataDeaths[dataLength - 1]);
      dataRecoveredSortedByGap.push(dataRecovered[dataLength - 1]);
      timelineSortedByGap.push(timeline[dataLength - 1]);
    }
    return {
      cases: dataCasesSortedByGap,
      deaths: dataDeathsSortedByGap,
      recovered: dataRecoveredSortedByGap,
      timeStampList: timeStampList,
      timeline: timelineSortedByGap,
      gap: gap,
    };
  }

  return {
    cases: dataCases,
    deaths: dataDeaths,
    recovered: dataRecovered,
    timeStampList: timeStampList,
    timeline: timeline,
    gap: gap,
  };
};

// Create option for line chart
export const createOptionForLineChart = (chartData, title, subTitle) => {
  const breakPoint = chartData?.timeStampList?.length;
  return {
    chart: {
      zoomType: "x",
      resetZoomButton: {
        position: {
          y: -30,
        },
      },
    },
    title: {
      text: title,
    },

    subtitle: {
      text: subTitle,
    },

    yAxis: {
      title: {
        text: "Number",
      },
    },

    xAxis: {
      // type: chartData.gap < 30 ? "datetime" : "category",
      // categories: chartData.gap < 30 ? "" : chartData.timeline,
      type: "datetime",
      accessibility: {
        // rangeDescription: "As of today",
      },
      offset: 10,
      startOnTick: false,
      minPadding: 0.01,
      tickPixelInterval: 30,
      endOnTick: true,

      plotLines: [
        {
          label: {
            formatter: function () {
              const { timeStampList } = chartData;
              const endDate = moment(+timeStampList[breakPoint - 1]).format("LL");
              return "Update to: " + endDate;
            },
            style: { color: "red" },
          },
          value: +chartData?.timeStampList[breakPoint - 1],
          color: "red",
          zIndex: 1,
        },
      ],
    },

    legend: {
      layout: "horizontal",
      align: "right",
      verticalAlign: "bottom",
    },
    tooltip: {
      shared: true,
    },

    plotOptions: {
      series: {
        label: {
          connectorAllowed: false,
        },
        // pointStart: chartData.gap < 30 ? chartData?.timeStampList[0] : undefined,
        // pointInterval:
        //   chartData.gap < 30 ? 60 * 60 * 24 * 1000 * chartData?.gap : undefined,
        pointStart: +chartData?.timeStampList[0],
        pointInterval: 60 * 60 * 24 * 1000 * chartData?.gap,
        zoneAxis: "x",
        zones: [
          {
            value: +chartData?.timeStampList[breakPoint - 1],
          },
          {
            dashStyle: "dot",
          },
        ],
      },
    },

    series: [
      {
        name: "Cases",
        data: chartData?.cases,
      },
      {
        name: "Death",
        data: chartData?.deaths,
        color: "red",
      },
      {
        name: "Recovered",
        data: chartData?.recovered,
      },
    ],
  };
};

//Count day for filter covid case
export const countNumberOfDay = (dateFrom = "01-11-2019", dateTo = "") => {
  const pastDate = moment(dateFrom, "DD-MM-YYYY");
  if (!dateTo) {
    const today = moment();
    return today.diff(pastDate, "days");
  }
  const dateCount = moment(dateTo, "DD-MM-YYYY");
  return dateCount.diff(pastDate, "days");
};

export const filterContinentDataMapToChart = (data) => {
  const continentList = [];
  for (let i = 0; i < data.length; i++) {
    continentList.push({ name: data[i].continent, data: [data[i].cases] });
  }
  return continentList;
};

export const createOptionForBarChart = (
  Highcharts = null,
  chartData,
  title,
  subTitle
) => {
  return {
    chart: {
      type: "bar",
    },
    title: {
      text: title,
    },
    subtitle: {
      text: subTitle,
    },
    xAxis: {
      categories: [""],
      title: {
        text: "Continent",
      },
      labels: {
        enabled: false,
      },
    },

    yAxis: {
      title: {
        enabled: false,
      },
      labels: {
        overflow: "justify",
      },
    },
    plotOptions: {
      bar: {
        groupPadding: 0.1,
        dataLabels: {
          enabled: true,
        },
      },
    },
    legend: {
      layout: "horizontal",
      align: "center",
      verticalAlign: "bottom",
      shadow: true,
    },
    credits: {
      enabled: false,
    },

    series: chartData,
  };
};
