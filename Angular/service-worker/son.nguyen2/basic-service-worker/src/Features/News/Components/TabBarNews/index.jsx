import React, { useState } from "react";
import { makeStyles } from "@material-ui/core/styles";
import Paper from "@material-ui/core/Paper";
import Tabs from "@material-ui/core/Tabs";
import Tab from "@material-ui/core/Tab";

const useStyles = makeStyles({
  root: {
    flexGrow: 1,
    marginBottom: "16px",
  },
});

export default function CenteredTabs({ onChangeCategory }) {
  const classes = useStyles();
  const [category, setCategory] = useState("business");

  const handleChange = (e, newValue) => {
    if (onChangeCategory) {
      onChangeCategory(newValue);
      setCategory(newValue);
    }
  };

  return (
    <Paper className={classes.root}>
      <Tabs
        value={category}
        onChange={handleChange}
        indicatorColor="primary"
        textColor="primary"
        centered
      >
        <Tab label="Business" value="business" />
        <Tab label="Technology" value="technology" />
        <Tab label="Science" value="science" />
      </Tabs>
    </Paper>
  );
}
