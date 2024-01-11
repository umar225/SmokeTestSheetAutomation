import { adminCourseApi } from './adminCourseApi';
import { adminCourseTypes } from './adminCourseTypes';

const onChange = (prop, value) => {
  return (dispatch) => {
    dispatch({
      type: adminCourseTypes.ADMIN_COURSE_ONCHANGE,
      payload: { prop: prop, value: value },
    });
  };
};

const onGetAllCategory = () => async () => {
  return adminCourseApi.getAllCategory();
};

const onGetCategoryDetail = (catId) => async () => {
  return adminCourseApi.getCategoryDetail(catId);
};

const onGetCourseDetail = (crsId) => async () => {
  return adminCourseApi.getCourseDetail(crsId);
};

const onUpdateCourse = (crsData) => async () => {
  return adminCourseApi.updateCourse(crsData);
};
const onSelectedCrsLvl = (value) => async (dispatch) => {
  dispatch({ type: adminCourseTypes.SELECTED_COURSE_LEVEL, payload: value });
};

const onGetCourseLvl = () => async () => {
  return adminCourseApi.getCourseLevel();
};
const onAddCourse = (courseData) => async () => {
  return adminCourseApi.addCourse(courseData);
};

const onDeleteCourse = (courseId) => async () => {
  return adminCourseApi.deleteCourse(courseId);
};

const onAddCategory = (categoryData) => async () => {
  return adminCourseApi.addCategory(categoryData);
};

const onToggleCategory = (categoryData) => async () => {
  return adminCourseApi.toggleCategory(categoryData);
};

const onEditCategory = (categoryData) => async () => {
  return adminCourseApi.editCategory(categoryData);
};

const onGetIndustury = () => async () => {
  return adminCourseApi.getIndustory();
};
const onGetSkills = () => async () => {
  return adminCourseApi.getSkills();
};
const onGetLocation = () => async () => {
  return adminCourseApi.getLocations();
};
const onGetTitles = () => async () => {
  return adminCourseApi.getTitles();
};
const onAddAdminJob = (jobData) => async () => {
  return adminCourseApi.addAdminJob(jobData);
};
const onGetAllJobs = () => async () => {
  return adminCourseApi.getAllJobs();
};
const onGetJobById = (jobId) => async () => {
  return adminCourseApi.getJobById(jobId);
};
const onDeleteJob = (jobId) => async () => {
  return adminCourseApi.deleteJob(jobId);
};
const onUploadImage = (imgData) => async () => {
  return adminCourseApi.uploadImage(imgData);
};
const onUpdateJob = (jobId, jobData) => async () => {
  return adminCourseApi.updateJob(jobId, jobData);
};
const onGetAllResources = () => async () => {
  return adminCourseApi.getAllResources();
};
const onAddResource = (resourceData) => async () => {
  return adminCourseApi.addAdminResource(resourceData);
};
const onGetResourceById = (resourceId) => async () => {
  return adminCourseApi.getResourceById(resourceId);
};
const searchCustomers = (data) => async () => {
  return adminCourseApi.searchCustomers(data);
};
const subscribeOneToOne = (data) => async () => {
  return adminCourseApi.subscribeOneToOne(data);
};
const onUpdateResource = (resourceData) => async () => {
  return adminCourseApi.updateAdminResource(resourceData);
};
const onUpdateResourceMedia = (resourceData) => async () => {
  return adminCourseApi.updateResourceMedia(resourceData);
};
const onDeleteResourceMedia = (resourceId) => async () => {
  return adminCourseApi.deleteResourceMedia(resourceId);
};
const onDeleteResource = (resourceId) => async () => {
  return adminCourseApi.deleteResource(resourceId);
};

export const adminCourseActions = {
  onChange: onChange,
  onGetAllCategory: onGetAllCategory,
  onGetCategoryDetail: onGetCategoryDetail,
  onGetCourseDetail: onGetCourseDetail,
  onUpdateCourse: onUpdateCourse,
  onSelectedCrsLvl: onSelectedCrsLvl,
  onGetCourseLvl: onGetCourseLvl,
  onAddCourse: onAddCourse,
  onDeleteCourse: onDeleteCourse,
  onAddCategory: onAddCategory,
  onToggleCategory: onToggleCategory,
  onEditCategory: onEditCategory,
  onGetIndustury: onGetIndustury,
  onGetSkills: onGetSkills,
  onGetLocation: onGetLocation,
  onGetTitles: onGetTitles,
  onAddAdminJob: onAddAdminJob,
  onGetAllJobs: onGetAllJobs,
  onGetJobById: onGetJobById,
  onDeleteJob: onDeleteJob,
  onUploadImage: onUploadImage,
  onUpdateJob: onUpdateJob,
  onGetAllResources: onGetAllResources,
  onAddResource: onAddResource,
  onGetResourceById: onGetResourceById,
  onUpdateResource: onUpdateResource,
  onUpdateResourceMedia: onUpdateResourceMedia,
  onDeleteResourceMedia: onDeleteResourceMedia,
  onDeleteResource: onDeleteResource,
  searchCustomers: searchCustomers,
  subscribeOneToOne: subscribeOneToOne,
};
