import { userJobApi } from './userJobApi';
import { userJobTypes } from './userJobTypes';

const onChange = (prop, value) => {
  return (dispatch) => {
    dispatch({
      type: userJobTypes.USER_JOB_ONCHANGE,
      payload: { prop: prop, value: value },
    });
  };
};

const getJobsByFilters = (filter, isInfinite) => async (dispatch) => {
  const data = await userJobApi.getJobs(filter);
  if (data?.success) {
    dispatch({
      type: userJobTypes.USER_JOB_ONCHANGE,
      payload: { prop: 'allJobs', value: data.data?.jobs },
    });
    if (!isInfinite) {
      dispatch({
        type: userJobTypes.USER_JOB_ONCHANGE,
        payload: { prop: 'totalRolesCount', value: data.data?.totalRolesCount },
      });
    }
  }
  return data;
};

const onGetAllFilterValue = () => async () => {
  return userJobApi.getAllFilterValue();
};

const onGetUserJobById = (jobId) => async () => {
  return userJobApi.getUserJobById(jobId);
};

const onApplyJob = (jobData) => async () => {
  return userJobApi.applyJob(jobData);
};

const onSelectTitles = (value) => async (dispatch) => {
  dispatch({ type: userJobTypes.SELECTED_TITLES, payload: value });
};
const onSelectSkills = (value) => async (dispatch) => {
  dispatch({ type: userJobTypes.SELECTED_SKILLS, payload: value });
};
const onSelectIndustries = (value) => async (dispatch) => {
  dispatch({ type: userJobTypes.SELECTED_INDUSTRIESS, payload: value });
};
const onSelectLocations = (value) => async (dispatch) => {
  dispatch({ type: userJobTypes.SELECTED_LOCATIONS, payload: value });
};
const onSortRelevantSelection = (value) => async (dispatch) => {
  dispatch({ type: userJobTypes.SELECTED_SORTRELEVANTTYPE, payload: value });
};
const onSortRecentSelection = (value) => async (dispatch) => {
  dispatch({ type: userJobTypes.SELECTED_SORTRECENTTYPE, payload: value });
};
const onClearFilter = () => async (dispatch) => {
  dispatch({ type: userJobTypes.CLEAR_FILTER_VALUE });
};
const onGetResources = (date) => async () => {
  return userJobApi.getResources(date);
};
const onGetFeaturedResource = () => async () => {
  return userJobApi.getFeaturedResource();
};
const onGetResourceByUrl = (url) => async () => {
  return userJobApi.getResourceByUrl(url);
};
const onResourceApplause = (resourceId) => async () => {
  return userJobApi.resourceApplause(resourceId);
};

export const userJobActions = {
  onChange: onChange,
  getJobsByFilters: getJobsByFilters,
  onGetAllFilterValue: onGetAllFilterValue,
  onSelectTitles: onSelectTitles,
  onSelectSkills: onSelectSkills,
  onSelectIndustries: onSelectIndustries,
  onSelectLocations: onSelectLocations,
  onSortRelevantSelection: onSortRelevantSelection,
  onSortRecentSelection: onSortRecentSelection,
  onClearFilter: onClearFilter,
  onGetUserJobById: onGetUserJobById,
  onApplyJob: onApplyJob,
  onGetResources: onGetResources,
  onGetResourceByUrl: onGetResourceByUrl,
  onResourceApplause: onResourceApplause,
  onGetFeaturedResource: onGetFeaturedResource,
};
