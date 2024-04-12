import React from "react";
import PropTypes from "prop-types";
import { Controller } from "react-hook-form";
import { TextField } from "@material-ui/core";
import { useTranslation } from "react-i18next";

InputTextField.propTypes = {
  name: PropTypes.string.isRequired,
  form: PropTypes.object.isRequired,
  label: PropTypes.string,
  isDisable: PropTypes.bool,
};

function InputTextField({ name, form, label, isDisable, variant }) {
  const [t] = useTranslation();
  const error = form?.formState.errors;
  const errorMessage = error[name]?.message;
  const hasError = !!errorMessage;
  return (
    <Controller
      name={name}
      control={form.control}
      render={({ field: { onChange, onBlur, value, name, ref } }) => (
        <TextField
          variant={variant}
          margin="normal"
          fullWidth
          label={t(`common.${label}`)}
          autoFocus
          name={name}
          value={value}
          onChange={onChange}
          onBlur={onBlur}
          error={hasError}
          helperText={errorMessage}
          disabled={isDisable}
        />
      )}
    />
  );
}

export default InputTextField;
