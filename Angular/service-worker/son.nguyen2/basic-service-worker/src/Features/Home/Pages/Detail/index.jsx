import { Container, Grid, Typography } from "@material-ui/core";
import { covidApi } from "Api/covidApi";
import MainLayout from "Components/Layouts";
import ButtonGroup from "Features/Home/Components/ButtonGroup";
import CountryMap from "Features/Home/Components/CountryMap";
import LineChartCovid from "Features/Home/Components/LineChart";
import moment from "moment";
import React, { useEffect, useState } from "react";
import { Trans, useTranslation } from "react-i18next";
import { useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import { countNumberOfDay, transformDataMapToChart } from "Utilise/utilise";

function CountryTitle({ countrySummary }) {
  const { updatedAt, cases, deaths, recovered, country } = countrySummary;
  const [t] = useTranslation();

  return (
    <>
      <Trans i18nKey="homeDetail.title" t={t}>
        In <strong>{{ country }}</strong>, as of
        <strong style={{ color: "green" }}>{{ updatedAt }}</strong>, there have been
        <strong style={{ color: "blue" }}>{{ cases }}</strong> confirmed cases of
        COVID-19, including <strong style={{ color: "red" }}>{{ deaths }}</strong> deaths,
        <strong style={{ color: "blue" }}>{{ recovered }}</strong> recovered reported to
        WHO
      </Trans>
    </>
  );
}

function CountryDetail() {
  const [countryDataTimeline, setCountryDataTimeline] = useState();
  const [countrySummary, setCountrySummary] = useState();
  const [isError, setIsError] = useState(false);
  const [timeGap, setTimeGap] = useState(1);
  const globalState = useSelector((state) => state.global);
  const [t] = useTranslation();
  const { countryID } = useParams();

  // eslint-disable-next-line react-hooks/exhaustive-deps
  useEffect(() => getCountryDetail(), [timeGap]);

  useEffect(() => {
    getCountrySummary(countryID);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [countryID]);

  // useEffect(() => registerServiceWorker(), []);

  // const registerServiceWorker = async () => {
	// 	if ('serviceWorker' in navigator) {
	// 		try {
	// 			const registration = await navigator.serviceWorker.register('/service-worker.js',  {scope : '/news'});
	// 			if (registration.installing) {
	// 				console.log('sw installing');
	// 			}
	// 			if (registration.waiting) {
	// 				console.log('sw installed');
	// 			}
	// 			if (registration.active) {
	// 				console.log('sw active');
	// 			}
	// 		} catch (error) {
  //       console.log(error);
  //     }
	// 	}
	// };


  const getCountryDetail = async () => {
    try {
      const numberOfDay = countNumberOfDay();
      const timelineData = await covidApi.getByCountry(countryID, {
        lastdays: numberOfDay,
      });
      const dataMapToChart = transformDataMapToChart(timelineData.timeline, timeGap);
      setCountryDataTimeline(dataMapToChart);
    } catch (error) {
      setIsError(true);
    }
  };

  const getCountrySummary = async (country) => {
    try {
      const summaryData = await covidApi.getSummaryOfCountry(countryID);
      const { updated, cases, deaths, recovered, country } = summaryData;
      console.log({ summaryData });
      setCountrySummary({
        country: country,
        updatedAt:
          globalState.language === "en"
            ? moment(updated).format("LL")
            : moment(updated).format("DD/MM/YYYY"),
        cases: new Intl.NumberFormat().format(+cases),
        deaths: new Intl.NumberFormat().format(+deaths),
        recovered: new Intl.NumberFormat().format(+recovered),
      });
    } catch (error) {
      console.log(error);
    }
  };

  const handleChangeShowTimelyLineChart = (value) => {
    console.log({ value });
    setTimeGap(value);
  };

  return (
    <div>
      <MainLayout>
        <Container>
          <Grid container spacing={1}>
            <Grid item xs={12}>
              {countryID && <CountryMap countryID={countryID} />}
            </Grid>
            <Grid item xs={12}>
              {countrySummary && (
                <Typography variant="h5">
                  <CountryTitle countrySummary={countrySummary} />
                </Typography>
              )}
            </Grid>
            <Grid item xs={12} style={{ minHeight: "400px" }}>
              {isError && <Typography> {t("error.failLoadingData")}</Typography>}
              {countryDataTimeline && (
                <ButtonGroup onChangeShowType={handleChangeShowTimelyLineChart} />
              )}
              {countryDataTimeline && (
                <LineChartCovid timelineData={countryDataTimeline} />
              )}
            </Grid>
          </Grid>
        </Container>
      </MainLayout>
    </div>
  );
}

export default CountryDetail;
