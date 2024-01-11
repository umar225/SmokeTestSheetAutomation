import { courseApi } from './courseApi';
import { courseTypes } from './courseTypes';

const onChange = (prop, value) => {
  return (dispatch) => {
    dispatch({
      type: courseTypes.COURSE_ONCHANGE,
      payload: { prop: prop, value: value },
    });
  };
};

const onGetAllCourseLibrary = () => async () => {
  const data = await courseApi.getCourseLibrary();
  return data;
};
const onGetCourseDetails = async (url) => {
  const data = await courseApi.getCourseDetails(url);
  return data;
};

export const courseActions = {
  onChange: onChange,
  onGetAllCourseLibrary: onGetAllCourseLibrary,
  onGetCourseDetails: onGetCourseDetails,
};
