import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import CustomButton from "../Button";

const schema = yup
  .object({
    email: yup.string().required(),
    password: yup.string().required(),
  })
  .required();

const CustomForm = ({ header, subHeader = "", onSubmit, status }) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
  });

  return (
    <form className={`form ${status}`} onSubmit={handleSubmit(onSubmit)}>
      {header && (
        <div className="form__header">
          {subHeader && <p className="form__sub-header">{subHeader}</p>}
          {header}
        </div>
      )}

      <div className="form__body">
        <div className="form-item">
          <label className="form-item__label">Email</label>
          <input
            type="text"
            className="form-item__input"
            placeholder="Enter your email"
            {...register("email")}
          />
          {errors.email && (
            <span className="text-error">{errors.email?.message}</span>
          )}
        </div>

        <div className="form-item">
          <label className="form-item__label">Password</label>
          <input
            type="text"
            className="form-item__input"
            placeholder="Enter your password"
            {...register("password")}
          />
          {errors.password && (
            <span className="text-error">{errors.password?.message}</span>
          )}
        </div>
      </div>
      <div className="form__footer">
        <CustomButton label="Submit" type="submit" color="primary" />
      </div>
    </form>
  );
};

export default CustomForm;
