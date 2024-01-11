import { axios } from '../../config/axiosConfig';
import { endpoints } from '../../config/apiConfig';

async function getAllCategory() {
  return axios
    .get(endpoints.admin.allCategory)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function getCategoryDetail(catId) {
  return axios
    .get(`${endpoints.admin.getCategoryDetail}${catId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function getCourseDetail(crsId) {
  return axios
    .get(`${endpoints.admin.getAdminCourseDetail}${crsId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function updateCourse(crsData) {
  let crsPayload = JSON.stringify(crsData);
  return axios
    .put(endpoints.admin.updataCourse, crsPayload)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function getCourseLevel() {
  return axios
    .get(endpoints.courses.getCourseLevel)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function addCourse(crsData) {
  let crsPayload = JSON.stringify(crsData);
  return axios
    .post(endpoints.courses.addCourse, crsPayload)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function deleteCourse(crsId) {
  return axios
    .delete(`${endpoints.courses.deleteCourse}${crsId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function addCategory(catData) {
  let catPayload = JSON.stringify(catData);
  return axios
    .post(endpoints.category.addCategory, catPayload)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function toggleCategory(catData) {
  let catPayload = JSON.stringify(catData);
  return axios
    .post(endpoints.category.toggleCategory, catPayload)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}
async function editCategory(catData) {
  let catPayload = JSON.stringify(catData);
  return axios
    .put(endpoints.category.editCategory, catPayload)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function getIndustory() {
  return axios
    .get(endpoints.adminJobs.getAllIndustory)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function getSkills() {
  return axios
    .get(endpoints.adminJobs.getAllSkills)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function getLocations() {
  return axios
    .get(endpoints.adminJobs.getAllLocation)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function getTitles() {
  return axios
    .get(endpoints.adminJobs.getAllTitles)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function addAdminJob(jobData) {
  const config = {
    params: {
      form: true,
    },
}
  let url = endpoints.adminJobs.addAdminJob;
  let body = new FormData();
  body.append('name', '');
  body.append('company', jobData.company);
  body.append('file', jobData.file);
  body.append('companyLink', jobData.companyLink);
  body.append('noOfRole', jobData.noOfRole);
  body.append('shortDescription', jobData.shortDescription);
  body.append('description', jobData.description);
  body.append('category', jobData.category);

  jobData.jobLocations.forEach((item, index) => {
    body.append(`jobLocations[${index}].locationId`, item.value);
  });
  jobData.jobSkills.forEach((item, index) => {
    body.append(`jobSkills[${index}].skillId`, item.value);
  });
  jobData.jobIndustry.forEach((item, index) => {
    body.append(`jobIndustry[${index}].industryId`, item.value);
  });
  jobData.jobTitles.forEach((item, index) => {
    body.append(`jobTitles[${index}].titleId`, item.value);
  });

  return axios
    .post(url, body,config)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}

async function getAllJobs() {
  return axios
    .get(endpoints.adminJobs.getAllJobs)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function getAllResources() {
  return axios
    .get(endpoints.adminResource.getAllResources)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}
async function searchCustomers(data) {
  let customerPayload = JSON.stringify(data);
  return axios
    .post(endpoints.adminOneToOne.searchCustomers, customerPayload)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function subscribeOneToOne(data) {
  let subsPayload = JSON.stringify(data);
  return axios
    .post(endpoints.adminOneToOne.subscribeOneToOne, subsPayload)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function deleteJob(jobId) {
  return axios
    .delete(`${endpoints.adminJobs.deleteJob}${jobId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function getJobById(jobId) {
  return axios
    .get(`${endpoints.adminJobs.getJobById}${jobId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function uploadImage(jobData) {
  const config = {
    params: {
      form: true,
    },
}
  let url = endpoints.adminJobs.uploadImg;
  let body = new FormData();
  body.append('id', jobData.id);
  body.append('file', jobData.file);
  return axios
    .put(url, body,config)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}

async function updateJob(jobId, updateData) {
  const config = {
    params: {
      form: true,
    },
}
  let url = endpoints.adminJobs.updateJob;
  let body = new FormData();
  body.append('id', jobId);
  body.append('name', '');
  body.append('company', updateData.company);
  body.append('companyLink', updateData.companyLink);
  body.append('noOfRole', updateData.noOfRole);
  body.append('shortDescription', updateData.shortDescription);
  body.append('description', updateData.description);
  body.append('category', updateData.category);

  updateData.jobLocations.forEach((item, index) => {
    body.append(`jobLocations[${index}].locationId`, item.value);
  });
  updateData.jobSkills.forEach((item, index) => {
    body.append(`jobSkills[${index}].skillId`, item.value);
  });
  updateData.jobIndustry.forEach((item, index) => {
    body.append(`jobIndustry[${index}].industryId`, item.value);
  });
  updateData.jobTitles.forEach((item, index) => {
    body.append(`jobTitles[${index}].titleId`, item.value);
  });

  return axios
    .put(url, body,config)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}

async function addAdminResource(resourceData) {
  const config = {
    params: {
      form: true,
    },
}
  let url = endpoints.admin.addResource;
  let body = new FormData();
  body.append('Title', resourceData.title);
  body.append('Preview', resourceData.preview);
  body.append('Description', resourceData.description);
  body.append('Length', resourceData.length);
  body.append('IsFeatured', resourceData.isFeatured);
  body.append('Media', resourceData.media);
  body.append('ResourceType', resourceData.resourceType);
  body.append('VideoLink', resourceData.videoLink);
  body.append('SendNotification', resourceData.SendNotification);

  return axios
    .post(url, body,config)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}

async function getResourceById(id) {
  return axios
    .get(`${endpoints.admin.getResource}${id}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function updateAdminResource(resourceData) {
  let resourcePayload = JSON.stringify(resourceData);
  return axios
    .put(endpoints.admin.updateResource, resourcePayload)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

async function updateResourceMedia(resourceData) {
  const config = {
    params: {
      form: true,
    },
}
  let url = endpoints.admin.updateResourceMedia;
  let body = new FormData();
  body.append('ResourceId', resourceData.id);
  body.append('File', resourceData.media);
  return axios
    .put(url, body,config)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      return error;
    });
}

async function deleteResourceMedia(resourceId) {
  return axios
    .delete(`${endpoints.admin.deleteResourceMedia}${resourceId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}
async function deleteResource(resourceId) {
  return axios
    .delete(`${endpoints.admin.deleteResource}${resourceId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log(error);
      return null;
    });
}

export const adminCourseApi = {
  getAllCategory: getAllCategory,
  getCategoryDetail: getCategoryDetail,
  getCourseDetail: getCourseDetail,
  updateCourse: updateCourse,
  getCourseLevel: getCourseLevel,
  addCourse: addCourse,
  deleteCourse: deleteCourse,
  addCategory: addCategory,
  toggleCategory: toggleCategory,
  editCategory: editCategory,
  getIndustory: getIndustory,
  getSkills: getSkills,
  getLocations: getLocations,
  getTitles: getTitles,
  addAdminJob: addAdminJob,
  getAllJobs: getAllJobs,
  deleteJob: deleteJob,
  getJobById: getJobById,
  uploadImage: uploadImage,
  updateJob: updateJob,
  getAllResources: getAllResources,
  addAdminResource: addAdminResource,
  searchCustomers: searchCustomers,
  subscribeOneToOne: subscribeOneToOne,
  getResourceById: getResourceById,
  updateAdminResource: updateAdminResource,
  updateResourceMedia: updateResourceMedia,
  deleteResourceMedia: deleteResourceMedia,
  deleteResource: deleteResource,

};
