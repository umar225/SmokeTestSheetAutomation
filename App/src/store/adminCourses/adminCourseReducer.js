import { adminCourseTypes } from './adminCourseTypes';
import _ from 'lodash';

const INITIAL_STATE = {
  errorMessage: '',
  selectedLevels: [],
};

export default (state, action) => {
  if (!state) state = INITIAL_STATE;
  switch (action.type) {
    case adminCourseTypes.ADMIN_COURSE_ONCHANGE:
      return {
        ...state,
        [action.payload.prop]: action.payload.value,
      };
    case adminCourseTypes.SELECTED_COURSE_LEVEL: {
      const newState = { ...state };
      const selectedItem = action.payload;
      const isExist = newState.selectedLevels.find((lvl) => {
        return lvl.levelId === selectedItem.levelId;
      });
      if (isExist) {
        _.remove(newState.selectedLevels, (lvel) => {
          return lvel.levelId === selectedItem.levelId;
        });
      } else {
        newState.selectedLevels.push(selectedItem);
      }
      return newState;
    }
    default:
      return state;
  }
};
