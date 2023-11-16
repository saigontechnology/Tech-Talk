import { Axios } from "axios";
import { InjectionKey } from "vue";

export class ForceLoginService {
  axios = new Axios({
    baseURL: "https://api.realworld.io/api/users",
    responseType: "json",
    headers: {
      accept: "application/json, text/plain, */*",
      "Content-Type": "application/json;charset=UTF-8",
    },
    transformResponse: (data) => JSON.parse(data),
  });

  login(): Promise<unknown> {
    return this.axios.post(
      "/login",
      JSON.stringify({
        user: { email: "phat.nguyensts40@gmail.com", password: "dkmm111111" },
      })
    );
  }
}

export const FORCE_LOGIN_SERVICE_TOKEN: InjectionKey<ForceLoginService> = Symbol(
  "FORCE_LOGIN_SERVICE_TOKEN"
);
