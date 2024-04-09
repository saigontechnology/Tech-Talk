export const defaultTheme = {
  colors: [
    "#7cb5ec",
    "#434348",
    "#90ed7d",
    "#f7a35c",
    "#8085e9",
    "#f15c80",
    "#e4d354",
    "#2b908f",
    "#f45b5b",
    "#91e8e1",
  ],
  chart: {
    borderWidth: 0,
    backgroundColor: "#ffffff",
    borderColor: "#335cad",
    // styleMode: true,
    style: {
      fontFamily:
        '"Lucida Grande", "Lucida Sans Unicode", Verdana, Arial, Helvetica, sans-serif',
      fontSize: "12px",
      maxWidth: "100%",
    },
  },
  tooltip: {
    backgroundColor: "rgba(247,247,247,0.85)",
    style: {
      color: "#333333",
    },
  },
  xAxis: {
    gridLineColor: "#e6e6e6",
    gridLineDashStyle: "Solid",
    gridLineInterpolation: undefined,
    gridLineWidth: undefined,
    gridZIndex: 1,
    tickColor: "#ccd6eb",
    lineColor: "#ccd6eb",
    minorGridLineColor: "#f2f2f2",
    minorGridLineDashStyle: "Solid",
    minorGridLineWidth: 1,
    minorTickColor: "#999999",
    tickWidth: undefined,
    labels: {
      style: {
        color: "#666",
        cursor: "default",
        fontSize: "11px",
        lineHeight: "14px",
      },
    },
    title: {
      style: {
        color: "#4d759e",
        fontWeight: "bold",
      },
    },
  },
  yAxis: {
    gridLineColor: "#e6e6e6",
    gridLineDashStyle: "Solid",
    gridLineInterpolation: undefined,
    gridLineWidth: 1,
    gridZIndex: 1,
    minorTickInterval: null,
    lineColor: "#ccd6eb",
    lineWidth: 1,
    tickWidth: undefined,
    tickColor: "#ccd6eb",
    labels: {
      style: {
        color: "#666",
        cursor: "default",
        fontSize: "11px",
        lineHeight: "14px",
      },
    },
    title: {
      style: {
        color: "#4d759e",
        fontWeight: "bold",
      },
    },
  },
  plotOptions: {
    series: {
      dataLabels: {
        color: undefined,
      },
      style: {
        fontSize: undefined,
      },
    },
  },

  legend: {
    backgroundColor: "#ffffff",
    borderColor: "#999999",
    itemStyle: {
      color: "#333333",
      cursor: "pointer",
      fontSize: "12px",
      fontWeight: "bold",
      textOverflow: "ellipsis",
    },
    itemHoverStyle: {
      color: "#000000",
    },
    itemHiddenStyle: {
      color: "#cccccc",
    },
  },
  labels: {
    style: {
      color: "#333333",
    },
  },
  title: {
    text: "",
    style: {
      color: "#333333",
    },
  },
  subtitle: {
    text: "",
    style: {
      color: "#333333",
    },
  },

  navigation: {
    activeColor: "#003399",
    inactiveColor: "#cccccc",

    buttonOptions: {
      theme: {
        stroke: "#CCCCCC",
      },
    },
  },
};
export const darkTheme = {
  colors: [
    "#2b908f",
    "#90ee7e",
    "#f45b5b",
    "#7798BF",
    "#aaeeee",
    "#ff0066",
    "#eeaaee",
    "#55BF3B",
    "#DF5353",
    "#7798BF",
    "#aaeeee",
  ],
  chart: {
    backgroundColor: {
      linearGradient: {
        x1: 0,
        y1: 0,
        x2: 1,
        y2: 1,
      },
      stops: [
        [0, "#2a2a2b"],
        [1, "#3e3e40"],
      ],
    },
    plotBorderColor: "#606063",
  },
  title: {
    style: {
      color: "#E0E0E3",
      fontSize: "18px",
    },
  },
  subtitle: {
    style: {
      color: "#E0E0E3",
    },
  },
  xAxis: {
    gridLineColor: "#707073",
    labels: {
      style: {
        color: "#E0E0E3",
      },
    },
    lineColor: "#707073",
    minorGridLineColor: "#505053",
    tickColor: "#707073",
    title: {
      style: {
        color: "#A0A0A3",
      },
    },
    plotLines: [
      {
        label: {
          style: {
            color: "#E0E0E3",
          },
        },
      },
    ],
  },
  yAxis: {
    gridLineColor: "#707073",
    labels: {
      style: {
        color: "#E0E0E3",
      },
    },
    lineColor: "#707073",
    minorGridLineColor: "#505053",
    tickColor: "#707073",
    tickWidth: 1,
    title: {
      style: {
        color: "#A0A0A3",
      },
    },
  },
  tooltip: {
    backgroundColor: "rgba(0, 0, 0, 0.85)",
    style: {
      color: "#F0F0F0",
    },
  },
  plotOptions: {
    series: {
      dataLabels: {
        color: "#F0F0F3",
        style: {
          fontSize: "13px",
        },
      },
      marker: {
        lineColor: "#333",
      },
    },
    boxplot: {
      fillColor: "#505053",
    },
    candlestick: {
      lineColor: "white",
    },
    errorbar: {
      color: "white",
    },
  },
  legend: {
    backgroundColor: "rgba(0, 0, 0, 0.5)",
    itemStyle: {
      color: "#E0E0E3",
    },
    itemHoverStyle: {
      color: "#FFF",
    },
    itemHiddenStyle: {
      color: "#606063",
    },
    title: {
      style: {
        color: "#C0C0C0",
      },
    },
  },
  credits: {
    style: {
      color: "#666",
    },
  },
  labels: {
    style: {
      color: "#707073",
    },
  },

  drilldown: {
    activeAxisLabelStyle: {
      color: "#F0F0F3",
    },
    activeDataLabelStyle: {
      color: "#F0F0F3",
    },
  },

  navigation: {
    buttonOptions: {
      symbolStroke: "#DDDDDD",
      theme: {
        fill: "#505053",
      },
    },
  },

  // scroll charts
  rangeSelector: {
    buttonTheme: {
      fill: "#505053",
      stroke: "#000000",
      style: {
        color: "#CCC",
      },
      states: {
        hover: {
          fill: "#707073",
          stroke: "#000000",
          style: {
            color: "white",
          },
        },
        select: {
          fill: "#000003",
          stroke: "#000000",
          style: {
            color: "white",
          },
        },
      },
    },
    inputBoxBorderColor: "#505053",
    inputStyle: {
      backgroundColor: "#333",
      color: "silver",
    },
    labelStyle: {
      color: "silver",
    },
  },

  navigator: {
    handles: {
      backgroundColor: "#666",
      borderColor: "#AAA",
    },
    outlineColor: "#CCC",
    maskFill: "rgba(255,255,255,0.1)",
    series: {
      color: "#7798BF",
      lineColor: "#A6C7ED",
    },
    xAxis: {
      gridLineColor: "#505053",
    },
  },

  scrollbar: {
    barBackgroundColor: "#808083",
    barBorderColor: "#808083",
    buttonArrowColor: "#CCC",
    buttonBackgroundColor: "#606063",
    buttonBorderColor: "#606063",
    rifleColor: "#FFF",
    trackBackgroundColor: "#404043",
    trackBorderColor: "#404043",
  },
};
