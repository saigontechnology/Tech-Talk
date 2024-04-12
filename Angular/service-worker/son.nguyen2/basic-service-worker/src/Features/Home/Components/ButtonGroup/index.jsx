import React, { useState } from "react";
import PropTypes from "prop-types";
import { ToggleButton, ToggleButtonGroup } from "@material-ui/lab";
import { Box, makeStyles } from "@material-ui/core";

ButtonGroup.propTypes = {
  onChangeShowType: PropTypes.func,
};

const typeList = [
  { type: "Daily", gap: 1 },
  { type: "Weekly", gap: 7 },
  { type: "Monthly", gap: 30 },
];

const useStyles = makeStyles((theme) => ({
  root: {
    display: "flex",
    flexFlow: "row nowrap",
    justifyContent: "flex-end",
  },
  buttonGroup: {
    "& .Mui-selected": {
      background: "linear-gradient(45deg, #FE6B8B 30%, #FF8E53 90%)",
      color: "white",
      boxShadow: "0 3px 5px 2px rgba(255, 105, 135, .3)",
    },
  },
}));

function ButtonGroup({ onChangeShowType }) {
  const [showType, setShowType] = useState(1);

  const handleSelectType = (e, value) => {
    if (value === null) return;
    if (onChangeShowType) {
      onChangeShowType(value);
      setShowType(value);
    }
  };
  const classes = useStyles();
  return (
    <Box className={classes.root}>
      <ToggleButtonGroup
        className={classes.buttonGroup}
        value={showType}
        exclusive
        onChange={handleSelectType}
        aria-label="show type"
        size="small"
      >
        {typeList.map((type) => (
          <ToggleButton key={type.gap} value={type.gap} aria-label={type.type}>
            {type.type}
          </ToggleButton>
        ))}
      </ToggleButtonGroup>
    </Box>
  );
}

export default ButtonGroup;
