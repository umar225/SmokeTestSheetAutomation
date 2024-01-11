import { combineReducers } from 'redux';
import HomeReducer from './home/homeReducer';
import CourseReducer from './course/courseReducer';
import AccountReducer from './account/accountReducer';
import AdminCourseReducer from './adminCourses/adminCourseReducer';
import UserJobReducer from './user/userJobReducer';

export default combineReducers({
  HomeReducer: HomeReducer,
  CourseReducer: CourseReducer,
  AccountReducer: AccountReducer,
  AdminCourseReducer: AdminCourseReducer,
  UserJobReducer: UserJobReducer,
});
