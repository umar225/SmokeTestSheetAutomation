import { homeTypes } from './homeTypes';
import { homeApi } from './homeApi';

const onChange = (prop, value) => {
  return (dispatch) => {
    dispatch({
      type: homeTypes.HOME_ONCHANGE,
      payload: { prop: prop, value: value },
    });
  };
};

const onGetTFResponse = (responseId) => async (_dispatch) => {
  const data = await homeApi.getTFResponse(responseId);
  return data;
};
const onGetPmpCourses = async () => {
  const data = await homeApi.getPmpCourses();
  return data;
};
const onGetCoachingCourses = async (level) => {
  const data = await homeApi.getCoachingCoursesByLevel(level);
  return data;
};
const onGetLeadershipCourses = async (level) => {
  const data = await homeApi.getLeadershipCoursesByLevel(level);
  return data;
};
const onGetNedTraining = async () => {
  const data = await homeApi.getNedTraining();
  return data;
};

export const homeActions = {
  onChange: onChange,
  onGetTFResponse: onGetTFResponse,
  onGetPmpCourses: onGetPmpCourses,
  onGetCoachingCourses: onGetCoachingCourses,
  onGetLeadershipCourses: onGetLeadershipCourses,
  onGetNedTraining: onGetNedTraining,
};
