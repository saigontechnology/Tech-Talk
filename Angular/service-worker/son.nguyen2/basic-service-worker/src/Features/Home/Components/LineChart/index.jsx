import Highcharts from "highcharts";
import HighchartsReact from "highcharts-react-official";
import PropTypes from "prop-types";
import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useSelector } from "react-redux";
import { createOptionForLineChart } from "Utilise/utilise";

LineChartCovid.propTypes = {
  timelineData: PropTypes.object,
};

function LineChartCovid({ timelineData }) {
  const [option, setOption] = useState({});

  const globalState = useSelector((state) => state.global);
  const [t] = useTranslation();

  const title = t("home.titleLineChart");
  const subTitle = t("home.subTitleChart");

  useEffect(() => {
    (async () => {
      try {
        const options = createOptionForLineChart(timelineData, title, subTitle);
        setOption(options);
      } catch (error) {}
    })();
  }, [timelineData, globalState.themeMode, title, subTitle]);

  return <HighchartsReact highcharts={Highcharts} options={option} immutable />;
}

export default React.memo(LineChartCovid);
