import { Avatar, Box, Chip, makeStyles, TextField } from "@material-ui/core";
import { DataGrid } from "@material-ui/data-grid";
import { Autocomplete } from "@material-ui/lab";
import { router } from "Constants/constants";
import _ from "lodash";
import PropTypes from "prop-types";
import React, { useCallback, useMemo, useState } from "react";
import { useTranslation } from "react-i18next";
import { useHistory } from "react-router-dom";

const transformDataMapToTable = (data) => {
  if (data?.length) {
    const tableData = [];
    for (let i = 0; i < data?.length; ++i) {
      tableData.push(
        _.set(
          _.pick(data[i], ["country", "cases", "deaths", "recovered", "countryInfo"]),
          ["id"],
          data[i].country
        )
      );
    }
    return tableData;
  }
};

const useStyles = makeStyles(
  (theme) => ({
    root: {
      padding: theme.spacing(0.5, 0.5, 0),
      justifyContent: "space-between",
      display: "flex",
      alignItems: "flex-start",
      flexWrap: "wrap",
      margin: "1rem",
    },
    countryFlag: {
      display: "none",
      textField: {
        [theme.breakpoints.down("xs")]: {
          width: "100%",
        },
        margin: theme.spacing(1, 0.5, 1.5),
        "& .MuiSvgIcon-root": {
          marginRight: theme.spacing(0.5),
        },
        "& .MuiInput-underline:before": {
          borderBottom: `1px solid ${theme.palette.divider}`,
        },
      },
    },

    [theme.breakpoints.up("sm")]: {
      countryFlag: {
        display: "block",
      },
    },
  })

  //   { defaultTheme }
);

function QuickSearchToolbar(props) {
  const classes = useStyles();

  return (
    <div className={classes.root}>
      <Autocomplete
        autoHighlight
        fullWidth
        filterSelectedOptions
        noOptionsText="No country match"
        limitTags={5}
        size="small"
        multiple
        id="tags-filled"
        options={props.options}
        getOptionLabel={(option) => option.country}
        renderTags={(value, getTagProps) =>
          value.map((option, index) => (
            <Chip variant="outlined" label={option.country} {...getTagProps({ index })} />
          ))
        }
        onChange={(e, value) => props.onSelect(value)}
        renderInput={(params) => (
          <TextField {...params} variant="outlined" label="Search country" />
        )}
      />
    </div>
  );
}

QuickSearchToolbar.propTypes = {
  clearSearch: PropTypes.func,
  onChange: PropTypes.func,
  value: PropTypes.string,
};

function TableCountriesCovid({ countriesData }) {
  const history = useHistory();
  const [searchResult, setSearchResult] = useState([]);
  const [t] = useTranslation();

  const dataTable = useMemo(
    () => transformDataMapToTable(countriesData),
    [countriesData]
  );

  const debounce = _.debounce((value) => requestSearch(value), 200);
  // eslint-disable-next-line react-hooks/exhaustive-deps
  const handleSearch = useCallback((value) => debounce(value), [searchResult]);

  const requestSearch = (value) => {
    console.log({ value });
    setSearchResult(value);
  };
  const handleSelectRow = (params) => {
    console.log({ params });
    const idCountry = params.row.countryInfo.iso2;
    history.push(`${router.homeDetail}/${idCountry}`);
  };

  const classes = useStyles();

  return (
    <div style={{ height: "80vh", minWidth: "100%" }} className={classes.table}>
      <DataGrid
        components={{
          Toolbar: QuickSearchToolbar,
        }}
        componentsProps={{
          toolbar: {
            onSelect: (value) => handleSearch(value),
            options: dataTable,
          },
        }}
        columns={[
          {
            disableColumnMenu: true,
            field: "country",
            headerName: `${t("common.country")}`,
            minWidth: 175,
            flex: 1,
            renderCell: (params) => (
              <Box
                display="flex"
                flexDirection="row"
                flexWrap="nowrap"
                alignItems="center"
              >
                <Avatar
                  alt={params.value}
                  src={params.row.countryInfo.flag}
                  className={classes.countryFlag}
                ></Avatar>
                <Box marginLeft="16px">{params.value}</Box>
              </Box>
            ),
          },
          {
            type: "number",
            field: "cases",
            headerName: `${t("common.cases")}`,
            flex: 0.5,
            disableColumnMenu: true,
            minWidth: 125,
          },
          {
            type: "number",
            field: "deaths",
            headerName: `${t("common.deaths")}`,
            flex: 0.5,
            disableColumnMenu: true,
            minWidth: 125,
          },
          {
            type: "number",
            field: "recovered",
            headerName: `${t("common.recovered")}`,
            flex: 0.5,
            disableColumnMenu: true,
            minWidth: 125,
          },
        ]}
        onRowClick={(params) => handleSelectRow(params)}
        rows={searchResult?.length > 0 ? searchResult : dataTable}
      />
    </div>
  );
}

export default TableCountriesCovid;
