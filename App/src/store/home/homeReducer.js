import { homeTypes } from "./homeTypes";

const INITIAL_STATE = {
  errorMessage: ""
};

export default (state, action) => {
  if (!state) state = INITIAL_STATE;
  if (action.type === homeTypes.HOME_ONCHANGE) {
    return {
      ...state,
      [action.payload.prop]: action.payload.value,
    };
  }
  else {
    return state;
  }
};
