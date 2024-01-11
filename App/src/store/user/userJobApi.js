import { axios } from '../../config/axiosConfig';
import { endpoints } from '../../config/apiConfig';

async function getJobs(filters) {
  const payload = JSON.stringify(filters);
  return axios
    .post(endpoints.userJobs.getJobsByFilters, payload)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function getAllFilterValue() {
  return axios
    .get(endpoints.userJobs.getAllFilterValue)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function getUserJobById(jobId) {
  return axios
    .get(`${endpoints.userJobs.getUserJobById}${jobId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function getResources(date) {
  return axios
    .get(`${endpoints.resourceCenter.getResources}?${date}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function getFeaturedResource() {
  return axios
    .get(`${endpoints.resourceCenter.getFeaturedResource}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function applyJob(jobData) {
  let url = endpoints.userJobs.applyJob;
  const config = {
    params: {
      form: true,
    },
}
  let body = new FormData();
  body.append('name', jobData.name);
  body.append('email', jobData.email);
  body.append('description', jobData.description);
  body.append('jobId', jobData.jobId);
  body.append('file', jobData.file);
  return axios
    .post(url, body, config)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}

async function getResourceByUrl(url) {
  return axios
    .get(`${endpoints.resourceCenter.getResourceByUrl}${url}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function resourceApplause(resourceId) {
  return axios
    .put(`${endpoints.resourceCenter.resourceApplause}${resourceId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

export const userJobApi = {
  getJobs: getJobs,
  getAllFilterValue: getAllFilterValue,
  getUserJobById: getUserJobById,
  applyJob: applyJob,
  getResources: getResources,
  getResourceByUrl: getResourceByUrl,
  resourceApplause: resourceApplause,
  getFeaturedResource: getFeaturedResource,
};
