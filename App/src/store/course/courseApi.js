import { axios } from '../../config/axiosConfig';
import { endpoints } from '../../config/apiConfig';

async function getCourseLibrary() {
  return axios
    .get(endpoints.courseLibrary.getAllCourseLibrary)
    .then((response) => {
      let data = response.data;
      return data;
    })
    .catch((error) => {
      return error;
    });
}

async function getCourseDetails(url) {
  return axios
    .get(`${endpoints.courses.getCourseDetail}/${url}`)
    .then((response) => {
      let data = response.data;
      return data;
    })
    .catch((error) => {
      return error;
    });
}

export const courseApi = {
  getCourseLibrary: getCourseLibrary,
  getCourseDetails: getCourseDetails,
};
