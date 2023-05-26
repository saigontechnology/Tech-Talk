import React from "react";

const CustomButton = (
  { label = "", color = "primary", variant = "filled" },
  props
) => {
  return (
    <button {...props} className={`btn btn-${variant} btn-${color}`}>
      {label}
    </button>
  );
};

export default CustomButton;
