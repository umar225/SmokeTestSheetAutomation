import { axios } from '../../config/axiosConfig';
import { endpoints } from '../../config/apiConfig';

async function dashboard() {
  return axios
    .get(endpoints.home.dashboard)
    .then((response) => {
        let data = response.data;
        return data;
      })
      .catch((error) => {
        return error;
      });
}

export const dashboardApi = {
    dashboard: dashboard,
};
