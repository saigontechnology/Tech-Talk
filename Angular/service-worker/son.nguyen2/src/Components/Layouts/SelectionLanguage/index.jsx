import { makeStyles, Select } from "@material-ui/core";
import { languageList } from "Constants/constants";
import React, { useState } from "react";
import PropTypes from "prop-types";

SelectionLanguage.propTypes = {
  onChangeLanguage: PropTypes.func,
  currentLanguage: PropTypes.string,
};

const useStyles = makeStyles((theme) => ({
  [theme.breakpoints.up("sm")]: {
    selectionLanguage: {
      // backgroundColor: "transparent",
      color: "white",
      "& option": {
        color: "black",
      },
    },
  },
}));

function SelectionLanguage({ onChangeLanguage, currentLanguage }) {
  const [language, setLanguage] = useState("en");
  const classes = useStyles();
  const handleChangeLanguage = (e) => {
    if (onChangeLanguage) {
      setLanguage(e.target.value);
      onChangeLanguage(e.target.value);
    }
  };
  return (
    <Select
      native
      className={classes.selectionLanguage}
      onChange={handleChangeLanguage}
      value={currentLanguage || language}
      variant="standard"
      inputProps={{
        name: "age",
        id: "filled-age-native-simple",
      }}
    >
      <option value={languageList.vietNam}>Tiếng Việt</option>
      <option value={languageList.english}>English</option>
    </Select>
  );
}

export default SelectionLanguage;
