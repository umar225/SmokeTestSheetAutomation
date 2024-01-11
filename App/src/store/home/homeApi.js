import { axios } from '../../config/axiosConfig';
import { endpoints } from '../../config/apiConfig';

async function getTFResponse(responseId) {
  return axios
    .get(`${endpoints.home.getFormResponse}${responseId}`)
    .then((response) => {
      let data = response.data;
      return data;
    })
    .catch((error) => {
      return error;
    });
}

async function getPmpCourses() {
  return axios
    .get(`${endpoints.courses.getPmpCourses}`)
    .then((response) => {
      let data = response.data;
      return data;
    })
    .catch((error) => {
      return error;
    });
}

async function getCoachingCoursesByLevel(level) {
  return axios
    .get(`${endpoints.courses.getCoachingCourses}/${level}`)
    .then((response) => {
      let data = response.data;
      return data;
    })
    .catch((error) => {
      return error;
    });
}

async function getLeadershipCoursesByLevel(level) {
  return axios
    .get(`${endpoints.courses.getLeadershipCourses}/${level}`)
    .then((response) => {
      let data = response.data;
      return data;
    })
    .catch((error) => {
      return error;
    });
}

async function getNedTraining() {
  return axios
    .get(`${endpoints.courses.getNedTraining}`)
    .then((response) => {
      let data = response.data;
      return data;
    })
    .catch((error) => {
      return error;
    });
}

export const homeApi = {
  getTFResponse: getTFResponse,
  getPmpCourses: getPmpCourses,
  getCoachingCoursesByLevel: getCoachingCoursesByLevel,
  getLeadershipCoursesByLevel: getLeadershipCoursesByLevel,
  getNedTraining: getNedTraining,
};
